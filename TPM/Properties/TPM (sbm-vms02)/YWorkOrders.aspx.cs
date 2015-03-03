using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.HtmlControls;


namespace TPM
{
    public partial class YWorkOrders : System.Web.UI.Page
    {
        public string mwoid = "0";
        public TPMHelper F = new TPMHelper();
        public string status = "";
        public MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack){
                if (!session.IsPublic)
                {
                    mwoid = Request.QueryString["v"] != null ? Request.QueryString["v"].ToString() : "0";
                    prepare();
                }
                else {
                    Server.Transfer(session.redirection);
                }
            }
        }
        protected void prepare()
        {
            List<SqlParameter> sql = new List<SqlParameter>();
            sql.Add(new SqlParameter("@mwoid",mwoid));

            DataSet ds = SqlHelper.ExecuteDataset(F.TPMDBConnection(),CommandType.StoredProcedure,"usp_getWorkOrder",sql.ToArray());
            DataTable mwo = ds.Tables[0];
            DataTable lwo = ds.Tables[1];
            string LWO_ID = "";
            string LWO_REMARKS = "";
            string MWO_ID = "";
            HtmlGenericControl htm;
            HiddenField hid;

            TableRow tr;
            TableCell tc;
          
            foreach (DataRow dr in mwo.Rows) {
                status =dr[6].ToString();
                LWO_ID = dr["LWO_ID"].ToString();
                LWO_REMARKS = dr["LWO_REMARKS"].ToString();
                MWO_ID = dr[0].ToString();
                for (int i = 0; i < mwo.Columns.Count-2; i++) {
                    tr = new TableRow();
                    tc = new TableCell();
                    tc.Text = mwo.Columns[i].ColumnName.Replace('_', ' '); ;
                    tr.Controls.Add(tc);
                    tc = new TableCell();
                    if (mwo.Columns[i].DataType == System.Type.GetType("System.DateTime"))
                    {
                        tc.Text = ((DateTime)dr[i]).ToString("f");
                    }
                    else
                    {
                        tc.Text = dr[i].ToString();
                    }
                    tr.Controls.Add(tc);
                    tblLastStatus.Rows.Add(tr);
                }
            }

            List<string> btntext = new List<string>();
            switch (status){
                case"ISSUED":
                    if (session.IsEngineering){
                        btntext.Add("CONFIRM");
                    }
                    break;
                case "CONFIRMED": if (session.IsEngineering) { btntext.Add("START THE WORK"); } break;
                case "WORK STARTED": if (session.IsEngineering) { btntext.Add("FINISH THE WORK"); } break;
                case "COMPLETED": if ((session.IsLeader) && (session.IsManagement)) { btntext.Add("REVIEW"); } break;
            }
           
            if (btntext.Count > 0) {

                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "ACTION TO DO";
                tr.Cells.Add(tc);
                tblLastStatus.Rows.Add(tr);
                tc = new TableCell();
                
                htm = new HtmlGenericControl("h4");
                htm.Attributes.Add("id", "button1");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.Attributes.Add("value",btntext[0]);
                htm.InnerHtml = btntext[0];
                tc.Controls.Add(htm);
              

                hid = new HiddenField();
                hid.ID = "mwoid";
                hid.ClientIDMode = ClientIDMode.Static;
                hid.Value = MWO_ID;
                tc.Controls.Add(hid);

                

                hid = new HiddenField();
                hid.ID = "lwoid";
                hid.ClientIDMode = ClientIDMode.Static;
                hid.Value = LWO_ID;
                tc.Controls.Add(hid);

                tr.Cells.Add(tc);
                tblLastStatus.Rows.Add(tr);
               
            }
            if (status == "CLOSED") {

                tr = new TableRow();
                tc = new TableCell();
                tc.Text = "REMARKS";
                tr.Cells.Add(tc);
                
                tc = new TableCell();
                tc.Text = LWO_REMARKS;
                tr.Cells.Add(tc);
                tblLastStatus.Rows.Add(tr);

            }
            if ((status != "")&&(status!="CLOSED"))
            {
                tr = new TableRow();
                htm = new HtmlGenericControl("h4");
                htm.InnerHtml = "REMARKS";
                tc = new TableCell();
                tc.Controls.Add(htm);
                tr.Cells.Add(tc);
                tblInfo.Rows.Add(tr);
                tr = new TableRow();
                TextBox txtRemarks = new TextBox();
                txtRemarks.ID = "txtRemarks";
                txtRemarks.ClientIDMode = ClientIDMode.Static;
                txtRemarks.TextMode = TextBoxMode.MultiLine;
                txtRemarks.Rows = 6;
                txtRemarks.Text = LWO_REMARKS;
                tc = new TableCell();
                tc.Controls.Add(txtRemarks);
                tr.Cells.Add(tc);
                tblInfo.Rows.Add(tr);

                tr = new TableRow();
                htm = new HtmlGenericControl("h4");
                htm.Attributes.Add("id", "btnRemarks");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.InnerHtml = "SAVE";
                htm.Attributes.Add("value", "remarks");
                tc = new TableCell();
                if (session.IsEngineering)
                {
                    tc.Controls.Add(htm);
                }
                tr.Cells.Add(tc);
                tblInfo.Rows.Add(tr);
            }
            int w = lwo.Rows.Count;
            if (w > 0)
            {
                tr = new TableRow();
                tr.TableSection = TableRowSection.TableHeader;
                tc = new TableCell();
                tc.Text = "STEP";
                tr.Controls.Add(tc);

                for (int y = 0; y < lwo.Columns.Count; y++)
                {
                    tc = new TableCell();
                    tc.Text = lwo.Columns[y].ColumnName.Replace('_', ' '); ;
                    tr.Controls.Add(tc);
                }
                tblHistory.Rows.Add(tr);


                foreach (DataRow dr in lwo.Rows)
                {

                    tr = new TableRow();
                    tc = new TableCell();
                    tc.Text = (w--).ToString();
                    tr.Controls.Add(tc);

                    for (int i = 0; i < lwo.Columns.Count; i++)
                    {
                        tc = new TableCell();
                        if (lwo.Columns[i].DataType == System.Type.GetType("System.DateTime"))
                        {
                            tc.Text = ((DateTime)dr[i]).ToString("f");
                        }
                        else
                        {
                            tc.Text = dr[i].ToString();
                        }
                        tr.Controls.Add(tc);

                    }
                    tblHistory.Rows.Add(tr);
                }
            }
        }
    }
}