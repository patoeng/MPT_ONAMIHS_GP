using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for downtime
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class downtime : System.Web.Services.WebService
    {
        public class flotData{
            public flotData() {
                _data = new List<ArrayList>();
            }
            private string _label= "data";
            private string _color= "blue";
            private List<ArrayList> _data;

            public string label {
                get { return _label; }
                set { _label = value; }
            }
            public string color {
                get { return _color; }
                set { _color = value; }
            }
            public List<ArrayList> data
            {
                get { return _data; }
                set { _data = value; }
            }
            
        }
      
        public class flot {
            public flot() {
                _chart = new List<flotData>();
                _ticks = new List<ArrayList>();
            }
            private int _total;
            private List<flotData> _chart;
            private List<ArrayList> _ticks;
            public List<flotData> chart
            {
                get { return _chart; }
                set { _chart = value; }  
            }
            public List<ArrayList> ticks
            {
                get { return _ticks; }
                set { _ticks = value; }
            }
            public int total{
                get { return _total; }
                set { _total = value; }
            }
        }
        public class downtimePackage {
            public downtimePackage() {
                _flotchart = new flot();
            }
            private flot _flotchart;
            private string _htmltable;
            public flot flotchart {
                get { return _flotchart; }
                set { _flotchart = value; }
            }
            public string htmltable {
                get { return _htmltable; }
                set { _htmltable = value; }
            }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string showtable(string asset_id,string startdate, string enddate="")
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@asset_id",asset_id));
            sqlparams.Add(new SqlParameter("@date_begin", startdate));
            sqlparams.Add(new SqlParameter("@date_end", enddate));
            sqlparams.Add(new SqlParameter("@option", 1));
            Table tbl = new Table();
            tbl.ID = "jsonTable";
            tbl.ClientIDMode = ClientIDMode.Static;
            tbl.CssClass = "table table-bordered table-striped";
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            DataSet ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_mainpage_DownTime", sqlparams.ToArray());
            DataTable dt= ds.Tables[0];
            tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            dt = ds.Tables[0];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                tc = new TableHeaderCell();
                tc.Text = dt.Columns[i].ColumnName;
                tr.Cells.Add(tc);
            }
            tbl.Rows.Add(tr);
            downtimePackage dp = new downtimePackage();
            int rowid = 0;
            flotData fdWO = new flotData();
            flotData fdPM = new flotData();
            fdWO.label = "Asset Break Down";
            fdPM.label = "Asset PM";
            fdPM.color = "yellow";
            foreach (DataRow dr in dt.Rows)
            {
                ArrayList alticks = new ArrayList();
                rowid++;
                alticks.Add(rowid);
                alticks.Add(dt.Columns[0].DataType == System.Type.GetType("System.DateTime") ? ((DateTime)(dr[0])).ToString("d MMM yyyy") : dr[0].ToString());
                ArrayList aldataWO = new ArrayList();
                aldataWO.Add(rowid);
                aldataWO.Add((int)dr[1]);
               
                fdWO.data.Add(aldataWO);
                ArrayList aldataPM = new ArrayList();
                aldataPM.Add(rowid);
                aldataPM.Add((int)dr[2]);

                fdPM.data.Add(aldataPM);
                dp.flotchart.ticks.Add(alticks);
                tr = new TableRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    tc = new TableCell();
                    tc.Text = dt.Columns[i].DataType == System.Type.GetType("System.DateTime") ? ((DateTime)(dr[i])).ToString("d MMMM yyyy") : dr[i].ToString();
                    tr.Cells.Add(tc);
                }
                tbl.Rows.Add(tr);
            }
            dp.flotchart.chart.Add(fdWO);
            dp.flotchart.chart.Add(fdPM);
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            tbl.RenderControl(htw);
            string s = sw.ToString();

            dp.flotchart.total = 700;
            dp.htmltable = s;
            JavaScriptSerializer js = new JavaScriptSerializer();
            s=js.Serialize(dp);
            return s;
        }
    }
}
