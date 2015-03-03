using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;


namespace TPM
{
    public partial class YPMMaster : System.Web.UI.Page
    {
        TPMHelper Functions = new TPMHelper();
        public int depid = 1;
        public int mon;
        public int year;
        public string serialnumber;
        public DateTime saiki = DateTime.Now;
        public int checktype = 2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                depid = (Request.QueryString["dept"] == "") || (Request.QueryString["dept"] == null) ? 2 : Convert.ToInt32(Request.QueryString["dept"]);
                mon = (Request.QueryString["mon"] == "") || (Request.QueryString["mon"] == null) ? DateTime.Now.Month : Convert.ToInt32(Request.QueryString["mon"]);
                year = (Request.QueryString["year"] == "") || (Request.QueryString["year"] == null) ? DateTime.Now.Year : Convert.ToInt32(Request.QueryString["year"]);
                checktype = (Request.QueryString["ty"] == "") || (Request.QueryString["ty"] == null) ? 2 : Convert.ToInt32(Request.QueryString["ty"]);
                serialnumber = (Request.QueryString["sn"] == "") || (Request.QueryString["sn"] == null) ? string.Empty : Request.QueryString["sn"];
                if (!DateTime.TryParse(year.ToString(CultureInfo.InvariantCulture) + "-" + mon + "-01", out saiki))
                {
                    saiki = DateTime.Now;
                }
                prepareTable();
                ddlDepartment.Items.FindByValue(depid.ToString(CultureInfo.InvariantCulture)).Selected = true;
                ddlYear.Items.FindByValue(year.ToString(CultureInfo.InvariantCulture)).Selected = true;
                ddlMonth.Items.FindByValue(mon.ToString(CultureInfo.InvariantCulture)).Selected = true;
            }
        }
        protected void prepareTable()
        {
            serialcode.Text = serialnumber;
            var l =  new Dictionary<string, string>
                {
                    {"1","Scheduled"},
                    {"2","Schedule Approved"},
                    {"3","Due Date"},
                    {"4","On PM"},
                    {"5","Completed"},
                    {"6","Done"}
                };
            if (checktype == 1)
            {
                l = new Dictionary<string, string>
                    {
                        {"1", "Checklist Open"},
                        {"2", "Checklist Fail"},
                        {"3", "Checklist Pass"},
                        {"4", "Machine Not Running"},
                        {"5", "Loan Out"},
                        {"6", "Modification"}
                    };
            }
            var htm2 = new HtmlGenericControl("ul");

            foreach (string ss in l.Keys)
            {          
                htm2.Controls.Add(new HtmlGenericControl("li") { InnerText = ss+" : "+ l[ss]});
                legend.Controls.Add(htm2);
            }
            

            DateTime today = DateTime.Now;
            int tanggalHariIni = -1;
            var sqlparams = new List<SqlParameter>();
            SqlDataReader sdr = SqlHelper.ExecuteReader(Functions.TPMDBConnection(), CommandType.Text, "Select * from MDepartments");
            ListItem li;
            while (sdr.Read())
            {
                li = new ListItem {Text = sdr["Descriptions"].ToString(), Value = sdr["Id"].ToString()};
                ddlDepartment.Items.Add(li);

            }
            for (int b = 1; b < 13; b++)
            {
                li = new ListItem
                    {
                        Text = TPMHelper.Namabulan()[b][1],
                        Value = (b).ToString(CultureInfo.InvariantCulture)
                    };
                ddlMonth.Items.Add(li);
            }
            for (int b = 0; b < 100; b++)
            {
                li = new ListItem
                    {
                        Text = (b + 2013).ToString(CultureInfo.InvariantCulture),
                        Value = (b + 2013).ToString(CultureInfo.InvariantCulture)
                    };
                ddlYear.Items.Add(li);
            }
            TableRow tr = null;
            tr = new TableRow {TableSection = TableRowSection.TableHeader};
            TableCell tc;
            TableHeaderCell thc = null;
            thc = new TableHeaderCell {Text = "SERIAL CODE"};
            thc.Style.Add("vertical-align", "middle");
            thc.Style.Add("text-align", "center");
            tr.Cells.Add(thc);

            thc = new TableHeaderCell {Text = "MACHINE"};
            thc.Style.Add("vertical-align", "middle");
            thc.Style.Add("text-align", "center");
          
            tr.Cells.Add(thc);
            thc = new TableHeaderCell {Text = "&nbsp;"};
            tr.Cells.Add(thc);


            int jumlahharidalam1sasi = DateTime.DaysInMonth(saiki.Year, saiki.Month);
            int firstSunday = 0;
            for (int i = 0; i < jumlahharidalam1sasi; i++)
            {

                thc = new TableHeaderCell
                    {
                        Text = ((i + 1).ToString(CultureInfo.InvariantCulture).Length > 1 ? (i + 1).ToString(CultureInfo.InvariantCulture) : "&nbsp;" + (i + 1).ToString())
                    };
                if (DateTime.Parse((i + 1).ToString(CultureInfo.InvariantCulture) + " " + saiki.ToString("MMM yyyy")).DayOfWeek == DayOfWeek.Sunday)
                {
                    // tc.Text += "S";
                    thc.Style.Add("background-color", "#cceedd");
                    firstSunday = i;
                }
                if (today.Date == DateTime.Parse((i + 1).ToString() + " " + saiki.ToString("MMM yyyy")).Date)
                {
                    thc.Style.Add("background-color", "#ff0");
                    tanggalHariIni = i;
                    saiki.AddDays(0 - tanggalHariIni);
                }
                tr.Cells.Add(thc);
            }
            firstSunday %= 7;
            thc = new TableHeaderCell {Text = "PIC"};
            tr.Cells.Add(thc);
            tblSchedule.Rows.Add(tr);
            //

            var description = new Dictionary<string, string>();
            var pics = new Dictionary<string, string>();
            var Container = new List<Dictionary<string, Dictionary<int, PMrecords>>>();


            sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@Department_Id", depid),
                    new SqlParameter("@CheckListTypes_id", checktype),
                    new SqlParameter("@month", mon),
                    new SqlParameter("@year", year),
                    new SqlParameter("@SerialNumber", serialnumber)
                };

            var ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MPMSchedulesSelect_byDept", sqlparams.ToArray());
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                Container.Add(new Dictionary<string, Dictionary<int, PMrecords>>());
                Container.Add(new Dictionary<string, Dictionary<int, PMrecords>>());

                DataTable dtt = ds.Tables[1];

                foreach (DataRow dr in dtt.Rows)
                {
                    Container[0].Add(dr["assetkey"].ToString(), new Dictionary<int, PMrecords>());
                    Container[1].Add(dr["assetkey"].ToString(), new Dictionary<int, PMrecords>());
                    description[dr["assetkey"].ToString()] = dr["descriptions"] == null ? "" : dr["descriptions"].ToString();
                }

                foreach (DataRow dr in dt.Rows)
                {

                    if (description.ContainsKey(dr["AssetKey"].ToString()) == false)
                    {
                        description.Add(dr["AssetKey"].ToString(), "");
                    }


                    if (pics.ContainsKey(dr["AssetKey"].ToString()) == false)
                    {
                        pics.Add(dr["AssetKey"].ToString(), "");
                    }
                    pics[dr["AssetKey"].ToString()] = dr["pic"] == null ? "" : dr["pic"] + "/ ";

                    var ps = new PMrecords
                        {
                            id = (dr["id"] == null ? "-1" : dr["id"].ToString()),
                            checklist = (dr["checklist"] == null ? "0" : dr["checklist"].ToString()),
                            status = 1,
                            pmdatetime = (DateTime) (dr["scheduled_date"]),
                            cresult =
                                 dr["isrunning"].ToString().ToUpper() == "TRUE"
                                     ? dr["cresult"].ToString().ToUpper()
                                     : "4"
                        };
                    if (ps.pmdatetime != null)
                    {
                        if (Container[0].ContainsKey(dr["AssetKey"].ToString()) == false)
                        {
                            Container[0].Add(dr["AssetKey"].ToString(), new Dictionary<int, PMrecords>());
                        }
                        if (Container[0][dr["AssetKey"].ToString()].ContainsKey(ps.pmdatetime.Day) == false)
                        {
                            Container[0][dr["AssetKey"].ToString()].Add(ps.pmdatetime.Day, ps);
                        }
                        else
                        {
                            Container[0][dr["AssetKey"].ToString()][ps.pmdatetime.Day] = ps;
                        }
                    }
                    var ff = dr["cresult"].ToString().ToUpper();
                    ff = ff == string.Empty ? "1" : (ff == "TRUE" ? "3" : "2");
                    ff = dr["isrunning"].ToString().ToUpper() == "TRUE" ? ff : "4";
                    ff = dr["Machine_Status"].ToString() == "1"
                             ? ff
                             : (Convert.ToInt32(dr["Machine_Status"].ToString())+3).ToString(CultureInfo.InvariantCulture);
                    ps = new PMrecords
                        {
                            id = (dr["id"] == null ? "-1" : dr["id"].ToString()),
                            status = (int) (dr["pm_status_id"] == DBNull.Value ? -1 : dr["pm_status_id"]),
                            pmdatetime = (DateTime) (dr["date"]),
                            checklist = (dr["checklist"] == null ? "0" : dr["checklist"].ToString()),
                            
                            cresult = ff
                              
                        };

                    if (ps.pmdatetime != null)
                    {
                        if (Container[1].ContainsKey(dr["AssetKey"].ToString()) == false)
                        {
                            Container[1].Add(dr["AssetKey"].ToString(), new Dictionary<int, PMrecords>());
                        }

                        if (Container[1][dr["AssetKey"].ToString()].ContainsKey(ps.pmdatetime.Day) == false)
                        {
                            Container[1][dr["AssetKey"].ToString()].Add(ps.pmdatetime.Day, ps);
                        }
                        else
                        {
                            Container[1][dr["AssetKey"].ToString()][ps.pmdatetime.Day] = ps;
                        }
                    }
                }

                var session = new MySessions();
                foreach (string kunci in Container[0].Keys)
                {
                    tr = new TableRow {TableSection = TableRowSection.TableBody};
                    tc = new TableCell {RowSpan = 2};
                    tc.Style.Add("vertical-align", "middle");
                    tc.Style.Add("text-align", "center");
                    if (session.IsAdministrator)
                    {
                        var html = new HtmlGenericControl("a") {InnerHtml = kunci};
                        html.Attributes.Add("href", "PrintChecklist.aspx?v=" + kunci);
                        html.Attributes.Add("target", "_blank");
                        tc.Controls.Add(html);
                    }
                    else
                    {
                        tc.Text = kunci;
                    }
                    tr.Cells.Add(tc);
                    tc = new TableCell {RowSpan = 2};
                    tc.Style.Add("vertical-align", "middle");
                    tc.Style.Add("text-align", "center");

                    tc.Text = description[kunci];
                    tr.Cells.Add(tc);
                    tc = new TableCell {Text = "Sched"};
                    tr.Cells.Add(tc);
                    for (int ii = 0; ii < jumlahharidalam1sasi; ii++)
                    {
                        tc = new TableCell {Text = ""};
                        if (ii % 7 == firstSunday) { tc.Style.Clear(); tc.Style.Add("background-color", "#cceedd"); }
                        if (ii == tanggalHariIni)
                        {
                            tc.Style.Clear();
                            tc.Style.Add("background-color", "#ff0");
                        }
                        if (Container[0][kunci].Keys.Contains(ii + 1))
                        {
                            if (Container[0][kunci][ii + 1].pmdatetime == null)
                            {
                                tc.Text = "";
                            }
                            else
                            {
                                if (Container[0][kunci][ii + 1].pmdatetime.Date == DateTime.Parse((ii + 1).ToString(CultureInfo.InvariantCulture) + " " + saiki.ToString("MMM yyyy")).Date)
                                {
                                    tc.Text = "X";
                                    tc.Style.Clear();
                                    tc.Style.Add("background-color", "#ebe2e2");
                                }

                            }
                        }
                        else
                        {
                            tc.Text = "";
                        }
                        tr.Cells.Add(tc);
                    }
                    tc = new TableCell
                        {
                            RowSpan = 2,
                            Text = pics.ContainsKey(kunci) ? pics[kunci].Remove(pics[kunci].Length - 2, 2) : ""
                        };

                    tc.Style.Add("text-align", "center");
                    tc.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(tc);
                    tblSchedule.Rows.Add(tr);


                    //status
                    tr = new TableRow {TableSection = TableRowSection.TableBody};
                    tc = new TableCell {Text = "Res"};
                    tr.Cells.Add(tc);
                    for (int i = 0; i < jumlahharidalam1sasi; i++)
                    {
                        tc = new TableCell {Text = ""};
                        if (i % 7 == firstSunday) { tc.Style.Clear(); tc.Style.Add("background-color", "#cceedd"); }
                        if (i == tanggalHariIni)
                        {
                            tc.Style.Clear();
                            tc.Style.Add("background-color", "#ff0");
                        }
                        if (Container[1][kunci].ContainsKey((i + 1)))
                        {
                            if (Container[1][kunci][i + 1].pmdatetime == null)
                            {
                                tc.Text = "";
                            }
                            else
                            {
                                if (Container[1][kunci][i + 1].pmdatetime.ToString("d MMM yyyy") == ((i + 1).ToString(CultureInfo.InvariantCulture) + " " + saiki.ToString("MMM yyyy")))
                                {
                                    var htm = new HtmlGenericControl("a");
                                    if (checktype == 2)
                                    {
                                        htm.Attributes.Add("href", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + "/YPMStatus.aspx?pid=" + Container[1][kunci][i + 1].id);
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(Container[1][kunci][i + 1].cresult) < 5)
                                        {

                                            htm.Attributes.Add("href",
                                                               HttpContext.Current.Request.Url.GetLeftPart(
                                                                   UriPartial.Authority) +
                                                               HttpContext.Current.Request.ApplicationPath +
                                                               "/Ychecklist.aspx?id=" +
                                                               Container[1][kunci][i + 1].checklist);
                                        }
                                    }
                                    var cresult =
                                        Container[1][kunci][i + 1].cresult.ToString(CultureInfo.InvariantCulture);
                                   
                                    htm.InnerText = checktype == 1?  cresult : Container[1][kunci][i + 1].status.ToString(CultureInfo.InvariantCulture);
                                    tc.Controls.Add(htm);
                                    tc.Style.Clear();
                                    tc.Style.Add("background-color", "#eec766");
                                }
                                else {
                                    tc.Text = "";
                                }
                            }
                        }
                        else
                        {
                            tc.Text = "";
                        }
                        tr.Cells.Add(tc);

                    }
                    tblSchedule.Rows.Add(tr);


                }

            }
        }
    }
}