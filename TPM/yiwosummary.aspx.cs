using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class yiwosummary : System.Web.UI.Page
    {
        public string Tanggal;
        private string _dp;
        private string _st;
        private string _rt;
        private string _sd;
        private string _ed;
        public string m;
        protected void Page_Load(object sender, EventArgs e)
        {
            _dp = Request.QueryString["dp"] ?? "";
            _st = Request.QueryString["st"] ?? "";
            _rt = Request.QueryString["rt"] ?? "";
            _sd = Request.QueryString["sd"] ?? "";
            _ed = Request.QueryString["ed"] ?? "";
            m = Request.QueryString["m"] ?? "";
            Prepare();
        }
        protected void Prepare(){

            DataSet ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MDepartmentsSelect");
            Department.Items.Add(new ListItem("ALL", "0"));
            if (ds.Tables.Count > 0) { 
              foreach (DataRow dr in ds.Tables[0].Rows){
                  Department.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString())); 
              }
            }
            ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, "select id,descriptions from iwostatus");
            status_id.Items.Add(new ListItem("ALL", "0"));
           /* if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    status_id.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString()));
                }
            }*/
            status_id.Items.Add(new ListItem("OPEN","110"));
            status_id.Items.Add(new ListItem("CLOSED","111"));
            status_id.Items.Add(new ListItem("CANCELED", "8"));
            var d = DateTime.Now;
            
            var y = d.Year;
            Tanggal = "1-" + d.ToString("MMM") + "-" + y.ToString(CultureInfo.InvariantCulture);
            ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, "select id,descriptions from iwotype");
            adhoc.Items.Add(new ListItem("ALL", "0"));
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    adhoc.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString()));
                }
            }
            startdate.Value = Tanggal;
            if (m != "")
            {
                adhoc.SelectedValue = _rt;
                startdate.Value = _sd;
                enddate.Value = _ed;
                status_id.SelectedValue = _st;
                Department.SelectedValue = _dp;
               var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startdate", _sd),
                    new SqlParameter("@todate", _ed),
                    new SqlParameter("@deptid", _dp),
                    new SqlParameter("@statusid", _st),
                    new SqlParameter("@req_type",_rt)
                };
            ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_summary_MIWorkOrdersSelect", sqlparams.ToArray());

                var tbl = new Table
                    {
                        ID = "jsonTable",
                        ClientIDMode = ClientIDMode.Static,
                        CssClass = "table table-bordered table-striped table-condensed"
                    };
                TableCell tc;
                var tr = new TableRow {TableSection = TableRowSection.TableHeader};
                var dt = ds.Tables[0];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    tc = new TableHeaderCell {Text = dt.Columns[i].ColumnName};
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
                                        ? ((DateTime) (dr[i])).ToString("d MMMM yyyy")
                                        : dr[i].ToString()
                            };

                        if ((dt.Columns[i].ColumnName == "REMARKS") || (dt.Columns[i].ColumnName == "CAUSES") || (dt.Columns[i].ColumnName == "REPORT"))
                        {
                            tc.Style.Add("white-space", "pre-line");
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
                htm = new HtmlGenericControl("h3");
                tableContainer.Controls.Add(htm);
                tableContainer.Controls.Add(tbl);
            }
        }
    }
}