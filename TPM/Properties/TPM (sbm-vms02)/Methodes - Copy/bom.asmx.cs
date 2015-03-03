using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;


namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for bom
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class bom : System.Web.Services.WebService
    {
        public TPMHelper Functions = new TPMHelper();
        [WebMethod]
        public string GetBOMInfobyasmod(string id)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@Asset_Model_id",id));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MBomsSelect_byasmod", sqlparams.ToArray());
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

        [WebMethod]
        public string GetInventory()
        {
            DataSet ds  =MySqlHelper.ExecuteDataset(Functions.TInventoryConnection(), "select id,code,name,qty_min,qty_max from product order by code");
            
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

        [WebMethod]
        public string GetBOMInfobyasmod2(string id)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@Asset_Model_id", id));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MBomsSelect_byasmod", sqlparams.ToArray());
            DataTable dt = (ds.Tables[0]!=null)? ds.Tables[0]: new DataTable();
            

            List<List<string>> data = new List<List<string>>();
            Dictionary<string, List<string>> dc = new Dictionary<string, List<string>>(); 
            List<string> dat = new List<string>();
            
            foreach (DataRow dr in dt.Rows)
            {
                dat = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dat.Add(dr[i].ToString());
                }
                dc.Add(dr[0].ToString(), dat);
                data.Add(dat);
            }
            string where = "";
            if (data.Count > 0) {
                
                where = " IN (";
                foreach (List<string> ls in data) {
                    if (ls[0] != "") 
                    { 
                        where +="'"+ls[0]+"',";
                    }
                }
                where = where.Remove(where.Length-1, 1);
                where += " )";
                System.Diagnostics.Debug.WriteLine("sss"+where);
                ds = MySqlHelper.ExecuteDataset(Functions.TInventoryConnection(), "select code,qty_min,(select qty_current+qty_adjustment from apq where product_code=code) as qty_stock from product where code " + where + "");
                
                DataTable dts = ds.Tables[0];
                foreach (DataRow dr in dts.Rows) { 
                    dc[dr["code"].ToString()].Add(dr["qty_stock"].ToString());
                    dc[dr["code"].ToString()].Add(dr["qty_min"].ToString());
                }
                data = new List<List<string>>();
                foreach (string se in dc.Keys)
                {
                    data.Add(dc[se]);
                }
            }
            
            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        public string action(string asmod, string act, string code, string desc, string qty)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@Asset_Model_id", asmod));
            sqlparams.Add(new SqlParameter("@Descriptions", desc));
            sqlparams.Add(new SqlParameter("@Minimum_QTY_For_PM", qty));
            sqlparams.Add(new SqlParameter("@inventory_code", code));
            
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MBoms"+act+"_ymboms", sqlparams.ToArray());
            List<string> data = new List<string>();
            if (act.ToUpper() != "DELETE")
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    data = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data.Add(dr[i].ToString());
                    }

                }
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        public string action2(string act, string code, string qty, string pmid,string reason,string name)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@pmid", pmid));
            sqlparams.Add(new SqlParameter("@qty", qty));
            sqlparams.Add(new SqlParameter("@inventory_code", code));
            sqlparams.Add(new SqlParameter("@reason", reason));
            sqlparams.Add(new SqlParameter("@done_by", new MySessions().EmployeeNo));
            sqlparams.Add(new SqlParameter("@inventory_name", name));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_LPMBoms" + act , sqlparams.ToArray());
            List<string> data = new List<string>();
            if (act.ToUpper() != "DELETE")
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    data = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data.Add(dr[i].ToString());
                    }

                }
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        public string action3(string act, string code, string qty, string woid, string reason, string name)
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@woid", woid));
            sqlparams.Add(new SqlParameter("@qty", qty));
            sqlparams.Add(new SqlParameter("@inventory_code", code));
            sqlparams.Add(new SqlParameter("@reason", reason));
            sqlparams.Add(new SqlParameter("@done_by", new MySessions().EmployeeNo));
            sqlparams.Add(new SqlParameter("@inventory_name", name));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_LWOBoms" + act, sqlparams.ToArray());
            List<string> data = new List<string>();
            if (act.ToUpper() != "DELETE")
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    data = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data.Add(dr[i].ToString());
                    }

                }
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
    }
}
