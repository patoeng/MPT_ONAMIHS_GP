using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;
using TPM.Classes;

namespace TPM
{
    public partial class SparePartUsage : System.Web.UI.Page
    {
        public string tanggal = "";
   
        private string _sn;
        private string _sp;
        private string _dp;
        private string _ut;
        private string _sd;
        private string _ed;
        private string _id;
        public string m;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _sn = Request.QueryString["sn"] ?? "";
                _sp = Request.QueryString["sp"] ?? "";
                _dp = Request.QueryString["dp"] ?? "";
                _ut = Request.QueryString["ut"] ?? "";
                _sd = Request.QueryString["sd"] ?? "";
                _ed = Request.QueryString["ed"] ?? "";
                _id = Request.QueryString["id"] ?? "";
                m = Request.QueryString["m"] ?? "";
                prepare();
            }
            
        }
        protected void prepare()
        {
           
            DataSet ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MDepartmentsSelect");
            Department.Items.Add(new ListItem("ALL", "0"));
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Department.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString()));
                }
            }
            
            usageType.Items.Add(new ListItem("ALL", "0"));
            usageType.Items.Add(new ListItem("Preventive Maintenance", "1"));
            usageType.Items.Add(new ListItem("Work Order / Break Down", "2"));
            var d = DateTime.Now;

            var y = d.Year;
            tanggal = "1-" + d.ToString("MMM") + "-" + y.ToString(CultureInfo.InvariantCulture);
            startdate .Value = tanggal;
            if (m != "")
            {
                var inPm = "";
                var inWo = "";
                startdate.Value = _sd;
                enddate.Value = _ed;
                usageType.SelectedValue = _ut;
                Department.SelectedValue = _dp;
                id2.Value = _id;
                serialcode.Value = _sn;
                sparepartcode.Value = _sp;

                _sd = (_sd != string.Empty) ? Convert.ToDateTime(_sd).ToString("yyyy-MM-dd") : string.Empty;
                _ed = (_ed != string.Empty) ? Convert.ToDateTime(_ed).ToString("yyyy-MM-dd") : string.Empty;
                string sql =
                    "select A.id,A.product_code,b.name as description,IFNULL(A.qty,'0')as qty,IFNULL(A.category,'0') as category,A.reference,A.remarks,A.updated_by,A.updated_at,IFNULL(c.price,'0') As Price,'SGD' AS currency from stockbalance A ";
                sql += "Left Join product B on b.code = a.product_code ";
                sql += "left Join product_supplier C on c.product_code = b.code ";
                sql += "where A.status='WITHDRAW'  ";
                sql += (_sd != string.Empty) ? "AND A.updated_at>='" + _sd + "' " : string.Empty;
                sql += (_ed != string.Empty) ? "AND A.updated_at<'" + _ed + "' " : string.Empty;
                sql += (_sp != string.Empty) ? "AND A.product_code like '%" + _sp + "___'" : string.Empty;
                sql += (_ut != "0") ? "AND A.category ='" + _ut + "'" : string.Empty;
                sql += (_id != string.Empty) ? "AND A.reference like '%" + _id + "%' " : string.Empty;
                sql += "AND (A.category='1' OR A.category='2') ";
                ds = MySqlHelper.ExecuteDataset(TPMHelper.DBINVENTORYstring, sql);
                var dt = ds.Tables[0];

                var sparePartUsage = new Dictionary<string, List<UsageSummary>>();
                foreach (DataRow dr in dt.Rows)
                {
                    var key = dr["category"].ToString() + dr["reference"];
                    var values = new UsageSummary
                    {
                        SparePartCode = dr["product_code"].ToString(),
                        Quantity = Convert.ToInt32(dr["qty"]),
                        UsageType = Convert.ToInt32(dr["category"]),
                        Id = dr["reference"].ToString(),
                        Reason = dr["remarks"].ToString(),
                        Date = dr["updated_at"].ToString(),
                        Price = Convert.ToSingle(dr["price"]),
                        DoneBy = dr["updated_by"].ToString(),
                        Currency = dr["currency"].ToString(),
                        UsageName =
                            dr["category"].ToString() == "1" ? "Preventive Maintenance" : "Work Order/Break Down",
                        SparePartName = dr["description"].ToString()
                    };
                    if (sparePartUsage.ContainsKey(key) == false)
                    {
                        sparePartUsage.Add(key, new List<UsageSummary>());
                    }
                    sparePartUsage[key].Add(values);

                    if (dr["category"].ToString() == "1")
                    {
                        inPm += "'" + dr["reference"] + "',";
                    }
                    else
                    {
                        inWo += "'" + dr["reference"] + "',";
                    }
                }
                inWo = (inWo == string.Empty) ? string.Empty : "in (" + inWo.TrimEnd(',') + ")";
                inPm = (inPm == string.Empty) ? string.Empty : "in (" + inPm.TrimEnd(',') + ")";
                if (inWo != string.Empty)
                {
                    var SqlWO = "Select A.WorkorderKey, B.Serial_Number, B.Descriptions AS [MachineName], C.Descriptions As [Department] " +
                                "from MworkOrders A " +
                                "left join MAssets B " +
                                "On A.Asset_Id = B.id " +
                                "left join MDepartments C " +
                                "ON B.Department_Id = C.id " +
                                "Where ";
                    SqlWO += "A.WorkorderKey " + inWo + " ";
                    // SqlWO += (depid == string.Empty || depid == "0") ? string.Empty : "AND C.id='" + depid + "' ";
                    //SqlWO += (sn == string.Empty) ? string.Empty : "AND B.Serial_Number like '%"+sn+"%'";
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, SqlWO);
                    dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int c = 0; c < sparePartUsage["2" + dr["WorkorderKey"]].Count; c++)
                        {
                            sparePartUsage["2" + dr["WorkorderKey"]][c].Department = dr["Department"].ToString();
                            sparePartUsage["2" + dr["WorkorderKey"]][c].MachineSn = dr["Serial_Number"].ToString();
                            sparePartUsage["2" + dr["WorkorderKey"]][c].MachineName = dr["MachineName"].ToString();
                        }
                    }
                }

                if (inPm != string.Empty)
                {
                    var SqlPM = "Select A.PMID, B.Serial_Number, B.Descriptions AS [MachineName], C.Descriptions As [Department] " +
                                "from MPMSchedules A " +
                                "left join MAssets B " +
                                "On A.Asset_Id = B.id " +
                                "left join MDepartments C " +
                                "ON B.Department_Id = C.id " +
                                "Where ";
                    SqlPM += "A.PMID " + inPm + " ";
                    SqlPM += (_dp == string.Empty || _dp == "0") ? string.Empty : "AND C.id='" + _dp + "' ";
                    SqlPM += (_sn == string.Empty) ? string.Empty : "AND B.Serial_Number like '%" + _sn + "%'";
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, SqlPM);
                    dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int c = 0; c < sparePartUsage["1" + dr["PMID"]].Count; c++)
                        {
                            sparePartUsage["1" + dr["PMID"]][c].Department = dr["Department"].ToString();
                            sparePartUsage["1" + dr["PMID"]][c].MachineSn = dr["Serial_Number"].ToString();
                            sparePartUsage["1" + dr["PMID"]][c].MachineName = dr["MachineName"].ToString();
                        }

                    }
                }

                var tr = new TableRow { TableSection = TableRowSection.TableHeader };
                TableCell tc;
                var tbl = new Table
                {
                    ID = "jsonTable",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "table table-bordered table-striped"
                };
                var header = new List<string>
                {
                    "Usage Type",
                    "PMID /WO Number",
                    "Spare Part Code",
                    "Spare Part Name",
                    "Quantity",
                    "Amount Per Unit",
                    "Total Amount",
                    "Currency",
                    "Reason",
                    "Date",
                    "Done By",
                    "MC Serial Code",
                    "MC Name",
                    "Department"
                };
                foreach (string sss in header)
                {
                    tc = new TableHeaderCell { Text = sss };
                    tr.Cells.Add(tc);
                }
                tbl.Rows.Add(tr);


                foreach (string us in sparePartUsage.Keys)
                {
                    for (int c = 0; c < sparePartUsage[us].Count; c++)
                    {
                        tr = new TableRow();

                        tc = new TableCell { Text = sparePartUsage[us][c].UsageName };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].Id };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].SparePartCode };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].SparePartName };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].Quantity.ToString(CultureInfo.InvariantCulture) };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].Price.ToString(CultureInfo.InvariantCulture) };
                        tr.Cells.Add(tc);
                        tc = new TableCell
                        {
                            Text =
                                (sparePartUsage[us][c].Quantity * sparePartUsage[us][c].Price).ToString(CultureInfo.InvariantCulture)
                        };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].Currency };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].Reason };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].Date };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].DoneBy };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].MachineSn };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].MachineName };
                        tr.Cells.Add(tc);
                        tc = new TableCell { Text = sparePartUsage[us][c].Department };
                        tr.Cells.Add(tc);
                        tbl.Rows.Add(tr);
                    }
                }
               

                var htm = new HtmlGenericControl("h4") { InnerText = "Export To Excel" };
                htm.Attributes.Add("class", "btn btn-primary");
                tableContainer.Controls.Add(htm);
                htm = new HtmlGenericControl("hr");
                tableContainer.Controls.Add(htm);
                tableContainer.Controls.Add(tbl);
            }
        }
    }
}