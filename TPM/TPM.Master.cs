using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;

namespace TPM
{
    public partial class TPM : System.Web.UI.MasterPage
    {
        public TPMHelper Functions = new TPMHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.Controls.Add(Functions.StyleBootstrap);
            Page.Header.Controls.Add(Functions.StyleDataTables);
            Page.Header.Controls.Add(Functions.StyleDatePicker);
            Page.Header.Controls.Add(Functions.jQueryRef);
            Page.Header.Controls.Add(Functions.jQForm);
            Page.Header.Controls.Add(Functions.jQMetaData);
            Page.Header.Controls.Add(Functions.jQValidate);
            Page.Header.Controls.Add(Functions.jQDataTables);
            Page.Header.Controls.Add(Functions.json2);
            Page.Header.Controls.Add(Functions.jqueryCSV);
            Page.Header.Controls.Add(Functions.jdatepicker);
            Page.Header.Controls.Add(Functions.jeditable);
            Page.Header.Controls.Add(Functions.highchart);
            Page.Header.Controls.Add(Functions.hcdata);
            Page.Header.Controls.Add(Functions.hcexport);
        }
        
    }
       
}