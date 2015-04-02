using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for tool
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class tool : System.Web.Services.WebService
    {

        [WebMethod]
        public string EditableMachine(string id,string value)
        {
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MAssetsSelect_bySerialNumber",new SqlParameter("@serialnumber",value));
            var data = new string[2];
            if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count>0))
            {
                data[0] = ds.Tables[0].Rows[0][0].ToString();
                data[1] = ds.Tables[0].Rows[0][1].ToString();
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        public string EditableToolLifeSpec(string id, string value)
        {
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MAssetsSelect_bySerialNumber", new SqlParameter("@serialnumber", value));
            var data = new string[2];
            if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                data[0] = ds.Tables[0].Rows[0][0].ToString();
                data[1] = ds.Tables[0].Rows[0][1].ToString();
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        public string EditablePosition(string id, string value)
        {
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MAssetsSelect_bySerialNumber", new SqlParameter("@serialnumber", value));
            var data = new string[2];
            if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                data[0] = ds.Tables[0].Rows[0][0].ToString();
                data[1] = ds.Tables[0].Rows[0][1].ToString();
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        public string EditableDescription(string id, string value)
        {
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MAssetsSelect_bySerialNumber", new SqlParameter("@serialnumber", value));
            var data = new string[2];
            if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                data[0] = ds.Tables[0].Rows[0][0].ToString();
                data[1] = ds.Tables[0].Rows[0][1].ToString();
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
    }
}
