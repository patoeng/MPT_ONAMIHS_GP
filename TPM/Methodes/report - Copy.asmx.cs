using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections;
using System.Web.UI.HtmlControls;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.DataValidation;
using System.Globalization;

namespace TPM.Methodes
{
    /// <summary>
    /// Summary description for report
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class reportCopy : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld(string nama)
        {
            return "Hello World " + nama;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string reportWorkOrderStatus(int deptID, int statusID, DateTime? startDate, DateTime? endDate)
        {
            //tanggal default
            String sekarang = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sekarang.ToString()));

            int d = datevalue.Day;
            int m = datevalue.Month;
            int y = datevalue.Year;

            //jumlah data yang ditampilkan per bulan
            int bln = 7;

            var secondHalfStart = new DateTime();
            var secondHalfEnd = new DateTime(); 
            var firstHalfStart = new DateTime();
            var firstHalfEnd = new DateTime();

            //set 2ndHalf previously year
            String secondHalfStart_Str = "6/1/" + (y - 1).ToString();
            String secondHalfEnd_Str = "1/1/" + y.ToString();
            secondHalfStart = DateTime.Parse(secondHalfStart_Str);
            secondHalfEnd = DateTime.Parse(secondHalfEnd_Str);

            //set 1stHalf current year
            String firstHalfStart_Str = "1/1/" + y.ToString();
            String firstHalfEnd_Str = "7/1/" + y.ToString();
            firstHalfStart = DateTime.Parse(firstHalfStart_Str);
            firstHalfEnd = DateTime.Parse(firstHalfEnd_Str);

            String tempDate = datevalue.Month.ToString() + "/" + datevalue.Day.ToString() + "/" + datevalue.Year.ToString();
            DateTime sd = DateTime.Parse(tempDate);

