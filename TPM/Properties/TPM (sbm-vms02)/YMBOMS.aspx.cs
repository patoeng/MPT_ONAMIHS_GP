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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TPM
{
    public partial class YMBOMS : System.Web.UI.Page
    {
        TPMHelper Functions = new TPMHelper();
        public string asset_model_id = "";
        public string asset_model_name = "";
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                if (session.IsAdministrator)
                {
                    asset_model_id = Request.QueryString["a"] != null ? Request.QueryString["a"].ToString() : "";
                    prepareAll();
                    prepareForm();
                }
                else
                {
                    Server.Transfer(session.redirection);
                }
               
                
            }
        }
        protected void prepareAll()
        {
            List<SqlParameter> sql = new List<SqlParameter>();
            sql.Add(new SqlParameter("@id",DBNull.Value));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MAssetModelsSelect", sql.ToArray());
            DataTable dt = ds.Tables[0];

            foreach(DataRow dr in dt.Rows){
                lbAssetModels.Items.Add(new ListItem(dr["Descriptions"].ToString(),dr["Id"].ToString()));
            }
            if (asset_model_id != "")
            {
                lbAssetModels.ClearSelection(); lbAssetModels.Items.FindByValue(asset_model_id.ToString()).Selected = true;
                asset_model_name = lbAssetModels.Items.FindByValue(asset_model_id).Text;
            }
            List<string> stringA = new List<string>();
            stringA.Add("Product Code");
            stringA.Add("Descriptions");
            stringA.Add("QTY for PM");

            TableRow tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            TableCell tc = new TableCell();
            TableHeaderCell thc = new TableHeaderCell();
            foreach (string s in stringA) {      
                thc = new TableHeaderCell();
                thc.Text = s;
                tr.Cells.Add(thc);
            }
            tblBOM.Rows.Add(tr);


            sql.Clear();
            sql.Add(new SqlParameter("@Asset_Model_id", asset_model_id));
            ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MBomsSelect_byasmod", sql.ToArray());
            dt = (ds.Tables[0] != null) ? ds.Tables[0] : new DataTable();
            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    tc = new TableCell();
                    tc.Text = dr[i].ToString();
                    tr.Cells.Add(tc);
                }
                tblBOM.Rows.Add(tr);
            }
            stringA.Clear();
            stringA.Add("ID");
            stringA.Add("CODE");
            stringA.Add("NAME");
            stringA.Add("MIN QTY");
            stringA.Add("MAX QTY");

            tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            thc = new TableHeaderCell();
            foreach (string s in stringA)
            {
                thc = new TableHeaderCell();
                thc.Text = s;
                tr.Cells.Add(thc);
            }
            tblInventory.Rows.Add(tr);

           
            
            
        }
        protected void prepareForm()
        {
            TableRow tr = new TableRow();
            TableHeaderCell thc = new TableHeaderCell();
            TableCell tc = new TableCell();

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
            thc = new TableHeaderCell();
            thc.ColumnSpan = 2;
            thc.Controls.Add(img);
            thc.Controls.Add(head);
            tr.Cells.Add(thc);
            FormTbl.Rows.Add(tr);

            tr = new TableRow();
            tr.ClientIDMode = ClientIDMode.Static;
            tc = new TableCell();
            tc.Text = "Code";
            tr.Cells.Add(tc);
            tc = new TableCell();
            TextBox txt = new TextBox();
            txt.ID = "txtCode";
            txt.ClientIDMode = ClientIDMode.Static;
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            FormTbl.Rows.Add(tr);

            tr = new TableRow();
            tr.ClientIDMode = ClientIDMode.Static;
            tc = new TableCell();
            tc.Text = "Descriptions";
            tr.Cells.Add(tc);
            tc = new TableCell();
            txt = new TextBox();
            txt.ID = "txtDescriptions";
            txt.ClientIDMode = ClientIDMode.Static;
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            FormTbl.Rows.Add(tr);

            tr = new TableRow();
            tr.ClientIDMode = ClientIDMode.Static;
            tc = new TableCell();
            tc.Text = "Min Qty For PM";
            tr.Cells.Add(tc);
            tc = new TableCell();
            txt = new TextBox();
            txt.ID = "txtQty";
            txt.ClientIDMode = ClientIDMode.Static;
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            FormTbl.Rows.Add(tr);

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