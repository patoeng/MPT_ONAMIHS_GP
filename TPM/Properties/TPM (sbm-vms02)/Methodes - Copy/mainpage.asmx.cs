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
namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for mainpage
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class mainpage : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string getlist(string id)
        {
            
            string[] ss = id.Split('_');

            Table tbl = new Table();
            tbl.ID = "jsonTable";
            tbl.ClientIDMode = ClientIDMode.Static;
            tbl.CssClass = "table table-bordered table-striped";
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            DataSet ds=null;
            DataTable dt;
            switch (ss[0])
            {
                case "wo": ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MWorkOrdersSelect_byStatus", new SqlParameter("@status_id", ss[1]));
                        break;
                case "dp": ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_mainpage_DailyPromptStatus", new SqlParameter("@option", ss[1]));
                        break;
                case "pm": ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_mainpage_PMStatus", new SqlParameter("@option", ss[1]));
                        break;
                case "bom": ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_mainpage_AllAssetModel");
                        break;
                case "bl": AssetModels.AssetModelCode = ss[1]; ds = AssetModels.BomList;
                        break;
                case "bon": AssetModels.AssetModelCode = ss[1]; ds = AssetModels.BomListLow;
                        break;
                case "bos": AssetModels.AssetModelCode = ss[1]; ds = AssetModels.BomListLowAll;
                        break;
                case "sum": ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_mainpage_DownTime", new SqlParameter("@option", ss[1]));
                        break;
             }
            tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            dt =  ds.Tables[0];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                tc = new TableHeaderCell();
                tc.Text = dt.Columns[i].ColumnName;
                tr.Cells.Add(tc);
            }
            tbl.Rows.Add(tr);
            
            
            foreach(DataRow dr in dt.Rows){
                tr = new TableRow();
                for (int i=0; i < dt.Columns.Count;i++ )
                {
                    tc = new TableCell();
                    tc.Text = dt.Columns[i].DataType == System.Type.GetType("System.DateTime") ? ((DateTime)(dr[i])).ToString("d MMMM yyyy") : dr[i].ToString();
                    tr.Cells.Add(tc);
                }
                tbl.Rows.Add(tr);
            }
               StringWriter sw = new StringWriter();
               HtmlTextWriter htw = new HtmlTextWriter(sw);
               tbl.RenderControl(htw);
               string s = sw.ToString();
               
               return s;
        }
    }
}
