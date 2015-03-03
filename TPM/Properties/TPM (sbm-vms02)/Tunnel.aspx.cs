using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;

namespace TPM
{
    public partial class Tunnel : System.Web.UI.Page
    {
        TPMHelper Functions = new TPMHelper();
        public string url = "";
        public string d = "";
        public string s = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.Controls.Add(Functions.jQueryRef);
            url = Request.QueryString["u"] != null ? Request.QueryString["u"].ToString() : "";
            s = Request.QueryString["s"] != null ? Request.QueryString["s"].ToString() : "";
            d = Request.QueryString["d"] != null ? Request.QueryString["d"].ToString() : "";

            if (d != "") { Session.Add("tpm_employeeno", s); }
        }
    }
}