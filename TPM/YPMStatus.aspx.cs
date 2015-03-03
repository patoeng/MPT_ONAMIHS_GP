using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TPM.Classes;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace TPM
{
    public partial class YPMStatus : System.Web.UI.Page
    {
        TPMHelper Functions = new TPMHelper();
        public string schedule_id;
        public int schedule_id_true;
        public int lid;
        public int lid2;
        public string sched = "";
        public int asmod = 0;
        public int status_id = 0;
        public MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                if (!session.IsPublic)
                {
                    schedule_id = Request.QueryString["pid"] == null ? "" : Request.QueryString["pid"].ToString();
                    prepare_page();
                }
                else
                {
                    Server.Transfer(session.Redirection);
                }
            }
            
        }
        protected void prepare_page()
        {
            var htmlTag = new HtmlGenericControl();
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@pmkey", schedule_id) {SqlDbType = SqlDbType.NVarChar, Size = 100}
                };
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_getLPMSchedules", sqlparams.ToArray());
            TableRow tr;
            TableCell tc;

            DataTable dt = ds.Tables[0];

            foreach (DataRow sdr in dt.Rows)
            {
                schedule_id_true = (int)sdr["PMID"];
                tr = new TableRow();
                tc = new TableCell {Text = "PM ID"};
                tr.Cells.Add(tc);
                tc = new TableCell {Text = schedule_id};
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell {Text = "Machine Name"};
                tr.Cells.Add(tc);
                tc = new TableCell {Text = sdr["Asset_Name"].ToString()};
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell {Text = "Serial Number"};
                tr.Cells.Add(tc);
                tc = new TableCell {Text = sdr["SN"].ToString()};
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                asmod = (int)sdr["Asset_Model_id"];
                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "Department";
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.Text = sdr["Department"].ToString();
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "Maintenance Type";
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.Text = sdr["ChecklistType"].ToString();
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "Initial Scheduled Date";
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.Text = ((DateTime)(sdr["Scheduled_Date"])).ToString("D"); ;
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "Status";
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.Text = ((string) (sdr["status"]));
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                string Status_label = "";
                 status_id = (int)(sdr["PM_status_Id"]);

                Status_label = "New Scheduled Date";



                tr = new TableRow();
                tc = new TableCell();
                tc.Text = Status_label;
                tr.Cells.Add(tc);
                tc = new TableCell();
                sched = ((DateTime)(sdr["Date"])).ToString("d-MMM-yyyy");
                tc.Text = ((DateTime)(sdr["Date"])).ToString("D");
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "Status Updated At";
                tr.Cells.Add(tc);
                tc = new TableCell {Text = ((DateTime) (sdr["Generated_Date"])).ToString("f")};
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);
                lid2 = (int)(sdr["CID"].ToString()!=""?sdr["CID"]:0);
                lid =(int)(sdr["id"]);
                tr = new TableRow();
                tc = new TableCell {Text = "Status Updated By"};
                tr.Cells.Add(tc);
                tc = new TableCell {Text = sdr["done_by"].ToString()};
                 tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell {Text = "Current Condition"};
                tr.Cells.Add(tc);
                tc = new TableCell {Text = (sdr["request"].ToString())};
                if (status_id == 4)
                {
                    tc.CssClass = "editable";
                    tc.Attributes.Add("id", sdr["PMID"] + "-Request");

                }
                tc.Style.Add("white-space", "pre-line");
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell {Text = "Preventive Action"};
                tr.Cells.Add(tc);
                tc = new TableCell {Text = (sdr["request_type"].ToString())};
                if (status_id == 4)
                {
                    tc.CssClass = "editable2";
                    tc.Attributes.Add("id", sdr["PMID"] + "-request_type");

                }
                tc.Style.Add("white-space", "pre-line");
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);
           

                tr = new TableRow();
                tc = new TableCell {Text = "Remarks"};
                tr.Cells.Add(tc);
                tc = new TableCell {Text = (sdr["remarks2"].ToString())};
                if (status_id < 6)
                {
                    tc.CssClass = "text_editable";
                    tc.Attributes.Add("id", sdr["PMID"] + "-remarks");
                   
                }
                tc.Style.Add("white-space", "pre-line");
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell {Text = "Actions"};
                tr.Cells.Add(tc);

                
                var btn = new HtmlGenericControl("h4");
                switch (status_id)
                {
                    case 1:
                        btn = new HtmlGenericControl("h4")
                            {
                                InnerHtml = "ALLOW SCHEDULE EXECUTION",
                                ID = "allow",
                                ClientIDMode = ClientIDMode.Static
                            };

                        btn.Attributes.Add("class", "btn btn-primary tombol");
                        tc = new TableCell();
                        if (session.IsManagement)
                        {
                            tc.Controls.Add(btn);
                        }

                        btn = new HtmlGenericControl("h4")
                            {
                                InnerHtml = "RE-SCHEDULE",
                                ID = "schedule",
                                ClientIDMode = ClientIDMode.Static
                            };

                        btn.Attributes.Add("class", "btn btn-primary datepicker");
                        if (session.IsManagement)
                        {
                            tc.Controls.Add(btn);
                        }
                        tr.Cells.Add(tc);
                        break;
                    case 2:
                        btn = new HtmlGenericControl("h4")
                            {
                                InnerHtml = "RE-SCHEDULE",
                                ID = "schedule",
                                ClientIDMode = ClientIDMode.Static
                            };
                        btn.Attributes.Add("class", "btn btn-primary  datepicker");
                        tc = new TableCell();
                        if (session.IsManagement)
                        {
                            tc.Controls.Add(btn);
                        }
                        tr.Cells.Add(tc);
                        break;
                    case 3:
                        btn = new HtmlGenericControl("h4")
                            {
                                InnerHtml = "START EXECUTION",
                                ID = "start",
                                ClientIDMode = ClientIDMode.Static
                            };
                        btn.Attributes.Add("class", "btn btn-primary tombol ");
                        tc = new TableCell();
                        if (session.IsEngineering)
                        {
                            tc.Controls.Add(btn);
                        }
                        if ((int)sdr["CheckListtypes_id"]==2){
                            btn = new HtmlGenericControl("h4")
                                {
                                    InnerHtml = "RE-SCHEDULE",
                                    ID = "schedule",
                                    ClientIDMode = ClientIDMode.Static
                                };
                            btn.Attributes.Add("class", "btn btn-primary  datepicker");
                            if (session.IsManagement)
                            {
                                tc.Controls.Add(btn);
                            }
                        }
                        tr.Cells.Add(tc);
                        break;
                    case 4:
                        btn = new HtmlGenericControl("h4")
                            {
                                InnerHtml = "FINISH EXECUTION",
                                ID = "finish",
                                ClientIDMode = ClientIDMode.Static
                            };
                        btn.Attributes.Add("class", "btn btn-primary tombol");
                        tc = new TableCell();
                        if (session.IsEngineering)
                        {
                            tc.Controls.Add(btn);
                        }
                        tr.Cells.Add(tc);
                        break;
                    case 5:
                        btn = new HtmlGenericControl("h4")
                            {
                                InnerHtml = "REVIEW",
                                ID = "review",
                                ClientIDMode = ClientIDMode.Static
                            };
                        btn.Attributes.Add("class", "btn btn-primary tombol");
                        tc = new TableCell();
                        if (session.IsLeader)
                        {
                            tc.Controls.Add(btn);
                        }
                        tr.Cells.Add(tc);
                        break;
                    case 6:
                          tc = new TableCell();
                        tc.Controls.Add(btn);
                        tr.Cells.Add(tc);
                        break;
                }
                tblLayOut.Rows.Add(tr);
               // txtRemarks.Text = sdr["Remarks"].ToString();
            }
            //sqlparams.Clear();
            //sqlparams.Add(new SqlParameter("@pm_schedule_id",schedule_id));
            //ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_LPMSchedulesSelect_byPMID", sqlparams.ToArray());
           
            if (ds.Tables.Count > 0) {
                dt = ds.Tables[1];
                var thead = new List<string> {"Step", "Status", "Done By", "Updated At", "Remarks"};
                tr = new TableRow {TableSection = TableRowSection.TableHeader};
                foreach (string t in thead)
                {
                    tc = new TableHeaderCell {Text = t};
                    tr.Cells.Add(tc);
                }
                tblHistory.Rows.Add(tr);
                int o = 0;
                foreach (DataRow dr in dt.Rows) {
                    o++;
                    tr = new TableRow();
                    tc = new TableCell {Text = o.ToString()};
                    tr.Cells.Add(tc);
                    tc = new TableCell {Text = dr["status"].ToString().ToUpper()};
                    tr.Cells.Add(tc);
                    tc = new TableCell {Text = dr["done_by"].ToString().ToUpper()};
                    tr.Cells.Add(tc);
                    tc = new TableCell {Text = ((DateTime) dr["generated_date"]).ToString("f").ToUpper()};
                    tr.Cells.Add(tc);
                    tc = new TableCell {Text = dr["remarks"].ToString()};
                    if (status_id < 5)
                    {
                        tc.CssClass = "text_editable";
                        tc.Attributes.Add("id", dr["iD"] + "-remarks2"); 
                    }
                    tc.Style.Add("white-space", "pre-line");
                    tr.Cells.Add(tc);
                    tblHistory.Rows.Add(tr);                
                }
            }
        }

    }
}