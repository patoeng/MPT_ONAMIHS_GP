using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace TPM
{
    public partial class MChecklist : System.Web.UI.Page
    {
        public TPMHelper Functions = new TPMHelper();
        public string mode;
        public int lchecklistId;
        public int AssetId;
        public string m = "";
        public bool checklist_exist = false;
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            { 
                if (session.IsAdministrator)
                {
                    mode = Request.QueryString["mode"];
                    m = (Request.QueryString["m"] == null) || (Request.QueryString["m"] == "") ? "" : "master";
                    AssetId = Convert.ToInt32(Request.QueryString["assetid"]);
                    lchecklistId = Convert.ToInt32(Request.QueryString["id"]);
                    Debug.WriteLine("aasasb ==>" + m);
                    CreateForm();
                }
                else
                {
                    Server.Transfer(session.redirection);
                }

            }

        }
        protected void CreateForm()
        {
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter("@CheckListId", lchecklistId));
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_getMasterCheckList", sqlparams.ToArray());
            string tag = "b";
            TableRow tr = null;
            TableCell tc = null;
            DataTable dt1 = ds.Tables[1];
            selectChecklist.Items.Add(new ListItem("Please Select ...", ""));
           
                sqlparams.Clear();
                sqlparams.Add(new SqlParameter("@id", DBNull.Value));
                DataSet dsss = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MChecklistsSelect", sqlparams.ToArray());
                foreach (DataRow drm in dsss.Tables[0].Rows)
                {
                    selectChecklist.Items.Add(new ListItem(drm["descriptions"].ToString(), drm["id"].ToString()));
                }




            
            foreach (DataRow dr1 in dt1.Rows)
            {
                checklist_exist = true;
                LblForm.Text = dr1["Form_Number"].ToString();
                LblTitle.Text = dr1["Check_List_Type"].ToString();
                int checklisttypeid = (int)dr1["ChecklistType_id"];

                tr = new TableRow();
                tc = new TableCell();

                HtmlGenericControl NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = "Department";
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);
                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = dr1["Department"].ToString();
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = "Machine";
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                if (m != string.Empty)
                {

                    selectChecklist.Items.FindByValue(dr1["id"].ToString()).Selected = true;

                    NewControl.InnerText = dr1["descriptions"].ToString();
                }
                else
                {

                    NewControl.InnerText = dr1["Asset_Number"].ToString();

                }
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = "Month";
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                DateTime date = DateTime.Now;
                NewControl = new HtmlGenericControl(tag);
                if (checklisttypeid == 1)
                {
                    NewControl.InnerText = date.ToString("d MMM yyyy");
                }
                else
                {
                    NewControl.InnerText = date.ToString("MMMM");
                }
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);
                TblAtasKiri.Rows.Add(tr);

                //kanan atas
                tr = new TableRow();
                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = "Revision No";
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = dr1["Revision"].ToString();
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);
                TblAtasKanan.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = "Prepared By";
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = dr1["Prepared_By"].ToString();
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);
                TblAtasKanan.Rows.Add(tr);

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

                tr = new TableRow();
                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = "Approved By";
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);

                tc = new TableCell();
                NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = dr1["Approved_By"].ToString();
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);
                TblAtasKanan.Rows.Add(tr);
            }

            int iii;
            //List item Table
            DataTable dt2 = ds.Tables[2];
            tr = new TableRow();
            TableHeaderCell thc = new TableHeaderCell();
            tr.TableSection = TableRowSection.TableHeader;
            //Create Header
            for (iii = 1; iii < dt2.Columns.Count; iii++)
            {
                thc = new TableHeaderCell();
                thc.Text = dt2.Columns[iii].ColumnName.ToString();
                tr.Cells.Add(thc);
            }
            TblListItems.Rows.Add(tr);

            foreach (DataRow dr2 in dt2.Rows)
            {
                tr = new TableRow();
                tr.TableSection = TableRowSection.TableBody;
                for (iii = 1; iii < dt2.Columns.Count - 1; iii++)
                {
                    tc = new TableCell();
                    tc.Text = dr2[iii].ToString();
                    tr.Cells.Add(tc);
                }
                tc = new TableCell();
                tc.ID = dt2.Columns[iii].ColumnName.ToString() + "_" + dr2[0].ToString();
                tc.ClientIDMode = ClientIDMode.Static;
                //if ((ds.Tables[0].Rows[0]["Check_By_Operator"].ToString() == string.Empty) && (m == "")) { tc.CssClass = "editable"; }
                tc.CssClass = "editable";
                tc.Text = dr2[iii].ToString() == "" ? "" : ((dr2[iii].ToString().ToUpper() == "TRUE") ? "OK" : "NC");
                tr.Cells.Add(tc);
                TblListItems.Rows.Add(tr);
            }
            DataTable dt3 = ds.Tables[3];
            int rownumber = dt3.Rows.Count;
            Image img = null;
            System.Drawing.Image img2 = null;
            int colnumber = 2;
            int imagewidth = 250;
            int o = 0;
            tr = new TableRow();
            foreach (DataRow dr3 in dt3.Rows)
            {
                string filename = "./UploadedFiles/" + dr3["Descriptions"];
                img = new Image();
                img2 = System.Drawing.Image.FromFile(Server.MapPath(filename));

                img.ImageUrl = filename;
                float ratio = img2.Width / imagewidth;
                float height = img2.Height / ratio;
                img.Width = imagewidth;
                img.Height = (int)height;

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

            string checked_by = "";
            string DT_Checked = "";
            string confirmed_by = "";
            string DT_Confirmed = "";
            string checklistid = "";
            string result = "";
            string confirmed = "";
            string status = "";
            
            DataTable dt0 = ds.Tables[0];
            foreach (DataRow dr0 in dt0.Rows)
            {
                checked_by = dr0["Check_By_Operator"].ToString();
                DT_Checked = dr0["Checked_Date"].ToString();
                confirmed_by = dr0["Confirmed_By_Leader"].ToString(); ;
                DT_Confirmed = dr0["Confirmed_Date"].ToString(); ;
                checklistid = dr0["id"].ToString();
                result = dr0["result"].ToString().ToUpper() == "TRUE" ? "PASS" : "FAIL";
                confirmed = dr0["confirmed"].ToString().ToUpper() == "TRUE" ? "CONFIRMED" : "NOT CONFIRMED";
                status = dr0["status"].ToString().ToUpper() == "TRUE" ? "CLOSED" : "OPEN";
            }
            tr = new TableRow();
            tc = new TableCell();
            Label lb2 = new Label();
            lb2.Text = "Checked By Operator";
            tc.Controls.Add(lb2);

            HiddenField hd = new HiddenField();
            hd.ID = "checklistid";
            hd.ClientIDMode = ClientIDMode.Static;
            hd.Value = checklistid;
            tc.Controls.Add(hd);
            tr.Cells.Add(tc);

            Label lb = new Label();
            lb.Text = checked_by;

            if (checked_by == string.Empty)
            {
                hd = new HiddenField();
                hd.ID = "userid";
                hd.ClientIDMode = ClientIDMode.Static;
                hd.Value = new MySessions().EmployeeNo;
                tc = new TableCell();
                tc.Controls.Add(hd);

                hd = new HiddenField();
                hd.ID = "mode";
                hd.ClientIDMode = ClientIDMode.Static;
                hd.Value = "checked";
                tc.Controls.Add(hd);

                lb.Text = hd.Value;
                tc.Controls.Add(lb);
                tr.Cells.Add(tc);

                Button btn1 = new Button();
                btn1.CssClass = "btn btn-primary";
                btn1.ID = "btnSubmit";
                btn1.ClientIDMode = ClientIDMode.Static;
                btn1.Text = "Submit The Form";
                tc = new TableCell();
                tc.Controls.Add(btn1);
                tr.Cells.Add(tc);
                TblConfirm.Rows.Add(tr);
            }
            else
            {
                tc = new TableCell();
                tc.Controls.Add(lb);
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.ColumnSpan = 2;
                tc.Text = "at " + Convert.ToDateTime(DT_Checked).ToString("F");
                tr.Cells.Add(tc);

                tc = new TableCell();
                lb = new Label();
                lb.Text = "Result : ";
                lb.Style.Add("display", "inline");
                tc.Controls.Add(lb);

                HtmlGenericControl NewControl = new HtmlGenericControl(tag);
                NewControl.InnerText = result;
                tc.Controls.Add(NewControl);
                tr.Cells.Add(tc);


                TblConfirm.Rows.Add(tr);
                if ((result == "FAIL"))
                {
                    if ((status == "OPEN"))
                    {
                        tr = new TableRow();
                        tc = new TableCell();
                        lb2 = new Label();
                        lb2.Text = "Confirmed By Leader/CPS";
                        tc.Controls.Add(lb2);
                        tr.Cells.Add(tc);

                        lb = new Label();
                        lb.Text = confirmed_by;
                        if (confirmed_by == string.Empty)
                        {
                            hd = new HiddenField();
                            hd.ID = "userid";
                            hd.ClientIDMode = ClientIDMode.Static;
                            hd.Value = "Session User";
                            tc = new TableCell();
                            tc.Controls.Add(hd);
                            hd = new HiddenField();
                            hd.ID = "mode";
                            hd.ClientIDMode = ClientIDMode.Static;
                            hd.Value = "confirmed";
                            tc.Controls.Add(hd);

                            lb.Text = "Session User";
                            tc.Controls.Add(lb);
                            tr.Cells.Add(tc);

                            Button btn1 = new Button();
                            btn1.CssClass = "btn btn-primary";
                            btn1.ID = "btnConfirm";
                            btn1.ClientIDMode = ClientIDMode.Static;
                            btn1.Text = "Confirm The Form";
                            tc = new TableCell();
                            tc.Controls.Add(btn1);
                            tr.Cells.Add(tc);
                            tc = new TableCell();
                            btn1 = new Button();
                            btn1.CssClass = "btn btn-primary";
                            btn1.ID = "btnNotConfirm";
                            btn1.ClientIDMode = ClientIDMode.Static;
                            btn1.Text = "Not Confirm The Form";
                            tc.Controls.Add(btn1);

                            tr.Cells.Add(tc);
                            tc = new TableCell();
                            tc.Text = "&nbsp;";
                            tr.Cells.Add(tc);
                            TblConfirm.Rows.Add(tr);

                        }


                    }
                    else
                    {
                        tr = new TableRow();
                        tc = new TableCell();
                        lb2 = new Label();
                        lb2.Text = confirmed + " By Leader/CPS";
                        tc.Controls.Add(lb2);
                        tr.Cells.Add(tc);

                        tc = new TableCell();
                        lb = new Label();
                        lb.Text = "Session User";
                        tc.Controls.Add(lb);
                        tr.Cells.Add(tc);

                        tc = new TableCell();
                        tc.ColumnSpan = 2;
                        tc.Text = "at " + Convert.ToDateTime(DT_Confirmed).ToString("F");
                        tr.Cells.Add(tc);

                        tc = new TableCell();
                        tc.Text = "&nbsp;";
                        tr.Cells.Add(tc);
                        TblConfirm.Rows.Add(tr);


                    }
                }
            }

        }
    }
}