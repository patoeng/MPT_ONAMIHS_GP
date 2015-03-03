using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using TPM.Classes;


namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for crud
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class crud : System.Web.Services.WebService
    {
        TPMHelper Helper= new TPMHelper();

        [WebMethod]
        public string GetOneRow(int id, string tablename)
        {
            string s = "";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@id",id));
            sqlparams.Add(new SqlParameter("@TableName",tablename));
            sqlparams.Add(new SqlParameter("@Active",DBNull.Value));
            sqlparams.Add( new SqlParameter("@ForeignColNum",id));

            DataSet AssetDS = new DataSet();
            AssetDS = SqlHelper.ExecuteDataset(Helper.TPMDBConnection(), CommandType.StoredProcedure, "usp_SelectForList", sqlparams.ToArray());
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
            s = json.Serialize(data);
            return s;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string InsertUpdate(List<Form> anoman)
        {
            string s = "";
            Dictionary<string, string> FormFields = new Dictionary<string, string>();
            foreach (Form a in anoman)
            {
                FormFields[a.name] = a.value.Trim();
            }
            
            string TableName = FormFields["TableName"];
            FormFields.Remove("TableName");
            string dbaction = FormFields["BtnSubmit"];
            FormFields.Remove("BtnSubmit");
            FormFields.Remove("__VIEWSTATE");
            FormFields.Remove("__EVENTVALIDATION"); 

            
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            foreach (string ss in FormFields.Keys){
                sqlparams.Add(new SqlParameter("@"+ss,FormFields[ss]));
            }

            DataSet AssetDS = new DataSet();
            AssetDS = SqlHelper.ExecuteDataset(Helper.TPMDBConnection(), CommandType.StoredProcedure, "usp_"+TableName+dbaction, sqlparams.ToArray());
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
            s = json.Serialize(data);
            return s;

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Jeditable(string id, string value)
        {
            string[] idd = id.Split('_');
            id = idd[1];

            int i = SqlHelper.ExecuteNonQuery(Helper.TPMDBConnection(), CommandType.Text, "Update LListItems SET result='"+value+"' where id='"+id+"'");
            
            return i!=0? (value=="1" ?"OK":"NC") :"";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Confirm(List<Form> anoman)
        {
            Dictionary<string, string> field = new Dictionary<string, string>();
            foreach (Form ss in anoman) {
                field.Add(ss.name, ss.value);
            }
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@ID", Convert.ToDecimal(field["checklistid"].ToString())));
            sqlparams.Add(new SqlParameter("@userid", new MySessions().EmployeeNo));
          
            SqlParameter result = new SqlParameter()
            {
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Bit,
                ParameterName = "@result",
                DbType = DbType.Int32
            };
            sqlparams.Add(result);
            SqlParameter message = new SqlParameter()
            {
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.VarChar,
                ParameterName = "@message",
                Size = 1000
             
            };
            sqlparams.Add(message);
            int exec = SqlHelper.ExecuteNonQuery(Helper.TPMDBConnection(), CommandType.StoredProcedure, "usp_LCheckListsUpdate"+field["mode"], sqlparams.ToArray());
            if (exec != 0)
            {
                field.Clear();
                field.Add("result", result.Value.ToString());
                field.Add("message", message.Value.ToString());
            }
            
            JavaScriptSerializer js = new JavaScriptSerializer();
            string ret =js.Serialize(field);
            return ret;
        }
    }
}
