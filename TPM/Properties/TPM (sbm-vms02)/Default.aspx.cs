using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;

namespace TPM
{
    public partial class Default : System.Web.UI.Page
    {
        
        public TPMHelper Functions = new TPMHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack) {
               
            }
          
        }
        protected void prepare(string employeeno)
        {
            
        }
    }
}