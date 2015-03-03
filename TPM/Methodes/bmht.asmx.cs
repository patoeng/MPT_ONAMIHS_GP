using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.Script.Serialization;
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
    ///     Summary description for bmht
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class bmht : WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string summary(string batchid = null, string bm1code = null, string order_number = null,
                              int isdownload = 0, string stage = null, string startdate = null, string enddate = null,
                              string ovenid = null)
        {
            if (batchid != null) batchid = batchid.Trim();
            if (bm1code != null) bm1code = bm1code.Trim();
            if (order_number != null) order_number = order_number.Trim();

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
                    new SqlParameter("@OvenID", ovenid),
                    new SqlParameter("@batchid", batchid),
                    new SqlParameter("@order_number", order_number),
                    new SqlParameter("@bm1code", bm1code),
                    new SqlParameter("@startdate", startdate),
                    new SqlParameter("@todate", enddate),
                    new SqlParameter("@ovenstatus", stage)
                };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBBMHTstring, CommandType.StoredProcedure, "searchAction",
                                                  sqlparams.ToArray());
            DataTable dt;
            if (isdownload == 0)
            {
                var tbl = new Table
                    {
                        ID = "jsonTable",
                        ClientIDMode = ClientIDMode.Static,
                        CssClass = "table table-bordered table-striped table-condensed"
                    };
                TableCell tc;
                var tr = new TableRow {TableSection = TableRowSection.TableHeader};
                dt = ds.Tables[0];
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
                var dp = new downtime.DowntimePackage();
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                tbl.RenderControl(htw);
                var s = sw.ToString();


                dp.htmltable = s;

                var htm = new HtmlGenericControl("h3");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.Attributes.Add("id", "download");
                htm.InnerText = "Export To Excel";
                sw = new StringWriter();
                htw = new HtmlTextWriter(sw);
                htm.RenderControl(htw);
                s = sw.ToString();
                //dp.htmltable = s + "<hr />" + dp.htmltable;
                s += "<hr />" + dp.htmltable;
                //var js = new JavaScriptSerializer();
                //s = js.Serialize(dp); //reuse s;
                return s;
            }
            else
            {
                //create excels file
                var t = new DateTime();
                t = DateTime.Now;
                var xlsx = "summary_bd_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
                var FileName = Server.MapPath("~/UploadedFiles/" + xlsx);
                var newFile = new FileInfo(FileName);
                if (newFile.Exists)
                {
                    newFile.Delete(); // ensures we create a new workbook
                    newFile = new FileInfo(FileName);
                }
                var package = new ExcelPackage();
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("BM Heatreatment Summary");
                dt = ds.Tables[0];
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
                                        ? (col == 4 || col == 6
                                               ? ((DateTime) (drr[col])).ToString("HH:mm:ss")
                                               : ((DateTime) (drr[col])).ToString("d MMMM yyyy"))
                                        : drr[col].ToString());
                    }
                }

                ws.InsertRow(1, 1);
                int column = 0;
                for (column = 0; column < fieldnumber; column++)
                {
                    ws.SetValue(1, column + 1, dt.Columns[column].ColumnName.ToString(CultureInfo.InvariantCulture));
                    ws.Column(column + 1).Style.Locked = false;
                }


                using (ExcelRange rng = ws.Cells[1, 1, 1, fieldnumber])
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
                ws.InsertRow(1, 1);
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "To");
                ws.SetValue(1, 2, enddate);
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "From");
                ws.SetValue(1, 2, startdate);


                package.SaveAs(newFile);
                return "./UploadedFiles/" + xlsx;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string summary2(int isdownload = 0, string range = null, string startdate = null, string enddate = null,
                              string ovenid = null)
        {          
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@OvenID", ovenid),
                    new SqlParameter("@startdate", startdate),
                    new SqlParameter("@todate", enddate),
                    new SqlParameter("@range",range)
                };
            var ds = SqlHelper.ExecuteDataset(TPMHelper.DBBMHTstring, CommandType.StoredProcedure, "searchQTY",
                                                  sqlparams.ToArray());
            DataTable dt;
            if (isdownload == 0)
            {
                var tbl = new Table
                {
                    ID = "jsonTable",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "table table-bordered table-striped table-condensed"
                };
                TableCell tc;
                var tr = new TableRow { TableSection = TableRowSection.TableHeader };
                dt = ds.Tables[0];
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
                                dt.Columns[i].DataType == Type.GetType("System.DateTime") &&
                                (dr[i].ToString() != "")
                                    ? (i == 3 || i == 5
                                           ? ((DateTime)(dr[i])).ToString("HH:mm:ss")
                                           : ((DateTime)(dr[i])).ToString("d MMMM yyyy"))
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
                var dp = new downtime.DowntimePackage();
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                tbl.RenderControl(htw);
                var s = sw.ToString();


                dp.htmltable = s;

                var htm = new HtmlGenericControl("h3");
                htm.Attributes.Add("class", "btn btn-primary");
                htm.Attributes.Add("id", "download");
                htm.InnerText = "Export To Excel";
                sw = new StringWriter();
                htw = new HtmlTextWriter(sw);
                htm.RenderControl(htw);
                s = sw.ToString();
                //dp.htmltable = s + "<hr />" + dp.htmltable;
                s += "<hr />" + dp.htmltable;
                //var js = new JavaScriptSerializer();
                //s = js.Serialize(dp); //reuse s;
                return s;
            }
            else
            {
                //create excels file
                var t = new DateTime();
                t = DateTime.Now;
                var xlsx = "summary_bd_" + t.ToString("yyyyddMMHHmmss") + ".xlsx";
                var FileName = Server.MapPath("~/UploadedFiles/" + xlsx);
                var newFile = new FileInfo(FileName);
                if (newFile.Exists)
                {
                    newFile.Delete(); // ensures we create a new workbook
                    newFile = new FileInfo(FileName);
                }
                var package = new ExcelPackage();
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("BM Heatreatment Summary");
                dt = ds.Tables[0];
                int row = 0;
                int fieldnumber = dt.Columns.Count;// -1;
                foreach (DataRow drr in dt.Rows)
                {
                    row++;
                    for (int col = 0; col < fieldnumber; col++)
                    {
                        ws.SetValue(row, col + 1,
                                    dt.Columns[col].DataType == Type.GetType("System.DateTime") &&
                                    (drr[col].ToString() != "")
                                        ? (col == 3 || col == 5
                                               ? ((DateTime)(drr[col])).ToString("HH:mm:ss")
                                               : ((DateTime)(drr[col])).ToString("d MMMM yyyy"))
                                        : drr[col].ToString());
                    }
                }

                ws.InsertRow(1, 1);
                int column = 0;
                for (column = 0; column < fieldnumber; column++)
                {
                    ws.SetValue(1, column + 1, dt.Columns[column].ColumnName.ToString(CultureInfo.InvariantCulture));
                    ws.Column(column + 1).Style.Locked = false;
                }


                using (ExcelRange rng = ws.Cells[1, 1, 1, fieldnumber])
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
                ws.InsertRow(1, 1);
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "To");
                ws.SetValue(1, 2, enddate);
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "From");
                ws.SetValue(1, 2, startdate);


                package.SaveAs(newFile);
                return "./UploadedFiles/" + xlsx;
            }
        }
    }
}