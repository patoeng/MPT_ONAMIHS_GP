using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                if (session.IsAdministrator)
                {
                    prepareUserlist();
                }
                else
                {
                    Server.Transfer(session.redirection);
                }


            }
        }
        protected void prepareUserlist()
        {
            //get user info from DBVMSQA
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@userkey",DBNull.Value));
            string query = @"select A.EmpLoyeeNO,A.Username,A.UserEmail,A.UserPhone,B.DeptName from muser A
                            left join MDepartment B ON A.DeptKey = B.DeptKey                               
                          ";
            //string query = @"select A.EmpLoyeeNO,A.Username,A.UserEmail,A.UserPhone,'anoman' as DeptName from muser A
                                                        
            //                ";
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TDBVMSQAConnection(),CommandType.Text, query);
            DataTable dt = ds.Tables[0];

            TableRow tr = new TableRow();
            TableHeaderCell thc = new TableHeaderCell();
            TableCell tc = new TableCell();

            tr.TableSection = TableRowSection.TableHeader;
            List<string> tableheader = new List<string>();
            tableheader.Add("ERN");
            tableheader.Add("User Name");
            tableheader.Add("Email");
            tableheader.Add("Phone");
            tableheader.Add("Department");

            foreach (string ss in tableheader)
            {
                thc = new TableHeaderCell();
                thc.Text = ss;
                tr.Cells.Add(thc);
            }
            tblUserList.Rows.Add(tr);

            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                tableheader = new List<string>();
                tableheader.Add("employeeno");
                tableheader.Add("username");
                tableheader.Add("userEmail");
                tableheader.Add("userPhone");
                tableheader.Add("deptname");
                foreach (string ss in tableheader)
                {
                    tc = new TableCell();
                    tc.Text = dr[ss].ToString();
                    tr.Cells.Add(tc);
                }
                tblUserList.Rows.Add(tr);
            }
           
            sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@id", DBNull.Value));
            ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MRolesSelect", sqlparams.ToArray());
            dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                lbCat.Items.Add(new ListItem(dr["descriptions"].ToString() + " Role", "roles_" + dr["id"].ToString()));
                listAvailableRole.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString()));
            }
            lbCat.Items.Add(new ListItem("Email Receivers", "email_"));
            lbCat.Items.Add(new ListItem("SMS Receivers", "mobile_"));


            tr = new TableRow();
            thc = new TableHeaderCell();
            tc = new TableCell();

            tr.TableSection = TableRowSection.TableHeader;
            tableheader = new List<string>();
            tableheader.Add("Id");
            tableheader.Add("User Name");
            tableheader.Add("ERN");
            tableheader.Add("Category Info");


            foreach (string ss in tableheader)
            {
                thc = new TableHeaderCell();
                thc.Text = ss;
                tr.Cells.Add(thc);
            }
            tblUserByCat.Rows.Add(tr);
        }
    }
}