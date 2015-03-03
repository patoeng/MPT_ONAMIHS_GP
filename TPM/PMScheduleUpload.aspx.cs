using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using OfficeOpenXml;
using TPM.Classes;

namespace TPM
{
    public partial class PMScheduleUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected void btnUploadClick(object sender, EventArgs e)
        {
            
            
            if ((fileUpload.PostedFile != null) && (fileUpload.PostedFile.ContentLength > 0))
            {
                tblErrors.Rows.Clear();

                string  fn = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
                string saveLocation = Server.MapPath("..\\TPM\\UploadedFiles") + "\\";
                saveLocation += fn;
                var newFile = new FileInfo(saveLocation);
                if (newFile.Exists){newFile.Delete();}
                fileUpload.PostedFile.SaveAs(saveLocation);
                //var pck = new ExcelPackage(newFile);
                try
                {
                    var row = new TableRow();
                    var cell = new TableCell
                        {
                            Text = fn
                        };
                    row.Cells.Add(cell);
                    tblErrors.Rows.Add(row);

                    var outresult = new SqlParameter
                        {
                            ParameterName = "@OutResult",
                            Direction = ParameterDirection.Output,
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 1000
                        };
                            var par = new List<SqlParameter>
                                {

                                    new SqlParameter("@Uploaded_By", new MySessions().EmployeeName),
                                    new SqlParameter("@FileLink", saveLocation)

                                };


                    var o = SqlHelper.ExecuteNonQuery(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                      "usp_MPMSchedulesInsert_BULK", par.ToArray());
                    row=new TableRow();
                    cell = new TableCell
                    {
                        Text = "OK"
                    };
                    row.Cells.Add(cell);
                    tblErrors.Rows.Add(row);
                  
                    newFile.Delete();

                }
                catch (Exception ex)
                {

                    //System.Diagnostics.Debug.Write("dfasd " + ex.Message);
                    var row = new TableRow();
                    var cell = new TableCell
                        {
                            Text = ex.Message 
                        };
                    row.Cells.Add(cell);
                    tblErrors.Rows.Add(row);
                    //Note: Exception.Message returns a detailed message that describes the current exception. 
                    //For security reasons, we do not recommend that you return Exception.Message to end users in 
                    //production environments. It would be better to return a generic error message. 
                }
                finally
                {
                   
                    newFile.Delete();
                   
                }
                
            }
            else
            {
                var row = new TableRow();
                var cell = new TableCell
                {
                    Text = "Please Select a File!!!"
                };
                row.Cells.Add(cell);
                tblErrors.Rows.Add(row);
            }
        }
    }
}