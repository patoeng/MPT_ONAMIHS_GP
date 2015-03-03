using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;


namespace TPM
{
    public partial class YDownTime : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack){Prepare();}
        }
        protected void Prepare() {
           var list = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MDepartmentsSelect", new SqlParameter("@id", DBNull.Value));
           var dt = list.Tables.Count>0 ? list.Tables[0]: new DataTable();


               ddlDepartment.Items.Add(new ListItem("ALL", "0"));
               foreach (DataRow dr in dt.Rows)
               {
                   ddlDepartment.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString()));
               }
           ddlMonth.Items.Add(new ListItem("Please Select",""));     
           for (int i = 1; i < 13; i++)
           {
               ddlMonth.Items.Add(new ListItem(TPMHelper.Namabulan()[i][1],i.ToString(CultureInfo.InvariantCulture)));
           }
           ddlYear.Items.Add(new ListItem("Please Select", ""));     
           for (int i = 0; i < 200; i++)
           {
               ddlYear.Items.Add(new ListItem((i + 2013).ToString(CultureInfo.InvariantCulture), (i + 2013).ToString(CultureInfo.InvariantCulture)));
           } 
        }
    }
}