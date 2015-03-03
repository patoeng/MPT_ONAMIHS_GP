using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BarcodeLib;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;
using Image = System.Web.UI.WebControls.Image;

namespace TPM
{
    public partial class PrintChecklist : Page
    {
        public TPMHelper Functions = new TPMHelper();
        public string Mode;
        public int LchecklistId;
        public string SerialNumber;
        public string M = "";
        public bool O = false; //
        public MySessions session;
        public int Checklisttypeid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Mode = Request.QueryString["mode"];
                M = (Request.QueryString["m"] == null) || (Request.QueryString["m"] == "") ? "" : "master";
                SerialNumber = Request.QueryString["v"];
                
                // Debug.WriteLine("aasasb ==>"+m);
                if (SerialNumber != null)
                {
                    session = new MySessions();
                    CreateForm();
                }
            }
        }
        protected void CreateForm()
        {
            var sqlparams = new List<SqlParameter> { new SqlParameter("@Serial_Number", SerialNumber) };
            var ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_getCheckList_master", sqlparams.ToArray());
            const string tag = "b";
            TableRow tr;
            TableCell tc;
            var dt1 = ds.Tables[1];

            foreach (DataRow dr1 in dt1.Rows)
            {
                LblForm.Text = dr1["Form_Number"].ToString();
                LblTitle.Text = "Daily";
                Checklisttypeid = 1;

                tr = new TableRow();


                var newControl = new HtmlGenericControl(tag) { InnerText = "Department" };
                tc = new TableCell();
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);
                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = dr1["Department"].ToString() };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = "Machine" };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = dr1["Asset_Number"].ToString() };

                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = "Serial Number" };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);
                var serialNumber = dr1["Serial_Code"].ToString();
                var fileName = "/" + TPMHelper.WebDirectory + "/UploadedFiles/MC_" + serialNumber + ".PNG";
                var localpathfileName = Server.MapPath(fileName);
                var file = new FileInfo(localpathfileName);
                if (file.Exists)
                {
                    file.Delete();
                }
                var barcode = new Barcode()
                {
                    //IncludeLabel = true,
                    Alignment = AlignmentPositions.CENTER,
                    Width = 500,
                    Height = 50,
                    RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                };

                System.Drawing.Image img = barcode.Encode(TYPE.CODE39, serialNumber);
                img.Save(localpathfileName);

                var imgWeb = new Image { ImageUrl = fileName };
                barcodeHolder.Controls.Add(imgWeb);
                //load image
                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = dr1["Serial_Code"].ToString() };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);

                
                //kanan atas
                tr = new TableRow();
                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = "Revision No" };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = dr1["Revision"].ToString() };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);
                TblAtasKanan.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = "Prepared By" };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = dr1["Prepared_By"].ToString() };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);
                TblAtasKanan.Rows.Add(tr);
                /*
                tr = new TableRow();
                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = "Reviewed By QC";
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = dr1["Reviewed_By_QC"].ToString();
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);
                TblAtasKanan.Rows.Add(tr);
                */
                tr = new TableRow();
                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = "Approved By" };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                newControl = new HtmlGenericControl(tag) { InnerText = dr1["Approved_By"].ToString() };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);
                TblAtasKanan.Rows.Add(tr);
            }

            var checkedBy = "";
            var dtChecked = "";
            var checklistid = "";
            var result = "";
            var dt0 = ds.Tables[0];
            foreach (DataRow dr0 in dt0.Rows)
            {
                checkedBy = "";
                dtChecked = "";
                checklistid = "0";
                result = "";
            }
            var showbutton = ((Checklisttypeid == 1) && ((session.IsLeader) || (session.IsOperator))) ||
                            ((Checklisttypeid == 2) && (session.IsEngineering));
            int iii;
            //List item Table
            var dt2 = ds.Tables[2];
            tr = new TableRow { TableSection = TableRowSection.TableHeader };
            //Create Header
            for (iii = 1; iii < dt2.Columns.Count; iii++)
            {
                var thc = new TableHeaderCell { 
                    Text = dt2.Columns[iii].ColumnName,
                    CssClass = iii==1?"first":""
                };

                tr.Cells.Add(thc);
            }
            TblListItems.Rows.Add(tr);
            if (Checklisttypeid == 2)
            {
                foreach (DataRow dr2 in dt2.Rows)
                {
                    tr = new TableRow { TableSection = TableRowSection.TableBody };
                    for (iii = 1; iii < dt2.Columns.Count - 2; iii++)
                    {
                        tc = new TableCell
                        {
                            Text = dr2[iii].ToString(),
                            CssClass = iii == 1 ? "first" : ""
                        };
                        tr.Cells.Add(tc);
                    }

                    tc = new TableCell
                        {
                            ID = dt2.Columns[iii].ColumnName + "_" + dr2[0],
                            ClientIDMode = ClientIDMode.Static,
                            Text = ""
                        };

                    tr.Cells.Add(tc);
                    TblListItems.Rows.Add(tr);
                    iii++;
                    tc = new TableCell
                        {
                            ID = dt2.Columns[iii].ColumnName + "_" + dr2[0],
                            ClientIDMode = ClientIDMode.Static,
                            Text = dr2[iii].ToString()
                        };
                  //  if ((ds.Tables[0].Rows[0]["confirmed"].ToString().ToUpper() != "TRUE") && (dr2["methode"].ToString() != "") && ((checkedBy == string.Empty) || (result == "FAIL")) && showbutton) { tc.CssClass = "rem_editable"; }
                    tr.Cells.Add(tc);
                    TblListItems.Rows.Add(tr);
                }
            }
            else
            {
                foreach (DataRow dr2 in dt2.Rows)
                {
                    tr = new TableRow { TableSection = TableRowSection.TableBody };
                    for (iii = 1; iii < dt2.Columns.Count - 1; iii++)
                    {
                        tc = new TableCell { Text = dr2[iii].ToString() ,
                                             CssClass = iii == 1 ? "first" : ""
                        };
                        tr.Cells.Add(tc);
                    }

                    tc = new TableCell
                        {
                            ID = dt2.Columns[iii].ColumnName + "_" + dr2[0].ToString(),
                            ClientIDMode = ClientIDMode.Static,
                            Text = ""
                        };
                    //if ((ds.Tables[0].Rows[0]["confirmed"].ToString().ToUpper() != "TRUE") && (M == "") && ((checkedBy == string.Empty) || (result == "FAIL")) && showbutton) { tc.CssClass = "editable"; }

                    tr.Cells.Add(tc);
                    TblListItems.Rows.Add(tr);
                }
            }
            var dt3 = ds.Tables[3];
            var jumlahimage = dt3.Rows.Count;
            const int colnumber = 2;
            var imagewidth =jumlahimage>1? 270 : 550;
            var o = 0;
            tr = new TableRow();
            foreach (DataRow dr3 in dt3.Rows)
            {
                var filename = "./UploadedFiles/" + dr3["Descriptions"];
                var img = new Image();
                System.Drawing.Image img2 = System.Drawing.Image.FromFile(Server.MapPath(filename));

                img.ImageUrl = filename;
                var ratio = img2.Width / imagewidth;
                var height = img2.Height / ratio;
               // img.Width = imagewidth;
               // img.Height = height>600? 600:height;
                img.Style.Add("vertical-align", "middle");
                img.Style.Add("text-align", "center");
                img.Style.Add("width",  imagewidth.ToString(CultureInfo.InvariantCulture) + "px");
                img.Style.Add("height", (height > 600 ? 600 : height).ToString(CultureInfo.InvariantCulture) + "px");

                tc = new TableCell();
                tc.Style.Add("vertical-align", "middle");
                tc.Style.Add("text-align", "center");
                tc.Controls.Add(img);
                tr.Cells.Add(tc);
                if (o < colnumber - 1)
                {
                    o++;
                }
                else
                {
                    TblImage.Rows.Add(tr);
                    o = 0;
                    tr = new TableRow();
                }

            }
            if (o > 0)
            {
                tr.Cells[o - 1].ColumnSpan = colnumber - o + 1;
                TblImage.Rows.Add(tr);
            }


            tr = new TableRow();
            tc = new TableCell();
            var lb2 = new Label { Text = "" };
            tc.Controls.Add(lb2);

            var hd = new HiddenField { ID = "checklistid", ClientIDMode = ClientIDMode.Static, Value = checklistid };
            tc.Controls.Add(hd);
            tr.Cells.Add(tc);

            var lb = new Label { Text = checkedBy };

            if (checkedBy == string.Empty)
            {

                hd = new HiddenField { ID = "userid", ClientIDMode = ClientIDMode.Static, Value = "" };
                tc = new TableCell();
                tc.Controls.Add(hd);

                hd = new HiddenField { ID = "mode", ClientIDMode = ClientIDMode.Static, Value = "checked" };
                tc.Controls.Add(hd);

                lb.Text = "";
                lb.Attributes.Add("id", "span_userid");
                lb.CssClass = "editable2";
                if (showbutton)
                {
                    tc.Controls.Add(lb);
                }
                tr.Cells.Add(tc);
                tr.Cells.Add(tc);
                TblConfirm.Rows.Add(tr);
            }
            else
            {
                tc = new TableCell();
                tc.Controls.Add(lb);
                tr.Cells.Add(tc);

                tc = new TableCell { ColumnSpan = 2, Text = "at " + Convert.ToDateTime(dtChecked).ToString("F") };
                tr.Cells.Add(tc);

                tc = new TableCell();
                lb = new Label { Text = "Result : " };
                lb.Style.Add("display", "inline");
                tc.Controls.Add(lb);

                var newControl = new HtmlGenericControl(tag) { InnerText = result };
                tc.Controls.Add(newControl);
                tr.Cells.Add(tc);


                TblConfirm.Rows.Add(tr);

            }

        }
    }
}