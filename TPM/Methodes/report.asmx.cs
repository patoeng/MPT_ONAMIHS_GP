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
    public class report : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld(string nama)
        {
            return "Hello World " + nama;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string reportWorkOrderStatus(string deptID, string statusID, DateTime? startDate, DateTime? endDate, string RptType, string Url)
        {
            //tanggal default
            String sekarang = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sekarang));
            DateTime datevalue2 = (Convert.ToDateTime(sekarang));
            TPMHelper a = new TPMHelper();
           

            int currectdate = datevalue.Day;
            int currentMonth = datevalue.Month;
            int currentYear = datevalue.Year;

            var secondHalfStart = new DateTime();
            var secondHalfEnd = new DateTime(); 
            var firstHalfStart = new DateTime();
            var firstHalfEnd = new DateTime();

            //set 2ndHalf previously year
            String secondHalfStart_Str = "6/1/" + (currentYear - 1).ToString();
            String secondHalfEnd_Str = "1/1/" + currentYear.ToString();
            secondHalfStart = DateTime.Parse(secondHalfStart_Str);
            secondHalfEnd = DateTime.Parse(secondHalfEnd_Str);

            //set 1stHalf current year
            String firstHalfStart_Str = "1/1/" + currentYear.ToString();
            String firstHalfEnd_Str = "7/1/" + currentYear.ToString();
            firstHalfStart = DateTime.Parse(firstHalfStart_Str);
            firstHalfEnd = DateTime.Parse(firstHalfEnd_Str);

            String tempDate = datevalue.Month.ToString() + "/" + datevalue.Day.ToString() + "/" + datevalue.Year.ToString();
            DateTime sd = DateTime.Parse(tempDate);

            //series name (bln)
            string th1 = Convert.ToString(currentYear);
            var th1b = th1.Substring(th1.Length - Math.Min(2, th1.Length));
            string lblThnNow = "1stH-" + th1b;

            string th2 = Convert.ToString(currentYear - 1);
            var th2b = th2.Substring(th2.Length - Math.Min(2, th2.Length));
            string lblThnPre = "2ndH-" + th2b;

            //nama bulan untuk chart series name
            List<string> namaBulan = new List<string>();
            string[] namaBulanArray;

            //jenis query
            int jenisInput = 0;

            //jika tanggal tidak ada yang dipilih ========================== start ==== jenisInput 1 ================================
            if ((!startDate.HasValue) && (!endDate.HasValue))
            {
                jenisInput = 1;
                namaBulan = new List<string>();

                //set endDate saja
                endDate = DateTime.Parse(sekarang);

                //startDate hitung mundur 6 bulan dari endDate
                int prev6month = 0;
                if (currentMonth > 7) {
                    prev6month = currentMonth - 6;
                    tempDate = prev6month.ToString() + "/1/" + datevalue.Year.ToString();
                    startDate = DateTime.Parse(tempDate);

                    //add start dan end date 2ndHalf and 1st Half
                    namaBulan.Add(lblThnPre + ",7/1/" + Convert.ToString(currentYear-1) + ",1/1/" + Convert.ToString(currentYear));
                    namaBulan.Add(lblThnNow + ",1/1/" + Convert.ToString(currentYear) + ",7/1/" + Convert.ToString(currentYear));

                    for (int i = prev6month; i <= currentMonth; i++)
                    {
                        if (i.ToString().Length < 2)
                        {
                            namaBulan.Add(datevalue.Year.ToString() + "-0" + Convert.ToString(i) + 
                                "," + (i.ToString() + "/1/" + (currentYear).ToString()) +
                                "," + ((i + 1).ToString() + "/1/" + (currentYear).ToString()));
                        }
                        else
                        {
                            namaBulan.Add(datevalue.Year.ToString() + Convert.ToString(i) + 
                                "," + (i.ToString() + "/1/" + (currentYear).ToString()) +
                                "," + ((i + 1).ToString() + "/1/" + (currentYear).ToString()));
                        }
                    }
                } else {
                    prev6month = 12 - ( 6 - currentMonth );
                    String tempString = prev6month.ToString() + "/1/" + (currentYear - 1).ToString();
                    startDate = DateTime.Parse(tempString);

                    //add start dan end date 2ndHalf and 1st Half
                    namaBulan.Add(lblThnPre + ",7/1/" + Convert.ToString(currentYear - 1) + ",1/1/" + Convert.ToString(currentYear));
                    namaBulan.Add(lblThnNow + ",1/1/" + Convert.ToString(currentYear) + ",7/1/" + Convert.ToString(currentYear));

                    for (int i = prev6month; i <= 12; i++)
                    {
                        if (i.ToString().Length < 2)
                        {
                            namaBulan.Add(Convert.ToString(currentYear - 1) + "-0" + Convert.ToString(i) + 
                                "," + (i.ToString() + "/1/" + (currentYear - 1).ToString()) + 
                                "," + ((i).ToString() + "/1/" + (currentYear - 1).ToString()));
                        }
                        else
                        {
                            if (i == 12)
                            {
                                namaBulan.Add(Convert.ToString(currentYear - 1) + "-" + Convert.ToString(i) +
                                    "," + (i.ToString() + "/1/" + (currentYear - 1).ToString()) +
                                    "," + ("1/1/" + (currentYear).ToString()));
                            }
                            else
                            {
                                namaBulan.Add(Convert.ToString(currentYear - 1) + "-" + Convert.ToString(i) +
                                    "," + (i.ToString() + "/1/" + (currentYear - 1).ToString()) +
                                    "," + ((i + 1).ToString() + "/1/" + (currentYear - 1).ToString()));
                            }

                        }
                    }
                    for (int i = 1; i <= currentMonth; i++)
                    {
                        if (i.ToString().Length < 2)
                        {
                            namaBulan.Add(Convert.ToString(currentYear) + "-0" + Convert.ToString(i) + 
                                "," + (i.ToString() + "/1/" + (currentYear).ToString()) +
                                "," + ((i + 1).ToString() + "/1/" + (currentYear).ToString()));
                        }
                        else
                        {
                            namaBulan.Add(Convert.ToString(currentYear) + "-" + Convert.ToString(i) + 
                                "," + (i.ToString() + "/1/" + (currentYear).ToString()) +
                                "," + ((i + 1).ToString() + "/1/" + (currentYear).ToString()));
                        }
                    }
                } 
            }
            //========================== end ==== jenisInput 1 ================================

            //jika hanya pilih startDate ========================== start ==== jenisInput 2 ================================
            else if ((startDate.HasValue) && (!endDate.HasValue))
            {
                jenisInput = 2;

                //set endDate dari Now dan startDate dari selection
                endDate = DateTime.Parse(sekarang);
                datevalue = (Convert.ToDateTime(startDate.ToString()));

                int selectedDate = datevalue.Day;
                int selectedMonth = datevalue.Month;
                int selectedYear = datevalue.Year;

                //jika tahun sebelumnya
                if (selectedYear < currentYear)
                {
                    //jika selected month sebelum bulan juni
                    if (selectedMonth <= 6)
                    {
                        namaBulan = new List<string>();
                        //jika sebelum bulan juni, tampilkan 1stHalf previous year
                        namaBulan.Add("1stH-" + selectedYear + ",1/1/" + Convert.ToString(selectedYear) + ",7/1/" + Convert.ToString(selectedYear));
                        namaBulan.Add("2ndH-" + selectedYear + ",7/1/" + Convert.ToString(selectedYear) + ",1/1/" + Convert.ToString(selectedYear + 1));
                        namaBulan.Add("1stH-" + currentYear + ",1/1/" + Convert.ToString(selectedYear) + ",7/1/" + Convert.ToString(currentYear));
                        if (currentMonth > 6)
                        {
                            //jika current month sudah masuk 2nd Half, tampilkan 2nd Half current year
                            namaBulan.Add("2ndH-" + currentYear + ",7/1/" + Convert.ToString(currentYear) + ",1/1/" + Convert.ToString(currentYear));
                        }

                        //add selected startDate
                        namaBulan.Add(selectedYear + "-" + selectedMonth +
                            "," + (selectedMonth + "/" + selectedDate + "/" + selectedYear) +
                            "," + (selectedMonth + 1) + "/1/" + Convert.ToString(selectedYear));

                        //add next 7 monht from selected date
                        for (int b = (selectedMonth + 1); b < (selectedMonth + 6); b++)
                        {
                            namaBulan.Add(selectedYear + "-" + b +
                                "," + (b + "/1/" + selectedYear) +
                                "," + (b + 1) + "/1/" + Convert.ToString(selectedYear));
                        }
                    }
                    else
                    {
                        namaBulan = new List<string>();
                        //jika setelah bulan juni, sembunyikan 1stHalf previous year
                        namaBulan.Add("2ndH-" + (currentYear - 1) + ",7/1/" + Convert.ToString(currentYear - 1) + ",1/1/" + Convert.ToString(currentYear));
                        namaBulan.Add("1stH-" + currentYear + ",1/1/" + Convert.ToString(selectedYear) + ",7/1/" + Convert.ToString(currentYear));
                        if (currentMonth > 6)
                        {
                            namaBulan.Add("2ndH-" + currentYear + ",7/1/" + Convert.ToString(selectedYear) + ",1/1/" + Convert.ToString(currentYear));
                        }
                        //add selected startDate
                        namaBulan.Add(selectedYear + "-" + selectedMonth +
                            "," + (selectedMonth + "/" + selectedDate + "/" + selectedYear) +
                            "," + (selectedMonth + 1) + "/1/" + Convert.ToString(selectedYear));

                        //add next 7 monht from selected date
                        int b = selectedMonth;
                        int bMin = b + 1;
                        int bMax = b + 6;
                        for (b = bMin; b <= bMax; b++)
                        {
                            if (b > 13)
                            {
                                namaBulan.Add((selectedYear + 1) + "-" + (b - 12) +
                                    "," + ((b - 12) + "/1/" + (selectedYear + 1)) +
                                    "," + ((b + 1) - 12) + "/1/" + Convert.ToString(selectedYear + 1));
                            }
                            else if (b > 12)
                            {
                                namaBulan.Add((selectedYear + 1) + "-" + (b - 12) +
                                    "," + ((b - 12) + "/1/" + (selectedYear + 1)) +
                                    "," + ((b + 1) - 12) + "/1/" + Convert.ToString(selectedYear + 1));
                            }
                            else if (b == 12)
                            {
                                namaBulan.Add(selectedYear + "-" + b +
                                    "," + (b + "/1/" + selectedYear) +
                                    ",1/1/" + Convert.ToString(selectedYear + 1));
                            }
                            else
                            {
                                namaBulan.Add(selectedYear + "-" + b +
                                    "," + (b + "/1/" + selectedYear) +
                                    "," + (b + 1) + "/1/" + Convert.ToString(selectedYear));
                            }
                        }
                    }
                }
                else
                {
                    namaBulan = new List<string>();
                    //jika tahun ini
                    namaBulan.Add("2ndH-" + (currentYear - 1) + ",7/1/" + Convert.ToString(currentYear - 1) + ",1/1/" + Convert.ToString(currentYear));
                    namaBulan.Add("1stH-" + currentYear + ",1/1/" + Convert.ToString(currentYear) + ",7/1/" + Convert.ToString(currentYear));
                    if (selectedMonth > 6)
                    {
                        //jika current month sudah masuk 2nd Half, tampilkan 2nd Half current year
                        namaBulan.Add("2ndH-" + currentYear + ",7/1/" + Convert.ToString(currentYear) + "," + currentMonth + "/1/" + Convert.ToString(currentYear));

                        namaBulan.Add(selectedYear + "-" + selectedMonth +
                            "," + (selectedMonth + "/" + selectedDate + "/" + currentYear) +
                            "," + (selectedMonth + 1) + "/1/" + Convert.ToString(currentYear));

                        //add next monht from selected date
                        for (int c = (selectedMonth + 1); c <= currentMonth; c++)
                        {
                            namaBulan.Add(currentYear + "-" + c +
                                "," + (c + "/1/" + currentYear) +
                                "," + (c + 1) + "/1/" + Convert.ToString(currentYear));
                        }
                    }
                    else
                    {
                        //add selected startDate
                        namaBulan.Add(currentYear + "-" + selectedMonth +
                            "," + (selectedMonth + "/" + selectedDate + "/" + currentYear) +
                            "," + (selectedMonth + 1) + "/1/" + Convert.ToString(currentYear));

                        //add next 7 month from selected date
                        for (int b = (selectedMonth + 1); b < (selectedMonth + 6); b++)
                        {
                            namaBulan.Add(currentYear + "-" + b +
                                "," + (b + "/1/" + currentYear) +
                                "," + (b + 1) + "/1/" + Convert.ToString(currentYear));
                        }
                    }
                }
            }
            //========================== end ==== jenisInput 2 ================================
            //jika pilih startDate and endDate ============================ jenisInput 3 ================================
            else if ((startDate.HasValue) && (endDate.HasValue))
            {
                jenisInput = 3;

                datevalue = (Convert.ToDateTime(startDate.ToString()));
                datevalue2 = (Convert.ToDateTime(endDate.ToString()));

                //start Date
                int selectedDate = datevalue.Day;
                int selectedMonth = datevalue.Month;
                int selectedYear = datevalue.Year;

                //end Date
                int selectedDate2 = datevalue2.Day;
                int selectedMonth2 = datevalue2.Month;
                int selectedYear2 = datevalue2.Year;

                //jika tahun sama
                if (selectedYear == selectedYear2)
                {
                    if ((selectedMonth2 - selectedMonth) < 2)
                    {
                        namaBulan.Add(selectedYear + "-" + selectedMonth +
                            "," + (selectedMonth + "/" + selectedDate + "/" + selectedYear) +
                            "," + (selectedMonth2 + "/" + selectedDate2 + "/" + selectedYear));
                    }
                    if (((selectedMonth2 - selectedMonth) > 2) && ((selectedMonth2 - selectedMonth) <= 6))
                    {
                        namaBulan = new List<string>();
                        //add selected startDate
                        namaBulan.Add(selectedYear + "-" + selectedMonth +
                            "," + (selectedMonth + "/" + selectedDate + "/" + selectedYear) +
                            "," + (selectedMonth + 1) + "/1/" + Convert.ToString(selectedYear));

                        //add next 7 monht from selected date
                        for (int b = (selectedMonth + 1); b < (selectedMonth2); b++)
                        {
                            namaBulan.Add(selectedYear + "-" + b +
                                "," + (b + "/1/" + selectedYear) +
                                "," + (b + 1) + "/1/" + Convert.ToString(selectedYear));
                        }

                        //add selected endDate
                        namaBulan.Add(selectedYear + "-" + selectedMonth2 +
                            "," + (selectedMonth2 + "/1/" + selectedYear) +
                            "," + (selectedMonth2 + "/" + selectedDate2 + "/" + selectedYear));

                    }

                    if ((selectedMonth2 - selectedMonth) > 6)
                    {
                        namaBulan = new List<string>();
                        //jika sebelum bulan juni, tampilkan 1stHalf previous year
                        namaBulan.Add("1stH-" + selectedYear + ",1/1/" + Convert.ToString(selectedYear) + ",7/1/" + Convert.ToString(selectedYear));
                        namaBulan.Add("2ndH-" + selectedYear + ",7/1/" + Convert.ToString(selectedYear) + ",1/1/" + Convert.ToString(selectedYear + 1));

                        //add selected startDate
                        namaBulan.Add(selectedYear + "-" + selectedMonth +
                            "," + (selectedMonth + "/" + selectedDate + "/" + selectedYear) +
                            "," + (selectedMonth + 1) + "/1/" + Convert.ToString(selectedYear));

                        //add next 7 monht from selected date
                        for (int b = (selectedMonth + 1); b < (selectedMonth2 - 1); b++)
                        {
                            namaBulan.Add(selectedYear + "-" + b +
                                "," + (b + "/1/" + selectedYear) +
                                "," + (b + 1) + "/1/" + Convert.ToString(selectedYear));
                        }

                        //add selected endDate
                        namaBulan.Add(selectedYear + "-" + selectedMonth +
                            "," + (selectedMonth + "/" + selectedDate + "/" + selectedYear) +
                            "," + (selectedMonth + 1) + "/1/" + Convert.ToString(selectedYear));
                    }
                }


                //jika tahun sebelumnya
                if (selectedYear < selectedYear2)
                {
                    //add selected startDate
                    namaBulan.Add(selectedYear + "-" + selectedMonth +
                        "," + (selectedMonth + "/" + selectedDate + "/" + selectedYear) +
                        "," + (selectedMonth + 1) + "/1/" + Convert.ToString(selectedYear));

                    //add all month selected startdate
                    for (int b = (selectedMonth + 1); b < 13; b++)
                    {
                        namaBulan.Add(selectedYear + "-" + b +
                            "," + (b + "/1/" + selectedYear) +
                            ",1/1/" + Convert.ToString(selectedYear + 1));
                    }
                    //add all month selected startdate
                    for (int b = 1; b < selectedMonth2; b++)
                    {
                        namaBulan.Add(selectedYear2 + "-" + b +
                            "," + (b + "/1/" + selectedYear2) +
                            "," + (b + 1) + "/1/" + Convert.ToString(selectedYear2));
                    }
                    //add selected endDate
                    namaBulan.Add(selectedYear2 + "-" + selectedMonth2 +
                        "," + (selectedMonth2 + "/" + selectedDate2 + "/" + selectedYear2) +
                        "," + (selectedMonth2 + 1) + "/1/" + Convert.ToString(selectedYear2));
                }
            };
            //========================== end ==== jenisInput 3 ================================

            //siapkan array data value
            string[] s = {};
            var newDS = new DataSet();
            var newDV = new DataView();
            var newDT = new DataTable();
            var newList = new List<string>();
            
            var newDSx = new DataSet();
            var newDVx = new DataView();
            var newDTx = new DataTable();
            var newListx = new List<string>();

            //convert List to Array
            namaBulanArray = namaBulan.ToArray();

            if (RptType == "5")
            {
                // SQL =================================================================
                var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@deptId", Convert.ToInt32(deptID))
                };
                DataSet dsPM = new DataSet();
                DataSet dsBD = new DataSet();
                dsPM = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_report_DowntimePreventive", sqlparams.ToArray());
                dsBD = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_report_DowntimeBreakdown", sqlparams.ToArray());

                for (int i = 0; i < namaBulanArray.Length; i++)
                {
                    s = namaBulanArray[i].Split(',');

                    //filter PM
                    newDT = dsPM.Tables[0];
                    newDV = new DataView(newDT);
                    string str2H;
                    str2H = "StartDt >='" + DateTime.Parse(s[1].ToString()) + "' AND StartDt <= '" + DateTime.Parse(s[2].ToString()) + "'";
                    newDV.RowFilter = str2H;
                    DataTable newDT2 = new DataTable();
                    newDT2 = newDV.ToTable();

                    // jumlah total downtime (hour)
                    object sumPM;
                    sumPM = newDT2.Compute("Sum(Downtime)", "");
                    string pm = sumPM.ToString();

                    //filter BD
                    newDTx = dsBD.Tables[0];
                    newDVx = new DataView(newDTx);
                    string str2Hx;
                    str2Hx = "StartDt >='" + DateTime.Parse(s[1].ToString()) + "' AND StartDt <= '" + DateTime.Parse(s[2].ToString()) + "'";
                    newDVx.RowFilter = str2Hx;
                    DataTable newDT2x = new DataTable();
                    newDT2x = newDVx.ToTable();

                    // jumlah total downtime breakdown (hour)
                    object sumBd;
                    sumBd = newDT2x.Compute("Sum(Downtime)", "");
                    string bd = sumBd.ToString();

                    //filter ABNORMAL
                    string strAB;
                    strAB = "NC = 'ABNORMAL'";
                    var newDVxAb = new DataView();
                    newDVxAb = new DataView(newDT2x);
                    newDVxAb.RowFilter = strAB;
                    DataTable newDT2xAB = new DataTable();
                    newDT2xAB = newDVxAb.ToTable();

                    // jumlah total downtime breakdown ABNORMAL (hour)
                    object sumBdAb;
                    sumBdAb = newDT2xAB.Compute("Sum(Downtime)", "");
                    int bdab = 0;
                    if (!Convert.IsDBNull(sumBdAb))
                    {
                        bdab = Convert.ToInt32(sumBdAb);
                    }

                    //filter NC
                    string strNC;
                    strNC = "NC = 'NC'";
                    var newDVxNc = new DataView();
                    newDVxNc = new DataView(newDT2x);
                    newDVxNc.RowFilter = strNC;
                    DataTable newDT2xNC = new DataTable();
                    newDT2xNC = newDVx.ToTable();

                    // jumlah total downtime breakdown ABNORMAL (hour)
                    object sumBdNc;
                    sumBdNc = newDT2xNC.Compute("Sum(Downtime)", "");
                    int bdnc = 0;
                    if (!Convert.IsDBNull(sumBdNc))
                    {
                        bdnc = Convert.ToInt32(sumBdNc);
                    }
    
                    int bdTotal = bdab + bdnc;

                    //resize array and input hasil ke array
                    var s2 = s;
                    string tempD = "";

                    Array.Resize(ref s2, s2.Length + 1);
                    s2[s2.Length - 1] = pm;

                    Array.Resize(ref s2, s2.Length + 1);
                    s2[s2.Length - 1] = Convert.ToString(bdTotal);

                    Array.Resize(ref s2, s2.Length + 1);
                    s2[s2.Length - 1] = Convert.ToString(bdab);

                    Array.Resize(ref s2, s2.Length + 1);
                    s2[s2.Length - 1] = Convert.ToString(bdnc);

                    for (int x = 0; x < s2.Length; x++)
                    {
                        tempD = tempD + "," + s2[x].ToString();
                    }

                    tempD = tempD.Substring(1);
                    tempD = tempD.Replace(",", "#");

                    newList.Add(tempD);
                }
            }
            else if (RptType == "4")
            {
                var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@deptId", Convert.ToInt32(deptID))
                };
                DataSet top10 = new DataSet();
                top10 = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, Url, sqlparams.ToArray());

                //filter PM
                var top10Table = new DataTable();
                var top10View = new DataView();

                top10Table = top10.Tables[0];
                top10View = new DataView(top10Table);

                /*string strQuery;
                strQuery = "StartDt >='" + DateTime.Parse(s[1].ToString()) + "' AND StartDt <= '" + DateTime.Parse(s[2].ToString()) + "'";
                top10View.RowFilter = strQuery;
                DataTable top10Table2 = new DataTable();
                top10Table2 = top10View.ToTable();*/

                List<string> listMesin = new List<string>();
                Dictionary<string, List<string[]>> ListMesinTgl = new Dictionary<string, List<string[]>>();

                for (int i=0; i < top10Table.Rows.Count; i++)
                {
                    DataRow row = top10Table.Rows[i];
                    
                    var yy = new List<string[]>();
                    string[] bln = { };
                    for (int x = 0; x < namaBulanArray.Length; x++)
                    {

                        bln = namaBulanArray[x].Split(',');
                       // ListMesinTgl[row["id"].ToString()]= s1;
                        //listMesin.Add(s.ToString());
                        //yy.Add(s1);
                        string urlx = "usp_report_WorkOrderStatus";
                        var sqlparamsInner = new List<SqlParameter>
                        {
                            new SqlParameter("@startDate", DateTime.Parse(bln[1].ToString())),
                            new SqlParameter("@endDate", DateTime.Parse(bln[2].ToString())),
                            new SqlParameter("@deptID", Convert.ToInt32(deptID))
                        };
                        DataSet mcDS = new DataSet();
                        mcDS = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, urlx, sqlparamsInner.ToArray());
                        var mcDT = new DataTable();
                        mcDT = mcDS.Tables[0];
                        var mcDV = new DataView(mcDT);
                        mcDV.RowFilter = "generated_date >='" + DateTime.Parse(bln[1].ToString()) + "' AND generated_date <= '" + DateTime.Parse(bln[2].ToString()) + "'";
                        DataTable mcDT2 = mcDV.ToTable();
                        var mcQty = (mcDT2.Select("Asset_Id ='" + row["id"].ToString() + "'").Length).ToString();

                        Array.Resize(ref bln, bln.Length + 1);
                        bln[bln.Length - 1] = mcQty;
                        yy.Add(bln);
                    }
                    string xy = row["id"].ToString() + "," + row["Descriptions"].ToString();
                    ListMesinTgl.Add(xy, yy);
                }
                //string[] finalArray = ListMesinTgl
                JavaScriptSerializer json1 = new JavaScriptSerializer();
                return json1.Serialize(ListMesinTgl);    
            }
            else
            {
                // SQL =================================================================
                var sqlparams = new List<SqlParameter>
                {
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@deptID", Convert.ToInt32(deptID))
                };

                //Ambil all data (startdate to end date)
                DataSet dsMaster = new DataSet();
                //sMaster = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, "usp_report_WorkOrderStatus", sqlparams.ToArray());
                dsMaster = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure, Url, sqlparams.ToArray());

                for (int i = 0; i < namaBulanArray.Length; i++)
                {
                    s = namaBulanArray[i].Split(',');

                    //filter
                    newDT = dsMaster.Tables[0];
                    newDV = new DataView(newDT);

                    string str2H;

                    str2H = "generated_date >='" + DateTime.Parse(s[1].ToString()) + "' AND generated_date <= '" + DateTime.Parse(s[2].ToString()) + "'";

                    newDV.RowFilter = str2H;

                    //var newDT2
                    DataTable newDT2 = new DataTable();
                    newDT2 = newDV.ToTable();

                    var s2 = s;
                    string tempD = "";

                    string so = "";
                    string sc = "";
                    string ac = "";
                    string ab = "";
                    string nc = "";

                    if (RptType == "1")
                    {
                        so = (newDT2.Select("Status_Id < 5").Length).ToString();
                        sc = (newDT2.Select("Status_Id = 5").Length).ToString();
                        //resize s array and add new value to it
                        Array.Resize(ref s2, s2.Length + 1);
                        s2[s2.Length - 1] = so;

                        Array.Resize(ref s2, s2.Length + 1);
                        s2[s2.Length - 1] = sc;
                    }
                    else if (RptType == "2")
                    {
                        so = (newDT2.Select("Status_Id < 7").Length).ToString();
                        sc = (newDT2.Select("Status_Id = 7").Length).ToString();
                        //resize s array and add new value to it
                        Array.Resize(ref s2, s2.Length + 1);
                        s2[s2.Length - 1] = so;

                        Array.Resize(ref s2, s2.Length + 1);
                        s2[s2.Length - 1] = sc;
                    }
                    else if (RptType == "3")
                    {
                        ac = (newDT2.Select("NC = 'ACCIDENT'").Length).ToString();
                        ab = (newDT2.Select("NC = 'ABNORMAL'").Length).ToString();
                        nc = (newDT2.Select("NC = 'NC'").Length).ToString();

                        Array.Resize(ref s2, s2.Length + 1);
                        s2[s2.Length - 1] = ac;

                        Array.Resize(ref s2, s2.Length + 1);
                        s2[s2.Length - 1] = ab;

                        Array.Resize(ref s2, s2.Length + 1);
                        s2[s2.Length - 1] = nc;
                    }
                    for (int x = 0; x < s2.Length; x++)
                    {
                        tempD = tempD + "," + s2[x].ToString();
                    }

                    tempD = tempD.Substring(1);
                    tempD = tempD.Replace(",", "#");

                    newList.Add(tempD);
                }
            }
                                  
            string[] finalArray = newList.ToArray();
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(finalArray);       
        }
    }
}
