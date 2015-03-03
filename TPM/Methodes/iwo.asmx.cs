using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TPM.Classes;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for iwo
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Iwo : WebService
    {

        protected TPMHelper F = new TPMHelper();
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string NewIwo(string deptid, string reqid, string req, string cause)
        {
            /*      @department_id INT,
                    @request_type INT,
                    @request nvarchar(1000),
                    @causes nvarchar(1000),
                    @done_by nvarchar(100),
                    @iwokey varchar(100)output*/
            var iwokey = new SqlParameter
                {
                    ParameterName = "@iwokey",
                    Direction = ParameterDirection.Output,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 100
                };
            var sql = new[]
                {
                    new SqlParameter("@department_id",deptid),
                    new SqlParameter("@request_type",reqid),
                    new SqlParameter("@request",req),
                    new SqlParameter("@causes",cause),
                    new SqlParameter("@done_by",new MySessions().EmployeeName),
                    iwokey
                };
            var ds = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_generateImpWorkOrder", sql);
            return iwokey.Value != null ? iwokey.Value.ToString() : "";
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Jeditable(string id, string value)
        {
            string usp = "usp_MIworkordersActionUpdate";
            id = id.ToLower();
            string[] param = id.Split('-');
            var sqlparams = new List<SqlParameter>();
            switch (param[1])
            {
                case "remarks":
                    usp = "usp_LIWorkOrderRemarksUpdate";
                    sqlparams.Add(new SqlParameter("@LIWorkOrder_id", param[0]));
                    sqlparams.Add(new SqlParameter("@descriptions", value));
                    break;
                case "remarks2":
                    usp = "usp_MIWorkOrderRemarksUpdate";
                    sqlparams.Add(new SqlParameter("@MWorkOrder_id", param[0]));
                    sqlparams.Add(new SqlParameter("@descriptions", value));
                    break;
                case "cost_implementation":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@cost_implementation", value));
                    break;
                case "request_type":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@request_type", value));
                    break;
                case "request":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@request", value));
                    break;
                case "causes":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@causes", value));
                    break;
                case "report":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@report", value));
                    break;
              
            }



            int i = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(), CommandType.StoredProcedure, usp, sqlparams.ToArray());
            return value;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Jeditable_Cost(string id, string value)
        {
            string usp = "usp_MIworkordersActionUpdate";
            id = id.ToLower();
            string[] param = id.Split('-');
            var sqlparams = new List<SqlParameter>();
            switch (param[1])
            {
                case "remarks":
                    usp = "usp_LIWorkOrderRemarksUpdate";
                    sqlparams.Add(new SqlParameter("@LIWorkOrder_id", param[0]));
                    sqlparams.Add(new SqlParameter("@descriptions", value));
                    break;
                case "remarks2":
                    usp = "usp_MIWorkOrderRemarksUpdate";
                    sqlparams.Add(new SqlParameter("@MWorkOrder_id", param[0]));
                    sqlparams.Add(new SqlParameter("@descriptions", value));
                    break;
                case "cost_implementation":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@cost_implementation", value));
                    break;
                case "request_type":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@request_type", value));
                    break;
                case "request":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@request", value));
                    break;
                case "causes":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@causes", value));
                    break;
                case "report":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@report", value));
                    break;

            }



            int i = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(), CommandType.StoredProcedure, usp, sqlparams.ToArray());
            return value;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Action(string act, string id, string val)
        {
            string usp;
            var sql = new List<SqlParameter>();
            act = act.ToUpper();
            switch (act)
            {
                case "REMARKS":
                    usp = "usp_LWorkOrderRemarksUpdate";
                    sql.Add(new SqlParameter("@LWorkOrder_id", id));
                    sql.Add(new SqlParameter("@descriptions", val));
                    break;
                default:
                    usp = "usp_updateIWorkOrder";
                    sql.Add(new SqlParameter("@mwoid", id));
                    sql.Add(new SqlParameter("@done_by", new MySessions().EmployeeName));
                    sql.Add(new SqlParameter("@status_id", val));
                    break;
            }
            int i = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(), CommandType.StoredProcedure, usp, sql.ToArray());
            return i.ToString(CultureInfo.InvariantCulture);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string delete(string id)
        {
            var getid = id.Split('-');
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                              "usp_IwoAttachmentsDelete", new SqlParameter("@id", getid[1]));

            string path = HttpContext.Current.Server.MapPath("../UploadedFiles/"+getid[2]);
            if (File.Exists(@path))
            {
                File.Delete(@path);
            }

            return getid[0] + "_" + getid[1];
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string summary(int isdownload, string department, string status_id, string startdate, string req_type, string enddate = null)
        {         
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startdate", startdate),
                    new SqlParameter("@todate", enddate),
                    new SqlParameter("@deptid", department),
                    new SqlParameter("@statusid", status_id),
                    new SqlParameter("@req_type",req_type)
                };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_summary_MIWorkOrdersSelect", sqlparams.ToArray());
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

                        if ((dt.Columns[i].ColumnName == "REMARKS") || (dt.Columns[i].ColumnName == "CAUSES") || (dt.Columns[i].ColumnName == "REPORT"))
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
               // dp.htmltable = s + "<hr />" + dp.htmltable;
                //var js = new JavaScriptSerializer();
                //s = js.Serialize(dp);//reuse s;
                s += "<hr />" + dp.htmltable;
                return s;
            }
            else
            { //create excels file
                var t = DateTime.Now;
                var xlsx = "summary_iwo_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
                var fileName = Server.MapPath("~/UploadedFiles/" + xlsx);
                var newFile = new FileInfo(fileName);
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(fileName);
                }
                var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("Improvement Work Order Summary");
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

                package.SaveAs(newFile);
                return "./UploadedFiles/" + xlsx;
            }
        }
    }
}
