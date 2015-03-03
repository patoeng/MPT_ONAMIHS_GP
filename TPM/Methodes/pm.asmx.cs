using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;


namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for pm
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class pm : WebService
    {
        public TPMHelper Functions = new TPMHelper();

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string HelloWorld()
        {
            return new MySessions().EmployeeName;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string actions(int id,string act,string dby, string val)
        { 
            string usp = "";
            var sqlparams = new List<SqlParameter> {new SqlParameter("@PM_Schedule_Id", id)};
            var New_id = new SqlParameter()
            {
                Direction = ParameterDirection.Output,
                ParameterName = "@new_id",
                DbType = DbType.Int32
            };
            int status = 0;
            switch (act)
            {
                case "allow": status = 2;
                    break;
                case "start": status = 4;
                    break;
                case "schedule": status = 1;
                    break;
                case "finish": status = 5;
                    break;
                case "review": status = 6;
                    break;
                default: status = 1; break;
            }
            switch (act)
            {
                case "btnRemarks": 
                    sqlparams.Add(new SqlParameter("@descriptions", val));
                    usp = "usp_LPMSchedulesRemarksUpdate";
                    break;
                default:
                    sqlparams.Add(new SqlParameter("@done_by", new MySessions().EmployeeName));
                    sqlparams.Add(new SqlParameter("@Date", val));
                    sqlparams.Add(new SqlParameter("@status_id", status));
                    sqlparams.Add(New_id);
                    usp = "usp_LPMSchedulesInsert";
                    break;
            }


            
           

            int i = SqlHelper.ExecuteNonQuery(Functions.TPMDBConnection(), CommandType.StoredProcedure, usp, sqlparams.ToArray());
            return New_id.Value.ToString();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string getPMbyAssetCode(string id)
        {
            string[] sss = id.Split('_');
            var sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@AssetKey", sss[0]));
            sqlparams.Add(new SqlParameter("@Date", sss[1] + "/" + sss[2] + "/" + sss[3]));
          
            var AssetDS = new DataSet();
            AssetDS = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MPMSchedulesSelect_byAssetCode_date", sqlparams.ToArray());
            var data = new List<string>();
            if (AssetDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in AssetDS.Tables[0].Rows)
                {
                    for (int column = 0; column < AssetDS.Tables[0].Columns.Count; column++)
                    {
                        data.Add(dr[column].ToString());
                    }
                }
            }
            var json = new JavaScriptSerializer();
            string s= json.Serialize(data);
            return s;
           // return new MySessions().EmployeeName;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string setPMbyAssetCode(string id, string value)
        {
            string[] sss = id.Split('_');
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@AssetKey", sss[0]),
                    new SqlParameter("@Date", sss[1] + "/" + sss[2] + "/" + sss[3]),
                    new SqlParameter("@action",value),
                    new SqlParameter("@uploaded_By", new MySessions().EmployeeName)
                    
                };
            var pmid = new SqlParameter
                {
                    ParameterName = "@pmid",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
            sqlparams.Add(pmid);
            
            var i = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MPMSchedulesUpdate_byAssetCode_date", sqlparams.ToArray());
            string balik=pmid.Value.ToString() ;
            string balik2 = "-";
            if (balik !="")
            {
                balik2 = "X";
                if (balik == "-1")
                {
                    balik2 = balik;
                }
            }
            return balik2;
            // return new MySessions().EmployeeName;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string action2(string id, string pmid, string act, string pic, string appr)
        {
            var sql = new List<SqlParameter>();
            var usp = "";
            switch (act) {
                case "Insert":
                    string[] sss = id.Split('_');
                    sql.Add(new SqlParameter("@assetkey",sss[0]));
                    sql.Add(new SqlParameter("@Date", sss[1]+"/"+sss[2]+"/"+sss[3]));
                    sql.Add(new SqlParameter("@Approved_by", appr));
                    sql.Add(new SqlParameter("@PIC", pic));
                    sql.Add(new SqlParameter("@uploaded_by", new MySessions().EmployeeName));
                    usp = "usp_MPMSchedulesInsert_byassetcode";
                    break;
                case "Delete":
                    sql.Add(new SqlParameter("@id", pmid));
                    usp = "usp_MPMSchedulesDelete";
                    break;
            }
            
            SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, usp, sql.ToArray());
            
            return id ;
           
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string jeditable(string id, string value)
        {
            string usp = "usp_MPMschedulesActionUpdate";
            id = id.ToLower();
            string[] param = id.Split('-');
            var sqlparams = new List<SqlParameter>();
           
         
            switch (param[1])
            {
                case "remarks":
                    sqlparams.Add(new SqlParameter("@PM_Schedule_Id", param[0]));
                    sqlparams.Add(new SqlParameter("@descriptions", value));
                    usp = "usp_LPMSchedulesRemarksUpdate";
                    break;
                case "remarks2":
                    sqlparams.Add(new SqlParameter("@PM_Schedule_Id", param[0]));
                    sqlparams.Add(new SqlParameter("@descriptions", value));
                    usp = "usp_LPMSchedulesRemarksUpdate2";
                    break;
                case "action":
                    sqlparams.Add(new SqlParameter("@pmid", param[0]));
                    sqlparams.Add(new SqlParameter("@action", value));
                    break;
                case "result":
                    sqlparams.Add(new SqlParameter("@pmoid", param[0]));
                    sqlparams.Add(new SqlParameter("@result", value));
                    break;
                case "request":
                    sqlparams.Add(new SqlParameter("@pmid", param[0]));
                    sqlparams.Add(new SqlParameter("@request", value));
                    break;
                case "request_type":
                    sqlparams.Add(new SqlParameter("@pmid", param[0]));
                    sqlparams.Add(new SqlParameter("@request_type", value));
                    break;
            }



            int i = SqlHelper.ExecuteNonQuery(Functions.TPMDBConnection(), CommandType.StoredProcedure, usp, sqlparams.ToArray());
            return value;
        
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string exportMonthlyToExcel(string bulan, string tahun)
        {
            var par = new List<SqlParameter>
                {
                    new SqlParameter("@month",bulan),
                    new SqlParameter("@year",tahun),
                    new SqlParameter("@checklisttypes_id",2)
                };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                              "usp_MPMSchedulesSelect_byDept2", par.ToArray());
            var t = DateTime.Now;
          
            var xlsx = "pm_master_schedule_"+bulan+"_"+tahun+"_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
            var FileName = Server.MapPath("~/UploadedFiles/" + xlsx);
            var newFile = new FileInfo(FileName);
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(FileName);
            }
            var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Master PM Schedule");
            var dt = ds.Tables[0];
            var row = 0;
            var fieldnumber = dt.Columns.Count;
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
          


            package.SaveAs(newFile);
            return "./UploadedFiles/" + xlsx;

        
        }
    }
    

}
