using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security;
using System.Security.Cryptography;

namespace TPM
{
    public partial class StreamDownload : System.Web.UI.Page
    {
        public string url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack){
                url = Request.QueryString["u"] != null ? Request.QueryString["u"].ToString() : "";
                if (url!=""){
                    streamDownload();
                    //Response.AddHeader("Connection", "close");
                }
            }
        }
        protected void streamDownload()
        { 
              //Create a stream for the file
            Stream stream = null;

            //This controls how many bytes to read at a time and send to the client
            int bytesToRead = 10000;

            // Buffer to read bytes in chunk size specified above
            byte[] buffer = new Byte[bytesToRead];

            // The number of bytes read
            try
            {
              //Create a WebRequest to get the file
              HttpWebRequest fileReq = (HttpWebRequest) HttpWebRequest.Create(url);

              //Create a response for this request
              HttpWebResponse fileResp = (HttpWebResponse) fileReq.GetResponse();

              if (fileReq.ContentLength > 0)
                fileResp.ContentLength = fileReq.ContentLength;

                //Get the Stream returned from the response
                stream = fileResp.GetResponseStream();

                // prepare the response to the client. resp is the client Response
                var resp = HttpContext.Current.Response;

                //Indicate the type of data being sent
                resp.ContentType = "application/octet-stream";

                string fileName = System.IO.Path.GetFileName(url);
                //Name the file 
                resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

                int length;
                do
                {
                    // Verify that the client is connected.
                    if (resp.IsClientConnected)
                    {
                        // Read data into the buffer.
                        length = stream.Read(buffer, 0, bytesToRead);

                        // and write it out to the response's output stream
                        resp.OutputStream.Write(buffer, 0, length);

                        // Flush the data
                        resp.Flush();

                        //Clear the buffer
                        buffer = new Byte[bytesToRead];
                    }
                    else
                    {
                        // cancel the download if client has disconnected
                        length = -1;
                    }
                } while (length > 0); //Repeat until no data is read
            }
            finally
            {
                if (stream != null)
                {
                    //Close the input stream
                    stream.Close();
                }
            }
         }
        public static bool DownloadFileMethod(HttpContext httpContext, string filePath, long speed)
        {
            // Many changes: mostly declare variables near use
            // Extracted duplicate references to HttpContext.Response and .Request
            // also duplicate reference to .HttpMethod

            // Removed try/catch blocks which hid any problems
            var response = httpContext.Response;
            var request = httpContext.Request;
            var method = request.HttpMethod.ToUpper();
            if (method != "GET" &&
                method != "HEAD")
            {
                response.StatusCode = 501;
                return false;
            }

            if (!File.Exists(filePath))
            {
                response.StatusCode = 404;
                return false;
            }

            // Stream implements IDisposable so should be in a using block
            using (var myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var fileLength = myFile.Length;
                if (fileLength > Int32.MaxValue)
                {
                    response.StatusCode = 413;
                    return false;
                }

                var lastUpdateTiemStr = File.GetLastWriteTimeUtc(filePath).ToString("r");
                var fileName = Path.GetFileName(filePath);
                var fileNameUrlEncoded = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                var eTag = fileNameUrlEncoded + lastUpdateTiemStr;

                var ifRange = request.Headers["If-Range"];
                if (ifRange != null && ifRange.Replace("\"", "") != eTag)
                {
                    response.StatusCode = 412;
                    return false;
                }

                long startBytes = 0;

                // Just guessing, but I bet you want startBytes calculated before
                // using to calculate content-length
                var rangeHeader = request.Headers["Range"];
                if (rangeHeader != null)
                {
                    response.StatusCode = 206;
                    var range = rangeHeader.Split(new[] { '=', '-' });
                    startBytes = Convert.ToInt64(range[1]);
                    if (startBytes < 0 || startBytes >= fileLength)
                    {
                        // TODO: Find correct status code
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        response.StatusDescription =
                            string.Format("Invalid start of range: {0}", startBytes);
                        return false;
                    }
                }

                response.Clear();
                response.Buffer = false;
               
                //response.AddHeader("Content-MD5",  );
                response.AddHeader("Accept-Ranges", "bytes");
                response.AppendHeader("ETag", string.Format("\"{0}\"", eTag));
                response.AppendHeader("Last-Modified", lastUpdateTiemStr);
                response.ContentType = "application/octet-stream";
                response.AddHeader("Content-Disposition", "attachment;filename=" +
                                                            fileNameUrlEncoded.Replace("+", "%20"));
                var remaining = fileLength - startBytes;
                response.AddHeader("Content-Length", remaining.ToString());
                response.AddHeader("Connection", "Keep-Alive");
                response.ContentEncoding = Encoding.UTF8;

                if (startBytes > 0)
                {
                    response.AddHeader("Content-Range",
                                        string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                }

                // BinaryReader implements IDisposable so should be in a using block
                using (var br = new BinaryReader(myFile))
                {
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                    const int packSize = 1024 * 10; //read in block，every block 10K bytes
                    var maxCount = (int)Math.Ceiling((remaining + 0.0) / packSize); //download in block
                    for (var i = 0; i < maxCount && response.IsClientConnected; i++)
                    {
                        response.BinaryWrite(br.ReadBytes(packSize));
                        response.Flush();

                        // HACK: Unexplained sleep
                        var sleep = (int)Math.Ceiling(1000.0 * packSize / speed); //the number of millisecond
                        if (sleep > 1) Thread.Sleep(sleep);
                    }
                }
            }
            return true;
        }
    }

}