using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TPM.Classes;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;


namespace TPM
{
    public partial class YDownTime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack){prepare();}
        }
        protected void prepare() {
            DataSet _list = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MAssetsSelect", new SqlParameter("@id", DBNull.Value));
            DataTable dt = _list.Tables.Count>0 ? _list.Tables[0]: new DataTable();
            ddlAsset.Items.Add(new ListItem("Please Select ...", ""));
            ddlAsset.Items.Add(new ListItem("All Asset", "0"));   
            foreach(DataRow dr in dt.Rows){
                ddlAsset.Items.Add(new ListItem(dr["descriptions"].ToString(),dr["id"].ToString()));
            }
        }
    }
}