using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Services;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;



namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for workorder
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class workorder : System.Web.Services.WebService
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
        public string action(string act, string id, string val)
        {
            string usp = "";
            List<SqlParameter> sql = new List<SqlParameter>();
            act = act.ToUpper();
            switch (act)
            {
                case "REMARKS": 
                    usp = "usp_LWorkOrderRemarksUpdate";
                    sql.Add(new SqlParameter("@LWorkOrder_id", id));
                    sql.Add(new SqlParameter("@descriptions", val));
                    break;
                default:
                    usp = "usp_updateWorkOrder";
                    sql.Add(new SqlParameter("@mwoid", id));
                    sql.Add(new SqlParameter("@done_by", new MySessions().EmployeeNo));
                    sql.Add(new SqlParameter("@status_id", DBNull.Value));
                    break;
            }
            int i = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(),CommandType.StoredProcedure,usp,sql.ToArray());
            return i.ToString();
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string getDDL(string id)
        {
            string sql = "";
            if (id == "")
            {
                sql = "SELECT ID,DESCRIPTIONS FROM MDEPARTMENTS WHERE ACTIVE='1'";
            }
            else {
                sql = "SELECT ID,DESCRIPTIONS FROM MASSETS WHERE DEPARTMENT_ID='"+id+"' AND ACTIVE='1'";
            }
            
            DataSet AssetDS = new DataSet();
            AssetDS = SqlHelper.ExecuteDataset(F.TPMDBConnection(), CommandType.Text, sql);
            
            Dictionary<string,string> row = new Dictionary<string,string>();
            if (AssetDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in AssetDS.Tables[0].Rows)
                {
                    
                    row.Add(dr[0].ToString(), dr[1].ToString());
                    
                   
                }
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(row);
            return s;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string generate(string deptid,string assetid, string reason)
        {
            SqlParameter mwoid = new SqlParameter()
            {  
                ParameterName ="@mwoid",
                DbType = System.Data.DbType.Int32,
                Direction = ParameterDirection.Output
                
            };
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@checklist_id", DBNull.Value));
            sqlparams.Add(new SqlParameter("@department_id", deptid));
            sqlparams.Add(new SqlParameter("@reason", reason));
            sqlparams.Add(new SqlParameter("@asset_id",assetid));
            sqlparams.Add(new SqlParameter("@done_by", new MySessions().EmployeeNo));
            sqlparams.Add(mwoid);
            int i = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(),CommandType.StoredProcedure,"usp_generateworkorder",sqlparams.ToArray());

            string s = mwoid.Value!=null ? mwoid.Value.ToString() : "";
           
            return s;
        }


    }
}
