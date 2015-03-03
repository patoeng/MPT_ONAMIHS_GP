using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string Tanggal = "";
        private string _bi;
        private string _bc;
        private string _on;
        private string _od;
        private string _sg;
        private string _sd;
        private string _ed;
        public string m;

        protected void Page_Load(object sender, EventArgs e)
        {
            _bi = Request.QueryString["bi"] ?? "";
            _bc = Request.QueryString["bc"] ?? "";
            _on = Request.QueryString["on"] ?? "";
            _od = Request.QueryString["od"] ?? "";
            _sg = Request.QueryString["sg"] ?? "";
            _sd = Request.QueryString["sd"] ?? "";
            _ed = Request.QueryString["ed"] ?? "";
            m = Request.QueryString["m"] ?? "";
            prepare();
        }

        protected void prepare()
        {
            OvenID.Items.Add(new ListItem("ALL", ""));
            OvenID.Items.Add(new ListItem("Oven#1", "Oven#1"));
            OvenID.Items.Add(new ListItem("Oven#2", "Oven#2"));
            OvenID.Items.Add(new ListItem("Oven#3", "Oven#3"));
            OvenID.Items.Add(new ListItem("Oven#4", "Oven#4"));

            stage.Items.Add(new ListItem("ALL", ""));
            stage.Items.Add(new ListItem("FINISH", "FIN"));
            stage.Items.Add(new ListItem("READY", "READY"));
            stage.Items.Add(new ListItem("RUNNING", "RUN"));
            var d = DateTime.Now;
            var y = d.Year;
            Tanggal = "1-" + d.ToString("MMM") + "-" + y.ToString(CultureInfo.InvariantCulture);
            startdate.Value = Tanggal;
            if (m != "")
            {
                if (_bi != null) _bi = _bi.Trim();
                if (_bc != null) _bc = _bc.Trim();
                if (_on != null) _on = _on.Trim();
                startdate.Value = _sd;
                enddate.Value = _ed;
                order_number.Value = _on;
                OvenID.SelectedValue = _od;
                stage.SelectedValue = _sg;
                bm1code.Value = _bc;
                batchid.Value = _bi;
                /*
                 * @ovenID	nvarchar(50)=NULL,
		            @batchID nvarchar(50)=NULL,
		            @order_number nvarchar(50)=NULL,
		            @bp1code nvarchar(50)=NULL,
		            @startdate datetime=NULL,
		            @todate datetime=NULL,
		            @ovenstatus nvarchar(10)=NULL
                 */
                var sqlparams = new List<SqlParameter>
                    {
                        new SqlParameter("@OvenID", _od),
                        new SqlParameter("@batchid", _bi),
                        new SqlParameter("@order_number", _on),
                        new SqlParameter("@bm1code", _bc),
                        new SqlParameter("@startdate", _sd),
                        new SqlParameter("@todate", _ed),
                        new SqlParameter("@ovenstatus", _sg)
                    };
                var ds = SqlHelper.ExecuteDataset(TPMHelper.DBBMHTstring, CommandType.StoredProcedure, "searchAction",
                                                  sqlparams.ToArray());

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
                                        ? (i == 4 || i == 6
                                               ? ((DateTime) (dr[i])).ToString("HH:mm:ss")
                                               : ((DateTime) (dr[i])).ToString("d MMMM yyyy"))
                                        : dr[i].ToString()
                            };


                        if ((dt.Columns[i].ColumnName == "REMARKS") || (dt.Columns[i].ColumnName == "CAUSES"))
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
                htm = new HtmlGenericControl("hr");

                tableContainer.Controls.Add(htm);
                tableContainer.Controls.Add(tbl);




            }
        }
    }
}