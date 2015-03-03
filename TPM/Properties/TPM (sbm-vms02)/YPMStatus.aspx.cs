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
        public int schedule_id;
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
                    schedule_id = Request.QueryString["pid"] == null ? 0 : Convert.ToInt32(Request.QueryString["pid"]);
                    prepare_page();
                }
                else
                {
                    Server.Transfer(session.redirection);
                }
            }
            
        }
        protected void prepare_page()
        {
            HtmlGenericControl htmlTag = new HtmlGenericControl();
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@LPMId", schedule_id));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_getLPMSchedules", sqlparams.ToArray());
            TableRow tr;
            TableCell tc;

            DataTable dt = ds.Tables[0];

            foreach (DataRow sdr in dt.Rows)
            {
                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "PM ID";
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.Text = schedule_id.ToString();
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "Asset Name";
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.Text = sdr["Asset_Name"].ToString();
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
                tc.Text = ((string)(sdr["status"])).ToString(); ;
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
                tc = new TableCell();
                tc.Text = ((DateTime)(sdr["Generated_Date"])).ToString("f");
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);
                lid2 = (int)(sdr["CID"].ToString()!=""?sdr["CID"]:0);
                lid =(int)(sdr["id"]);
                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "Status Updated By";
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.Text = ((string)(sdr["done_by"])).ToString(); ;
                tr.Cells.Add(tc);
                tblLayOut.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "Actions";
                tr.Cells.Add(tc);

                
                HtmlGenericControl btn = new HtmlGenericControl("h4");
                switch (status_id)
                {
                    case 1:
                        btn = new HtmlGenericControl("h4");
                        btn.InnerHtml = "ALLOW SCHEDULE EXECUTION";
                        btn.Attributes.Add("class", "btn btn-primary tombol");
                        btn.ID = "allow";
                        btn.ClientIDMode = ClientIDMode.Static;
                        tc = new TableCell();
                        if (session.IsManagement)
                        {
                            tc.Controls.Add(btn);
                        }

                        btn = new HtmlGenericControl("h4");
                        btn.InnerHtml = "RE-SCHEDULE";
                        btn.Attributes.Add("class", "btn btn-primary datepicker");
                        btn.ID = "schedule";
                        btn.ClientIDMode = ClientIDMode.Static;
                        if (session.IsManagement)
                        {
                            tc.Controls.Add(btn);
                        }
                        tr.Cells.Add(tc);
                        break;
                    case 2:
                        tc = new TableCell();
                        btn = new HtmlGenericControl("h4");
                        btn.InnerHtml = "RE-SCHEDULE";
                        btn.ID = "schedule";
                        btn.ClientIDMode = ClientIDMode.Static;
                        btn.Attributes.Add("class", "btn btn-primary  datepicker");
                        tc = new TableCell();
                        if (session.IsManagement)
                        {
                            tc.Controls.Add(btn);
                        }
                        tr.Cells.Add(tc);
                        break;
                    case 3:
                        btn = new HtmlGenericControl("h4");
                        btn.InnerHtml = "START EXECUTION";
                        btn.ID = "start";
                        btn.ClientIDMode = ClientIDMode.Static;
                        btn.Attributes.Add("class", "btn btn-primary tombol ");
                        tc = new TableCell();
                        tc.Controls.Add(btn);
                        if ((int)sdr["CheckListtypes_id"]==2){
                            btn = new HtmlGenericControl("h4");
                            btn.InnerHtml = "RE-SCHEDULE";
                            btn.ID = "schedule";
                            btn.ClientIDMode = ClientIDMode.Static;
                            btn.Attributes.Add("class", "btn btn-primary  datepicker");
                            if (session.IsManagement)
                            {
                                tc.Controls.Add(btn);
                            }
                        }
                        tr.Cells.Add(tc);
                        break;
                    case 4:
                        btn = new HtmlGenericControl("h4");
                        btn.InnerHtml = "FINISH EXECUTION";
                        btn.ID = "finish";
                        btn.ClientIDMode = ClientIDMode.Static;
                        btn.Attributes.Add("class", "btn btn-primary tombol");
                        tc = new TableCell();
                        tc.Controls.Add(btn);
                        tr.Cells.Add(tc);
                        break;
                    case 5:
                        btn = new HtmlGenericControl("h4");
                        btn.InnerHtml = "SUMMARY";
                        btn.ID = "summary";
                        btn.ClientIDMode = ClientIDMode.Static;
                        btn.Attributes.Add("class", "btn btn-primary");
                        tc = new TableCell();
                        tc.Controls.Add(btn);
                        tr.Cells.Add(tc);
                        break;
                }
                tblLayOut.Rows.Add(tr);
                txtRemarks.Text = sdr["Remarks"].ToString();
            }
            sqlparams.Clear();
            sqlparams.Add(new SqlParameter("@pm_schedule_id",schedule_id));
            ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_LPMSchedulesSelect_byPMID", sqlparams.ToArray());
            if (ds.Tables.Count > 0) {
                dt = ds.Tables[0];
                List<string> thead = new List<string>();
                thead.Add("Step");
                thead.Add("Status");
                thead.Add("Done By");
                thead.Add("Updated At");
                thead.Add("Remarks");
                tr = new TableRow();
                tr.TableSection = TableRowSection.TableHeader;
                for (int i = 0; i < thead.Count; i++) {
                    tc = new TableHeaderCell();
                    tc.Text = thead[i];
                    tr.Cells.Add(tc);
                }
                tblHistory.Rows.Add(tr);
                int o = 0;
                foreach (DataRow dr in dt.Rows) {
                    o++;
                    tr = new TableRow();
                    tc = new TableCell();
                    tc.Text = o.ToString();
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                    tc.Text = dr["status"].ToString().ToUpper();
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                    tc.Text = dr["done_by"].ToString().ToUpper();
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                    tc.Text = ((DateTime)dr["generated_date"]).ToString("f").ToUpper();
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                    tc.Text = (dr["remarks"]).ToString();
                    tr.Cells.Add(tc);
                    tblHistory.Rows.Add(tr);                
                }
            }
        }

    }
}