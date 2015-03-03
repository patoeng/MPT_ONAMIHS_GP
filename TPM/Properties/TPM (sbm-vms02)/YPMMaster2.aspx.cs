using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class YPMMaster2 : System.Web.UI.Page
    {
        public string year = "";
        public int depid=0;
        public TPMHelper Functions = new TPMHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                depid = Request.QueryString["d"] == null ? 2 : (Request.QueryString["d"].ToString() == "" ? 2 : Convert.ToInt32(Request.QueryString["d"].ToString()));
                year = Request.QueryString["y"] == null ? DateTime.Now.Year.ToString() :( Request.QueryString["y"].ToString()==""?DateTime.Now.Year.ToString(): Request.QueryString["y"].ToString());
                if(year!=""){
                    prepareTable();
                }
                prepareSelector(year,depid.ToString());
            }
        }
        protected void prepareSelector(string yr, string dep)
        {
            ddlYear.Items.Clear();
            ddlDept.Items.Clear();

            for (int x = 0; x < 30; x++) { 
               ddlYear.Items.Add(new ListItem((2012+x).ToString(),(2012+x).ToString()));
            }
            ddlYear.Items.FindByValue(year).Selected = true;

            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MDepartmentsSelect", new SqlParameter("@id",DBNull.Value));
            foreach (DataRow dr in ds.Tables[0].Rows){
                ddlDept.Items.Add(new ListItem(dr["descriptions"].ToString(),dr["id"].ToString()));
            }
            if (ddlDept.Items.FindByValue(dep) != null) { ddlDept.Items.FindByValue(dep).Selected = true; }
        }
        protected void prepareTable()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, List<List<int>>>>> sched = new Dictionary<string, Dictionary<string, Dictionary<string, List<List<int>>>>>();
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableHeaderCell thc = new TableHeaderCell();
            tblSchedule.Style.Add("table-layout", "fixed");
            
            sqlparams.Add(new SqlParameter("@Department_Id", depid));
            sqlparams.Add(new SqlParameter("@CheckListTypes_id", 2));
            sqlparams.Add(new SqlParameter("@month", DBNull.Value));
            sqlparams.Add(new SqlParameter("@year", year));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MPMSchedulesSelect_byDept", sqlparams.ToArray());

            foreach (DataRow dr in ds.Tables[1].Rows) {
                sched.Add(dr["descriptions"].ToString(), new Dictionary<string, Dictionary<string, List<List<int>>>>());
            }
            
            foreach (DataRow dr in ds.Tables[0].Rows) { 
                if (sched.ContainsKey(dr["descriptions"].ToString())==false){
                    sched.Add(dr["descriptions"].ToString(), new Dictionary<string, Dictionary<string, List<List<int>>>>());
                }
                List<int> ins = new List<int>();
                ins.Add((int)dr["month"]);
                ins.Add((int)dr["week"]);
                ins.Add((int)dr["id"]);
                if (sched[dr["descriptions"].ToString()].ContainsKey(dr["month"].ToString())==false)
                {
                    sched[dr["descriptions"].ToString()].Add(dr["month"].ToString(),new  Dictionary<string,List<List<int>>>());
                }
                if (sched[dr["descriptions"].ToString()][dr["month"].ToString()].ContainsKey(dr["week"].ToString()) == false)
                {
                    sched[dr["descriptions"].ToString()][dr["month"].ToString()].Add(dr["week"].ToString(), new List<List<int>>());
                }
                sched[dr["descriptions"].ToString()][dr["month"].ToString()][dr["week"].ToString()].Add(ins);
            }
            
                

                tr.TableSection = TableRowSection.TableHeader;
                
                List<string> thead = new List<string>();
                thead.Add("Item");
                thead.Add("Asset Name");
                thead.Add("");
                for (int o = 1; o < 13; o++)
                {
                    thead.Add(Functions.Namabulan()[o]);
                }
                for (int o = 0; o < thead.Count;o++ )
                {
                    thc = new TableHeaderCell();
                    thc.Style.Add("text-align", "center");
                    if (o == 0) { thc.Style.Add("width","50px!important"); }
                    if (o == 1) { thc.CssClass = "newclass"; }
                    thc.Text = thead[o];
                    if (o > 2) { thc.ColumnSpan = 4; } else { thc.RowSpan = 2; }
                    tr.Cells.Add(thc);
                }
                tblSchedule.Rows.Add(tr);
                thead.Clear();
                thead.Add("1");
                thead.Add("2");
                thead.Add("3");
                thead.Add("4");
                tr = new TableRow();
                tr.TableSection = TableRowSection.TableHeader;
                for (int o = 1; o < 13; o++)
                {
                    for (int nn=0;nn<4;nn++){
                        thc = new TableHeaderCell();
                        thc.Style.Add("text-align", "center");
                        thc.Text = thead[nn];
                        tr.Cells.Add(thc);
                    }
                }
                tblSchedule.Rows.Add(tr);
                int iss=0;
                foreach(string ss in sched.Keys){
                    tr = new TableRow();
                    tc = new TableCell();
                    tc.Text = (++iss).ToString();
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                    tc.Text = ss;
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                    tc.Text = "&nbsp";
                    tr.Cells.Add(tc);
                    for (int m = 0; m < 12; m++) {
                        for (int w = 0; w < 4; w++) {
                            tc = new TableCell();
                            string me = (m+1).ToString();
                            if (sched[ss].ContainsKey(me)) {
                                string we = (w + 1).ToString();
                                if (sched[ss][me].ContainsKey(we))
                                {
                                    List<List<int>> ins =sched[ss][me][we];
                                    for (int pp=0; pp<ins.Count;pp++)
                                    {
                                        if (sched[ss][me][we][pp][0] != 0)
                                        {
                                            tc.Text += "x";
                                        }
                                    }
                                }
                            }
                            
                            tr.Cells.Add(tc);
                        }
                    }
                    tblSchedule.Rows.Add(tr);
                }
        }
    }
}