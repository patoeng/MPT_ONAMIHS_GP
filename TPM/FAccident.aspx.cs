using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;
using System.Data.SqlClient;

namespace TPM
{
    public partial class FAccident : Page
    {
        private MySessions _ms;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string v = Request.QueryString["v"] ?? "";
                if (v == string.Empty)
                {
                    PrepareTable(v,false);
                }
                else
                {
                    _ms = new MySessions();
                    PrepareTable(v, !_ms.IsLeader);
                }
            }
        }
        private void ShowData(string v)
        {
            if (v == string.Empty)
            {
                return;
            }
            var dset = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                "usp_LAccidentSelect_By_Key", new SqlParameter("@key", v));
            var tbl = dset.Tables.Count > 0 ? dset.Tables[0] : null;
            if (tbl != null)
            {
                foreach (DataRow dr in tbl.Rows)
                {
                    var tr = new TableRow();
                    var tc = new TableCell { Text = "Accident Report Key" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = dr["accidentKey"].ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Updated By" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = dr["uploadedBy"].ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Employee No Of Injured" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = dr["employeeNumber"].ToString() };                   
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);

                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Name Of Injured" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = dr["employeeName"].ToString() };  
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                   //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Department" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = dr["dept"].ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Date Of Accident" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = ((DateTime) dr["datetimeOfAccident"]).ToString("yyyy-MM-dd") };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Time Of Accident" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = ((DateTime)dr["datetimeOfAccident"]).ToString("HH:mm") };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Location" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = (dr["location"]).ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Job/Process" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = (dr["job"]).ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Machine No" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = (dr["machineNumber"]).ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Machine Description" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = (dr["machineDescription"]).ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Injuries" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = (dr["injuries"]).ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "MC Days" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = (dr["daysMC"]).ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Accident Type" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = (dr["accidentType"]).ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                    //

                    tr = new TableRow();
                    tc = new TableCell { Text = "Others" };
                    tr.Cells.Add(tc);
                    tc = new TableCell { Text = (dr["others"]).ToString() };
                    tr.Cells.Add(tc);
                    tblForm.Rows.Add(tr);
                }
            }
        }
        private void PrepareTable(string v,bool __readonly)
        {
            
            var dset = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                               "usp_LAccidentSelect_By_Key", new SqlParameter("@key", v));
            var tbl = dset.Tables.Count > 0 ? dset.Tables[0] : null;
            bool nodata= !((tbl != null)&& (tbl.Rows.Count > 0)) && (v != string.Empty);
            if (nodata)
            {
                return;
            }
             var show = v != string.Empty;
            var dr = show ? tbl.Rows[0] : null;
           
            TableCell tc;
            TableRow tr;
            var hd = new HiddenField {ID = "AccessKey", ClientIDMode = ClientIDMode.Static};

            if (show)
            {
                tr = new TableRow();
                tc = new TableCell {Text = "Accident Report Key"};
                tr.Cells.Add(tc);

                hd.Value = dr["accidentKey"].ToString();
                tc = new TableCell {Text = dr["accidentKey"].ToString()};
                tr.Cells.Add(tc);
                tblForm.Rows.Add(tr);
                //

                tr = new TableRow();
                tc = new TableCell {Text = "Updated By"};
                tr.Cells.Add(tc);
                tc = new TableCell {Text = dr["uploadedBy"].ToString()};
                tr.Cells.Add(tc);
                tblForm.Rows.Add(tr);
                //
            }
           

            //
             tr = new TableRow();
            tc = new TableCell { Text = "Employee No Of Injured" };

            tr.Cells.Add(tc);
    
            var txt = new TextBox
            {
                ID = "txtEmployeeNumber",
                CssClass = "required digits",
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly,
                Text = show ? dr["employeeNumber"].ToString() : ""
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tc.Controls.Add(hd);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);

            //
            tr = new TableRow();
            tc = new TableCell { Text = "Name Of Injured" };

            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtEmployeeName",
                CssClass = "required",
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200,
                ReadOnly = true,
                Text = show ? dr["employeeName"].ToString() : ""
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Department" };

            tr.Cells.Add(tc);

            var ddl = new DropDownList
                {
                    ID = "ddlDepartment",
                    ClientIDMode = ClientIDMode.Static,
                    Enabled = !__readonly,
                    CssClass = "required"
                };
            ddl.Items.Add(new ListItem("Please Select ...", ""));
            if (show)
            {
                var tbl1 = dset.Tables[1];
                foreach (DataRow drs in tbl1.Rows)
                {
                    ddl.Items.Add(new ListItem(drs[1].ToString(),drs[0].ToString()));
                }
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText(dr["dept"].ToString()));
            }
           
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);           
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Date Of Accident" };
            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtDate",
                CssClass = "required",
                ClientIDMode = ClientIDMode.Static,
                Text = show ? ((DateTime)dr["datetimeOfAccident"]).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"),
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Time Of Accident" };
            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtTime",
                CssClass = "required time",
                ClientIDMode = ClientIDMode.Static,
                Text =   show ? ((DateTime)dr["datetimeOfAccident"]).ToString("HH:mm") : DateTime.Now.ToString("HH:mm"),
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Location" };
            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtLocation",
                CssClass = "required",
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly,
                Text = show ? (dr["location"]).ToString() : ""
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Job/Process" };
            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtJob",
                CssClass = "required",
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly,
                Text = show ? (dr["job"]).ToString() : ""
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Machine No" };
            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtmachineNumber",
                CssClass = "digits",
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly,
                Text = show ? (dr["machineNumber"]).ToString() : ""
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Machine Descriptions" };
            tr.Cells.Add(tc);

            ddl = new DropDownList
                {
                    ID = "ddlAsset",
                    ClientIDMode = ClientIDMode.Static,
                    Enabled = !__readonly,
                   
                };
            ddl.Items.Add(new ListItem("NA", ""));
            if (show)
            {
                var tbl2 = dset.Tables[2];
                foreach (DataRow drr in tbl2.Rows)
                {
                    ddl.Items.Add(new ListItem(drr[1].ToString(), drr[0].ToString()));
                }
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(dr["machineNumber"].ToString()));
            }

            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Injuries" };
            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtInjuries",
                CssClass = "required",
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly,
                Text = show ? (dr["injuries"]).ToString() : ""
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "MC Days" };
            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtDaysMC",
                CssClass = "required digits",
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly,
                Text = show ? (dr["daysMC"]).ToString():""
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Accident Type" };

            tr.Cells.Add(tc);
            ////Fatal / Serious / Minor / Dangerous Occurrence /Dieases / Fire
            ddl = new DropDownList { ID = "ddlDAccidentType", ClientIDMode = ClientIDMode.Static,Enabled = !__readonly};
            ddl.Items.Add(new ListItem("Please Select ...", ""));
            ddl.Items.Add(new ListItem("Fatal", "Fatal"));
            ddl.Items.Add(new ListItem("Serious", "Serious"));
            ddl.Items.Add(new ListItem("Minor", "Minor"));
            ddl.Items.Add(new ListItem("Dangerous Occurrence", "Dangerous Occurrence"));
            ddl.Items.Add(new ListItem("Dieases", "Dieases"));
            ddl.Items.Add(new ListItem("Fire", "Fire"));
            ddl.CssClass = "required";
            ddl.SelectedValue = show ? (dr["accidentType"]).ToString() : "";
            tc = new TableCell();
            tc.Controls.Add(ddl);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //
            //

           

            if (show)
            {
                tr = new TableRow();
                tc = new TableCell { Text = "File Attachments" };
                tr.Cells.Add(tc);
                var link = new HtmlGenericControl("a");
                link.Attributes.Add("href", "YAccAttachment.aspx?v=" + v);
                
                link.InnerHtml = "Click To See Attachments";
                tc = new TableCell();
                tc.Controls.Add(link);
                tr.Cells.Add(tc);
                tblForm.Rows.Add(tr);
            }
           
            //

            tr = new TableRow();
            tc = new TableCell { Text = "Others" };
            tr.Cells.Add(tc);

            txt = new TextBox
            {
                ID = "txtOthers",
                TextMode = TextBoxMode.MultiLine,
                ClientIDMode = ClientIDMode.Static,
                Rows = 10,
                Width = 200,
                ReadOnly = __readonly,
                Text = show ? (dr["others"]).ToString() : ""
            };
            tc = new TableCell();
            tc.Controls.Add(txt);
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
            //


            tr = new TableRow();
            tc = new TableCell { Text = "&nbsp;" };
            tr.Cells.Add(tc);
            if (!show)
            {
                var htm = new HtmlGenericControl("h3");
                htm.Attributes.Add("id", "submit");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.InnerHtml = "SUBMIT";
                tc = new TableCell();
                tc.Controls.Add(htm);
            }
            else
            {
                if (_ms.IsLeader)
                {
                    var htm = new HtmlGenericControl("h3");
                    htm.Attributes.Add("id", "update");
                    htm.Attributes.Add("class", "btn btn-primary");
                    htm.InnerHtml = "UPDATE";
                    tc = new TableCell();
                    tc.Controls.Add(htm);
                }
            }
            tr.Cells.Add(tc);
            tblForm.Rows.Add(tr);
        }
    }
}