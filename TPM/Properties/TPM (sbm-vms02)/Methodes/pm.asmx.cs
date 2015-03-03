using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
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
    public class pm : System.Web.Services.WebService
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
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@PM_Schedule_Id",id));
            SqlParameter New_id = new SqlParameter()
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
                default: status = 1; break;
            }
            switch (act)
            {
                case "btnRemarks": 
                    sqlparams.Add(new SqlParameter("@descriptions", val));
                    usp = "usp_LPMSchedulesRemarksUpdate";
                    break;
                default:
                    sqlparams.Add(new SqlParameter("@done_by", new MySessions().EmployeeNo));
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
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@AssetCode", sss[0]));
            sqlparams.Add(new SqlParameter("@Date", sss[1] + "/" + sss[2] + "/" + sss[3]));
          
            DataSet AssetDS = new DataSet();
            AssetDS = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MPMSchedulesSelect_byAssetCode_date", sqlparams.ToArray());
            List<string> data = new List<string>();
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
            JavaScriptSerializer json = new JavaScriptSerializer();
            string s= json.Serialize(data);
            return s;
           // return new MySessions().EmployeeName;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string action2(string id, string pmid, string act, string pic, string appr)
        {
            List<SqlParameter> sql = new List<SqlParameter>();
            string usp = "";
            switch (act) {
                case "Insert":
                    string[] sss = id.Split('_');
                    sql.Add(new SqlParameter("@assetcode",sss[0]));
                    sql.Add(new SqlParameter("@Date", sss[1]+"/"+sss[2]+"/"+sss[3]));
                    sql.Add(new SqlParameter("@Approved_by", appr));
                    sql.Add(new SqlParameter("@PIC", pic));
                    sql.Add(new SqlParameter("@uploaded_by", new MySessions().EmployeeNo));
                    usp = "usp_MPMSchedulesInsert_byassetcode";
                    break;
                case "Delete":
                    sql.Add(new SqlParameter("@id", pmid));
                    usp = "usp_MPMSchedulesDelete";
                    break;
            }
            DataSet AssetDS = new DataSet();
            AssetDS = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, usp, sql.ToArray());
            
            return id ;
           
        }
    }
}
