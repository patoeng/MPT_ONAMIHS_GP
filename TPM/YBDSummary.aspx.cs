using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using TPM.Classes;

namespace TPM
{
    public partial class YSummary : System.Web.UI.Page
    {
        public string tanggal;
        private string _sn;
        private string _dp;
        private string _st;
        private string _sd;
        private string _ed;
        private string _wo;
        private string _sf;
        public string m;
        protected void Page_Load(object sender, EventArgs e)
        {
            _sn = Request.QueryString["sn"] ?? "";
            _dp = Request.QueryString["dp"] ?? "";
            _st = Request.QueryString["st"] ?? "";
            _sd = Request.QueryString["sd"] ?? "";
            _ed = Request.QueryString["ed"] ?? "";
            _wo = Request.QueryString["wo"] ?? "";
            _sf= Request.QueryString["sf"] ?? "";
            m = Request.QueryString["m"] ?? "";
            prepare();
        }
        protected void prepare(){

            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MDepartmentsSelect");
            Department.Items.Add(new ListItem("ALL", "0"));
            if (ds.Tables.Count > 0) { 
              foreach (DataRow dr in ds.Tables[0].Rows){
                  Department.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString())); 
              }
            }
            //ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, "select id,descriptions from mwostatus");
            status_id.Items.Add(new ListItem("ALL", "0"));
           /* if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    status_id.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString()));
                }
            }*/
            status_id.Items.Add(new ListItem("OPEN", "110"));
            status_id.Items.Add(new ListItem("CLOSED", "111"));
            var d = DateTime.Now;
            
            var y = d.Year;
            tanggal = "1-" + d.ToString("MMM") + "-" + y.ToString();
            
            adhoc.Items.Add(new ListItem("ALL",""));
            adhoc.Items[0].Selected = true;
            adhoc.Items.Add(new ListItem("ADHOC", "1"));
            adhoc.Items.Add(new ListItem("CHECKLIST", "0"));

            safety.Items.Add(new ListItem("ALL", ""));
            safety.Items.Add(new ListItem("Safety","1"));
            safety.Items[0].Selected = true;

            startdate.Value = tanggal;
            if (m != "")
            {
                serialcode.Value = _sn;
                status_id.SelectedValue = _st;
                Department.SelectedValue = _dp;
                startdate.Value = _sd;
                enddate.Value = _ed;
                safety.SelectedValue = _sf;
                var sqlparams = new List<SqlParameter>
                    {
                        new SqlParameter("@startdate", _sd),
                        new SqlParameter("@todate", _ed),
                        new SqlParameter("@deptid", _dp),
                        new SqlParameter("@statusid", _st),
                        new SqlParameter("@serialcode", _sn),
                        new SqlParameter("@adhoc",_wo),
                        new SqlParameter("@safety",_sf)
                    };
                ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                              "usp_summary_MWorkOrdersSelect", sqlparams.ToArray());


                var tbl = new Table
                {
                    ID = "jsonTable",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "table table-bordered table-striped table-condensed"
                };
                TableCell tc;
                var tr = new TableRow { TableSection = TableRowSection.TableHeader };
                var dt = ds.Tables[0];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    tc = new TableHeaderCell { Text = dt.Columns[i].ColumnName };
                    tr.Cells.Add(tc);
                }
                tbl.Rows.Add(tr);
                foreach (DataRow dr in dt.Rows)
                {

                    tr = new TableRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        tc = new TableCell
                        {
                            Text =
                                dt.Columns[i].DataType == Type.GetType("System.DateTime") &&
                                (dr[i].ToString() != "")
                                    ? ((DateTime)(dr[i])).ToString("d MMMM yyyy")
                                    : dr[i].ToString()
                        };

                        if ((dt.Columns[i].ColumnName == "REMARKS") || (dt.Columns[i].ColumnName == "CAUSES"))
                        {
                            tc.Style.Add("white-space", "pre-line");
                        }
                        if ((dt.Columns[i].ColumnName == "RESULT"))
                        {
                            tc.Text = dr[i].ToString() != ""
                                          ? (dr[i].ToString().ToUpper() == "TRUE" ? "PASS" : "FAIL")
                                          : "";
                        }
                        tr.Cells.Add(tc);
                    }
                    tbl.Rows.Add(tr);
                }





                var htm = new HtmlGenericControl("h3");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.Attributes.Add("id", "download");
                htm.InnerText = "Export To Excel";
                tableContainer.Controls.Add(htm);
                htm = new HtmlGenericControl("hr");

                tableContainer.Controls.Add(htm);
                tableContainer.Controls.Add(tbl); 
            }
        }
    }
}