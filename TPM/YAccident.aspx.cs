using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class YAccident : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PrepareForm();
            }
        }

        public string Tanggal;
        private void PrepareForm()
        {
            //get department
            Department.Items.Add(new ListItem("ALL",""));
            var dset = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text,
                                                "SELECT ID,DESCRIPTIONS FROM MDEPARTMENTS WHERE ACTIVE='1'");
            if (dset.Tables.Count > 0)
            {
                foreach (DataRow dr in dset.Tables[0].Rows) 
                {
                    Department.Items.Add(new ListItem(dr[1].ToString(),dr[0].ToString()));
                }
            }

            //get first date of the month
            Tanggal = DateTime.Now.ToString("yyyy-MM-") + "01";
            startdate.Value = Tanggal;

            //set accidentType
            AccType.Items.Add(new ListItem("ALL", ""));
            AccType.Items.Add(new ListItem("Fatal", "Fatal"));
            AccType.Items.Add(new ListItem("Serious", "Serious"));
            AccType.Items.Add(new ListItem("Minor", "Minor"));
            AccType.Items.Add(new ListItem("Dangerous Occurrence", "Dangerous Occurrence"));
            AccType.Items.Add(new ListItem("Dieases", "Dieases"));
            AccType.Items.Add(new ListItem("Fire", "Fire"));

            
        }
    }
}