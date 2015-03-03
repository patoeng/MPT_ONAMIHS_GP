using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM.Methodes
{
    /// <summary>
    ///     Summary description for workorder
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class workorder : WebService
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
                    usp = "usp_updateWorkOrder";
                    sql.Add(new SqlParameter("@mwoid", id));
                    sql.Add(new SqlParameter("@done_by", new MySessions().EmployeeName));
                    sql.Add(new SqlParameter("@status_id", DBNull.Value));
                    break;
            }
            var i = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(), CommandType.StoredProcedure, usp, sql.ToArray());
            return i.ToString(CultureInfo.InvariantCulture);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string getDDL(string id)
        {
            string sql;
            if (id == "")
            {
                sql = "SELECT ID,DESCRIPTIONS FROM MDEPARTMENTS WHERE ACTIVE='1'";
            }
            else
            {
                sql = "SELECT ID,DESCRIPTIONS FROM MASSETS WHERE DEPARTMENT_ID='" + id + "' AND ACTIVE='1'";
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
        public string generate(string deptid, string assetid, string reason, string nc)
        {
            var mwoid = new SqlParameter
                {
                    ParameterName = "@mwoidkey",
                    DbType = DbType.String,
                    Size = 100,
                    Direction = ParameterDirection.Output
                };
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@checklist_id", DBNull.Value),
                    new SqlParameter("@department_id", deptid),
                    new SqlParameter("@reason", reason),
                    new SqlParameter("@asset_id", assetid),
                    new SqlParameter("@nc", nc),
                    new SqlParameter("@done_by", new MySessions().EmployeeName),
                    mwoid
                };
            int i = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(), CommandType.StoredProcedure, "usp_generateworkorder",
                                              sqlparams.ToArray());

            string s = mwoid.Value != null ? mwoid.Value.ToString() : "";

            return s;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string jeditable(string id, string value)
        {
            string usp = "usp_MworkordersActionUpdate";
            id = id.ToLower();
            string[] param = id.Split('-');
            var sqlparams = new List<SqlParameter>();
            switch (param[1])
            {
                case "remarks":
                    usp = "usp_LWorkOrderRemarksUpdate";
                    sqlparams.Add(new SqlParameter("@LWorkOrder_id", param[0]));
                    sqlparams.Add(new SqlParameter("@descriptions", value));
                    break;
                case "remarks2":
                    usp = "usp_MWorkOrderRemarksUpdate";
                    sqlparams.Add(new SqlParameter("@MWorkOrder_id", param[0]));
                    sqlparams.Add(new SqlParameter("@descriptions", value));
                    break;
                case "action":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@action", value));
                    break;
                case "action2":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@action2", value));
                    break;
                case "result":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@result", value));
                    break;
                case "request_type":
                    sqlparams.Add(new SqlParameter("@mwoid", param[0]));
                    sqlparams.Add(new SqlParameter("@request_type", value));
                    break;
            }


            SqlHelper.ExecuteNonQuery(F.TPMDBConnection(), CommandType.StoredProcedure, usp, sqlparams.ToArray());
            return value;
        }
    }
}