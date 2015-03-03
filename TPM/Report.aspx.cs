using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Web.UI.DataVisualization.Charting;

namespace TPM
{
    public partial class Report : System.Web.UI.Page
    {
        TPMHelper Functions = new TPMHelper();
        public string reportType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                reportType = Request.QueryString["r"] ?? "";
                //Prepare();

                //ambil tanggal, bulan dan tahun
                string currentDate = DateTime.Now.Date.ToString("dd");
                string currentMonth = DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                string currentYear = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

                // Buat dropdown list Department3
                SqlDataReader sdr = SqlHelper.ExecuteReader(Functions.TPMDBConnection(), CommandType.Text, "Select * from MDepartments");
                ListItem li;
                ddlDepartment.Items.Add(new ListItem("ALL", "0"));
                while (sdr.Read())
                {
                    li = new ListItem {Text = sdr["Descriptions"].ToString(), Value = sdr["Id"].ToString()};
                    ddlDepartment.Items.Add(li);
                }

                // Buat dropdown list Status
                ddlStatus.Items.Add(new ListItem("ALL", "0"));
                ddlStatus.Items.Add(new ListItem("Open", "1"));
                ddlStatus.Items.Add(new ListItem("Close", "5"));

                //lblTitle.Text = "Please select Report Menu";
                //lblTitle.Style.Add("font-size", "20px");
                lblTitle.CssClass = "lblTitle";

                switch (reportType) {
                    case "1":
                        lblTitle.Text = "Work Order Status";

                        ddl2.Visible = false;
                        ddl3.Visible = false;
                        ddlRequestType.Visible = false;
                        ddlNC.Visible = false;

                        break;
                    case "2":
                        lblTitle.Text = "Improvement Work Order Status";

                        ddl2.Visible = false;
                        ddl3.Visible = false;
                        ddlRequestType.Visible = false;
                        ddlNC.Visible = false;

                        break;
                    case "3":
                        lblTitle.Text = "Summary Breakdown By Case";
                        ddlStatus.Items.Clear();
                        ddlStatus.Items.Add(new ListItem("ALL", "0"));
                        ddlStatus.Items.Add(new ListItem("ACCIDENT", "1"));
                        ddlStatus.Items.Add(new ListItem("ABNORMAL", "2"));
                        ddlStatus.Items.Add(new ListItem("NC", "3"));

                        ddl2.Visible = false;
                        ddl3.Visible = false;
                        ddlRequestType.Visible = false;
                        ddlNC.Visible = false;

                        break;
                    case "4":
                        lblTitle.Text = "Top 10 Case Maintenance Base on Main Machines";

                        ddlRequestType.Visible = false;
                        ddlNC.Visible = false;
                        ddl2.Visible = false;
                        break;
                    case "5":
                        lblTitle.Text = "Summary PM vs BD by Hours";

                        ddl1.Visible = false;
                        ddlStatus.Visible = false;

                        ddl2.Visible = true;
                        ddl3.Visible = true;
                        ddlRequestType.Visible = true;
                        ddlNC.Visible = true;

                        ddlRequestType.Items.Clear();
                        ddlRequestType.Items.Add(new ListItem("ALL", "0"));
                        ddlRequestType.Items.Add(new ListItem("Preventive", "1"));
                        ddlRequestType.Items.Add(new ListItem("Breakdown", "2"));

                        ddlNC.Items.Clear();
                        ddlNC.Items.Add(new ListItem("Default (NC)", "0"));
                        ddlNC.Items.Add(new ListItem("ABNORMAL", "1"));
                        ddlNC.Items.Add(new ListItem("ALL", "2"));
                        ddlNC.Enabled = true;
                        break;
                };
            }
            else
            {
                lblTitle.Text = "Elseeeee.....";
            }

        }
        protected void Prepare()
        {

        }
    }
}