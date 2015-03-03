using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Net;
using TPM.Classes;
using MySql.Data;
using MySql.Data.MySqlClient;


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
        public LiteralControl StyleBootstrap = new LiteralControl("<link href='./Styles/reset.css' media=\"screen\" rel='stylesheet' type='text/css' />\r\n<link href='./Styles/bootstrap.min.css' media=\"screen\" rel='stylesheet' type='text/css' />\r\n<link href='./Styles/bootstrap-responsive.min.css' rel='stylesheet'  media=\"screen\" type='text/css'/>\r\n<link href='./Styles/menu.css' rel='stylesheet' type='text/css'/>\r\n");
        public LiteralControl StyleIE6 = new LiteralControl("\r\n<!--[if lte IE 6]>\r\n <link rel=\"stylesheet\" href=\"./Styles/ie6.1.1.css\" media=\"screen, projection\">\r\n<![endif]-->\r\n");
        public LiteralControl StyleDatePicker = new LiteralControl("<link href='./Styles/default.css' rel='stylesheet' type='text/css'/>\r\n");
        public LiteralControl jdatepicker = new LiteralControl("<script type='text/javascript' src='./Scripts/zebra_datepicker.js'></script>\r\n");

        public LiteralControl StyleMenu = new LiteralControl("<link href='./Styles/menu.css' rel='stylesheet' type='text/css'/>\r\n");

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
        public string TPMDBConnection()
        {
            return ConfigurationManager.ConnectionStrings["strDBTPM"].ConnectionString.ToString();
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
            DataSet ds = new DataSet();
            return 1;
        }
        public string[] Namabulan() {
            List<string> monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();
            monthNames.Insert(0, "");
            return monthNames.ToArray();
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
            this.Add("Ignore");
            this.Add("Insert, Ignore If Already Exist");
            this.Add("Insert, Update If Already Exist");
            this.Add("Update, Ignore If Not Exist");
            this.Add("Update, Insert If Not Exist");
            this.Add("Delete");
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
        private int _id;
        
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
          public int id
          {
              get { return _id; }
              set { _id = value; }
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
        private bool _isOperator = false;
        private bool _isLeader = false;
        private bool _isEngineering = false;
        private bool _isPublic = true;
        private string _redirection = "UnAuthorized.aspx";

        private static int _role = 0;
        public MySessions()
        {
            check();
        }
        private void check() {
            string _current = (System.Web.HttpContext.Current.Session["BadgeNo"] == null) ? "" : System.Web.HttpContext.Current.Session["BadgeNo"].ToString();
            if (_current != _employeeno)
            {
                TPMHelper F = new TPMHelper();

                /* HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost:81/TTPM/getsessions.aspx");
                 myRequest.Method = "GET";
                 WebResponse myResponse = myRequest.GetResponse();
                 StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                 string result = sr.ReadToEnd();
                 System.Diagnostics.Debug.Write("result -->"+result);
                 sr.Close();
                 myResponse.Close();*/
                string result = (System.Web.HttpContext.Current.Session["BadgeNo"] == null) ? "" : System.Web.HttpContext.Current.Session["BadgeNo"].ToString();
                _employeeno = result;
                if (result != "")
                {
                  
                    SqlDataReader sdr = SqlHelper.ExecuteReader(F.TDBVMSQAConnection(), CommandType.Text, "Select EmployeeNo,UserName From Muser Where EmployeeNo='" + result + "'");
                    while (sdr.Read())
                    {
                        _employeename = sdr["username"].ToString();
                        _employeeno = sdr["employeeno"].ToString();
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
                            case 4: _isAdministrator = true; break;
                            case 16: _isEngineering = true; break;
                            case 8: _isLeader = true; break;
                        }
                    }

                    _isPublic = false;
                }
                else {
                    _isPublic = true;
                
                }
            }
        }
        public string redirection {
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
               
                 
                string sql = "SELECT A.Id,A.Name,A.Code,A.qty_min,A.qty_max,(B.qty_current + B.qty_adjustment) AS 'Qty In Stock' FROM PRODUCT A " +
                                    "LEFT JOIN apq B ON B.product_code = A.code " +
                                    "WHERE (isactive='1') AND ( (B.qty_current + B.qty_adjustment)< A.qty_min)";
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


    
}
