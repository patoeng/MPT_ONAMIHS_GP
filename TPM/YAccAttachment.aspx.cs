using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class YAccAttachment : System.Web.UI.Page
    {
        private string _iwokey;
        private int _status;
        private MySessions _ms;
        protected void Page_Load(object sender, EventArgs e)
        {
            _ms = new MySessions();
            _iwokey = Request.QueryString["v"] ?? "";
            _status =Convert.ToInt16(Request.QueryString["s"] ?? "0");
            if (!IsPostBack)
            {
                lblTittle.InnerHtml = "Accident Report Attachments key=#" + _iwokey;
                ShowAttachment();
                if (!_ms.IsLeader)
                {
                    containerku.Visible = false;
                }
            }
        }

        private void ShowAttachment()
        {
           
            var dss = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                _ms.IsLeader ? "usp_AccAttachmentsSelect" : "usp_AccAttachmentsSelect2", 
                                                       new SqlParameter("@acckey", _iwokey));
            var dt = dss.Tables[0];

           
            var tr = new TableRow {TableSection = TableRowSection.TableHeader};
         
            for (int i=0;i<dt.Columns.Count-1;i++)
            {
                var th = new TableHeaderCell {Text = dt.Columns[i].ColumnName};
                tr.Cells.Add(th);
            }
            TblImage.Rows.Add(tr);

            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                tr.Attributes.Add("id", "img_"+dr[dt.Columns.Count-1]);
                for (int i = 0; i < dt.Columns.Count-1; i++)
                {
                    var tc = new TableCell {Text = dr[i].ToString()};
                         tr.Cells.Add(tc);
                }

                TblImage.Rows.Add(tr);
            }
        }

        protected void Submit1_ServerClick(object sender, EventArgs e)
        {
            var error = "";
            if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            {
                var filename = File1.PostedFile.FileName;
                var extenstion = System.IO.Path.GetExtension(filename);
                var saveLocation = Server.MapPath("..\\TPM\\UploadedFiles") + "\\";
                try
                {
                    string fn = "AL_" + DateTime.Now.ToString("yyyyddMMHHmmss") + extenstion;
                    saveLocation += fn;
                    System.Diagnostics.Debug.Write("dfasd fff" + saveLocation);
                    File1.PostedFile.SaveAs(saveLocation);
                    /*[dbo].[usp_IWOAttachmentsInsert] 
                            @iwokey nvarchar(50),
                            @realname nvarchar(1000)=NULL,
                            @savedas nvarchar(1000)=NULL,
                            @done_by nvarchar(50)=NULL,
                            @Remarks nvarchar(1000)=NULL,
                            @attachment_id as Bigint Output               
                     */
                    var sqlparams = new List<SqlParameter>
                        {
                            new SqlParameter("@acckey", _iwokey),
                            new SqlParameter("@done_by", new MySessions().EmployeeName),
                            new SqlParameter("@realname", filename),
                            new SqlParameter("@savedas", fn),
                            new SqlParameter("@remarks", txtRemarks.Text)
                        };
                    var dss = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                       "usp_AccAttachmentsInsert", sqlparams.ToArray());

                    
                }
                catch (Exception ex)
                {
                    //postbacks = "Error: " + ex.Message;
                    System.Diagnostics.Debug.Write("dfasd " + ex.Message);
                    error = ex.Message;
                    //Note: Exception.Message returns a detailed message that describes the current exception. 
                    //For security reasons, we do not recommend that you return Exception.Message to end users in 
                    //production environments. It would be better to return a generic error message. 
                }
            }
            else
            {
                //postbacks = "Please select a file to upload.";
            }
            Response.Redirect(Request.Url.AbsolutePath + "?v=" + _iwokey + "&e=" + error);
        }


        protected void GotoClick(object sender, EventArgs e)
        {
            Response.Redirect("FAccident.aspx?v="+_iwokey);
        }
    }
}