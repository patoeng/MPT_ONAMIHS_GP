using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TPM
{
    public partial class YBOMStatus : System.Web.UI.Page
    {
        TPMHelper Functions = new TPMHelper();
        public string asset_model_id = "";
        public string asset_model_name = "";
        public MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                if ((session.IsEngineering)&&(session.IsManagement)){
                    asset_model_id = Request.QueryString["a"]!=null?  Request.QueryString["a"].ToString():"";
                    prepareAll();}
                else{
                   Server.Transfer(session.redirection);
                }
            }
        }
        protected void prepareAll()
        {
            List<SqlParameter> sql = new List<SqlParameter>();
            sql.Add(new SqlParameter("@id", DBNull.Value));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MAssetModelsSelect", sql.ToArray());
            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                lbAssetModels.Items.Add(new ListItem(dr["Descriptions"].ToString(), dr["Id"].ToString()));
            }
            if (asset_model_id != "") { 
                lbAssetModels.ClearSelection(); lbAssetModels.Items.FindByValue(asset_model_id.ToString()).Selected = true;
                asset_model_name = lbAssetModels.Items.FindByValue(asset_model_id).Text;
            }
            List<string> stringA = new List<string>();
            stringA.Add("Product Code");
            stringA.Add("Descriptions");
            stringA.Add("QTY for PM");
            stringA.Add("QTY in Stock");
            stringA.Add("Inventory Min QTY");

            TableRow tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            TableCell tc = new TableCell();
            TableHeaderCell thc = new TableHeaderCell();
            foreach (string s in stringA)
            {
                thc = new TableHeaderCell();
                thc.Text = s;
                tr.Cells.Add(thc);
            }
            tblBOM.Rows.Add(tr);
            sql.Clear();
            sql.Add(new SqlParameter("@Asset_Model_id", asset_model_id));
            ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MBomsSelect_byasmod", sql.ToArray());
            dt = (ds.Tables[0] != null) ? ds.Tables[0] : new DataTable();
            foreach (DataRow dr in dt.Rows) {
                tr = new TableRow();
                for (int i = 0; i < dt.Columns.Count; i++) {
                    tc = new TableCell();
                    tc.Text = dr[i].ToString();
                    tr.Cells.Add(tc);
                }
                tblBOM.Rows.Add(tr);
            }
        }
       

           
    }
}