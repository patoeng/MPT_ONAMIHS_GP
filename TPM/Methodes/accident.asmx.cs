using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
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
    /// Summary description for accident
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class accident : WebService
    {
        protected TPMHelper F = new TPMHelper();

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Generate(string ai,string eno,string enam,string dep, string dt,string jb, string lc, string mcno, string mcdis, string inj, string dmc,string at, string others )
        {
            var mwoid = new SqlParameter
            {
                ParameterName = "@accidentkey",
                DbType = DbType.String,
                Size = 100,
                Direction = ParameterDirection.InputOutput,
                Value = ai
                
            };
            var sqlparams = new List<SqlParameter>
                {   
                    new SqlParameter("@employeeNum", eno),
                    new SqlParameter("@employeeName", enam),
                    new SqlParameter("@department", dep),
                    new SqlParameter("@mcNum", mcno),
                    new SqlParameter("@mcDes", mcdis),
                    new SqlParameter("@location", lc),
                    new SqlParameter("@uploadedBy", new MySessions().EmployeeName+" ("+ new MySessions().EmployeeNo+")"),
                    new SqlParameter("@job", jb),
                    new SqlParameter("@injuries", inj),
                    new SqlParameter("@other", others),
                    new SqlParameter("@accType", at),
                    new SqlParameter("@numberMC", dmc),
                    new SqlParameter("@datetime", dt),mwoid
                  
                };
            int i = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(), CommandType.StoredProcedure,ai==string.Empty? "usp_generateAccident":"usp_updateAccident",
                                              sqlparams.ToArray());

            string s = mwoid.Value != null ? mwoid.Value.ToString() : "";

            return s;
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

            var assetDs = SqlHelper.ExecuteDataset(F.TPMDBConnection(), CommandType.Text, sql);

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
        public string GetEmployee(string id)
        {
            

            var assetDs = SqlHelper.ExecuteDataset(TPMHelper.DBVMSstring, CommandType.StoredProcedure, "usp_getEmployee_byNo", new SqlParameter("@EmpId",id));

            string s = "";
            if (assetDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in assetDs.Tables[0].Rows)
                {
                    s = dr[0].ToString();
                }
            }
                       
            return s;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Delete(string id)
        {
            var getid = id.Split('-');
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                              "usp_AccAttachmentsDelete", new SqlParameter("@id", getid[1]));

            string path = HttpContext.Current.Server.MapPath("../UploadedFiles/" + getid[2]);
            if (File.Exists(@path))
            {
                File.Delete(@path);
            }

            return getid[0] + "_" + getid[1];
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Summary(string serialcode, int isdownload, string department, string startdate, string accType, string enddate = null)
        {
            serialcode = serialcode.Trim();

            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startdate", startdate),
                    new SqlParameter("@todate", enddate),
                    new SqlParameter("@deptid", department),
                    new SqlParameter("@acctype", accType),
                    new SqlParameter("@serialcode", serialcode),
                   
                };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_summary_LAccident", sqlparams.ToArray());
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
                var tr = new TableRow { TableSection = TableRowSection.TableHeader };
                dt = ds.Tables[0];
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
                var xlsx = "summary_Accident_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
                var fileName = Server.MapPath("~/UploadedFiles/" + xlsx);
                var newFile = new FileInfo(fileName);
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(fileName);
                }
                var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("Accident Summary");
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
