using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using TPM.Classes;

namespace TPM
{
    public partial class YCheckListImg : System.Web.UI.Page
    {
        public string postbacks = "";
        public string  ddlvalue = "";
        public TPMHelper Functions = new TPMHelper();
        public string delete = "";
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
           
                if (session.IsAdministrator)
                {
                    if (!IsPostBack)
                    {
                        ddlvalue = Request.QueryString["i"] != null ? Request.QueryString["i"].ToString() : "0";
                        delete = Request.QueryString["iid"] != null ? Request.QueryString["iid"].ToString() : "";
                        imageTbl();
                    }
                    else
                    {
                        ddlvalue = ddlvalues.Value;

                    }
                }
                else
                {
                    Server.Transfer(session.redirection);
                }
        }
        public void imageTbl()
        {
            TblImage.Rows.Clear();
            ddlChecklist.Items.Clear();
            if (delete != "")
            {
                int dss = SqlHelper.ExecuteNonQuery(Functions.TPMDBConnection(), CommandType.Text, "DELETE MChecklistImages Where ID='"+delete+"'");
            }
            DataSet ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.Text, "SELECT * FROM MCheckListImages Where Check_list_id='" + ddlvalue + "' ORDER BY id");
            DataTable dt = ds.Tables[0];
            ddlvalues.Value = ddlvalue;
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            tr.TableSection = TableRowSection.TableHeader;
            tc = new TableHeaderCell();
            tc.Text = "Images";
            tr.Cells.Add(tc);
            TblImage.Rows.Add(tr);
            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                tc = new TableCell();
                Image img = new Image();
                System.Drawing.Image img2 = System.Drawing.Image.FromFile(Server.MapPath("./UploadedFiles/" + dr["Descriptions"].ToString()));
                float ratio = img2.Width / 250;
                img.ImageUrl = "./UploadedFiles/" + dr["Descriptions"].ToString();
                img.Height = (int)((float)img2.Height / ratio);
                img.Width = (int)((float)img2.Width / ratio);
                tc.Controls.Add(img);
                tr.Cells.Add(tc);




                System.Web.UI.HtmlControls.HtmlGenericControl link = new System.Web.UI.HtmlControls.HtmlGenericControl("a");
                link.Attributes.Add("role", "button");
                link.Attributes.Add("href", "?i=" + ddlvalue + "&iid=" + dr["id"].ToString());
                link.InnerHtml = "Remove This Image";
                link.Attributes.Add("class", "btn btn-primary");
                tc.Controls.Add(link);
                tr.Cells.Add(tc);
                TblImage.Rows.Add(tr);

            }
            ds.Dispose();
            ds = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.Text, "SELECT ID,DESCRIPTIONS FROM MCheckLists");
            dt = ds.Tables[0];
            
            foreach (DataRow dr in dt.Rows) {
                ddlChecklist.Items.Add(new ListItem(dr["descriptions"].ToString(), dr["id"].ToString()));
                
            }
            if (ddlChecklist.Items.FindByValue(ddlvalue) != null) { ddlChecklist.Items.FindByValue(ddlvalue).Selected = true; }
            
        }
        
    
        public void Submit1_ServerClick(object sender, System.EventArgs e)
        {
            
            if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            {
                string extenstion = System.IO.Path.GetExtension(File1.PostedFile.FileName);
                string SaveLocation = Server.MapPath("..\\TPM\\UploadedFiles") + "\\";
                try
                {
                    string fn = "K_"+ddlvalue + DateTime.Now.ToString("yyyyddMMHHmmss") + extenstion;
                    SaveLocation += fn;
                    System.Diagnostics.Debug.Write("dfasd fff" +SaveLocation);
                    File1.PostedFile.SaveAs(SaveLocation);
                    List<SqlParameter> sqlparams = new List<SqlParameter>();
                    sqlparams.Add(new SqlParameter("@id", DBNull.Value));
                    sqlparams.Add(new SqlParameter("@done_by", new MySessions().EmployeeNo));
                    sqlparams.Add(new SqlParameter("@Active", '1'));
                    sqlparams.Add(new SqlParameter("@Check_List_id", Convert.ToDecimal(ddlChecklist.SelectedItem.Value)));
                    sqlparams.Add(new SqlParameter("@Descriptions", fn));
                    DataSet dss = SqlHelper.ExecuteDataset(Functions.TPMDBConnection(), CommandType.StoredProcedure, "usp_MCheckListImagesInsert", sqlparams.ToArray());
                    
                }
                catch (Exception ex)
                {
                    postbacks = "Error: " + ex.Message;
                    System.Diagnostics.Debug.Write("dfasd " + ex.Message);
                    //Note: Exception.Message returns a detailed message that describes the current exception. 
                    //For security reasons, we do not recommend that you return Exception.Message to end users in 
                    //production environments. It would be better to return a generic error message. 
                }
            }
            else
            {
                postbacks = "Please select a file to upload.";
            }
            Response.Redirect(Request.Url.AbsolutePath + "?i=" + ddlvalue);
        }
    }
}