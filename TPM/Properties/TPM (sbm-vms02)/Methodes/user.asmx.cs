using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;
using System.Diagnostics;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for user
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class user : System.Web.Services.WebService
    {
        public TPMHelper Functions = new TPMHelper();
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string HelloWorld(string id)
        {
            return "Hello World "+id;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GetUserInfo(string id)
        {
            
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@id", id));
            Dictionary<string, string> ss = new Dictionary<string, string>();
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_getuserinfo", sqlparams.ToArray());
            DataTable dtRole = ds.Tables[0];
            DataTable dtEmail = ds.Tables[1];
            DataTable dtSms = ds.Tables[2];
            foreach (DataRow dr in dtRole.Rows)
            {
                ss.Add("role_" +  dr["role_id"].ToString(), dr["RoleDes"].ToString());
            }
            bool sms = false;
            foreach (DataRow dr in dtSms.Rows)
            {
                sms = true;
            }
            ss.Add("sms_" + sms.ToString(), sms.ToString());
            
            sms = false;
            foreach (DataRow dr in dtEmail.Rows)
            {
                sms = true;
            }
            ss.Add("email_" + sms.ToString(), sms.ToString());

            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(ss);
            return s ;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string UpdateRoleInfo(string val, string action)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            string[] values = val.Split('$');
            string id = (values[0]);
            int role_id = Convert.ToInt32(values[1]);
            sqlparams.Add(new SqlParameter("@user_id", id));
            sqlparams.Add(new SqlParameter("@role_id", role_id));
            sqlparams.Add(new SqlParameter("@name", values[2]));
            int y = SqlHelper.ExecuteNonQuery(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MUserRoles"+ action, sqlparams.ToArray());


            Dictionary<string, string> ss = new Dictionary<string, string>();
            ss.Add("role_" +values[0]+ "$" + values[1], values[3]);

            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(ss);
            return s;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string UpdateEmailInfo(string val, string action)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            string[] values = val.Split('$');
            string id = (values[0]);
            sqlparams.Add(new SqlParameter("@user_id", id));
            sqlparams.Add(new SqlParameter("@email", values[1]));
            sqlparams.Add(new SqlParameter("@name", values[2]));
            int y = SqlHelper.ExecuteNonQuery(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MEmailReceivers" + action, sqlparams.ToArray());

            bool checkeds =false;
            if (action.ToUpper() == "INSERT") {
                checkeds = true;               
            }

            Dictionary<string, string> ss = new Dictionary<string, string>();
            ss.Add("email_" + values[0] + "$" + checkeds.ToString(), checkeds.ToString());

            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(ss);
            return s;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string UpdateMobileInfo(string val, string action)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            string[] values = val.Split('$');
            string id = (values[0]);
            sqlparams.Add(new SqlParameter("@user_id", id));
            sqlparams.Add(new SqlParameter("@mobile", values[1]));
            sqlparams.Add(new SqlParameter("@name", values[2]));
            int y = SqlHelper.ExecuteNonQuery(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MSMSReceivers" + action, sqlparams.ToArray());

            bool checkeds = false;
            if (action.ToUpper() == "INSERT")
            {
                checkeds = true;
            }

            Dictionary<string, string> ss = new Dictionary<string, string>();
            ss.Add("sms_" + values[0] + "$" + checkeds.ToString(), checkeds.ToString());

            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(ss);
            return s;
        }
       
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string selectbyCAT(string param)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            string[] values = param.Split('_');
            string table = (values[0]);
            
            string usp="";
            switch (table) {
                case "roles": usp = "usp_MUserRolesSelect_byRole";
                    int roleid = Convert.ToInt32(values[1]);
                             sqlparams.Add(new SqlParameter("@role_id", roleid));
                    break;
                case "email": usp = "usp_MEmailReceiversSelect";
                            sqlparams.Add(new SqlParameter("@user_id", DBNull.Value));   
                    break;
                case "mobile": usp = "usp_MSMSReceiversSelect";
                            sqlparams.Add(new SqlParameter("@user_id", DBNull.Value));
                    break;
            }
            
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, usp, sqlparams.ToArray());
            DataTable dt = ds.Tables[0];
            List<List<string>> data = new List<List<string>>();
            List<string> ss = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                ss = new List<string>();
                for (int i = 0; i < dt.Columns.Count;i++ )
                {
                    ss.Add(dr[i].ToString());
                }
                data.Add(ss);
            }

         

            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string getmasteruser()
        {
           
           string query = @"select A.EmpLoyeeNO,A.Username,A.UserEmail,A.UserPhone,B.DeptName from muser A
                             left join MDepartment B ON A.DeptKey = B.DeptKey                               
                            ";
           // string query = @"select A.EmpLoyeeNO,A.Username,A.UserEmail,A.UserPhone,'anoman' as DeptName from muser A
                                                        
            //                ";
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TDBVMSQAConnection(), CommandType.Text, query);
            DataTable dt = ds.Tables[0];
            List<List<string>> data = new List<List<string>>();
            List<string> dat = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                dat = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dat.Add(dr[i].ToString());
                }
                data.Add(dat);
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
    }
}