            //jika tanggal tidak ada yang dipilih =================================================================
            if ((!startDate.HasValue) && (!endDate.HasValue))
            {
                //set endDate saja
                endDate = DateTime.Parse(sekarang);
                bln = 7;

                //startDate hitung mundur 6 bulan dari endDate
                int prev6month;
                if (m > 7) {
                    prev6month = m - 6;
                    tempDate = prev6month.ToString() + "/1/" + datevalue.Year.ToString();
                    startDate = DateTime.Parse(tempDate);

                } else {
                    prev6month = 12 - ( 6 - m );
                    String tempString = prev6month.ToString() + "/1/" + (y - 1).ToString();
                    startDate = DateTime.Parse(tempString);
                }
            };
            //jika hanya pilih startDate =================================================================
            if ((startDate.HasValue) && (!endDate.HasValue))
            {
                endDate = DateTime.Parse(sekarang);
                datevalue = (Convert.ToDateTime(startDate.ToString()));

                m = datevalue.Month;
                int y1 = datevalue.Year;
                
                //jumlah data yang ditampilkan per bulan

                //startDate hitung mundur 6 bulan dari endDate
                int prev6month;
                if ((m >= 7) &&  (y1 < y))
                {
                    prev6month = m - 6;
                    tempDate = prev6month.ToString() + "/1/" + datevalue.Year.ToString();
                    startDate = DateTime.Parse(tempDate);
                    bln = 7;

                }
                else
                {
                    prev6month = 12 - (6 - m);
                    String tempString = prev6month.ToString() + "/1/" + (y - 1).ToString();
                    startDate = DateTime.Parse(tempString);
                    bln = 8 - m;
                }

                //set 2ndHalf previously year
                secondHalfStart_Str = "6/1/" + (y - 1).ToString();
                secondHalfEnd_Str = "1/1/" + y.ToString();
                secondHalfStart = DateTime.Parse(secondHalfStart_Str);
                secondHalfEnd = DateTime.Parse(secondHalfEnd_Str);

                //set 1stHalf current year
                firstHalfStart_Str = "1/1/" + y.ToString();
                firstHalfEnd_Str = "7/1/" + y.ToString();
                firstHalfStart = DateTime.Parse(firstHalfStart_Str);
                firstHalfEnd = DateTime.Parse(firstHalfEnd_Str);
            }
            //jika hanya pilih endDate =================================================================
            else if ((!startDate.HasValue) && (endDate.HasValue))
            {
                endDate = DateTime.Parse(sekarang);
            }
            //jika tanggal dipilih start dan end =================================================================
            else if ((startDate.HasValue) && (endDate.HasValue))
            {
                
                DateTime sDate = (Convert.ToDateTime(startDate.ToString()));
                DateTime eDate = (Convert.ToDateTime(endDate.ToString()));
                y = eDate.Year;

                //set 2ndHalf from endDate year
                secondHalfStart_Str = "6/1/" + (y - 1).ToString();
                secondHalfEnd_Str = "1/1/" + y.ToString();
                secondHalfStart = DateTime.Parse(secondHalfStart_Str);
                secondHalfEnd = DateTime.Parse(secondHalfEnd_Str);

                //set 1stHalf from endDate year
                firstHalfStart_Str = "1/1/" + y.ToString();
                firstHalfEnd_Str = "7/1/" + y.ToString();
                firstHalfStart = DateTime.Parse(firstHalfStart_Str);
                firstHalfEnd = DateTime.Parse(firstHalfEnd_Str);

                //startDate hitung mundur 6 bulan dari endDate
                int prev6month;
                if (m >= 6)
                {
                    prev6month = m - 5;
                    tempDate = prev6month.ToString() + "/1/" + eDate.Year.ToString();
                    startDate = DateTime.Parse(tempDate);

                }
                else
                {
                    prev6month = 12 - (5 - m);
                    String tempString = prev6month.ToString() + "/1/" + (y - 1).ToString();
                    startDate = DateTime.Parse(tempString);
                }
                
            };
            // SQL =================================================================
            var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@deptID", deptID),
                    new SqlParameter("@statusID", statusID),
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@secondHalfStart", secondHalfStart),
                    new SqlParameter("@secondHalfEnd", secondHalfEnd),
                    new SqlParameter("@firstHalfStart", firstHalfStart),
                    new SqlParameter("@firstHalfEnd", firstHalfEnd),
                };

            DataSet dsMaster = new DataSet();
            dsMaster = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_report_WorkOrderStatus", sqlparams.ToArray());

            //merubah dataset to jason
            /*Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (DataTable dt in stWorkOrder.Tables)
            {
                object[] arr = new object[dt.Rows.Count + 1];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    arr[i] = dt.Rows[i].ItemArray;
                }
                dict.Add(dt.TableName, arr);
            }

            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(dict);*/
            int[,] data = new int[9,2];
            var newDS = new DataSet();

            //2nd Half Previously year
            var str2H = "generated_date >='" + secondHalfStart.Date + "' AND generated_date <= '" + secondHalfEnd.Date + "'";
            var dv2 = dsMaster.Tables[0].DefaultView;
            dv2.RowFilter = str2H;

            //var newDT2 = new DataTable("2ndHalf");
            var newDT2 = dv2.ToTable("2ndHalf");
            //newDS.Tables.Add(newDT2);
            int secondOpen = newDT2.Select("Status_Id < '5'").Length;
            int secondClose = newDT2.Select("Status_Id = '5'").Length;
            data[0,0] = secondClose;
            data[0,1] = secondOpen;

            //1st Half Current year
            var str1H = "generated_date >='" + firstHalfStart.Date + "' AND generated_date <= '" + firstHalfEnd.Date + "'";
            var dv1 = dsMaster.Tables[0].DefaultView;
            dv1.RowFilter = str1H;
         
            //var newDT1 = new DataTable("1stHalf");
            var newDT1 = dv1.ToTable("1stHalf");
            //newDS.Tables.Add(newDT1);
            int firstOpen = newDT1.Select("Status_Id < '5'").Length;
            int firstClose = newDT1.Select("Status_Id = '5'").Length;
            data[1,0] = firstClose;
            data[1,1] = firstOpen;

            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (DataTable dt in newDS.Tables)
            {
                object[] arr = new object[dt.Rows.Count + 1];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    arr[i] = dt.Rows[i].ItemArray;
                }
                dict.Add(dt.TableName, arr);
            }

            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(dict);
        }
        public DateTime GetStartDate(string tgl )
        {

            DateTime datevalue = (Convert.ToDateTime(tgl.ToString()));

            int d = datevalue.Day;
            int m = datevalue.Month;
            int y = datevalue.Year;

            String tempDate = datevalue.Day.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Year.ToString();
            DateTime sd = DateTime.Parse(tempDate);

            return sd;
        }
    }
}
