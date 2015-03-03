using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class FIWOrkOrder : System.Web.UI.Page
    {
        public TPMHelper Functions = new TPMHelper();
        MySessions _session;
        protected void Page_Load(object sender, EventArgs e)
        {
            _session = new MySessions();
            if (IsPostBack) return;
            if (!_session.IsPublic)
            {
                Prepareform();
            }
            else
            {
                Server.Transfer(_session.Redirection);
            }
        }
        protected void Prepareform()
        {
            var label = new List<string>();

            var tr = new TableRow();
            var tc = new TableCell {Text = "Department"};

            tr.Cells.Add(tc);

            var ddl = new DropDownList { ID = "ddlDepartment", ClientIDMode = ClientIDMode.Static };
            ddl.Items.Add(new ListItem("Please Select ...", ""));
            ddl.CssClass = "required";
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell {Text = "Request Type"};
            tr.Cells.Add(tc);

            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, "select * from iwotype");
            var dt =ds.Tables.Count>0? ds.Tables[0]: new DataTable();

            ddl = new DropDownList {ID = "ddlRequestType", ClientIDMode = ClientIDMode.Static, CssClass = "required"};
            ddl.Items.Add(new ListItem("Please Select ...", ""));
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new ListItem(dr["Descriptions"].ToString(), dr["id"].ToString()));
            }

           
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            

            tr = new TableRow();
            tc = new TableCell { Text = "Request" };
            tr.Cells.Add(tc);


            var txt = new TextBox
            {
                ID = "txtRequest",
                CssClass = "required",
                TextMode = TextBoxMode.MultiLine,
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            tr = new TableRow();
            tc = new TableCell { Text = "Causes" };
            tr.Cells.Add(tc);


            txt = new TextBox
                {
                    ID = "txtCause",
                    CssClass = "required",
                    TextMode = TextBoxMode.MultiLine,
                    ClientIDMode = ClientIDMode.Static,
                    Rows = 10,
                    Width = 200
                };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell {Text = "&nbsp;"};
            tr.Cells.Add(tc);

            var htm = new HtmlGenericControl("h3");
            htm.Attributes.Add("id", "submit");
            htm.Attributes.Add("class", "btn btn-primary");
            htm.InnerHtml = "SUBMIT";
            tc = new TableCell();
            tc.Controls.Add(htm);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

        }
    }
}