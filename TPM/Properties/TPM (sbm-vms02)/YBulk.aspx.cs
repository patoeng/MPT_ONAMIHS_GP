using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
namespace TPM
{
    public partial class YBulk : System.Web.UI.Page
    {
        public string TableName;
        public bool isBOMtable = false;
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                TableName = Request.QueryString["table"];
                if (session.IsAdministrator)
                {
                    CreateForm();
                }
                else { 
                    Server.Transfer(session.redirection);
                }
               
            }
           
        }
        
        protected void CreateForm()
        {
            
            HiddenField hd = new HiddenField();
            hd.ID = "TableName";
            hd.ClientIDMode = ClientIDMode.Static;
            hd.Value = TableName;
            form1.Controls.Add(hd);
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            tc.Text = "Please Select a CSV file";
            tr.Cells.Add(tc);

            tc = new TableCell();
            FileUpload file = new FileUpload();
            file.ID = "filename";
            file.ClientIDMode = ClientIDMode.Static;
            tc.Controls.Add(file);
            tr.Cells.Add(tc);
            // FormTable.Rows.Add(tr);

            Button BLB = new Button();
            BLB.ID = "BtnUploadExecute";
            BLB.ClientIDMode = ClientIDMode.Static;
            BLB.Text = "Upload File And Execute";
            BLB.Attributes.Add("onClick", "return false;");
            BLB.CssClass = "btn btn-primary";
            BLB.UseSubmitBehavior = false;
            // tr = new TableRow();
            tc = new TableCell();
            tc.Controls.Add(BLB);
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "&nbsp;";
            tr.Cells.Add(tc);
            FormTable.Rows.Add(tr);



            tr = new TableRow();

            tc = new TableCell();
            tc.Text = "Number of new Row";
            tr.Cells.Add(tc);

            tc = new TableCell();
            DropDownList ddl = new DropDownList();

            ddl.ID = "DdlPrepareRowInsert";
            ddl.ClientIDMode = ClientIDMode.Static;
            ListItem itm = null;
            for (int z = 1; z < 201; z++)
            {
                itm = new ListItem();
                itm.Value = (z * 50).ToString();
                itm.Text = (z * 50).ToString();
                ddl.Items.Add(itm);
            }

            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            // FormTable.Rows.Add(tr);

            //  tr = new TableRow();
            tc = new TableCell();
            BLB = new Button();
            BLB.ID = "BtnDownloadTemplate";
            BLB.ClientIDMode = ClientIDMode.Static;
            BLB.Text = "Download CSV Template";
            BLB.Attributes.Add("onClick", "return false;");
            BLB.CssClass = "btn btn-primary";
            BLB.UseSubmitBehavior = false;

            tc.Controls.Add(BLB);
            tr.Cells.Add(tc);
            //FormTable.Rows.Add(tr);

            //tr = new TableRow();
            tc = new TableCell();
            BLB = new Button();
            BLB.ID = "BtnDownloadData";
            BLB.ClientIDMode = ClientIDMode.Static;
            BLB.Text = "Export Existing Data";
            BLB.Attributes.Add("onClick", "return false;");
            BLB.CssClass = "btn btn-primary";
            BLB.UseSubmitBehavior = false;

            //BLB.NavigateUrl = "#";

            tc.Controls.Add(BLB);
            tr.Cells.Add(tc);
            FormTable.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "Row Executed : ";
            tr.Cells.Add(tc);

            Label lbl;
            lbl = new Label();
            lbl.ID = "RowExecuted";
            lbl.ClientIDMode = ClientIDMode.Static;
            lbl.Text = "";
            tc = new TableCell();
            tc.Controls.Add(lbl);
            tr.Cells.Add(tc);

            LogTbl.Rows.Add(tr);
            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "Row Succeeded : ";
            tr.Cells.Add(tc);

            lbl = new Label();
            lbl.ID = "RowSucceeded";
            lbl.ClientIDMode = ClientIDMode.Static;
            lbl.Text = "";
            tc = new TableCell();
            tc.Controls.Add(lbl);
            tr.Cells.Add(tc);

            LogTbl.Rows.Add(tr);
            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "Row Failed : ";
            tr.Cells.Add(tc);

            lbl = new Label();
            lbl.ID = "RowFailed";
            lbl.ClientIDMode = ClientIDMode.Static;
            lbl.Text = "";
            tc = new TableCell();
            tc.Controls.Add(lbl);
            tr.Cells.Add(tc);

            LogTbl.Rows.Add(tr);
            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "Row Ignored : ";
            tr.Cells.Add(tc);

            lbl = new Label();
            lbl.ID = "RowIgnored";
            lbl.ClientIDMode = ClientIDMode.Static;
            lbl.Text = "";
            tc = new TableCell();
            tc.Controls.Add(lbl);
            tr.Cells.Add(tc);

            LogTbl.Rows.Add(tr);
        }
    }
}