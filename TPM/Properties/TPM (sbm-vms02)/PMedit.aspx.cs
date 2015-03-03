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
    public partial class PMedit : System.Web.UI.Page
    {
        TPMHelper Functions = new TPMHelper();
        public int depid = 2;
        public int mon;
        public int year;
        public DateTime saiki = DateTime.Now;
        public int checktype = 2;
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                if (session.IsAdministrator)
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
                    prepareForm();
                }
                else
                {
                    Server.Transfer(session.redirection);
                }


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
                   // tc.RowSpan = 2;
                    tc.Style.Add("vertical-align", "middle");
                    tc.Style.Add("text-align", "center");
                    tc.Text = kunci;
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                  //  tc.RowSpan = 2;
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
                            tc.Text = "-";
                            tc.Style.Clear();
                            tc.Style.Add("background-color", "#fff");
                        }
                        tc.ID = kunci + "_" + year + "_" + mon + "_" + (ii+1).ToString();
                        tc.CssClass = "sched_date";
                        tc.ClientIDMode = ClientIDMode.Static;
                        if (DateTime.Now.AddDays(1).Date > DateTime.Parse((ii+1).ToString()+"-"+Functions.Namabulan()[Convert.ToInt32(mon)]+"-"+year).Date){
                            tc.Text = tc.Text=="X"? "X":"";
                            tc.CssClass = "";
                        }
                        if (ii % 7 == firstSunday) { tc.Style.Clear(); tc.Style.Add("background-color", "#cceedd"); }
                        if (ii == tanggalHariIni)
                        {  
                            tc.Style.Clear();
                            tc.Style.Add("background-color", "#ff0");
                        }
                        tr.Cells.Add(tc);
                    }
                    tc = new TableCell();
                   // tc.RowSpan = 2;
                    tc.Style.Add("vertical-align", "middle");
                    tc.Style.Add("text-align", "center");
                    tc.Text = pics.ContainsKey(kunci) == true ? pics[kunci].Remove(pics[kunci].Length - 2, 2) : "";
                    tr.Cells.Add(tc);
                    tblSchedule.Rows.Add(tr);


                    
                }

            }
        }
        protected void prepareForm()
        {
            TableRow tr = new TableRow();
            TableHeaderCell thc = new TableHeaderCell();
            TableCell tc = new TableCell();

            tr.TableSection = TableRowSection.TableHeader;
            ImageButton img = new ImageButton();
            img.ID = "BtnCloseForm";
            img.ClientIDMode = ClientIDMode.Static;
            img.ImageUrl = "./Images/delete.png";
            img.OnClientClick = "return false";
            img.Width = 15;
            img.BorderWidth = 0;
            img.ToolTip = "Close the Form";


            Label head = new Label();
            head.ID = "FormHeader";
            head.ClientIDMode = ClientIDMode.Static;
            head.Text = "Form Header";
            tr = new TableRow();
            tr.ID = "trhead";
            tr.ClientIDMode = ClientIDMode.Static;
            thc = new TableHeaderCell();
            thc.ColumnSpan = 2;
            thc.Controls.Add(img);
            thc.Controls.Add(head);
            tr.Cells.Add(thc);
            FormTbl.Rows.Add(tr);

            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("txtPIC","PIC");
            form.Add("txtApprovedby","Approved By");

            foreach (string ss in form.Keys){
                tr = new TableRow();
                tr.CssClass = "rowtxt";
                tr.ClientIDMode = ClientIDMode.Static;
                tc = new TableCell();
                tc.Text = form[ss];
                tr.Cells.Add(tc);
                tc = new TableCell();
                TextBox txt = new TextBox();
                txt.ID = ss;
                txt.ClientIDMode = ClientIDMode.Static;
                tc.Controls.Add(txt);
                tr.Cells.Add(tc);
                FormTbl.Rows.Add(tr);
            }
           

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);
            Button SubmitBtn = new Button();
            SubmitBtn.ID = "BtnSubmit1";
            SubmitBtn.ClientIDMode = ClientIDMode.Static;
            SubmitBtn.Text = "Submit";
            SubmitBtn.CssClass = "btn btn-primary tombol";
            SubmitBtn.OnClientClick = "return false;";
            tc = new TableCell();
            tc.Controls.Add(SubmitBtn);
            Label lbl = new Label();
            lbl.Text = "&nbsp;&nbsp;";
            tc.Controls.Add(lbl);
            SubmitBtn = new Button();
            SubmitBtn.ID = "BtnSubmit2";
            SubmitBtn.ClientIDMode = ClientIDMode.Static;
            SubmitBtn.Text = "Submit";
            SubmitBtn.CssClass = "btn btn-primary tombol";
            SubmitBtn.OnClientClick = "return false;";
            SubmitBtn.Style.Add("display", "none");
            tc.Controls.Add(SubmitBtn);
            tr.Cells.Add(tc);

            FormTbl.Rows.Add(tr);

        }
    }
}