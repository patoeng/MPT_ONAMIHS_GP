using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Web.UI;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using MySql.Data.MySqlClient;
using System.Web;
namespace TPM.Classes
{
    public class TPMHelper
    {
        public TPMHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet AssetDataSetResult = new DataSet();
        public LiteralControl jQueryRef = new LiteralControl("\r\n\r\n<script type='text/javascript' src='./Scripts/html5.js'></script>\r\n<script type='text/javascript' src='./Scripts/jquery-1.9.1.min.js'></script>\r\n");
        public LiteralControl jQValidate = new LiteralControl("<script type='text/javascript' src='./Scripts/jquery.validate.min.js'></script>\r\n");
        public LiteralControl jQMetaData = new LiteralControl("<script type='text/javascript' src='./Scripts/jquery.metadata.min.js'></script>\r\n");
        public LiteralControl jQForm = new LiteralControl("<script type='text/javascript' src='./Scripts/jquery.form.js'></script>\r\n");
        public LiteralControl jQDataTables = new LiteralControl("<script type='text/javascript' src='./Scripts/jquery.dataTables.min.js'></script>\r\n");
        public LiteralControl json2 = new LiteralControl("<script type='text/javascript' src='./Scripts/json2.js'></script>\r\n");
        public LiteralControl jqueryCSV = new LiteralControl("<script type='text/javascript' src='./Scripts/csv.js'></script>\r\n");
        public LiteralControl jeditable = new LiteralControl("<script type='text/javascript' src='./Scripts/jquery.jeditable.mini.js'></script>\r\n");
        public LiteralControl StyleDataTables = new LiteralControl("<link href='./Styles/jquery.dataTables.css' rel='stylesheet' type='text/css'/>\r\n");
        public LiteralControl StyleBootstrap = new LiteralControl("<link href='./Styles/reset.css' media=\"screen\" rel='stylesheet' type='text/css' />\r\n<link href='./Styles/bootstrap.min.css' media=\"screen\" rel='stylesheet' type='text/css' />\r\n<link href='./Styles/bootstrap-responsive.min.css' rel='stylesheet'  media=\"screen\" type='text/css'/>\r\n<link href='./Styles/menu.css' rel='stylesheet' type='text/css'/>\r\n<link href='./Styles/bootstrapper.css' rel='stylesheet' type='text/css'/>\r\n");
        public LiteralControl StyleIE6 = new LiteralControl("\r\n<!--[if lte IE 6]>\r\n <link rel=\"stylesheet\" href=\"./Styles/ie6.1.1.css\" media=\"screen, projection\">\r\n<![endif]-->\r\n");
        public LiteralControl StyleDatePicker = new LiteralControl("<link href='./Styles/default.css' rel='stylesheet' type='text/css'/>\r\n");
        public LiteralControl jdatepicker = new LiteralControl("<script type='text/javascript' src='./Scripts/zebra_datepicker.js'></script>\r\n");
        public LiteralControl highchart = new LiteralControl("<script type='text/javascript' src='./Scripts/highcharts.js'></script>\r\n");
        public LiteralControl hcdata = new LiteralControl("<script type='text/javascript' src='./Scripts/highcharts-data.js'></script>\r\n");
        public LiteralControl hcexport = new LiteralControl("<script type='text/javascript' src='./Scripts/highcharts-exporting.js'></script>\r\n");

        public LiteralControl StyleMenu = new LiteralControl("<link href='./Styles/menu.css' rel='stylesheet' type='text/css'/>\r\n");
        public static string WebDirectory = "TPM";
        public static string DBTPMstring {
            get { return ConfigurationManager.ConnectionStrings["strDBTPM"].ConnectionString.ToString(); }
        
        }
        public static string DBVMSstring
        {
            
            get { return ConfigurationManager.ConnectionStrings["strDBVMSQA"].ConnectionString.ToString(); }

        }
        public static string DBINVENTORYstring
        {
            get { return ConfigurationManager.ConnectionStrings["strInventory"].ConnectionString.ToString(); }

        }

        public static string DBBMHTstring
        {
            get { return ConfigurationManager.ConnectionStrings["strHT"].ConnectionString.ToString(); }
        }

