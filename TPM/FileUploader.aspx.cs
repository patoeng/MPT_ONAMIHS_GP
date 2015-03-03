using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;

namespace TPM
{
    public partial class FileUploader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpPostedFile file = Request.Files["fileUpload"];
            if ((file != null) && (file.ContentLength>0))
            {
                string fname = Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath(Path.Combine("/"+TPMHelper.WebDirectory+"/UploadedFiles/", fname)));
            } 
        }

    }
}