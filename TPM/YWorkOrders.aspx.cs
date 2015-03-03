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
        public string Mwoid = "0";
        public TPMHelper F = new TPMHelper();
        public string Status = "";
    public MySessions session;
    protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack){
                if (!session.IsPublic)
                {
                    Mwoid = Request.QueryString["v"] != null ? Request.QueryString["v"].ToString() : "0";
                    Prepare();
                }
                else {
                    Server.Transfer(session.Redirection);
                }
            }
        }
        protected void Prepare()
        {
            var sql = new List<SqlParameter> {new SqlParameter("@mwoidkey", Mwoid)};

            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring,CommandType.StoredProcedure,"usp_getWorkOrder",sql.ToArray());
            var mwo = ds.Tables[0];
            var lwo = ds.Tables[1];
            var lwoId = "";
            var mwoId = "";
            HtmlGenericControl htm;

            TableRow tr;
            TableCell tc;
          
            foreach (DataRow dr in mwo.Rows) {
                Status =dr["status"].ToString();
                lwoId = dr["LWO_ID"].ToString();
                
                mwoId = dr["ID"].ToString();
                for (int i = 0; i < mwo.Columns.Count-2; i++) {
                    tr = new TableRow();
                    tc = new TableCell {Text = mwo.Columns[i].ColumnName.Replace('_', ' ')};
                  
                    tr.Controls.Add(tc);
                    tc = new TableCell();
                    if ((Status == "WORK STARTED") || (Status == "COMPLETED"))
                    {
                        if (mwo.Columns[i].ColumnName == "WORK REQUEST CURRENT")
                        {
                            tc.CssClass = "editable";
                            tc.Attributes.Add("id", dr["iD"] + "-result");
                        }
                        if (mwo.Columns[i].ColumnName == "WORK REQUEST TYPE")
                        {
                            tc.CssClass = "editable2";
                            tc.Attributes.Add("id", dr["iD"] + "-request_type");
                        }
                        if (mwo.Columns[i].ColumnName == "ACTION")
                        {
                            tc.CssClass = "text_editable";
                            tc.Attributes.Add("id", dr["iD"] + "-action");
                        }
                        if (mwo.Columns[i].ColumnName == "ACTION2")
                        {
                            tc.CssClass = "text_editable";
                            tc.Attributes.Add("id", dr["iD"] + "-action2");
                        }
                    }
                    if ((mwo.Columns[i].ColumnName == "ACTION")||(mwo.Columns[i].ColumnName == "ACTION2"))
                    {
                        tc.Style.Add("white-space", "pre-line");
                    }
                    if (mwo.Columns[i].ColumnName == "REMARKS")
                        {
                            if (Status != "CLOSED")
                            {
                                tc.CssClass = "text_editable";
                                tc.Attributes.Add("id", Mwoid + "-remarks2");
                            }
                            tc.Style.Add("white-space", "pre-line");
                    }
                    if (mwo.Columns[i].ColumnName == "CAUSES")
                    {
                      tc.Style.Add("white-space", "pre-line");
                    }
                    if (mwo.Columns[i].DataType == Type.GetType("System.DateTime"))
                    {
                        tc.Text = ((DateTime)dr[i]).ToString("f");
                    }
                    else
                    {
                        
                        if (mwo.Columns[i].ColumnName.ToLower() == "checklist_id")
                        {
                            if (dr[i].ToString() != string.Empty)
                            {
                                htm = new HtmlGenericControl("a");
                                htm.Attributes.Add("href", "/" + TPMHelper.WebDirectory + "/ychecklist.aspx?id=" + dr[i]);
                                htm.InnerHtml = dr[i].ToString();
                                tc.Controls.Add(htm);
                            }
                            else
                            {
                                tc.Text = "AdHoc Work Order";
                            }
                        }
                        else
                        {
                            tc.Text =dr[i].ToString();
                        }
                    }
                    tr.Controls.Add(tc);
                    tblLastStatus.Rows.Add(tr);
                }
            }

            var btntext = new List<string>();
            switch (Status){
                case"ISSUED":
                    if (session.IsEngineering){
                        btntext.Add("CONFIRM");
                    }
                    break;
                case "CONFIRMED": if (session.IsEngineering) { btntext.Add("START THE WORK"); } break;
                case "WORK STARTED": if (session.IsEngineering) { btntext.Add("FINISH THE WORK"); } break;
                case "COMPLETED": if ((session.IsLeader)) { btntext.Add("REVIEW"); } break;
            }
           
            if (btntext.Count > 0) {

                tr = new TableRow();
                tc = new TableCell {Text = "ACTION TO DO"};
                tr.Cells.Add(tc);
                tblLastStatus.Rows.Add(tr);
                tc = new TableCell();
                
                htm = new HtmlGenericControl("h4");
                htm.Attributes.Add("id", "button1");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.Attributes.Add("value",btntext[0]);
                htm.InnerHtml = btntext[0];
                tc.Controls.Add(htm);
              

                var hid = new HiddenField {ID = "mwoid", ClientIDMode = ClientIDMode.Static, Value = mwoId};
                tc.Controls.Add(hid);

                

                hid = new HiddenField {ID = "lwoid", ClientIDMode = ClientIDMode.Static, Value = lwoId};
                tc.Controls.Add(hid);

                tr.Cells.Add(tc);
                tblLastStatus.Rows.Add(tr);
               
            }
           
            int w = lwo.Rows.Count;
            if (w > 0)
            {
                tr = new TableRow {TableSection = TableRowSection.TableHeader};
                tc = new TableHeaderCell {Text = "STEP"};
                tr.Controls.Add(tc);

                for (int y = 0; y < lwo.Columns.Count-1; y++)
                {
                    tc = new TableHeaderCell {Text = lwo.Columns[y].ColumnName.Replace('_', ' ')};
                    tr.Controls.Add(tc);
                }
                tblHistory.Rows.Add(tr);


                foreach (DataRow dr in lwo.Rows)
                {

                    tr = new TableRow();
                    tc = new TableCell {Text = (w--).ToString()};
                    tr.Controls.Add(tc);

                    for (int i = 0; i < lwo.Columns.Count-1; i++)
                    {
                        tc = new TableCell
                            {
                                Text =
                                    lwo.Columns[i].DataType ==Type.GetType("System.DateTime")
                                        ? ((DateTime) dr[i]).ToString("f")
                                        : dr[i].ToString()
                            };

                        if (lwo.Columns[i].ColumnName=="REMARKS"){
                            if (Status != "CLOSED")
                            {
                                tc.CssClass = "text_editable";
                                tc.Attributes.Add("id", dr["iD"] + "-remarks");
                            }
                            tc.Style.Add("white-space", "pre-line");
                        }
                        tr.Controls.Add(tc);

                    }
                    tblHistory.Rows.Add(tr);
                }
            }
        }
    }
}