        public string TPMDBConnection()
        {
            return ConfigurationManager.ConnectionStrings["strDBTPM"].ConnectionString.ToString();
        }
        public string TBMHTConnection()
        {
            return ConfigurationManager.ConnectionStrings["strHT"].ConnectionString.ToString();
        }
        public string TInventoryConnection()
        {
            return ConfigurationManager.ConnectionStrings["strInventory"].ConnectionString.ToString();
        }
        public string TDBVMSQAConnection()
        {
            return ConfigurationManager.ConnectionStrings["strDBVMSQA"].ConnectionString.ToString();
        }
        public int InsertAssets(int AssetModel, int Department, string AssetNumber)
        {
            var ds = new DataSet();
            return 1;
        }
        public static string CreateXlsx(DataSet ds,string fileName,string sheetName)
        {
            var newFile = new FileInfo(fileName);
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(fileName);
            }
            var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add(sheetName);
            DataTable dt = ds != null ? ds.Tables[0] : new DataTable();
            int row = 0;
            int fieldnumber = dt.Columns.Count;
            foreach (DataRow drr in dt.Rows)
            {
                row++;
                for (int col = 0; col < fieldnumber; col++)
                {
                    ws.SetValue(row, col + 1, dt.Columns[col].DataType == System.Type.GetType("System.DateTime") && (drr[col].ToString() != "") ? ((DateTime)(drr[col])).ToString("d MMMM yyyy") : drr[col].ToString());
                }
            }

            ws.InsertRow(1, 1);
            int column;
            for (column = 0; column < fieldnumber; column++)
            {

                ws.SetValue(1, column + 1, dt.Columns[column].ColumnName.ToString(CultureInfo.InvariantCulture));
                ws.Column(column + 1).Style.Locked = false;
            }


            using (var rng = ws.Cells["A1:" + (char)(65 + fieldnumber - 1) + "1"])
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
            
            return fileName;
        }
        public static string CreateXlsx(Dictionary<string,List<UsageSummary>> us, List<string> head ,string fileName, string sheetName)
        {

            var newFile = new FileInfo(fileName);
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(fileName);
            }
            var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add(sheetName);
           
            int row = 0;
            
            foreach (string sss in us.Keys)
            {
                int col = 0;
                row++;
                for (int c = 0; c < us[sss].Count; c++)
                {
                    ws.SetValue(row, ++col, us[sss][c].UsageName);
                    ws.SetValue(row, ++col, us[sss][c].Id);
                    ws.SetValue(row, ++col, us[sss][c].SparePartCode);
                    ws.SetValue(row, ++col, us[sss][c].SparePartName);
                    ws.SetValue(row, ++col, us[sss][c].Quantity);
                    ws.SetValue(row, ++col, us[sss][c].Price);
                    ws.SetValue(row, ++col, us[sss][c].Quantity * us[sss][c].Price);
                    ws.SetValue(row, ++col, us[sss][c].Currency);
                    ws.SetValue(row, ++col, us[sss][c].Reason);
                    ws.SetValue(row, ++col, us[sss][c].Date);
                    ws.SetValue(row, ++col, us[sss][c].DoneBy);
                    ws.SetValue(row, ++col, us[sss][c].MachineSn);
                    ws.SetValue(row, ++col, us[sss][c].MachineName);
                    ws.SetValue(row, ++col, us[sss][c].Department);
                }
            }

            ws.InsertRow(1, 1);
            int column=0;
            foreach (string ss  in head)
            {
                column++;
                ws.SetValue(1, column, ss);
                ws.Column(column).Style.Locked = false;
            }
            


            using (var rng = ws.Cells[1,1,1,column])
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

