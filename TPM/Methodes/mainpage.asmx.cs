using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TPM.Classes;

namespace TPM.Methodes
{
    /// <summary>
    ///     Summary description for mainpage
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class mainpage : WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string downloadlist(string id, string depid = null)
        {
            string[] ss = id.Split('_');
            string text = ss[2].Replace('-', ' ');
            DataSet ds = null;
            switch (ss[0])
            {
                case "wo":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_MWorkOrdersSelect_byStatus",
                                                  new SqlParameter("@status_id", ss[1]),
                                                  new SqlParameter("@dept", depid));
                    break;
                case "dp":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_DailyPromptStatus", new SqlParameter("@option", ss[1]),
                                                  new SqlParameter("@dept", depid));
                    break;
                case "pm":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_PMStatus", new SqlParameter("@option", ss[1]),
                                                  new SqlParameter("@dept", depid));
                    break;
                case "iwo":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_iwostatus", new SqlParameter("@option", ss[1]),
                                                  new SqlParameter("@dept", depid));
                    break;
                case "bom":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_AllAssetModel");
                    break;
                case "bl":
                    AssetModels.AssetModelCode = ss[1];
                    ds = AssetModels.BomList;
                    break;
                case "bon":
                    AssetModels.AssetModelCode = ss[1];
                    ds = AssetModels.BomListLow;
                    break;
                case "bos":
                    AssetModels.AssetModelCode = ss[1];
                    ds = AssetModels.BomListLowAll;
                    break;
                case "bol":
                    AssetModels.AssetModelCode = ss[1];
                    ds = AssetModels.SparePart;
                    break;
                case "sum":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_DownTime", new SqlParameter("@option", ss[1]));
                    break;
            }
            DateTime t = DateTime.Now;
            string xlsx = text + "_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
            string fileName = Server.MapPath("~/UploadedFiles/" + xlsx);
            var newFile = new FileInfo(fileName);
            if (newFile.Exists)
            {
                newFile.Delete(); // ensures we create a new workbook
                newFile = new FileInfo(fileName);
            }
            var package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add(text);
            DataTable dt = ds != null ? ds.Tables[0] : new DataTable();
            int row = 0;
            int fieldnumber = dt.Columns.Count - 1;
            foreach (DataRow drr in dt.Rows)
            {
                row++;
                for (int col = 0; col < fieldnumber; col++)
                {
                    ws.SetValue(row, col + 1,
                                dt.Columns[col].DataType == Type.GetType("System.DateTime") &&
                                (drr[col].ToString() != "")
                                    ? ((DateTime) (drr[col])).ToString("d MMMM yyyy")
                                    : drr[col].ToString());
                }
            }

            ws.InsertRow(1, 1);
            int column;
            for (column = 0; column < fieldnumber; column++)
            {
                ws.SetValue(1, column + 1, dt.Columns[column].ColumnName.ToString(CultureInfo.InvariantCulture));
                ws.Column(column + 1).Style.Locked = false;
            }


            using (var rng = ws.Cells["A1:" + (char) (65 + fieldnumber - 1) + "1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Font.Color.SetColor(Color.White);
                rng.Style.WrapText = false;
                rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                rng.AutoFitColumns();
            }


            package.SaveAs(newFile);
            return "./UploadedFiles/" + xlsx;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string getlist(string id, string text, string depid = null)
        {
            string[] ss = id.Split('_');

            var tbl = new Table
                {
                    ID = "jsonTable",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "table table-bordered table-striped"
                };
            TableCell tc;
            DataSet ds = null;
            switch (ss[0])
            {
                case "wo":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_MWorkOrdersSelect_byStatus",
                                                  new SqlParameter("@status_id", ss[1]),
                                                  new SqlParameter("@dept", depid));
                    break;
                case "dp":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_DailyPromptStatus", new SqlParameter("@option", ss[1]),
                                                  new SqlParameter("@dept", depid));
                    break;
                case "pm":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_PMStatus", new SqlParameter("@option", ss[1]),
                                                  new SqlParameter("@dept", depid));
                    break;
                case "bom":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_AllAssetModel");
                    break;
                case "iwo":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_iwostatus", new SqlParameter("@option", ss[1]),
                                                  new SqlParameter("@dept", depid));
                    break;
                case "bl":
                    AssetModels.AssetModelCode = ss[1];
                    ds = AssetModels.BomList;
                    break;
                case "bon":
                    AssetModels.AssetModelCode = ss[1];
                    ds = AssetModels.BomListLow;
                    break;
                case "bos":
                    AssetModels.AssetModelCode = ss[1];
                    ds = AssetModels.BomListLowAll;
                    break;
                case "bol":
                    AssetModels.AssetModelCode = ss[1];
                    ds = AssetModels.SparePart;
                    break;
                case "sum":
                    ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                  "usp_mainpage_DownTime", new SqlParameter("@option", ss[1]));
                    break;
            }
            var tr = new TableRow {TableSection = TableRowSection.TableHeader};
            var dt = ds != null ? ds.Tables[0] : new DataTable();
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
                                dt.Columns[i].DataType == Type.GetType("System.DateTime")
                                    ? ((DateTime) (dr[i])).ToString("d MMMM yyyy")
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
            text = text.Replace(' ', '-');
            var htm = new HtmlGenericControl("h4") {InnerText = "Export To Excel"};
            htm.Attributes.Add("class", "btn btn-primary");
            htm.Attributes.Add("id", id + "_" + text);


            sw = new StringWriter();
            htw = new HtmlTextWriter(sw);
            htm.RenderControl(htw);
            //
            if (ss[0] == "pm" || ss[0] == "wo" || ss[0] == "dp" || ss[0] == "iwo")
            {
                string j = sw.ToString();
                var ddlDept = new DropDownList
                    {
                        ClientIDMode = ClientIDMode.Static,
                        ID = "ddlDept",
                        CssClass = "ddl"
                    };
                ddlDept.Items.Add(new ListItem("ALL", "0"));
                var ds2 = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text,
                                                       "select id,descriptions from mdepartments order by descriptions");
                if (ds2.Tables.Count > 0)
                {
                    foreach (DataRow row in ds2.Tables[0].Rows)
                    {
                        ddlDept.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                    }
                }
                if (depid != null) ddlDept.Items.FindByValue(depid).Selected = true;
                sw = new StringWriter();
                htw = new HtmlTextWriter(sw);
                ddlDept.RenderControl(htw);
                s = j + "<hr/>Department : " + sw + s;
                //  
            }
            else
            {
                s = sw + "<hr/>" + s;
            }

            return s;
        }
    }
}