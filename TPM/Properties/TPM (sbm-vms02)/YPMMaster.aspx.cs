using System;
using System.Collections.Generic;
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
                if (!DateTime.TryParse(year.ToString() + "-" + mon.ToString() + "-01", out saiki))
                {
                    saiki = DateTime.Now;
                };
                prepareTable();
                ddlDepartment.Items.FindByValue(depid.ToString()).Selected = true;
                ddlYear.Items.FindByValue(year.ToString()).Selected = true;
                ddlMonth.Items.FindByValue(mon.ToString()).Selected = true;
            }
        }
        protected void prepareTable()
        {
            
            DateTime today = DateTime.Now;
            int tanggalHariIni = -1;
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            SqlDataReader sdr = SqlHelper.ExecuteReader(Functions.TPMDBConnection(), CommandType.Text, "Select * from MDepartments");
            ListItem li;
            while (sdr.Read())
            {
                li = new ListItem();
                li.Text = sdr["Descriptions"].ToString();
                li.Value = sdr["Id"].ToString();
                ddlDepartment.Items.Add(li);

            }
            for (int b = 1; b < 13; b++)
            {
                li = new ListItem();
                li.Text = Functions.Namabulan()[b];
                li.Value = (b).ToString();
                ddlMonth.Items.Add(li);
            }
            for (int b = 0; b < 100; b++)
            {
                li = new ListItem();
                li.Text = (b + 2013).ToString();
                li.Value = (b + 2013).ToString();
                ddlYear.Items.Add(li);
            }
            TableRow tr = null;
            tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            TableCell tc = null;
            TableHeaderCell thc = null;
            thc = new TableHeaderCell();
            thc.Text = "ASSET CODE";
            thc.Style.Add("vertical-align", "middle");
            thc.Style.Add("text-align", "center");
            tr.Cells.Add(thc);

            thc = new TableHeaderCell();
            thc.Style.Add("vertical-align", "middle");
            thc.Style.Add("text-align", "center");
            thc.Text = "MACHINE";
            tr.Cells.Add(thc);
            thc = new TableHeaderCell();
            thc.Text = "&nbsp;";
            tr.Cells.Add(thc);


            int jumlahharidalam1sasi = DateTime.DaysInMonth(saiki.Year, saiki.Month);
            int firstSunday = 0;
            for (int i = 0; i < jumlahharidalam1sasi; i++)
            {

                thc = new TableHeaderCell();
                thc.Text = ((i + 1).ToString().Length > 1 ? (i + 1).ToString() : "&nbsp;" + (i + 1).ToString());
                if (DateTime.Parse((i + 1).ToString() + " " + saiki.ToString("MMM yyyy")).DayOfWeek == DayOfWeek.Sunday)
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
            thc = new TableHeaderCell();
            thc.Text = "PIC";
            tr.Cells.Add(thc);
            tblSchedule.Rows.Add(tr);
            //

            Dictionary<string, string> description = new Dictionary<string, string>();
            Dictionary<string, string> pics = new Dictionary<string, string>();
            List<Dictionary<string, Dictionary<int, PMrecords>>> Container = new List<Dictionary<string, Dictionary<int, PMrecords>>>();


            sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@Department_Id", depid));
            sqlparams.Add(new SqlParameter("@CheckListTypes_id", checktype));
            sqlparams.Add(new SqlParameter("@month", mon));
            sqlparams.Add(new SqlParameter("@year", year));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MPMSchedulesSelect_byDept", sqlparams.ToArray());
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                Container.Add(new Dictionary<string, Dictionary<int, PMrecords>>());
                Container.Add(new Dictionary<string, Dictionary<int, PMrecords>>());

                DataTable dtt = ds.Tables[1];

                foreach (DataRow dr in dtt.Rows)
                {
                    Container[0].Add(dr["asset_code"].ToString(), new Dictionary<int, PMrecords>());
                    Container[1].Add(dr["asset_code"].ToString(), new Dictionary<int, PMrecords>());
                    description[dr["asset_code"].ToString()] = dr["descriptions"] == null ? "" : dr["descriptions"].ToString();
                }

                foreach (DataRow dr in dt.Rows)
                {

                    if (description.ContainsKey(dr["asset_code"].ToString()) == false)
                    {
                        description.Add(dr["asset_code"].ToString(), "");
                    }


                    if (pics.ContainsKey(dr["asset_code"].ToString()) == false)
                    {
                        pics.Add(dr["asset_code"].ToString(), "");
                    }
                    pics[dr["asset_code"].ToString()] = dr["pic"] == null ? "" : dr["pic"].ToString() + "/ ";

                    PMrecords ps = new PMrecords();
                    ps.id = (int)(dr["id"] == null ? -1 : dr["id"]);
                    ps.status = 1;
                    ps.pmdatetime = (DateTime)(dr["scheduled_date"] == null ? null : dr["scheduled_date"]);
                    if (ps.pmdatetime != null)
                    {
                        if (Container[0].ContainsKey(dr["asset_code"].ToString()) == false)
                        {
                            Container[0].Add(dr["asset_code"].ToString(), new Dictionary<int, PMrecords>());
                        }
                        if (Container[0][dr["asset_code"].ToString()].ContainsKey(ps.pmdatetime.Day) == false)
                        {
                            Container[0][dr["asset_code"].ToString()].Add(ps.pmdatetime.Day, ps);
                        }
                        else
                        {
                            Container[0][dr["asset_code"].ToString()][ps.pmdatetime.Day] = ps;
                        }
                    }

                    ps = new PMrecords();
                    ps.id = (int)(dr["id"] == null ? -1 : dr["id"]);
                    ps.status = (int)(dr["pm_status_id"] == DBNull.Value ? -1 : dr["pm_status_id"]);
                    ps.pmdatetime = (DateTime)(dr["date"] == null ? null : dr["date"]);


                    if (ps.pmdatetime != null)
                    {
                        if (Container[1].ContainsKey(dr["asset_code"].ToString()) == false)
                        {
                            Container[1].Add(dr["asset_code"].ToString(), new Dictionary<int, PMrecords>());
                        }

                        if (Container[1][dr["asset_code"].ToString()].ContainsKey(ps.pmdatetime.Day) == false)
                        {
                            Container[1][dr["asset_code"].ToString()].Add(ps.pmdatetime.Day, ps);
                        }
                        else
                        {
                            Container[1][dr["asset_code"].ToString()][ps.pmdatetime.Day] = ps;
                        }
                    }
                }


                foreach (string kunci in Container[0].Keys)
                {
                    tr = new TableRow();
                    tr.TableSection = TableRowSection.TableBody;
                    tc = new TableCell();
                    tc.RowSpan = 2;
                    tc.Style.Add("vertical-align", "middle");
                    tc.Style.Add("text-align", "center");
                    tc.Text = kunci;
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                    tc.RowSpan = 2;
                    tc.Style.Add("vertical-align", "middle");
                    tc.Style.Add("text-align", "center");

                    tc.Text = description[kunci];
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                    tc.Text = "Sched";
                    tr.Cells.Add(tc);
                    for (int ii = 0; ii < jumlahharidalam1sasi; ii++)
                    {
                        tc = new TableCell();
                        tc.Text = "";
                        if (ii % 7 == firstSunday) { tc.Style.Clear(); tc.Style.Add("background-color", "#cceedd"); }
                        if (ii == tanggalHariIni)
                        {
                            tc.Style.Clear();
                            tc.Style.Add("background-color", "#ff0");
                        }
                        if (Container[0][kunci].Keys.Contains(ii + 1) == true)
                        {
                            if (Container[0][kunci][ii + 1].pmdatetime == null)
                            {
                                tc.Text = "";
                            }
                            else
                            {
                                if ((DateTime)Container[0][kunci][ii + 1].pmdatetime.Date == DateTime.Parse((ii + 1).ToString() + " " + saiki.ToString("MMM yyyy")).Date)
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
                    tc = new TableCell();
                    tc.RowSpan = 2;
                    tc.Style.Add("vertical-align", "middle");
                    tc.Style.Add("text-align", "center");
                    tc.Text = pics.ContainsKey(kunci) == true ? pics[kunci].Remove(pics[kunci].Length - 2, 2) : "";
                    tr.Cells.Add(tc);
                    tblSchedule.Rows.Add(tr);


                    //status
                    tr = new TableRow();
                    tr.TableSection = TableRowSection.TableBody;
                    tc = new TableCell();
                    tc.Text = "Res";
                    tr.Cells.Add(tc);
                    for (int i = 0; i < jumlahharidalam1sasi; i++)
                    {
                        tc = new TableCell();
                        tc.Text = "";
                        if (i % 7 == firstSunday) { tc.Style.Clear(); tc.Style.Add("background-color", "#cceedd"); }
                        if (i == tanggalHariIni)
                        {
                            tc.Style.Clear();
                            tc.Style.Add("background-color", "#ff0");
                        }
                        if (Container[1][kunci].ContainsKey((i + 1)) == true)
                        {
                            if (Container[1][kunci][i + 1].pmdatetime == null)
                            {
                                tc.Text = "";
                            }
                            else
                            {
                                if (Container[1][kunci][i + 1].pmdatetime.ToString("d MMM yyyy") == ((i + 1).ToString() + " " + saiki.ToString("MMM yyyy")))
                                {
                                    HtmlGenericControl htm = new HtmlGenericControl("a");
                                    htm.Attributes.Add("href", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + "/YPMStatus.aspx?pid=" + Container[1][kunci][i + 1].id.ToString());
                                    htm.InnerText = Container[1][kunci][i + 1].status.ToString();
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
                    tr = new TableRow();



                    //end foreach lama
                }

            }
        }
    }
}