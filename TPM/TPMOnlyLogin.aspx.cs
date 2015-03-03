using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
namespace TPM
{
    public partial class TPMOnlyLogin : System.Web.UI.Page
    {
        private TPMHelper F;
        public bool loggedIn = false;
        public MySessions session;
        public string message="";
        protected void Page_Load(object sender, EventArgs e)
        {
            bool v = Request.QueryString["v"] != null ? (Request.QueryString["v"]=="out"? true:false) : false;
            message = Request.QueryString["m"] != null ? Request.QueryString["m"].ToString().Replace('_',' ') : "";
            if (v) {
                System.Web.HttpContext.Current.Session.Remove("BadgeNo");
                info.Text = message;
            }
            F = new TPMHelper();
            session = new MySessions();
            
            if (session.EmployeeNo != "") { loggedIn = true; };
            if (!IsPostBack) {
               
            }
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            
            List<SqlParameter> sql = new List<SqlParameter>();
            sql.Add(new SqlParameter("@employeeno",EmployeeNo.Text));
            sql.Add(new SqlParameter("@UserPwd",password.Text));
            DataSet ds = SqlHelper.ExecuteDataset(F.TPMDBConnection(), CommandType.StoredProcedure, "usp_MTPMLocalUserCheckPassword", sql.ToArray());
            DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            foreach (DataRow dr in dt.Rows){
              System.Web.HttpContext.Current.Session.Add("BadgeNo",dr["employeeno"].ToString());
              loggedIn = true;
            }
            
        }
    }
}