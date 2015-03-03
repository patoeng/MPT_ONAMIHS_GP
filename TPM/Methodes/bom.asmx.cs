using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;


namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for bom
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class bom : System.Web.Services.WebService
    {
        public TPMHelper Functions = new TPMHelper();
        [WebMethod]
        public string GetBOMInfobyasmod(string id)
        {
            var sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@Asset_Model_id",id));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MBomsSelect_byasmod", sqlparams.ToArray());
            DataTable dt = ds.Tables[0];
            var data = new List<List<string>>();
            var dat = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                dat = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dat.Add(dr[i].ToString());
                }
                data.Add(dat);
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }

        [WebMethod]
        public string GetInventory()
        {
            DataSet ds = MySqlHelper.ExecuteDataset(Functions.TInventoryConnection(), "select id,code,name,qty_min,qty_max,(select qty_current+qty_adjustment from apq where product_code=code) as qty_stock from product order by code");
            
            DataTable dt = ds.Tables[0];
            var data = new List<List<string>>();
            var dat = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                dat = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dat.Add(dr[i].ToString());
                }
                data.Add(dat);
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        public string GetInventory2(string connector)
        {
            DataSet ds = MySqlHelper.ExecuteDataset(Functions.TInventoryConnection(), 
                "select a.id,code,name,qty_min,qty_max,(b.qty_current+b.qty_adjustment)  as qty_stock,c.price as 'price per unit', 'SGD' as 'Currency' " +
                "from product a " +
                "left join apq b on a.code=b.product_code "+
                "left join product_supplier c on a.code=b.product_code "+
                "where code like '%"+connector+"___' order by code "
                );

            DataTable dt = ds.Tables[0];
            var data = new List<List<string>>();
            foreach (DataRow dr in dt.Rows)
            {
                var dat = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dat.Add(dr[i].ToString());
                }
                data.Add(dat);
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }

        [WebMethod]
        public string GetBOMInfobyasmod2(string id)
        {
            var sqlparams = new List<SqlParameter> {new SqlParameter("@Asset_Model_id", id)};
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MBomsSelect_byasmod", sqlparams.ToArray());
            DataTable dt = ds.Tables[0] ?? new DataTable();
            

            var data = new List<List<string>>();
            var dc = new Dictionary<string, List<string>>();

            foreach (DataRow dr in dt.Rows)
            {
                var dat = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dat.Add(dr[i].ToString());
                }
                dc.Add(dr[0].ToString(), dat);
                data.Add(dat);
            }
            if (data.Count > 0) {
                
                string where = data.Where(ls => ls[0] != "").Aggregate(" IN (", (current, ls) => current + ("'" + ls[0] + "',"));
                where = where.Remove(where.Length-1, 1);
                where += " )";
                System.Diagnostics.Debug.WriteLine("sss"+where);
                ds = MySqlHelper.ExecuteDataset(Functions.TInventoryConnection(), "select code,qty_min,(select qty_current+qty_adjustment from apq where product_code=code) as qty_stock from product where code " + where + "");
                
                DataTable dts = ds.Tables[0];
                foreach (DataRow dr in dts.Rows) { 
                    dc[dr["code"].ToString()].Add(dr["qty_stock"].ToString());
                    dc[dr["code"].ToString()].Add(dr["qty_min"].ToString());
                }
                data = dc.Keys.Select(se => dc[se]).ToList();
            }
            
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod]
        public string action(string asmod, string act, string code, string desc, string qty)
        {
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@Asset_Model_id", asmod),
                    new SqlParameter("@Descriptions", desc),
                    new SqlParameter("@Minimum_QTY_For_PM", qty),
                    new SqlParameter("@inventory_code", code)
                };

            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MBoms"+act+"_ymboms", sqlparams.ToArray());
            var data = new List<string>();
            if (act.ToUpper() != "DELETE")
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    data = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data.Add(dr[i].ToString());
                    }

                }
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod (EnableSession=true)]
        public string action2(string act, string code, string qty, string pmid,string reason,string name,string price, string currency)
        {
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@pmid", pmid),
                    new SqlParameter("@qty", qty),
                    new SqlParameter("@inventory_code", code),
                    new SqlParameter("@reason", reason),
                    new SqlParameter("@done_by", new MySessions().EmployeeName),
                    new SqlParameter("@inventory_name", name),
                    new SqlParameter("@price_per_unit", price),
                    new SqlParameter("@currency", currency)
                };
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_LPMBoms" + act , sqlparams.ToArray());
            var data = new List<string>();
            if (act.ToUpper() != "DELETE")
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    data = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data.Add(dr[i].ToString());
                    }

                }
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        
        [WebMethod(EnableSession = true)]
        public string action3(string act, string code, string qty, string woid, string reason, string name,string price, string currency)
        {
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@woid", woid),
                    new SqlParameter("@qty", qty),
                    new SqlParameter("@inventory_code", code),
                    new SqlParameter("@reason", reason),
                    new SqlParameter("@done_by", new MySessions().EmployeeName),
                    new SqlParameter("@inventory_name", name),
                    new SqlParameter("@price_per_unit", price),
                    new SqlParameter("@currency", currency)
                };
           
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_LWOBoms" + act, sqlparams.ToArray());
            var data = new List<string>();
            if (act.ToUpper() != "DELETE")
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    data = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data.Add(dr[i].ToString());
                    }

                }
            }
            var json = new JavaScriptSerializer();
            string s = json.Serialize(data);
            return s;
        }
        [WebMethod(EnableSession = true)]
        public string Usage(string sn, string spc, string depid, string utype, string fdate, string tdate, string id)
        {
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@serialcode", sn),
                    new SqlParameter("@sparepartcode", spc),
                    new SqlParameter("@departmentid", depid),
                    new SqlParameter("@usagetype", utype),
                    new SqlParameter("@startdate", fdate),
                    new SqlParameter("@todate", tdate),
                    new SqlParameter("@id", id)
                };
            var ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_summary_BOMusage", sqlparams.ToArray());
            var tr = new TableRow { TableSection = TableRowSection.TableHeader };
            var dt = ds.Tables[0];
            TableCell tc;
            var tbl = new Table
            {
                ID = "jsonTable",
                ClientIDMode = ClientIDMode.Static,
                CssClass = "table table-bordered table-striped"
            };
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
                            dt.Columns[i].DataType == Type.GetType("System.DateTime")
                                ? ((DateTime)(dr[i])).ToString("d MMMM yyyy")
                                : dr[i].ToString()
                    };
                    tr.Cells.Add(tc);
                }
                tbl.Rows.Add(tr);
            }
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            tbl.RenderControl(htw);
            var s = sw.ToString();
            var htm = new HtmlGenericControl("h4") {InnerText = "Export To Excel"};
            htm.Attributes.Add("class","btn btn-primary");
            sw = new StringWriter();
            htw = new HtmlTextWriter(sw);
            htm.RenderControl(htw);
            s = sw +"<hr/>" +s;
            return s;
           
        }
        [WebMethod(EnableSession = true)]
        public string UsageDownload(string sn, string spc, string depid, string utype, string fdate, string tdate, string id)
        {
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@serialcode", sn),
                    new SqlParameter("@sparepartcode", spc),
                    new SqlParameter("@departmentid", depid),
                    new SqlParameter("@usagetype", utype),
                    new SqlParameter("@startdate", fdate),
                    new SqlParameter("@todate", tdate),
                    new SqlParameter("@id", id)
                };
            var ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_summary_BOMusage", sqlparams.ToArray());
            var t = DateTime.Now;
            var xlsx = "Spare_Part_Usage_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
            var fileName = Server.MapPath("~/UploadedFiles/" + xlsx);
            TPMHelper.CreateXlsx(ds,fileName , "Spare_Part_Usage");
            return "./UploadedFiles/" + xlsx;

        }
        [WebMethod(EnableSession = true)]
        public string UsageDownload2(string sn, string spc, string depid, string utype, string fdate, string tdate, string id)
        {
            var InPM = "";
            var InWO = "";
            fdate = (fdate != string.Empty) ? Convert.ToDateTime(fdate).ToString("yyyy-MM-dd") : string.Empty;
            tdate = (tdate != string.Empty) ? Convert.ToDateTime(tdate).ToString("yyyy-MM-dd") : string.Empty;
            string sql =
                "select A.id,A.product_code,b.name as description,IFNULL(A.qty,'0')as qty,IFNULL(A.category,'0') as category,A.reference,A.remarks,A.updated_by,A.updated_at,IFNULL(c.price,'0') As Price,'SGD' AS currency from stockbalance A ";
            sql += "Left Join product B on b.code = a.product_code ";
            sql += "left Join product_supplier C on c.product_code = b.code ";
            sql += "where A.status='WITHDRAW'  ";
            sql += (fdate != string.Empty) ? "AND A.updated_at>='" + fdate + "' " : string.Empty;
            sql += (tdate != string.Empty) ? "AND A.updated_at<'" + tdate + "' " : string.Empty;
            sql += (spc != string.Empty) ? "AND A.product_code like '%" + spc + "___'" : string.Empty;
            sql += (utype != "0") ? "AND A.category ='" + utype + "'" : string.Empty;
            sql += (id != string.Empty) ? "AND A.reference like '%" + id + "%' " : string.Empty;
            sql += "AND (A.category='1' OR A.category='2') ";
            DataSet ds = MySqlHelper.ExecuteDataset(TPMHelper.DBINVENTORYstring, sql);
            DataTable dt = ds.Tables[0];

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
                    InPM += "'" + dr["reference"] + "',";
                }
                else
                {
                    InWO += "'" + dr["reference"] + "',";
                }
            }
            InWO = (InWO == string.Empty) ? string.Empty : "in (" + InWO.TrimEnd(',') + ")";
            InPM = (InPM == string.Empty) ? string.Empty : "in (" + InPM.TrimEnd(',') + ")";
            if (InWO != string.Empty)
            {
                var SqlWO = "Select A.WorkorderKey, B.Serial_Number, B.Descriptions AS [MachineName], C.Descriptions As [Department] " +
                            "from MworkOrders A " +
                            "left join MAssets B " +
                            "On A.Asset_Id = B.id " +
                            "left join MDepartments C " +
                            "ON B.Department_Id = C.id " +
                            "Where ";
                SqlWO += "A.WorkorderKey " + InWO + " ";
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

            if (InPM != string.Empty)
            {
                var SqlPM = "Select A.PMID, B.Serial_Number, B.Descriptions AS [MachineName], C.Descriptions As [Department] " +
                            "from MPMSchedules A " +
                            "left join MAssets B " +
                            "On A.Asset_Id = B.id " +
                            "left join MDepartments C " +
                            "ON B.Department_Id = C.id " +
                            "Where ";
                SqlPM += "A.PMID " + InPM + " ";
                SqlPM += (depid == string.Empty || depid == "0") ? string.Empty : "AND C.id='" + depid + "' ";
                SqlPM += (sn == string.Empty) ? string.Empty : "AND B.Serial_Number like '%" + sn + "%'";
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
            DateTime t = DateTime.Now;
            string xlsx = "Spare_Part_Usage_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
            string fileName = Server.MapPath("~/UploadedFiles/" + xlsx);
            TPMHelper.CreateXlsx(sparePartUsage,header, fileName, "Spare_Part_Usage");
            return "./UploadedFiles/" + xlsx;

        }
        
        [WebMethod(EnableSession = true)]
        public string Usage2(string sn, string spc, string depid, string utype, string fdate, string tdate, string id)
        {
            var InPM = "";
            var InWO = "";
            fdate = (fdate != string.Empty) ? Convert.ToDateTime(fdate).ToString("yyyy-MM-dd") : string.Empty;
            tdate = (tdate != string.Empty) ? Convert.ToDateTime(tdate).ToString("yyyy-MM-dd") : string.Empty;
            string sql =
                "select A.id,A.product_code,b.name as description,IFNULL(A.qty,'0')as qty,IFNULL(A.category,'0') as category,A.reference,A.remarks,A.updated_by,A.updated_at,IFNULL(c.price,'0') As Price,'SGD' AS currency from stockbalance A ";
            sql += "Left Join product B on b.code = a.product_code ";
            sql += "left Join product_supplier C on c.product_code = b.code ";
            sql += "where A.status='WITHDRAW'  ";
            sql += (fdate != string.Empty) ? "AND A.updated_at>='" + fdate + "' " : string.Empty;
            sql += (tdate != string.Empty) ? "AND A.updated_at<'" + tdate + "' " : string.Empty;
            sql += (spc != string.Empty) ? "AND A.product_code like '%" + spc + "___'" : string.Empty;
            sql += (utype != "0") ? "AND A.category ='" + utype + "'" : string.Empty;
            sql += (id != string.Empty) ? "AND A.reference like '%" + id + "%' " : string.Empty;
            sql += "AND (A.category='1' OR A.category='2') ";
            DataSet ds = MySqlHelper.ExecuteDataset(TPMHelper.DBINVENTORYstring, sql);
            DataTable dt = ds.Tables[0];

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
                if (sparePartUsage.ContainsKey(key)==false)
                {
                   sparePartUsage.Add(key,new List<UsageSummary>());  
                }
                sparePartUsage[key].Add(values);
                
                if (dr["category"].ToString() == "1")
                {
                    InPM += "'" + dr["reference"] + "',";
                }
                else
                {
                    InWO += "'" + dr["reference"] + "',";
                }
            }
            InWO = (InWO == string.Empty) ? string.Empty : "in (" + InWO.TrimEnd(',') + ")";
            InPM = (InPM == string.Empty) ? string.Empty : "in (" + InPM.TrimEnd(',') + ")";
            if (InWO != string.Empty)
            {
                var SqlWO = "Select A.WorkorderKey, B.Serial_Number, B.Descriptions AS [MachineName], C.Descriptions As [Department] " +
                            "from MworkOrders A " +
                            "left join MAssets B " +
                            "On A.Asset_Id = B.id " +
                            "left join MDepartments C " +
                            "ON B.Department_Id = C.id " +
                            "Where ";
                SqlWO += "A.WorkorderKey "+InWO + " ";
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
           
            if (InPM != string.Empty)
            {
                var SqlPM = "Select A.PMID, B.Serial_Number, B.Descriptions AS [MachineName], C.Descriptions As [Department] " +
                            "from MPMSchedules A " +
                            "left join MAssets B " +
                            "On A.Asset_Id = B.id " +
                            "left join MDepartments C " +
                            "ON B.Department_Id = C.id " +
                            "Where ";
                SqlPM += "A.PMID "+InPM + " ";
               SqlPM += (depid == string.Empty || depid=="0") ? string.Empty : "AND C.id='" + depid + "' ";
                SqlPM += (sn == string.Empty) ? string.Empty : "AND B.Serial_Number like '%" + sn + "%'";
                ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text, SqlPM);
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    for (int c = 0; c < sparePartUsage["1" + dr["PMID"]].Count;c++ )
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

                    tc = new TableCell {Text = sparePartUsage[us][c].UsageName};
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
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            tbl.RenderControl(htw);
            string s = sw.ToString();
            var htm = new HtmlGenericControl("h4") { InnerText = "Export To Excel" };
            htm.Attributes.Add("class", "btn btn-primary");
            sw = new StringWriter();
            htw = new HtmlTextWriter(sw);
            htm.RenderControl(htw);
            s = sw + "<hr/>"+ s;
            return s;

        }
    }
}
