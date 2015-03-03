using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace TPM
{
    public partial class MasterChecklist : System.Web.UI.Page
    {
        public int ChecklistTypeid = 0;
        public string SerialNumber;
        protected void Page_Load(object sender, EventArgs e)
        {
            ChecklistTypeid = Request.QueryString["v"] != null ? Convert.ToInt32(Request.QueryString["v"]) : 1;
            if (!IsPostBack)
            {
                Prepare();
            }
        }

        protected void Prepare()
        {
           
            LblForm.ClientIDMode = ClientIDMode.Static;
            var thead = new List<string>
                {
                    "Department",
                    "Machine",
                    "Serial Number",
                    ChecklistTypeid == 1 ? "Date" : "Month"
                };

            foreach (string t in thead)
            {
                var tc = new TableCell {Text = t};
                var tr = new TableRow();
                tr.Cells.Add(tc);
                tc = new TableCell() {Text = ""};
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);
            }
            var trr = TblAtasKiri.Rows[0].Cells[1];
            //get department
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MDepartmentsSelect");
            var ddl = new DropDownList {ID = "ddlDepartment", ClientIDMode = ClientIDMode.Static, CssClass = "ddl"};
            ddl.Items.Add(new ListItem("Please Select...",""));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddl.Items.Add(new ListItem {Text = row["Descriptions"].ToString(), Value = row["id"].ToString()});
            }
            trr.Controls.Add(ddl);

            trr = TblAtasKiri.Rows[1].Cells[1];
            ddl = new DropDownList { ID = "ddlMachine", ClientIDMode = ClientIDMode.Static, CssClass = "ddl" };
            ddl.Items.Add(new ListItem("Please Select...", ""));
            trr.Controls.Add(ddl);

            trr = TblAtasKiri.Rows[2].Cells[1];
            var tb = new TextBox
                {
                    ClientIDMode = ClientIDMode.Static,
                    ID ="tbSerialNumber",
                    Text="",
                    CssClass = "tb"
                };
            trr.Controls.Add(tb);
            thead = new List<string>
                {
                    "Revision No",
                    "Prepared By",
                    "Approved By"
                };

            foreach (string t in thead)
            {
                var tc = new TableCell { Text = t };
                var tr = new TableRow();
                tr.Cells.Add(tc);
                var lbl = new Label {ID = "Lbl" + t.Replace(' ', '_'),ClientIDMode = ClientIDMode.Static,CssClass = "editable_cl"};
                tc = new TableCell() { Text = "" };
                tc.Controls.Add(lbl);
                tr.Cells.Add(tc);
                TblAtasKanan.Rows.Add(tr);
            }


        }
    }
}