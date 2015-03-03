using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using TPM.Classes;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.DataValidation;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Net;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for Bulk
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Bulk : System.Web.Services.WebService
    {
        TPMHelper Helper= new TPMHelper();

        
        public MySessions session;
        public string CreateXlsx(string TableName, Boolean lists, int prepare )
        {

            bool isBOMtable = TableName.ToUpper() == "MBOMS" ? true : false;

            var sql = new SqlParameter("@ForeignColNum", 0)
                {
                    Direction = ParameterDirection.Output
                };
                var sqlparam = new List<SqlParameter>();
                sqlparam.Add(new SqlParameter("@id",DBNull.Value));
                sqlparam.Add(new SqlParameter("@TableName",TableName));
                sqlparam.Add(new SqlParameter("@Active",DBNull.Value));
                sqlparam.Add(sql);

                var usp = lists ? "usp_SelectPrepareCRUD" : "usp_SelectPrepareCRUDlessdata";
                var dslsr = SqlHelper.ExecuteDataset(Helper.TPMDBConnection(), CommandType.StoredProcedure, usp, sqlparam.ToArray());
                var ForeignColNumSS = sql.SqlValue.ToString();
                int ForeignColNum = Convert.ToInt16(ForeignColNumSS);
                
                var t = new DateTime();
                t = DateTime.Now;
                string xlsx = TableName+"_"+t.ToString("yyyyddMMHHmmss")+".xlsx";
                string FileName = Server.MapPath("~/UploadedFiles/"+xlsx);
                var newFile = new FileInfo(FileName);
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(FileName);
                }
                var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add(TableName);
                var FieldInfoTbl = dslsr.Tables[0];
                
                var Tbl = dslsr.Tables[ForeignColNum+1];
                var row = 1;
                foreach (DataRow drr  in FieldInfoTbl.Rows)
                {
                    ws.SetValue(row,1,drr[0].ToString());
                    ws.SetValue(row, 2, drr[1].ToString());
                    ws.Row(row).Hidden = true;
                    row++;
                }
                var fieldnumber = row;
                foreach (DataRow dr in Tbl.Rows )
                {
                    
                    int col ;
                    for (col = 1; col <= Tbl.Columns.Count; col++)
                    {

                        if(Tbl.Columns[col-1].DataType == Type.GetType("System.Int32")){
                           ws.SetValue(row, col,Convert.ToDecimal(dr[col - 1].ToString()));
                        }else{
                            ws.SetValue(row, col,(dr[col - 1].ToString()));
                        }

                       
                    }
                    ws.SetValue(row, col, "Ignore");
                    row++;
                }

                ws.InsertRow(fieldnumber, 1);
                int column;
                for (column = 0; column < Tbl.Columns.Count; column++)
                {

                    ws.Cells[(char)(65 + column) + fieldnumber.ToString(CultureInfo.InvariantCulture)].Value = TableName.ToUpper() == "DAILYPROMPTPOPUP" ? (Tbl.Columns[column].ColumnName.ToUpper() == "ASSET" ? "Machine Name" : Tbl.Columns[column].ColumnName) : Tbl.Columns[column].ColumnName;
                     ws.Column(column + 1).Style.Locked = false;
                }
                ws.Cells[(char)(65 + column + 1) + fieldnumber.ToString(CultureInfo.InvariantCulture)].Value = "Options";
                ws.Column(column + 1).Style.Locked =  false;
                ws.Column(column + 1).Hidden = true;
                ws.Cells[(char)(65 + column) + fieldnumber.ToString(CultureInfo.InvariantCulture)].Value = "Database Action";
                ws.Column(column).Style.Locked =  false;

                using (var rng = ws.Cells[(char)(65) + fieldnumber.ToString() + ":" + (char)(65 + Tbl.Columns.Count + 1) + fieldnumber.ToString(CultureInfo.InvariantCulture)])
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

                row += prepare;
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "ForeignField");
                ws.SetValue(1, 2, ForeignColNum);
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "FieldNumber");
                ws.SetValue(1, 2, Tbl.Columns.Count);
                ws.InsertRow(1, 1);
                ws.SetValue(1, 1, "TableName");
                ws.SetValue(1, 2, TableName);
                ws.Row(1).Hidden = true;
                ws.Row(2).Hidden = true;
                ws.Row(3).Hidden = true;


                int datarowstart = 4 + fieldnumber;
                // add a validation and set values
                var karakter = (char)(65 + 1 + Tbl.Columns.Count);
                var karakter2 = (char)(65 + Tbl.Columns.Count);
                var validation = ws.DataValidations.AddListValidation(karakter2 + (datarowstart).ToString() + ":" + karakter2 + (row + datarowstart-fieldnumber ).ToString());//24 = num
                // Alternatively:
                // var validation = sheet.Cells["A1"].DataValidation.AddListDataValidation();
                validation.ShowErrorMessage = true;
                validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                validation.ErrorTitle = "An invalid value was entered";
                validation.Error = "Select a value from the list";
                validation.Formula.ExcelFormula = "Options!A1:A6";

                var validationInt = ws.DataValidations.AddDecimalValidation("A" + (datarowstart).ToString() + ":A" + (row + datarowstart - fieldnumber).ToString());//24 = num
                validationInt.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                validationInt.PromptTitle = "Enter an integer value here";
                validationInt.Prompt = "Value should be greater Than 0";
                validationInt.ShowInputMessage = true;
                validationInt.ErrorTitle = "An invalid value was entered";
                validationInt.Error = "Value should be greater Than 0";
                validationInt.ShowErrorMessage = true;
                validationInt.Operator = ExcelDataValidationOperator.greaterThan;
                validationInt.Formula.Value = 0;
                //validationInt.Formula2.Value = 5;
                
                

                validation = ws.DataValidations.AddListValidation("B" + (datarowstart).ToString() + ":B" + (row + datarowstart-fieldnumber).ToString());//24 = num
                validation.ShowErrorMessage = true;
                validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                validation.ErrorTitle = "An invalid value was entered";
                validation.Error = "Select a value from the list";
                validation.Formula.ExcelFormula = "Options!D1:D2";

                ws.Cells[karakter +datarowstart.ToString() +":"+ karakter +(row+datarowstart-fieldnumber).ToString()].Formula = "IF(ISERROR(VLOOKUP(" + karakter2 +datarowstart.ToString() +",Options!$A$1:$B$6,2,FALSE)),\"\",VLOOKUP(" + karakter2 +datarowstart.ToString() +",Options!$A$1:$B$6,2,FALSE))";
                ws.Row(datarowstart - 1).Style.Locked = true;
                ws.Protection.SetPassword(TableName);
                ws.Protection.IsProtected = true;
                ws.Cells.AutoFitColumns();
                ws.Column(Tbl.Columns.Count + 2).Hidden = true;
         
                var wss = package.Workbook.Worksheets.Add("Options");
                var bo = new BulkOptions();
                for (int a = 0; a < bo.Count; a++)
                {
                    wss.SetValue(a+1, 1,bo[a]);
                    wss.SetValue(a+1, 2, a);
                }
                wss.SetValue(1, 4, "True");
                wss.SetValue(2, 4, "False");
                wss.Hidden = eWorkSheetHidden.Hidden;
                
               //special table MBOMS
                if (isBOMtable) {
                    var dss = MySqlHelper.ExecuteDataset(Helper.TInventoryConnection(),"select code,name from product order by code");
                    var dtt = dss.Tables[0];
                    var ws1 = package.Workbook.Worksheets.Add("inventory");
                    int b = 1;
                    foreach (DataRow drr in dtt.Rows)
                    {
                        ws1.SetValue(b, 1, drr["code"].ToString());
                        ws1.SetValue(b, 2, drr["name"].ToString());
                        ws1.SetValue(b, 3, drr["code"].ToString());
                        b++;
                    }
                    ws1.Hidden = eWorkSheetHidden.Hidden;
                    validation = ws.DataValidations.AddListValidation((char)(67 + ForeignColNum) + (datarowstart).ToString() + ":" + (char)(67 + ForeignColNum) + (row + datarowstart - fieldnumber).ToString());//24 = num
                    validation.ShowErrorMessage = true;
                    validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validation.ErrorTitle = "An invalid value was entered";
                    validation.Error = "Select a value from the list";
                    validation.Formula.ExcelFormula = "inventory!A1:A"+(b-1).ToString();


                    validation = ws.DataValidations.AddListValidation((char)(68 + ForeignColNum) + (datarowstart).ToString() + ":" + (char)(68 + ForeignColNum) + (row + datarowstart - fieldnumber).ToString());//24 = num
                    validation.ShowErrorMessage = true;
                    validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validation.ErrorTitle = "An invalid value was entered";
                    validation.Error = "Select a value from the list";
                    validation.Formula.ExcelFormula = "inventory!B1:B" + (b - 1).ToString();
                    karakter2 = (char) (67 + ForeignColNum);
                    karakter = (char)(68 + ForeignColNum);
                    ws.Cells[karakter2 + datarowstart.ToString() + ":" + karakter2 + (row + datarowstart - fieldnumber).ToString()].Formula = 
                        "IF(ISERROR(VLOOKUP(" + karakter + datarowstart.ToString() + "," + "inventory" + "!$B$1:$C$" + (b).ToString() + ",2,FALSE)),\"\",VLOOKUP(" 
                            + karakter + datarowstart.ToString() + "," +  "inventory" + "!$B$1:$C$" + (b).ToString() + ",2,FALSE))";
                    /* karakter = (char)(67 + ForeignColNum);
                    karakter2 = (char)(68 + ForeignColNum);
                    ws.Cells[karakter2 + datarowstart.ToString() + ":" + karakter2 + (row + datarowstart - fieldnumber).ToString()].Formula =
                        "IF(ISERROR(VLOOKUP(" + karakter + datarowstart.ToString() + "," + "inventory" + "!$A$1:$B$" + (b).ToString() + ",2,FALSE)),\"\",VLOOKUP("
                            + karakter + datarowstart.ToString() + "," + "inventory" + "!$A$1:$B$" + (b).ToString() + ",2,FALSE))";
                
                     */
                 }
               
                int i = 0;
                var foreignfieldrownumber = new List<int>();
                var fields = dslsr.Tables[0];
                foreach (DataRow dr in fields.Rows)
                {
                   
                   
                    if (dr[4] != DBNull.Value)
                    {
                        string foreigntablename = dr[5].ToString();
                        var foreigntable = package.Workbook.Worksheets.Add(foreigntablename);
                        DataTable dt = dslsr.Tables[i + 1];
                        for (int a = dt.Columns.Count; a >0 ; a--)
                        {
                            foreigntable.SetValue(1, a, dt.Columns[dt.Columns.Count - a].ColumnName.ToString());
                        }
                        foreignfieldrownumber.Add(0);
                        foreach(DataRow dr2 in dt.Rows)
                        {
                            foreignfieldrownumber[i]++;
                            foreigntable.SetValue(foreignfieldrownumber[i]+1, 2, dr2[0]);
                            foreigntable.SetValue(foreignfieldrownumber[i]+1, 1, dr2[1]);
                         }
  //                    
                        foreigntable.Hidden = eWorkSheetHidden.Hidden;
                        // add a validation and set values
                        karakter2 = (char)(65 + i + 2);
                        karakter = (char)(65 + Tbl.Columns.Count-ForeignColNum+ i );

                        validation = ws.DataValidations.AddListValidation(karakter + datarowstart.ToString()+":" + karakter + (row + datarowstart-fieldnumber).ToString());//24 = num
                        // Alternatively:
                        // var validation = sheet.Cells["A1"].DataValidation.AddListDataValidation();
                        validation.ShowErrorMessage = true;
                        validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                        validation.ErrorTitle = "An invalid value was entered";
                        validation.Error = "Select a value from the list";
                        validation.Formula.ExcelFormula = foreigntablename+"!A2:A"+(foreignfieldrownumber[i]+1).ToString();
                        ws.Cells[karakter2 + datarowstart.ToString()+":" + karakter2 + (row + datarowstart-fieldnumber).ToString()].Formula = "IF(ISERROR(VLOOKUP(" + karakter + datarowstart.ToString() + "," + foreigntablename + "!$A$2:$B$" + (foreignfieldrownumber[i] + 1).ToString() + ",2,FALSE)),\"\",VLOOKUP(" + karakter + datarowstart.ToString() + "," + foreigntablename + "!$A$2:$B$" + (foreignfieldrownumber[i] + 1).ToString() + ",2,FALSE))";
                        ws.Column(3 + i).Hidden = true;
                        using (var rng = ws.Cells["A" + (row + datarowstart-fieldnumber+1).ToString() + ":" + (char)(65 + Tbl.Columns.Count + 1) + (row + datarowstart-fieldnumber+1).ToString()]) 
                        {
                            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rng.Style.Fill.BackgroundColor.SetColor(Color.Black);
                            rng.Style.Font.Bold = true;
                            rng.Style.Font.Color.SetColor(Color.White);
                            rng.Style.WrapText = false;
                        
                        }
                        ws.SetValue(row + datarowstart-fieldnumber+1, 1, "Limit of new Data Insertion");
                        ws.SetValue(row + datarowstart-fieldnumber, Tbl.Columns.Count+2, 0);
                        char opt = (char)(65 + Tbl.Columns.Count);
                        ws.Cells[opt + (row - prepare - fieldnumber + datarowstart).ToString() + ":" + opt + (row + datarowstart - fieldnumber).ToString()].Value = "Ignore";
                       
                        i++;
                        //
                    }
                }

                package.SaveAs(newFile);
                return "./UploadedFiles/"+xlsx;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string CreateTemplate(string TableName,int prepare)
        {
            return CreateXlsx(TableName, false, prepare);    
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Export(string TableName, int prepare)
        {
            return CreateXlsx(TableName, true, prepare);
        }

        [WebMethod(EnableSession=true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Execute(List<Form> data)
        {
            
            var Ajax = new Dictionary<string, string>();
            foreach (Form frm in data)
            {
                Ajax.Add(frm.name, frm.value);
            }
           
            int option = Convert.ToInt32(Ajax["0option"]);
            Ajax.Remove("0option");
            int foreign = Convert.ToInt32(Ajax["0foreignfield"]);
            Ajax.Remove("0foreignfield");
            int fieldnumber = Convert.ToInt32(Ajax["0fieldnumber"]);
            Ajax.Remove("0fieldnumber");
            string tablename = Ajax["0tablename"];
            Ajax.Remove("0tablename");
            int  done_by =  Convert.ToInt32(Ajax["0done_by"]);
            Ajax.Remove("0done_by");
            
            var sqlparams = new List<SqlParameter>();
            foreach (string keys in Ajax.Keys)
            {
                string[] sss = keys.Split(new Char[]{':'},2);
                string ss = sss[0];
                switch (sss[1])
                {
                    case "56" : sqlparams.Add(new SqlParameter("@" + ss, Ajax[keys]==String.Empty? 0: Convert.ToDecimal(Ajax[keys])));
                        break;
                    case "104": sqlparams.Add(new SqlParameter("@" + ss, Ajax[keys].ToUpper()=="TRUE"?1:0));
                        break;
                    default: sqlparams.Add(new SqlParameter("@" + ss, Ajax[keys]));
                        break;
                }
            };
            sqlparams.Add(new SqlParameter("@option",option));
            sqlparams.Add(new SqlParameter("@done_by",new MySessions().EmployeeName));

            SqlParameter result = new SqlParameter("@result",0)
            {
                Direction = ParameterDirection.Output
            };
            sqlparams.Add(result);
            DataSet AssetDS = SqlHelper.ExecuteDataset(Helper.TPMDBConnection(), CommandType.StoredProcedure, "usp_bulk"+tablename, sqlparams.ToArray());
            string ForeignColNumSS = result.SqlValue.ToString();
           
            JavaScriptSerializer js = new JavaScriptSerializer();
            string ssss =js.Serialize(data);
            return ssss;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void StreamDownload(string url)
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
        
    }
}
