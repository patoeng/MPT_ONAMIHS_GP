using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class YIWorkOrders : System.Web.UI.Page
    {
        public string iwoid = "0";
        public TPMHelper F = new TPMHelper();
        public string Status = "";
        public MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                if (!session.IsPublic)
                {
                    iwoid = Request.QueryString["v"] ?? "0";
                    Prepare();
                }
                else
                {
                    Server.Transfer(session.Redirection);
                }
            }
        }
        protected void Prepare()
        {
            var sql = new List<SqlParameter> { new SqlParameter("@iwoidkey", iwoid) };

            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_getIWorkOrder", sql.ToArray());
            var mwo = ds.Tables[0];
            var lwo = ds.Tables[1];
            var lwoId = "";
            var mwoId = "";
            HtmlGenericControl htm;

            TableRow tr;
            TableCell tc;

            foreach (DataRow dr in mwo.Rows)
            {
                Status = dr["status"].ToString();
                lwoId = dr["LWO_ID"].ToString();

                mwoId = dr["ID"].ToString();
                for (int i = 0; i < mwo.Columns.Count - 2; i++)
                {
                    tr = new TableRow();
                    tc = new TableCell { Text = mwo.Columns[i].ColumnName.Replace('_', ' ') };

                    tr.Controls.Add(tc);
                    tc = new TableCell();
                    if (true)//(Status == "WORK STARTED") || (Status == "COMPLETED"))
                    {
                        if (mwo.Columns[i].ColumnName == "COST IMPLEMENTATION (IDR)")
                        {
                            if (session.IsLeader && Status == "ISSUED")
                            {
                                tc.CssClass = "editable_cost cost";
                            }
                            else
                            {
                                tc.CssClass = "cost"; 
                            }
                        
                        tc.Attributes.Add("id", dr["iD"] + "-Cost_Implementation");
                        }
                        if (mwo.Columns[i].ColumnName == "REQUEST TYPE")
                        {
                            //tc.CssClass = "editable2";
                            tc.Attributes.Add("id", dr["iD"] + "-request_type");
                        }
                        if (mwo.Columns[i].ColumnName == "REQUEST")
                        {
                            //tc.CssClass = "text_editable";
                            tc.Attributes.Add("id", dr["iD"] + "-request");
                        }
                        if (mwo.Columns[i].ColumnName == "CAUSES")
                        {
                           // tc.CssClass = "text_editable";
                            tc.Attributes.Add("id", dr["iD"] + "-causes");
                        }
                        if (mwo.Columns[i].ColumnName == "REPORT")
                        {
                            if (session.IsLeader && Status == "WORK STARTED")
                            {
                                tc.CssClass = "text_editable";
                            }
                            tc.Attributes.Add("id", dr["iD"] + "-report");
                        }
                    }
                    if ((mwo.Columns[i].ColumnName == "REPORT") || (mwo.Columns[i].ColumnName == "REQUEST") || (mwo.Columns[i].ColumnName == "CAUSES"))
                    {
                        tc.Style.Add("white-space", "pre-line");
                    }
                    if (mwo.Columns[i].ColumnName == "REMARKS")
                    {
                        if (!(Status == "CLOSED" ||Status == "CANCELED" ))
                        {
                            tc.CssClass = "text_editable";
                            tc.Attributes.Add("id", iwoid + "-remarks2");
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
                            tc.Text = dr[i].ToString();
                        }
                    }
                    tr.Controls.Add(tc);
                    tblLastStatus.Rows.Add(tr);
                }
            }

            var btntext = new List<string>();
            switch (Status)
            {
                case "ISSUED":if (session.IsEngineering){btntext.Add("COST UPDATE");}break;
                case "COST UPDATED": if (session.IsLeader) { btntext.Add("PRODUCTION SPV CONFIRM"); } break;
                case "PROD CONFIRMED": if (session.IsEngineering) {btntext.Add("ENGINEERING SPV CONFIRM"); } break;
                case "ENG CONFIRMED": if (session.IsEngineering) { btntext.Add("START THE WORK"); } break;
                case "WORK STARTED": if (session.IsEngineering) { btntext.Add("FINISH THE WORK"); } break;
                case "WORK FINISHED": if (session.IsLeader) { btntext.Add("REVIEW"); } break;
               // case "APPROVED":  { btntext.Add("REVIEW"); } break;
            }

            if (btntext.Count > 0)
            {

                tr = new TableRow();
                tc = new TableCell { Text = "ACTION TO DO" };
                tr.Cells.Add(tc);
                tblLastStatus.Rows.Add(tr);
                tc = new TableCell();

                htm = new HtmlGenericControl("h4");
                htm.Attributes.Add("id", "button_1");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.Attributes.Add("value", btntext[0]);
                htm.InnerHtml = btntext[0];
                tc.Controls.Add(htm);

                if ((Status == "COST UPDATED" && session.IsLeader) || (Status == "PROD CONFIRMED" && session.IsEngineering))
                {
                    htm = new HtmlGenericControl("span") {InnerHtml = "&nbsp;"};
                    tc.Controls.Add(htm);
                    htm = new HtmlGenericControl("h4");
                    htm.Attributes.Add("id", "button_2");
                    htm.Attributes.Add("class", "btn btn-primary");
                    htm.Attributes.Add("value", "CANCEL");
                    htm.InnerHtml = "CANCEL";
                    tc.Controls.Add(htm);
                }

                var hid = new HiddenField { ID = "mwoid", ClientIDMode = ClientIDMode.Static, Value = mwoId };
                tc.Controls.Add(hid);



                hid = new HiddenField { ID = "lwoid", ClientIDMode = ClientIDMode.Static, Value = lwoId };
                tc.Controls.Add(hid);

                tr.Cells.Add(tc);
                tblLastStatus.Rows.Add(tr);

            }

            int w = lwo.Rows.Count;
            if (w > 0)
            {
                tr = new TableRow { TableSection = TableRowSection.TableHeader };
                tc = new TableHeaderCell { Text = "STEP" };
                tr.Controls.Add(tc);

                for (int y = 0; y < lwo.Columns.Count - 1; y++)
                {
                    tc = new TableHeaderCell { Text = lwo.Columns[y].ColumnName.Replace('_', ' ') };
                    tr.Controls.Add(tc);
                }
                tblHistory.Rows.Add(tr);


                foreach (DataRow dr in lwo.Rows)
                {

                    tr = new TableRow();
                    tc = new TableCell { Text = (w--).ToString() };
                    tr.Controls.Add(tc);

                    for (int i = 0; i < lwo.Columns.Count - 1; i++)
                    {
                        tc = new TableCell
                        {
                            Text =
                                lwo.Columns[i].DataType == Type.GetType("System.DateTime")
                                    ? ((DateTime)dr[i]).ToString("f")
                                    : dr[i].ToString()
                        };

                        if (lwo.Columns[i].ColumnName == "REMARKS")
                        {
                            if (!(Status == "CLOSED" || Status == "CANCELED"))
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