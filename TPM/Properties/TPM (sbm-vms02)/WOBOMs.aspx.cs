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

namespace TPM
{
    public partial class WOBOMs : System.Web.UI.Page
    {
        public string WOId = "";
        public TPMHelper Functions = new TPMHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               WOId = Request.QueryString["i"] == null ? "" : Request.QueryString["i"].ToString();

                prepareAll();
                prepareForm();
            }
        }
        protected void prepareAll()
        {
            List<SqlParameter> sql = new List<SqlParameter>();
            sql.Add(new SqlParameter("@woid",WOId));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_LPMBOMSSelect_BYWOID", sql.ToArray());
            DataTable dt = ds.Tables[0];
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            List<string> thead = new List<string>();
            thead.Add("ID");
            thead.Add("Inventory Code");
            thead.Add("Inventory Name");
            thead.Add("Qty");
            thead.Add("Reason");
            thead.Add("Done By");

            tr.TableSection = TableRowSection.TableHeader;
            for (int i = 0; i < thead.Count; i++)
            {
                tc = new TableHeaderCell();
                tc.Text = thead[i];
                tr.Cells.Add(tc);
            }
            tblBOM.Rows.Add(tr);
            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                for (int j = 0; j < thead.Count; j++)
                {
                    tc = new TableCell();
                    tc.Text = dr[j].ToString();
                    tr.Cells.Add(tc);
                }
                tblBOM.Rows.Add(tr);
            }

            thead.Clear();
            thead.Add("ID");
            thead.Add("CODE");
            thead.Add("NAME");
            thead.Add("MIN QTY");
            thead.Add("MAX QTY");
            tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            for (int i = 0; i < thead.Count; i++)
            {
                tc = new TableHeaderCell();
                tc.Text = thead[i];
                tr.Cells.Add(tc);
            }
            tblInventory.Rows.Add(tr);
        }
        protected void prepareForm()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("txtCode", "Inventory Code");
            dic.Add("txtName", "Inventory Name");
            dic.Add("txtQty", "Quantity");
            dic.Add("txtReason", "Reason of Replacement");
            TextBox txt;
            TableRow tr = new TableRow();
            TableCell tc;
            tr.TableSection = TableRowSection.TableHeader;
            ImageButton img = new ImageButton();
            img.ID = "BtnCloseForm";
            img.ClientIDMode = ClientIDMode.Static;
            img.ImageUrl = "./Images/delete.png";
            img.OnClientClick = "return false";
            img.Width = 15;
            img.BorderWidth = 0;
            img.ToolTip = "Close the Form";


            Label head = new Label();
            head.ID = "FormHeader";
            head.ClientIDMode = ClientIDMode.Static;
            head.Text = "Form Header";
            tr = new TableRow();
            tr.ID = "trhead";
            tr.ClientIDMode = ClientIDMode.Static;
            tc = new TableHeaderCell();
            tc.ColumnSpan = 2;
            tc.Controls.Add(img);
            tc.Controls.Add(head);
            tr.Cells.Add(tc);
            FormTbl.Rows.Add(tr);


            foreach (string s in dic.Keys)
            {
                txt = new TextBox();
                txt.ID = s;
                txt.ClientIDMode = ClientIDMode.Static;
                tr = new TableRow();
                tc = new TableCell();
                tc.Text = dic[s];
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Controls.Add(txt);
                tr.Cells.Add(tc);
                FormTbl.Rows.Add(tr);
            }
            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);
            Button SubmitBtn = new Button();
            SubmitBtn.ID = "BtnSubmit1";
            SubmitBtn.ClientIDMode = ClientIDMode.Static;
            SubmitBtn.Text = "Submit";
            SubmitBtn.CssClass = "btn btn-primary tombol";
            SubmitBtn.OnClientClick = "return false;";
            tc = new TableCell();
            tc.Controls.Add(SubmitBtn);
            Label lbl = new Label();
            lbl.Text = "&nbsp;&nbsp;";
            tc.Controls.Add(lbl);
            SubmitBtn = new Button();
            SubmitBtn.ID = "BtnSubmit2";
            SubmitBtn.ClientIDMode = ClientIDMode.Static;
            SubmitBtn.Text = "Submit";
            SubmitBtn.CssClass = "btn btn-primary tombol";
            SubmitBtn.OnClientClick = "return false;";
            SubmitBtn.Style.Add("display", "none");
            tc.Controls.Add(SubmitBtn);
            tr.Cells.Add(tc);

            FormTbl.Rows.Add(tr);
        }
    }
}