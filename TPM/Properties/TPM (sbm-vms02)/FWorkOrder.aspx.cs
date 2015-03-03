using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.HtmlControls;

namespace TPM
{
    public partial class FWorkOrder : System.Web.UI.Page
    {
        public TPMHelper Functions = new TPMHelper();
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                if (!session.IsPublic)
                {
                    prepareform();
                }
                else
                {
                    Server.Transfer(session.redirection);
                }


            }
        }
        protected void prepareform()
        {
            List<string> label = new List<string>();

            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            tc.Text = "Department";
            tr.Cells.Add(tc);

            DropDownList ddl = new DropDownList();
            ddl.ID = "ddlDepartment";
            ddl.ClientIDMode = ClientIDMode.Static;
            ddl.Items.Add(new ListItem("Please Select ...",""));
            ddl.CssClass = "required";
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "Asset Name";
            tr.Cells.Add(tc);

            ddl = new DropDownList();
            ddl.ID = "ddlAsset";
            ddl.ClientIDMode = ClientIDMode.Static;
            ddl.Items.Add(new ListItem("Please Select ...",""));
            ddl.CssClass = "required";
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "Reason";
            tr.Cells.Add(tc);

            TextBox txt = new TextBox();
            txt.ID = "txtReason";
            txt.CssClass = "required";
            txt.TextMode = TextBoxMode.MultiLine;
            txt.ClientIDMode = ClientIDMode.Static;
            txt.Rows = 10;
            txt.Width = 200;
            tc= new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "&nbsp;";
            tr.Cells.Add(tc);

            HtmlGenericControl htm = new HtmlGenericControl("h3");
            htm.Attributes.Add("id","submit");
            htm.Attributes.Add("class", "btn btn-primary");
            htm.InnerHtml = "SUBMIT";
            tc = new TableCell();
            tc.Controls.Add(htm);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

        }
    }
}