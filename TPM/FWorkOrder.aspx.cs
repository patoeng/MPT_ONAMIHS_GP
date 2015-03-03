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
                    Server.Transfer(session.Redirection);
                }


            }
        }
        protected void prepareform()
        {
            

            TableRow tr = new TableRow();
            TableCell tc = new TableCell {Text = "Department"};

            tr.Cells.Add(tc);

            DropDownList ddl = new DropDownList {ID = "ddlDepartment", ClientIDMode = ClientIDMode.Static};
            ddl.Items.Add(new ListItem("Please Select ...",""));
            ddl.CssClass = "required";
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell {Text = "Machine Name"};
            tr.Cells.Add(tc);

            ddl = new DropDownList {ID = "ddlAsset", ClientIDMode = ClientIDMode.Static};
            ddl.Items.Add(new ListItem("Please Select ...",""));
            ddl.CssClass = "required";
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell {Text = "Machine Condition"};
            tr.Cells.Add(tc);

            ddl = new DropDownList { ID = "ddlNC", ClientIDMode = ClientIDMode.Static };
            ddl.Items.Add(new ListItem("Please Select ...", ""));
            ddl.Items.Add(new ListItem("ABNORMAL", "ABNORMAL"));
            ddl.Items.Add(new ListItem("NC", "NC"));
            ddl.CssClass = "required";
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell { Text = "Safety Category" };
            tr.Cells.Add(tc);

            ddl = new DropDownList { ID = "ddlSC", ClientIDMode = ClientIDMode.Static };
        
            ddl.Items.Add(new ListItem("NO", "0"));
            ddl.Items.Add(new ListItem("YES", "1"));
            ddl.SelectedIndex = 0;
            ddl.CssClass = "required";
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell {Text = "Causes"};
            tr.Cells.Add(tc);


            var txt = new TextBox
                {
                    ID = "txtReason",
                    CssClass = "required",
                    TextMode = TextBoxMode.MultiLine,
                    ClientIDMode = ClientIDMode.Static,
                    Rows = 10,
                    Width = 200
                };
            tc= new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell {Text = "&nbsp;"};
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