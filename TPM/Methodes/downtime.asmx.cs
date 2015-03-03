using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections;
using System.Web.UI.HtmlControls;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.DataValidation;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for downtime
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class downtime : System.Web.Services.WebService
    {
        public class FlotData{
            public FlotData() {
                _data = new List<ArrayList>();
            }
            private string _label= "data";
            private string _color= "blue";
            private List<ArrayList> _data;

            public string label {
                get { return _label; }
                set { _label = value; }
            }
            public string color {
                get { return _color; }
                set { _color = value; }
            }
            public List<ArrayList> data
            {
                get { return _data; }
                set { _data = value; }
            }
            
        }
      
        public class Flot {
            public Flot() {
                _chart = new List<FlotData>();
                _ticks = new List<ArrayList>();
            }
            private int _total;
            private List<FlotData> _chart;
            private List<ArrayList> _ticks;
            public List<FlotData> chart
            {
                get { return _chart; }
                set { _chart = value; }  
            }
            public List<ArrayList> ticks
            {
                get { return _ticks; }
                set { _ticks = value; }
            }
            public int total{
                get { return _total; }
                set { _total = value; }
            }
        }
        public class DowntimePackage {
            public DowntimePackage() {
                _flotchart = new Flot();
            }
            private Flot _flotchart;
            private string _htmltable;
            public Flot flotchart {
                get { return _flotchart; }
                set { _flotchart = value; }
            }
            public string htmltable {
                get { return _htmltable; }
                set { _htmltable = value; }
            }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Showtable(string dept_id,string year, string month)
        {
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@dept_id", dept_id),
                    new SqlParameter("@year", year),
                    new SqlParameter("@month", month)
                 
                };
            var tbl = new Table
                {
                    ID = "jsonTable",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "table table-bordered table-striped"
                };
            TableCell tc;
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_mainpage_DownTime", sqlparams.ToArray());
           
            var tr = new TableRow {TableSection = TableRowSection.TableHeader};
            var dt = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                tc = new TableHeaderCell {Text = dt.Columns[i].ColumnName};
                tr.Cells.Add(tc);
            }
            tbl.Rows.Add(tr);
            var dp = new DowntimePackage();
            var rowid = 0;
            var fdWo = new FlotData();
            var fdPm = new FlotData();
            fdWo.label = "Open";
            fdWo.color = Color.Red.Name;
            fdPm.label = "Close";
            fdPm.color = Color.Blue.Name;
            foreach (DataRow dr in dt.Rows)
            {
                var alticks = new ArrayList();
                rowid++;
                alticks.Add(rowid);
                alticks.Add(dt.Columns[0].DataType == Type.GetType("System.DateTime") ? ((DateTime)(dr[0])).ToString("d MMM yyyy") : dr[0].ToString());
                var aldataWo = new ArrayList {rowid,  dr[1].ToString()};

                fdWo.data.Add(aldataWo);
                var aldataPm = new ArrayList {rowid,  dr[2].ToString()};

                fdPm.data.Add(aldataPm);
                dp.flotchart.ticks.Add(alticks);
                tr = new TableRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    tc = new TableCell
                        {
                            Text =
                                dt.Columns[i].DataType == Type.GetType("System.DateTime")
                                    ? ((DateTime) (dr[i])).ToString("d MMMM yyyy")
                                    : dr[i].ToString()
                        };
                    tr.Cells.Add(tc);
                }
                tbl.Rows.Add(tr);
            }
            dp.flotchart.chart.Add(fdWo);
            dp.flotchart.chart.Add(fdPm);
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            tbl.RenderControl(htw);
            var s = sw.ToString();

            dp.flotchart.total = 700;
            dp.htmltable = s;
            var js = new JavaScriptSerializer();
            s=js.Serialize(dp);
            return s;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string summary(string serialcode,int isdownload,string department, string status_id, string startdate, string adhoc,string safety, string enddate = null)
        {
            serialcode = serialcode.Trim();

            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startdate", startdate),
                    new SqlParameter("@todate", enddate),
                    new SqlParameter("@deptid", department),
                    new SqlParameter("@statusid", status_id),
                    new SqlParameter("@serialcode", serialcode),
                    new SqlParameter("@adhoc",adhoc),
                    new SqlParameter("@safety",safety)
                };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_summary_MWorkOrdersSelect", sqlparams.ToArray());
                DataTable dt;
                if (isdownload == 0)
                {
                    var tbl = new Table
                        {
                            ID = "jsonTable",
                            ClientIDMode = ClientIDMode.Static,
                            CssClass = "table table-bordered table-striped table-condensed"
                        };
                    TableCell tc;
                    var tr = new TableRow {TableSection = TableRowSection.TableHeader};
                    dt = ds.Tables[0];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        tc = new TableHeaderCell {Text = dt.Columns[i].ColumnName};
                        tr.Cells.Add(tc);
                    }
                    tbl.Rows.Add(tr);
                    foreach (DataRow dr in dt.Rows)
                    {

                        tr = new TableRow();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            tc = new TableCell
                                {
                                    Text =
                                        dt.Columns[i].DataType == Type.GetType("System.DateTime") &&
                                        (dr[i].ToString() != "")
                                            ? ((DateTime) (dr[i])).ToString("d MMMM yyyy")
                                            : dr[i].ToString()
                                };

                            if ((dt.Columns[i].ColumnName == "REMARKS") || (dt.Columns[i].ColumnName == "CAUSES"))
                            {
                                tc.Style.Add("white-space", "pre-line");
                            }
                           
                            tr.Cells.Add(tc);

                        }
                        tbl.Rows.Add(tr);
                    }
                    var dp = new DowntimePackage();
                    var sw = new StringWriter();
                    var htw = new HtmlTextWriter(sw);
                    tbl.RenderControl(htw);
                    var s = sw.ToString();


                    dp.htmltable = s;

                    var htm = new HtmlGenericControl("h3");
                    htm.Attributes.Add("class", "btn btn-primary");
                    htm.Attributes.Add("id", "download");
                    htm.InnerText = "Export To Excel";
                    sw = new StringWriter();
                    htw = new HtmlTextWriter(sw);
                    htm.RenderControl(htw);
                    s = sw.ToString();
                    //dp.htmltable = s + "<hr />" + dp.htmltable;
                    //var js = new JavaScriptSerializer();
                    //s = js.Serialize(dp);//reuse s;
                    s += "<hr />" + dp.htmltable;
                    return s;
                }
                else
                { //create excels file
                    var t = DateTime.Now;
                    var xlsx = "summary_bd_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
                    var fileName = Server.MapPath("~/UploadedFiles/" + xlsx);
                    var newFile = new FileInfo(fileName);
                    if (newFile.Exists)
                    {
                        newFile.Delete();  // ensures we create a new workbook
                        newFile = new FileInfo(fileName);
                    }
                    var package = new ExcelPackage();
                    var ws = package.Workbook.Worksheets.Add("Break Down Summary");
                    dt = ds.Tables[0];
                    var row = 0;
                    var fieldnumber = dt.Columns.Count - 1;
                    foreach (DataRow drr in dt.Rows)
                    {
                        row++;
                        for (int col = 0; col < fieldnumber; col++)
                        {
                            ws.SetValue(row, col + 1,  dt.Columns[col].DataType == Type.GetType("System.DateTime") && (drr[col].ToString() != "") ? ((DateTime)(drr[col])).ToString("d MMMM yyyy") : drr[col].ToString());
                        }
                    }

                    ws.InsertRow(1, 1);
                    int column;
                    for (column = 0; column < fieldnumber; column++)
                    {

                        ws.SetValue(1, column + 1, dt.Columns[column].ColumnName); 
                        ws.Column(column + 1).Style.Locked = false;
                    }
                  

                    using (var rng = ws.Cells[1,1,1,fieldnumber])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Color.SetColor(Color.White);
                        rng.Style.WrapText = false;
                        rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        rng.AutoFitColumns();
                    }
                    ws.InsertRow(1, 1);
                    ws.InsertRow(1, 1);
                    ws.SetValue(1, 1, "To");
                    ws.SetValue(1, 2, enddate);
                    ws.InsertRow(1, 1);
                    ws.SetValue(1, 1, "From");
                    ws.SetValue(1, 2, startdate);
                   
                    
                    package.SaveAs(newFile);
                    return  "./UploadedFiles/" + xlsx;
                }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string summary2(string serialcode, int isdownload, string department, string status_id, string startdate, string enddate = null)
        {
            serialcode = serialcode.Trim();

            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startdate", startdate),
                    new SqlParameter("@todate", enddate),
                    new SqlParameter("@deptid", department),
                    new SqlParameter("@statusid", status_id),
                    new SqlParameter("@serialcode", serialcode)
                };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_summary_MPMSchedulesSelect", sqlparams.ToArray());
            DataTable dt;
            if (isdownload == 0)
            {
                var tbl = new Table
                    {
                        ID = "jsonTable",
                        ClientIDMode = ClientIDMode.Static,
                        CssClass = "table table-bordered table-striped table-condensed"
                    };
                TableCell tc;
                var tr = new TableRow {TableSection = TableRowSection.TableHeader};
                dt = ds.Tables[0];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    tc = new TableHeaderCell {Text = dt.Columns[i].ColumnName};
                    tr.Cells.Add(tc);
                }
                tbl.Rows.Add(tr);
                foreach (DataRow dr in dt.Rows)
                {

                    tr = new TableRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        tc = new TableCell
                            {
                                Text =
                                    dt.Columns[i].DataType == System.Type.GetType("System.DateTime") &&
                                    (dr[i].ToString() != "")
                                        ? ((DateTime) (dr[i])).ToString("d MMMM yyyy")
                                        : dr[i].ToString()
                            };

                        if ((dt.Columns[i].ColumnName == "REMARKS") || (dt.Columns[i].ColumnName == "CAUSES"))
                        {
                            tc.Style.Add("white-space", "pre-line");
                        }
                        if ((dt.Columns[i].ColumnName == "RESULT"))
                        {
                            tc.Text = dr[i].ToString() != ""
                                          ? (dr[i].ToString().ToUpper() == "TRUE" ? "PASS" : "FAIL")
                                          : "";
                        }
                        tr.Cells.Add(tc);
                    }
                    tbl.Rows.Add(tr);
                }
                var dp = new DowntimePackage();
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                tbl.RenderControl(htw);
                string s = sw.ToString();


                dp.htmltable = s;

                var htm = new HtmlGenericControl("h3");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.Attributes.Add("id", "download");
                htm.InnerText = "Export To Excel";
                sw = new StringWriter();
                htw = new HtmlTextWriter(sw);
                htm.RenderControl(htw);
                s = sw.ToString();
                //dp.htmltable = s + "<hr />" + dp.htmltable;
                //var js = new JavaScriptSerializer();
                //s = js.Serialize(dp);//reuse s;
                s += "<hr />" + dp.htmltable;
                return s;
            }
            else
            { //create excels file
                DateTime t;
                t = DateTime.Now;
                var xlsx = "summary_pm_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
                var fileName = Server.MapPath("~/UploadedFiles/" + xlsx);
                var newFile = new FileInfo(fileName);
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(fileName);
                }
                var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("Preventive Maintenance Summary");
                dt = ds.Tables[0];
                var row = 0;
                var fieldnumber = dt.Columns.Count - 1;
                foreach (DataRow drr in dt.Rows)
                {
                    row++;
                    for (int col = 0; col < fieldnumber; col++)
                    {
                        ws.SetValue(row, col + 1, dt.Columns[col].DataType == Type.GetType("System.DateTime") && (drr[col].ToString() != "") ? ((DateTime)(drr[col])).ToString("d MMMM yyyy") : drr[col].ToString());
                    }
                }

                ws.InsertRow(1, 1);
                int column;
                for (column = 0; column < fieldnumber; column++)
                {

                    ws.SetValue(1, column + 1, dt.Columns[column].ColumnName);
                    ws.Column(column + 1).Style.Locked = false;
                }


                using (var rng = ws.Cells["A1:" + (char)(65 + fieldnumber - 1) + "1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.White);
                    rng.Style.WrapText = false;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    rng.AutoFitColumns();
                }
                ws.InsertRow(1, 1);
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "To");
                ws.SetValue(1, 2, enddate);
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "From");
                ws.SetValue(1, 2, startdate);


                package.SaveAs(newFile);
                return "./UploadedFiles/" + xlsx;
            }
        }
    }
}
