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
    public partial class PMBoms : System.Web.UI.Page
    {
        public string pmScheduleId = "";
        public TPMHelper Functions = new TPMHelper();
        public string Connector = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                pmScheduleId = Request.QueryString["i"] == null ? "" : Request.QueryString["i"].ToString();

                prepareAll();
                prepareForm();
            }
        }
        protected void prepareAll()
        {
            var connector = new SqlParameter
            {
                ParameterName = "@connector",
                Direction = ParameterDirection.InputOutput,
                SqlDbType = SqlDbType.NVarChar,
                Size = 4
            };
            var sql = new List<SqlParameter>
                {
                    new SqlParameter("@PMid", pmScheduleId),
                    connector
                };
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(),CommandType.StoredProcedure,"usp_LPMBOMSSelect_BYPMID",sql.ToArray());
            DataTable dt = ds.Tables[0];
            var tr = new TableRow();
            var tc = new TableCell();

            var thead = new List<string> { 
                    "ID",
                    "Inventory Code",
                    "Inventory Name",
                    "Qty",
                    "Price Per Unit",
                    "Currency",
                    "Reason",
                    "Done By"
            };

            tr.TableSection = TableRowSection.TableHeader;
            foreach (string t in thead)
            {
                tc = new TableHeaderCell {Text = t};
                tr.Cells.Add(tc);
                
            }
            tblBOM.Rows.Add(tr);
            foreach (DataRow dr in dt.Rows){
                tr = new TableRow();
                for (int j = 0; j < thead.Count; j++) {
                    tc = new TableCell {Text = dr[j].ToString()};
                    tr.Cells.Add(tc);                
               }
               
                tblBOM.Rows.Add(tr);
            }
            Connector = connector.Value.ToString();
            thead = new List<string> { "ID", "CODE", "NAME", "MIN QTY", "MAX QTY", "IN-STOCK QTY", "PRICE PER UNIT", "CURRENCY" };
            tr = new TableRow {TableSection = TableRowSection.TableHeader};
            foreach (string t in thead)
            {
                tc = new TableHeaderCell {Text = t};
                tr.Cells.Add(tc);
            }
            tblInventory.Rows.Add(tr);
        }
        protected void prepareForm() {
            var dic = new Dictionary<string, string>
                {
                    {"txtCode", "Inventory Code"},
                    {"txtName", "Inventory Name"},
                    {"txtQty", "Quantity"},
                    {"txtPrice", "Price Per Unit"},
                    {"txtCurrency", "Currency"},
                    {"txtReason", "Reason of Replacement"}
                };
            TextBox txt;
            var tr= new TableRow();
            TableCell tc;
            tr.TableSection = TableRowSection.TableHeader;
            var img = new ImageButton
                {
                    ID = "BtnCloseForm",
                    ClientIDMode = ClientIDMode.Static,
                    ImageUrl = "./Images/delete.png",
                    OnClientClick = "return false",
                    Width = 15,
                    BorderWidth = 0,
                    ToolTip = "Close the Form"
                };


            var head = new Label {ID = "FormHeader", ClientIDMode = ClientIDMode.Static, Text = "Form Header"};
            tr = new TableRow {ID = "trhead", ClientIDMode = ClientIDMode.Static};
            tc = new TableHeaderCell {ColumnSpan = 2};
            tc.Controls.Add(img);
            tc.Controls.Add(head);
            tr.Cells.Add(tc);
            FormTbl.Rows.Add(tr);
            
             
            foreach(string s in dic.Keys){
                txt = new TextBox {ID = s, ClientIDMode = ClientIDMode.Static};
                tr = new TableRow();
                tc = new TableCell {Text = dic[s]};
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Controls.Add(txt);
                tr.Cells.Add(tc);
                FormTbl.Rows.Add(tr);
            }
            tr = new TableRow();
            tc = new TableCell {Text = ""};
            tr.Cells.Add(tc);
            var SubmitBtn = new Button
                {
                    ID = "BtnSubmit1",
                    ClientIDMode = ClientIDMode.Static,
                    Text = "Submit",
                    CssClass = "btn btn-primary tombol",
                    OnClientClick = "return false;"
                };
            tc = new TableCell();
            tc.Controls.Add(SubmitBtn);
            var lbl = new Label {Text = "&nbsp;&nbsp;"};
            tc.Controls.Add(lbl);
            SubmitBtn = new Button
                {
                    ID = "BtnSubmit2",
                    ClientIDMode = ClientIDMode.Static,
                    Text = "Submit",
                    CssClass = "btn btn-primary tombol",
                    OnClientClick = "return false;"
                };
            SubmitBtn.Style.Add("display", "none");
            tc.Controls.Add(SubmitBtn);
            tr.Cells.Add(tc);

            FormTbl.Rows.Add(tr);
        }
    }
}