            return fileName;
        }
       
        public static List<List<string>> Namabulan()
        {
            var monthNames = new List<List<string>>()
                {
                   new List<string> {"Jan","January"},
                   new List<string> {"Feb","February"},
                   new List<string> {"Mar","March"},
                   new List<string> {"Apr","April"},
                   new List<string> {"May","May"},
                   new List<string> {"Jun","June"},
                   new List<string> { "Jul","July"},
                   new List<string> {"Aug","August"},
                   new List<string> {"Sep","September"},
                   new List<string> {"Oct","October"},
                   new List<string> {"Nov","November"},
                   new List<string> {"Dec","December"},
                };
            monthNames.Insert(0,  new List<string> {"",""});
            var i = new string[13,2];

            return monthNames;
        }
        
        private string ReturnExtension(string fileExtension)
        {
            
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

    }
    public class ChecklistStatus
    {
        public static string GetStatus(int statusId=0)
        {
            var l = new List<string>
                {
                    "",
                    "OK",
                    "ABNORMAL",
                    "NC",
                };
            return l[statusId];
        }
    }
    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    public class BulkOptions : List<string>
    {
        public BulkOptions()
        {
            Add("Ignore");
            Add("Insert, Ignore If Already Exist");
            Add("Insert, Update If Already Exist");
            Add("Update, Ignore If Not Exist");
            Add("Update, Insert If Not Exist");
            Add("Delete");
        }
    }
    
    public class Form
    {
        private string _name;
        private string _value;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                string s = value;
                string[] ss = s.Split('$');
                try
                {
                    _name = ss[2];
                }
                catch (Exception err)
                {
                    _name = s;
                }
            }
        }
       public string value
        {
            get { return _value; }
            set { _value = value; }
        }

    }
    public class PMrecords
    {
        private DateTime _pmdatetime;
        private int _status;
        private string _id;
        private string _checklist;
        public string cresult { get; set; }
        public DateTime pmdatetime
        {
            get { return _pmdatetime; }
            set { _pmdatetime = value; }
        }
       
          public int status
        {
            get { return _status; }
            set { _status = value; }
        }
          public string id
          {
              get { return _id; }
              set { _id = value; }
          }
          public string checklist
          {
              get { return _checklist; }
              set { _checklist = value; }
          }
    }
    public class Bulk
    {
        private string _databaseName;
        private string _action;
        private string _doneby;
        public List<Form> Fields
        {
            get;
            set;
        }
        public string Operation
        {
            get { return _action; }
            set { _action = value; }
        }
        public string DatabaseName 
         {
             get { return _databaseName;}
             set { _databaseName = value; }
         }
        public string DoneBy
        {
            get { return _doneby; }
            set { _doneby = value; }
        }

    }

    public class MySessions
    {
        private string _employeename="Guest";
        private string _employeeno="00000";
        private bool _isAdministrator = false;
        private bool _isManagement = false;
        private bool _isOperator = true;
        private bool _isLeader = false;
        private bool _isEngineering = false;
        private bool _isPublic = true;
        private string _redirection = "UnAuthorized.aspx";

        private static int _role;
        public MySessions()
        {
            check();
        }
        private void check() {
            string current = (HttpContext.Current.Session["BadgeNo"] == null) ? "" : HttpContext.Current.Session["BadgeNo"].ToString();
            if (current != _employeeno)
            {
                var F = new TPMHelper();

                /* HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost:81/T<%=TPMHelper.WebDirectory%>/getsessions.aspx");
                 myRequest.Method = "GET";
                 WebResponse myResponse = myRequest.GetResponse();
                 StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                 string result = sr.ReadToEnd();
                 System.Diagnostics.Debug.Write("result -->"+result);
                 sr.Close();
                 myResponse.Close();*/
                string result = (HttpContext.Current.Session["BadgeNo"] == null) ? "" : HttpContext.Current.Session["BadgeNo"].ToString();
                _employeeno = result;
                if (result != "")
                {
                  
                    SqlDataReader sdr = SqlHelper.ExecuteReader(F.TDBVMSQAConnection(), CommandType.Text, "Select EmployeeNo,UserName From Muser Where EmployeeNo='" + result + "'");
                    while (sdr.Read())
                    {
                        _employeename = sdr["username"].ToString();
                        _employeeno = sdr["employeeno"].ToString();
                    }
                    // if not found in DBVMS check for local TPM 'POP UP daily PROMPT

                    if (_employeename=="Guest"){
                        sdr = SqlHelper.ExecuteReader(F.TPMDBConnection(), CommandType.StoredProcedure, "usp_MTPMLocalUserSelect", new SqlParameter("@employeeno", result));
                        while (sdr.Read())
                        {
                            _employeename = sdr["employeename"].ToString();
                            _employeeno = sdr["employeeno"].ToString();
                        }
                    }
                    sdr = SqlHelper.ExecuteReader(F.TPMDBConnection(), CommandType.StoredProcedure, "usp_MuserRolesSelect", new SqlParameter()
                    {
                        Direction = ParameterDirection.Input,
                        SqlDbType = SqlDbType.VarChar,
                        ParameterName = "@user_id",
                        Value = _employeeno
                    });
                    while (sdr.Read())
                    {
                        _role = (int)sdr["role_id"];
                        switch (_role)
                        {
                            case 1: _isOperator = true; break;
                            case 2: _isManagement = true; break;
                            case 8: _isAdministrator = true; break;
                            case 16: _isEngineering = true; break;
                            case 4: _isLeader = true; break;
                        }
                    }

                    _isPublic = false;
                }
                else {
                    _isPublic = true;
                
                }
            }
        }
        public string Redirection {
            get { return _redirection;}
            set { _redirection = value; }
        }

        public string EmployeeNo {
            get {
                check();
                return _employeeno;
            }

        }
        public string EmployeeName {
            get
            {
                check();
                return _employeename;
            }
        }
        public bool IsAdministrator
        {
            get
            {
                check();
                return _isAdministrator;
            }
        }
        public bool IsManagement
        {
            get {
                check(); 
                return _isManagement; 
            }
        }

        public bool IsOperator
        {
            get { check();
                return _isOperator; }
            set
            {

                _isOperator = value;
            }
        }
        public bool IsEngineering
        {
            get { check(); 
                return _isEngineering; }
        }
        public bool IsLeader
        {
            get { check(); 
                return _isLeader; }
        }
        public bool IsPublic
        {
            get
            {
                check();
                return _isPublic;
            }
            set
            {
                
                 _isPublic=value;
            }
        }
        
        
    }
      
    public class Role{
        public Role(){

            
        }
        public static int Operator = 1;
        public static int Management = 2;
        public static int Administrator = 4;
        public static int Leader = 8;
        public static int Engineering = 16;

    }
    public class AssetModels{
        private static string _modelId ="";
        private static DataSet _bomlist= null;
        private static void getBomlist() {
            if (_modelId == "")
            {
                _bomlist = null;
            }
            else
            {
                string sql =    "SELECT A.Id,A.Name,A.Code,A.qty_min,A.qty_max,(B.qty_current + B.qty_adjustment) AS 'Qty In Stock' FROM PRODUCT A " +
                                "LEFT JOIN apq B ON B.product_code = A.code " +
                                "WHERE (A.CODE LIKE '%" + _modelId + "___') AND (isactive='1')";
                _bomlist = MySqlHelper.ExecuteDataset(TPMHelper.DBINVENTORYstring, sql);
            }
        }
        public static DataSet BomListLow {
            get {

                if (_modelId == "")
                {
                    _bomlist = null;
                }
                else
                {
                    string sql = "SELECT A.Id,A.Name,A.Code,A.qty_min,A.qty_max,(B.qty_current + B.qty_adjustment) AS 'Qty In Stock' FROM PRODUCT A " +
                                    "LEFT JOIN apq B ON B.product_code = A.code " +
                                    "WHERE (A.CODE LIKE '%" + _modelId + "___') AND (isactive='1') AND ( (B.qty_current + B.qty_adjustment)< A.qty_min)";
                    _bomlist = MySqlHelper.ExecuteDataset(TPMHelper.DBINVENTORYstring, sql);
                }
                return _bomlist;
            }
        
        }
        public static DataSet BomListLowAll
        {
            get
            {
                const string sql = "SELECT A.Code As 'Sparepart Code',A.Name,A.qty_min,A.qty_max,(B.qty_current + B.qty_adjustment) AS 'Qty In Stock'," +
                                    "C.price AS 'Supplier Price',A.qty_max-(B.qty_current + B.qty_adjustment) AS 'Proposed Order Qty',D.name AS 'Proposed Vendor', C.Leadtime," +
                                   "((A.qty_max-(B.qty_current + B.qty_adjustment))*C.price) AS 'Total Amount' " +
                                   " " +
                                   " FROM PRODUCT A " +
                                   "LEFT JOIN apq B ON B.product_code = A.code " +
                                   "LEFT JOIN product_supplier C ON C.product_code=A.code "+
                                   "LEFT JOIN supplier D ON D.code=C.supplier_code "+
                                   "WHERE (A.isactive='1') AND ( (B.qty_current + B.qty_adjustment)< A.qty_min) " +
                                   "";
                    _bomlist = MySqlHelper.ExecuteDataset(TPMHelper.DBINVENTORYstring, sql);
                
                return _bomlist;
            }

        }
        public static DataSet SparePart
        {
            get
            {
                const string sql = "SELECT A.Code As 'Sparepart Code',A.Name,A.Qty_Min,A.Qty_Max,(B.qty_current + B.qty_adjustment) AS 'Qty In Stock', " +
                                   "c.price AS 'Unit Price', (B.qty_current + B.qty_adjustment)* c.price AS 'Total Amount'" +
                                   "FROM PRODUCT A " +
                                   "LEFT JOIN apq B ON B.product_code = A.code " +
                                   "LEFT JOIN product_supplier C ON C.product_code=A.code " +
                                   "LEFT JOIN supplier D ON D.code=C.supplier_code " +
                                   "WHERE (A.isactive='1')";
                _bomlist = MySqlHelper.ExecuteDataset(TPMHelper.DBINVENTORYstring, sql);

                return _bomlist;
            }
        }
        public static string AssetModelCode{
            set { _modelId = value; }
            get { return _modelId; }
        }
        public static DataSet BomList {
            get
            {
                getBomlist(); 
                return _bomlist;
            }
        }
        public static DataTable AllAssetModels {
            get {
                DataSet _list = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MAssetModelsSelect", new SqlParameter("@id",DBNull.Value));
                return _list != null ? (_list.Tables.Count > 0 ? _list.Tables[0] : null) : null; 
            }
        }
        public static DataTable AssetModel
        {
            get
            {
                DataSet _list = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_MAssetModelsSelect", new SqlParameter("@id", _modelId));
                return _list != null ? (_list.Tables.Count > 0 ? _list.Tables[0] : null) : null;
            }
        }
    }
    public class UsageSummary
    {
        public string Id { get; set; }
        public string SparePartCode { get; set; }
        public string SparePartName { get; set; }
        public string DoneBy { get; set; }
        public int UsageType { get; set; }
        public string UsageName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public string Reason { get; set; }
        public string Date { get; set; }
        public string Department { get; set; }
        public string MachineSn { get; set; }
        public string MachineName { get; set; }
    }

    
}
