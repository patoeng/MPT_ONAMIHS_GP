using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TPM
{
    public partial class YHeaterMonitoring : System.Web.UI.Page
    {
        public string m;
        public string Tanggal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Prepare();
            }
        }

        private void Prepare()
        {
            Tanggal = "01-" + DateTime.Now.ToString("MMM-yyyy");
            OvenID.Items.Add(new ListItem("ALL", "0"));
            OvenID.Items.Add(new ListItem("Oven#1", "Oven#1"));
            OvenID.Items.Add(new ListItem("Oven#2", "Oven#2"));
            OvenID.Items.Add(new ListItem("Oven#3", "Oven#3"));
            OvenID.Items.Add(new ListItem("Oven#4", "Oven#4"));

            Range.Items.Add(new ListItem("ALL", "0"));
            Range.Items.Add(new ListItem("<51", "1"));
            Range.Items.Add(new ListItem(">50 AND <200", "2"));
            Range.Items.Add(new ListItem(">200", "3"));

            startdate.Value = Tanggal;
        }
    }
}