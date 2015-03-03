using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for Emailing
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Emailing : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Update(string id, string origValue, string value)
        {
            var result = new SqlParameter
                {
                    ParameterName = "@result",
                    Direction = ParameterDirection.Output,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 200
                };
             var param = new List<SqlParameter>
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@value",value),
                    result
                };
            var i = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                   "usp_UpdateEmailRecipient", param.ToArray());
            return result.Value.ToString();
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Remove(string id)
        {
            var param = new List<SqlParameter>
                {
                    new SqlParameter("@id", id),
                   
                };
            var i = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                   "usp_DeleteEmailRecipient", param.ToArray());
            return "OK";
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Insert(string id)
        {
            var result = new SqlParameter
            {
                ParameterName = "@result",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.NVarChar,
                Size = 200
            };
            var param = new List<SqlParameter>
                {
                    new SqlParameter("@id", id),
                    result
                };
            var i = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                   "usp_InsertEmailRecipient", param.ToArray());
            return result.Value.ToString();
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string UpdateFrequency(string check, string id, string value)
        {
            var param = new List<SqlParameter>
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@check", check),
                    new SqlParameter("@value", value),
                };
            var i = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                   "usp_UpdateFrequencyMEmailNew", param.ToArray());
            return "OK";
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string UpdateEmail(string EmailId, string id,string origValue, string value)
        {
            var param = new List<SqlParameter>
                {
                    new SqlParameter("@column", id),
                    new SqlParameter("@Id", EmailId),
                    new SqlParameter("@value", value),
                };
            var i = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                   "usp_UpdateTxtMEmailNew", param.ToArray());
            return id=="lblDescription"?value : value=="1"? "Yes":"No" ;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string UpdateEmail2(string EmailId, string id, string origValue, string value)
        {
            var param = new List<SqlParameter>
                {
                    new SqlParameter("@column", id),
                    new SqlParameter("@Id", EmailId),
                    new SqlParameter("@value", value),
                };
            var i = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                   "usp_UpdateTxtMEmailNew", param.ToArray());
            return value == "1" ? "Event Triggered" : "Regular";
        }
    }
}
