using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using TPM.Classes;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for Report2
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Report2 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        
        public DataTable InvertTable(DataTable dt)
        {
            var dtIn=new DataTable {TableName = dt.TableName};
            var pk = new DataColumn[1];
            var ll = new List<String> { dt.Columns[0].ColumnName };
            dtIn.Columns.Add(new DataColumn(dt.Columns[0].ColumnName));
            pk[0] = dtIn.Columns[dt.Columns[0].ColumnName];
            dtIn.PrimaryKey = pk;

            foreach (DataRow dr in dt.Rows)
            {
                dtIn.Columns.Add(new DataColumn(dr[0].ToString()));
            }
            var i = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                if (i > 0)
                {
                    dtIn.Rows.Add(new object[] { dc.ColumnName });
                }
                i++;
            }
            
            foreach (DataRow drs in dt.Rows)
            {
               
              for (int j = 0; j < dt.Columns.Count; j++)
              {
                  if (j > 0)
                  {
                      dtIn.Rows.Find(dt.Columns[j].ColumnName).SetField(drs[0].ToString(), drs[j].ToString());
                  }
              }
            }
            

            return dtIn;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GetDdl(string id)
        {
            string sql;
            if (id == "")
            {
                sql = "SELECT ID,DESCRIPTIONS FROM MDEPARTMENTS WHERE ACTIVE='1'";
            }
            else
            {
                sql = "SELECT SERIAL_NUMBER,DESCRIPTIONS FROM MASSETS WHERE DEPARTMENT_ID='" + id + "' AND ACTIVE='1'";
            }

            var assetDs = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, sql);

            var row = new Dictionary<string, string>();
            if (assetDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in assetDs.Tables[0].Rows)
                {
                    row.Add(dr[0].ToString(), dr[1].ToString());
                }
            }
            var json = new JavaScriptSerializer();
            var s = json.Serialize(row);
            return s;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GetAsset(string id)
        {
            string sql;
            if (id == "")
            {
                sql = "SELECT SERIAL_NUMBER,DESCRIPTIONS FROM MASSETS WHERE  ACTIVE='1'";
            }
            else
            {
                sql = "SELECT SERIAL_NUMBER,DESCRIPTIONS FROM MASSETS WHERE DEPARTMENT_ID='" + id + "' AND ACTIVE='1'";
            }

            var assetDs = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, sql);

            var row = new Dictionary<string, string>();
            if (assetDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in assetDs.Tables[0].Rows)
                {
                    row.Add(dr[0].ToString(), dr[1].ToString());
                }
            }
            var json = new JavaScriptSerializer();
            var s = json.Serialize(row);
            return s;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Summary(int isdownload, string department, string startdate, string ddlFlexy,string rType,string report4=null, string enddate = null)
         {
             string usp="";
             string flexyParam="";
             string chartTittle = "";
             var chartType = eChartType.Area;

             switch (rType)
            {
                case "1" :  
                    usp = "usp_report2_workorderStatus";
                    flexyParam = "@status";
                    chartTittle = "Workorder Status";
                    chartType = eChartType.ColumnStacked;
                    break;
                case "2":
                    usp = "usp_report2_ImpWorkorderStatus";
                     flexyParam = "@status";
                     chartTittle = "Improvement Work Order Status";
                     chartType = eChartType.ColumnStacked;
                    break;
                case "3":
                    usp = "usp_report2_summaryBreakdownByCases";
                     flexyParam = "@mcCond";
                     chartTittle = "Summary Breakdown By Cases";
                     chartType = eChartType.ColumnClustered;
                    break;
                case "4":
                    usp = "usp_report2_summaryBreakdownByTotalHours";
                     flexyParam = "@totalHour";
                     chartTittle = "Summary PM vs BD by Hours";
                     chartType = eChartType.ColumnStacked;
                    break;
                case "5":
                    usp = "usp_report2_summaryBDTop10";
                     flexyParam = "@machine";
                     chartTittle = "Summary TOP 10 Break Down Machine by Case";
                    chartType = eChartType.ColumnClustered;
                    break;
            }

            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startdate", startdate),
                    new SqlParameter("@todate", enddate),
                    new SqlParameter("@deptid", department),
                    new SqlParameter(flexyParam, ddlFlexy)
                   
                };
            if (rType == "4")
            {
                sqlparams.Add(new SqlParameter("@bdtype",report4));
            }
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, usp, sqlparams.ToArray());
            DataTable dt = ds.Tables[0];
            
            var dtTemp = new DataTable("Temp");
            if (rType == "5")
            {

                var pk = new DataColumn[1];
                var ll = new List<String> { "MACHINES" };
                dtTemp.Columns.Add(new DataColumn("MACHINES"));
                pk[0] = dtTemp.Columns["MACHINES"];

                dtTemp.PrimaryKey = pk;

                foreach (DataRow drs in dt.Rows)
                {
                    if (ll.IndexOf(drs["series"].ToString()) <= 0)
                    {
                        ll.Add(drs["series"].ToString());

                        dtTemp.Columns.Add(new DataColumn(drs["series"].ToString()));

                    }
                }
                foreach (DataRow drs2 in dt.Rows)
                {
                    if (dtTemp.Rows.Find(drs2["MC"].ToString()) == null)
                    {
                        dtTemp.Rows.Add(new object[] { drs2["MC"].ToString() });
                    }
                    dtTemp.Rows.Find(drs2["MC"].ToString()).SetField(drs2["series"].ToString(), drs2["cs"].ToString());
                }

            } 
            if (isdownload == 0)
            {
                var tbl = new Table
                {
                    ID = "jsonTable",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "table table-bordered table-striped table-condensed"
                };
                TableCell tc;
                var tr = new TableRow { TableSection = TableRowSection.TableHeader };
                dt = rType == "5" ? dtTemp: InvertTable(ds.Tables[0]);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    tc = new TableHeaderCell { Text = dt.Columns[i].ColumnName };
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
                                    ? ((DateTime)(dr[i])).ToString("d MMMM yyyy")
                                    : dr[i].ToString()
                        };
                        if ((dt.Columns[i].ColumnName == "REMARKS"))
                        {
                            tc.Style.Add("white-space", "pre-line");
                        }

                        tr.Cells.Add(tc);

                    }
                    tbl.Rows.Add(tr);
                }
                var dp = new downtime.DowntimePackage();
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
                var xlsx = "summary_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
                var fileName = Server.MapPath("~/UploadedFiles/" + xlsx);
                var newFile = new FileInfo(fileName);
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(fileName);
                }
                var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("Summary");

                dt = InvertTable(rType == "5" ? dtTemp : ds.Tables[0]);
                var row = 0;
                var fieldnumber = dt.Columns.Count;// -1;
                var totalCol = new int[fieldnumber];
                foreach (DataRow drr in dt.Rows)
                {
                    row++;
                    for (int col = 0; col < fieldnumber; col++)
                    {
                        
                        Int32 j;
                        if (Int32.TryParse(drr[col].ToString(), out j))
                        {
                            ws.SetValue(row, col + 1, j);
                            if(((row != 3) || (rType != "3"))&&(drr[0].ToString().Contains("-") || rType!="5"))
                            {
                                totalCol[col] += j;
                            }
                        }
                        else
                        {
                            ws.SetValue(row, col + 1, drr[col].ToString());
                        }
                    }
                }
                row++;

                ws.SetValue(row,  1, "TOTAL");
                    for (int col = 1; col < fieldnumber; col++)
                    {

                        
                            ws.SetValue(row, col + 1, totalCol[col]);
                        
                    }

                row--;

                ws.InsertRow(1, 1);
                int column;
                for (column = 0; column < fieldnumber; column++)
                {

                    ws.SetValue(1, column + 1, dt.Columns[column].ColumnName);
                    ws.Column(column + 1).Style.Locked = false;
                }


                using (var rng = ws.Cells[1, 1, 1, fieldnumber])
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
               
                var chart = ws.Drawings.AddChart("chart", chartType);
                chart.SetPosition(10,0,0,0);
                chart.Style = eChartStyle.Style2;
                chart.SetSize(1000,600);
                var ct =chart.Title.RichText.Add(chartTittle);
                ct.Bold = true;
                ct.Size = 20;
                var lg = chart.Legend;
                lg.Font.Bold = true;
                lg.Font.Size = 10;
                lg.Position = eLegendPosition.Bottom;
                chart.XAxis.Font.Bold = true;
                chart.YAxis.Font.Bold = true;
                for (int o = 5; o < row+5; o++)
                {
                    var address =((char)((byte) 'A' + fieldnumber-1)).ToString(CultureInfo.InvariantCulture) + o.ToString(CultureInfo.InvariantCulture);
                    var series = chart.Series.Add("B" + o.ToString(CultureInfo.InvariantCulture) + ":" + address, "B4:" + ((char)((byte)'A' + fieldnumber - 1)).ToString(CultureInfo.InvariantCulture) + "4").
                    HeaderAddress = new ExcelAddress("'Summary'!A"+o.ToString(CultureInfo.InvariantCulture));
                }
                package.SaveAs(newFile);
                return "./UploadedFiles/" + xlsx;
            }
        }

    }
    
}
