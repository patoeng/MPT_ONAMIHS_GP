using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace TPM
{
    public partial class YUser : System.Web.UI.Page
    {
        public TPMHelper Functions = new TPMHelper();
        MySessions _session;
        protected void Page_Load(object sender, EventArgs e)
        {
            _session = new MySessions();
            if (!IsPostBack)
            {
                if (_session.IsAdministrator)
                {
                    PrepareUserlist();
                }
                else
                {
                    Server.Transfer(_session.Redirection);
                }


            }
        }
        protected void PrepareUserlist()
        {
            //get user info from DBVMSQA

            //const string query = @"select A.EmpLoyeeNO,A.Username,A.UserEmail,A.UserPhone,B.DeptName from muser A        left join MDepartment B ON A.DeptKey = B.DeptKey         ";
            const string query = @"select A.EmpLoyeeNO,A.Username,A.UserEmail,A.UserPhone,'anoman' as DeptName from muser A  ";                                          
                         
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TDBVMSQAConnection(),CommandType.Text, query);
            DataTable dt = ds.Tables[0];

            var tr = new TableRow();
            TableHeaderCell thc;

            tr.TableSection = TableRowSection.TableHeader;
            var tableheader = new List<string> {"ERN", "User Name", "Email", "Phone", "Department"};

            foreach (string ss in tableheader)
            {
                thc = new TableHeaderCell {Text = ss};
                tr.Cells.Add(thc);
            }
            tblUserList.Rows.Add(tr);

            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                tableheader = new List<string> {"employeeno", "username", "userEmail", "userPhone", "deptname"};
                foreach (var tc in tableheader.Select(ss => new TableCell {Text = dr[ss].ToString()}))
                {
                    tr.Cells.Add(tc);
                }
                tblUserList.Rows.Add(tr);
            }
           
            var sqlparams = new List<SqlParameter> {new SqlParameter("@id", DBNull.Value)};
            ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MRolesSelect", sqlparams.ToArray());
             dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                lbCat.Items.Add(new ListItem(dr["descriptions"].ToString() + " Role", "roles_" + dr["id"].ToString()));
                listAvailableRole.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString()));
            }
            lbCat.Items.Add(new ListItem("Email Receivers", "email_"));
            lbCat.Items.Add(new ListItem("SMS Receivers", "mobile_"));


             tr = new TableRow {TableSection = TableRowSection.TableHeader};

            tableheader = new List<string> {"Id", "User Name", "ERN", "Category Info"};


            foreach (string ss in tableheader)
            {
                thc = new TableHeaderCell {Text = ss};
                tr.Cells.Add(thc);
            }
            tblUserByCat.Rows.Add(tr);
        }
    }
}