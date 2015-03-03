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
    public partial class TPMOnlyChangePwd : System.Web.UI.Page
    {
        private TPMHelper F;
        public bool loggedIn = false;
        public MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            F = new TPMHelper();
            session = new MySessions();
            loggedIn = session.EmployeeNo != "" ? true : false;
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            
            List<SqlParameter> sql = new List<SqlParameter>();
            sql.Add(new SqlParameter("@employeeno", session.EmployeeNo));
            sql.Add(new SqlParameter("@UserPwd", oldpassword.Text));
            sql.Add(new SqlParameter("@UserPwdNew", newpassword.Text));
     
            SqlParameter result = new SqlParameter()
            {
                Direction = ParameterDirection.Output,
                ParameterName = "@result",
                DbType = DbType.Int32
            };
            sql.Add(result);
            int ds = SqlHelper.ExecuteNonQuery(F.TPMDBConnection(), CommandType.StoredProcedure, "usp_MTPMLocalUserChangePassword", sql.ToArray());
            if ((int)result.Value == 1) { Response.Redirect("~/TPMOnlyLogin.aspx?v=out&m=Please_login_with_your_new_password!"); }
            else
            {
              info.Text ="Failed To Change Password";
           };
        }
    }
}