using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class TpmEmailNew : System.Web.UI.Page
    {
        public string EmailingId;
        public string Department;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EmailingId = (Request.QueryString["i"] == null) || (Request.QueryString["i"] == "") ? "0" : Request.QueryString["i"];
                Prepare();
            }
        }
        private void Prepare()
        {
            EmailId.Value = EmailingId;
            var sqlparams = new List<SqlParameter> { new SqlParameter("@Id", EmailingId) };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_GetEmailing", sqlparams.ToArray());

            if (ds.Tables.Count > 0)
            {
                var emailingTbl = ds.Tables[0];

                lblDescription.Text = emailingTbl.Rows[0]["Description"].ToString();
                lblEmailing.Text = emailingTbl.Rows[0]["MailingType"].ToString()=="1"?"Event Triggered":"Regular";

                lblRepeat.Text = (bool)emailingTbl.Rows[0]["Repeat"] ? "Yes" : "No";
                lblActive.Text = (bool)emailingTbl.Rows[0]["Active"] ? "Yes" : "No";
                lblEachDept.Text = (bool)emailingTbl.Rows[0]["EachDept"] ? "Yes" : "No";

                lblRemarks.Text = emailingTbl.Rows[0]["Remarks"].ToString();

               var day = (int) emailingTbl.Rows[0]["Frequency"];
                /**/
                day_1.Checked = (day & 1) == 1;
                day_2.Checked = (day & 2) == 2;
                day_3.Checked = (day & 4) == 4;
                day_4.Checked = (day & 8) == 8;
                day_5.Checked = (day & 16) == 16;
                day_6.Checked = (day & 32) == 32;
                day_7.Checked = (day & 64) == 64;
                /**/
                var recipientTbl = ds.Tables[1];

                var thr = new TableHeaderRow {TableSection = TableRowSection.TableHeader};

                TableHeaderCell thc;
                for (int i = 2; i < recipientTbl.Columns.Count; i++ )
                {
                    thc = new TableHeaderCell {Text = recipientTbl.Columns[i].ColumnName};
                    thr.Controls.Add(thc);
                }
                thc = new TableHeaderCell();
                thr.Controls.Add(thc);
                tblRecipient.Rows.Add(thr);
               
                foreach (DataRow  ss in recipientTbl.Rows)
                {
                    var tr = new TableRow {TableSection = TableRowSection.TableBody};
                    tr.Attributes.Add("id",ss[0] + "-row");

                    TableCell tc;
                    for (int i = 2; i < recipientTbl.Columns.Count; i++)
                    {
                        tc = new TableCell { Text = ss[i].ToString()};
                        tc.Attributes.Add("class",
                                          i < recipientTbl.Columns.Count - 1 ? "text_editable" : "dept_editable");
                        tc.Attributes.Add("id", recipientTbl.Columns[i].ColumnName + "_" + ss[0]);
                        tr.Controls.Add(tc);
                    }
                   tc = new TableCell();
                   var ht= new HtmlGenericControl("a") { InnerText = "REMOVE" };
                   ht.Attributes.Add("class","btn btn-primary btnremove");
                   ht.Attributes.Add("id", "btn_" + ss[0]);
                   tc.Controls.Add(ht);
                   tr.Controls.Add(tc);
                   tblRecipient.Rows.Add(tr);
                }

                var tblDepartment = ds.Tables[2];

                Department = "{";
                foreach (DataRow dr in tblDepartment.Rows)
                {
                    Department +="'"+ dr["id"] + "':'" + dr["Descriptions"] + "',";
                }
                Department = Department.Remove(Department.Length - 1)+"}";               
            }
        }
    }
}