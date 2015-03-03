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
    public partial class Report2 : System.Web.UI.Page
    {
       
        public string ReportType = "";
       
        public string Tittle;
        private string _ddlFlexyTittle;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportType = Request.QueryString["r"] ?? "";
                Prepare(ReportType);

            }

        }
        protected void Prepare(string w)
        {
           ddlDepartment.Items.Add(new ListItem("ALL",""));
            switch (w)
            {
                case "1" :  
                   
                    Tittle = "Workorder Status";
                    _ddlFlexyTittle = "Status";
                    break;
                case "2":
                   
                    Tittle = "Improvement Work Order Status";
                    _ddlFlexyTittle = "Status";
                    break;
                case "3":
                  
                    Tittle = "Summary Breakdown By Cases";
                    _ddlFlexyTittle = "Status Machine Condition";
                    break;
                case "4":
                    
                    Tittle = "Summary Breakdown By Total Hours";
                    _ddlFlexyTittle = "Total Hours";
                    break;
                case "5":

                    Tittle = "Summary TOP 10 Break Down by Case";
                    _ddlFlexyTittle = "Work Request Type";
                    break;
            }
            lblTitle.Text = Tittle;
            lblddlFlexy.Text = _ddlFlexyTittle;

            ddlFlexy.Items.Add(new ListItem("ALL", ""));
            switch (w)
            {
                case "1":
                  
                    ddlFlexy.Items.Add(new ListItem("NC", "NC"));
                    ddlFlexy.Items.Add(new ListItem("ABNORMAL", "ABNORMAL"));
                  
                    break;
                case "2":

                    ddlFlexy.Items.Add(new ListItem("OPEN", "OPEN"));
                    ddlFlexy.Items.Add(new ListItem("CLOSED", "CLOSED"));

                    break;
                case "3":
                    ddlFlexy.Items.Add(new ListItem("OPEN", "OPEN"));
                    ddlFlexy.Items.Add(new ListItem("CLOSED", "CLOSED"));
                    
                    break;
                case "4":
                    ddlFlexy.Items.Add(new ListItem("Preventive Maintenance","PM"));
                    ddlFlexy.Items.Add(new ListItem("Break Down","BD"));
                    break;
                case "5":
                  
                    break;
            }

            txtStartdate.Value = DateTime.Now.ToString("yyyy-") + "01-01";
            txtEnddate.Value = DateTime.Now.ToString("yyyy-MM-dd");


        }
    }
}