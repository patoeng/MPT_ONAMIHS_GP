<%@ Page Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="TPM.Report" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">

    <div class="row-fluid">
        <div id="divMain" class="span10">
            <form id="form1" runat="server">
                <div class="row-fluid">
                    <div class="span12">
                        <asp:Label ID="lblTitle" runat="server" ClientIDMode="Static" cssClass="lblTitle" Font-Size="24px" Font-Bold="true"></asp:Label><br /><br />
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span3" style="display:inline;">
                        Department :<br />
                        <asp:DropDownList runat="server" ID="ddlDepartment" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                    </div>
                    <div class="span3" style="display:inline;">
                        <asp:Label ID="ddl1" runat="server" ClientIDMode="Static" cssClass="lbl">Status :</asp:Label>
                        <asp:Label ID="ddl2" runat="server" ClientIDMode="Static" cssClass="lbl">Work request type :</asp:Label>
                        <asp:DropDownList runat="server" ID="ddlStatus" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                        <asp:DropDownList runat="server" ID="ddlRequestType" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                    </div>
                    <div class="span3 defHide" style="display:inline;">
                        <asp:Label ID="ddl3" runat="server" ClientIDMode="Static" cssClass="lbl"> NC or ABNORMAL :</asp:Label>
                        <asp:DropDownList runat="server" ID="ddlNC" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                    </div>
                    <div class="span3">
                        Start Date :<br />
                        <asp:TextBox id="txtstartDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="span3">
                        To Date :<br />
                        <asp:TextBox id="txtendDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
            </form>
        </div>
        <div class="span2" style="padding-top:60px;">
           <span class="btn btn-primary btnShowReport" id="btnRp" >Show Report</span>
        </div>
    </div>
    <div class="row-fluid">
        <div id="divMain2"class="span12">
            <div id="hChart" style="width:95%; height:550px;"></div>
            <div id="tableData" style="width:1024px; height:200px; display:none;"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">

  <script type="text/javascript">
      $(document).ready(function () {

          var reportType = '<%= reportType %>';
          var reportTittle = "";
          var RptType = 0;
          var strUrl = "";
          var dept = "0";
          var stat = "0";
          var req = "0";
          var abnc = "0";
          $('.defHide').hide();

          // datetime picker
          $('#txtstartDate').Zebra_DatePicker({
              direction: false,
              inside: true,
              yearRange: "-1:+0"
          });
          $('#txtendDate').Zebra_DatePicker({
              direction: false,
              inside: true,
              yearRange: "-1:+0"
          });

          // jika dropdown di click
          $('#ddlDepartment').click(function () {
              var e = document.getElementById("ddlDepartment");
              dept = e.options[e.selectedIndex].value;
          });

          $('#ddlStatus').click(function () {
              var e = document.getElementById("ddlStatus");
              stat = e.options[e.selectedIndex].value;
          });

          $('#ddlRequestType').click(function () {
              var e = document.getElementById("ddlRequestType");
              req = e.options[e.selectedIndex].value;

              document.getElementById("ddlNC").disabled = true;
              switch (req) {
                  case "2":
                      document.getElementById("ddlNC").disabled = false;
                      break;
              }
          });

          $('#ddlNC').click(function () {
              var e = document.getElementById("ddlNC");
              abnc = e.options[e.selectedIndex].value;
          });

          // ambil parameter value
          switch (reportType) {
              case "1":
                  RptType = 1;
                  strUrl = "usp_report_WorkOrderStatus";
                  reportTittle = "Work Order Status";
                  $('#h3Title').value = "Work Order Status";
                  break;
              case "2":
                  RptType = 2;
                  strUrl = "usp_report_ImprovementWorkOrderStatus";
                  reportTittle = "Improvement Work Order Status";
                  $('#h3Title').text = "Improvement Work Order Status";
                  break;
              case "3":
                  RptType = 3;
                  strUrl = "usp_report_SummaryBreakdownByCase";
                  reportTittle = "Summary Breakdown By Case";
                  $('#h3Title').text = "Summary Breakdown By Case";
                  break;
              case "4":
                  RptType = 4;
                  strUrl = "usp_report_Top10";
                  reportTittle = "Top 10 Case Maintenance Base on Main Machines";
                  $('#h3Title').text = "Top 10 Case Maintenance Base on Main Machines";
                  break;
              case "5":
                  RptType = 5;
                  strUrl = "usp_report_SummaryPMvsBD";
                  reportTittle = "Summary PM vs BD by Hours";
                  $('#h3Title').text = "Summary PM vs BD by Hours";
                  $('.defHide').show();
                  document.getElementById("ddlNC").disabled = false;
                  break;
          };

          //highchart global option
          var namaFile = "Chart" + RptType.toString();

          Highcharts.setOptions({
              chart: {
                  type: 'column'
              },
              title: {
                  text: reportTittle
              },
              xAxis: {
                  title: {
                      text: 'Period'
                  }
              },
              yAxis: {
                  min: 0,
                  title: {
                      text: 'Work Order'
                  },
                  stackLabels: {
                      enabled: true,
                      style: {
                          fontWeight: 'bold',
                          color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                      }
                  },
                  labels: {
                      overflow: 'justify'
                  }
              },
              legend: {
                  align: 'right',
                  verticalAlign: 'top',
                  x: -50,
                  y: 0,
                  floating: true,
                  backgroundColor: '#FFF',
                  borderColor: '#CCC',
                  borderWidth: 1,
                  shadow: false
              },
              exporting: {
                  url: 'HighchartsExport.aspx',
                  width: 2200,
                  filename: namaFile
              },
              tooltip: {
                  formatter: function () {
                      return '<b>' + this.x + '</b><br/>' +
                          this.series.name + ': ' + this.y + '<br/>' +
                          'Total: ' + this.point.stackTotal;
                  }
              },
              plotOptions: {
                  column: {
                      dataLabels: {
                          enabled: false,
                          color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'gray',
                          shadow: false
                      }
                  }
              }
          });
          //jika button report di click
          $('#btnRp').click(function () {
              //console.log(dept);
              //console.log(stat);
              //console.log(RptType);
              //console.log(strUrl);
              $.ajax({
                  url: './Methodes/report.asmx/reportWorkOrderStatus',
                  cache: false,
                  type: 'POST',
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  data: JSON.stringify({
                      startDate: $('#txtstartDate').val(),
                      endDate: $('#txtendDate').val(),
                      deptID: dept,
                      statusID: stat,
                      RptType: RptType,
                      Url: strUrl
                  }, null, 2),
                  success: function(hasil) {
                      var obj = hasil.d;
                      var data = JSON.parse(obj);
                      var dataStr = String(data);
                      var dataArr = dataStr.split(",");
                      var dataArr2 = [];
                      var seriesName = [];
                      var dataOpen = [];
                      var dataClose = [];
                      var ac = [];
                      var ab = [];
                      var nc = [];
                      var pm = [];
                      var bm = [];

                      switch (RptType) {
                      case 1:
                          if (stat == "1") {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      dataOpen.push(parseInt(dataArr2[3]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#cc3333'],
                                      xAxis: { categories: seriesName },
                                      plotOptions: { column: { stacking: 'normal' } },
                                      series: [
                                          { name: 'Open', data: dataOpen }
                                      ]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } //end if 1

                          else if (stat == "5") {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      dataClose.push(parseInt(dataArr2[4]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#0099cc'],
                                      xAxis: { categories: seriesName },
                                      plotOptions: { column: { stacking: 'normal' } },
                                      series: [
                                          { name: 'Close', data: dataClose }
                                      ]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } else {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      dataOpen.push(parseInt(dataArr2[3]));
                                      dataClose.push(parseInt(dataArr2[4]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#cc3333', '#0099cc', '#9933cc'],
                                      plotOptions: { column: { stacking: 'normal' } },
                                      xAxis: { categories: seriesName },
                                      series: [
                                          { name: 'Open', data: dataOpen },
                                          { name: 'Close', data: dataClose }
                                      ]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } // end IF 
                          break;
                      case 2:
                          if (stat == "1") {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      dataOpen.push(parseInt(dataArr2[3]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#3366cc'],
                                      plotOptions: { column: { stacking: 'normal' } },
                                      xAxis: { categories: seriesName },
                                      series: [
                                          { name: 'Open', data: dataOpen }
                                      ]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } //end if 1

                          else if (stat == "5") {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      dataClose.push(parseInt(dataArr2[4]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#ff9933'],
                                      plotOptions: { column: { stacking: 'normal' } },
                                      xAxis: { categories: seriesName },
                                      series: [
                                          { name: 'Close', data: dataClose }
                                      ]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } else {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      dataOpen.push(parseInt(dataArr2[3]));
                                      dataClose.push(parseInt(dataArr2[4]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#3366cc', '#ff9933'],
                                      plotOptions: { column: { stacking: 'normal' } },
                                      xAxis: { categories: seriesName },
                                      series: [{
                                              name: 'Open',
                                              data: dataOpen
                                          }, {
                                              name: 'Close',
                                              data: dataClose
                                          }]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } // end IF 
                          break;
                      case 3:
//============================================================== Summary By case
                          if (stat == "0") {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      ac.push(parseInt(dataArr2[3]));
                                      ab.push(parseInt(dataArr2[4]));
                                      nc.push(parseInt(dataArr2[5]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart', type: 'column' },
                                      colors: ['#3399cc', '#cc3333', '#99cc33'],
                                      tooltip: {
                                          formatter: function() {
                                              return '<b>' + this.x + '</b><br/>' +
                                                  this.series.name + ': ' + this.y;
                                          }
                                      },
                                      plotOptions: {
                                          column: {
                                              dataLabels: {
                                                  enabled: true,
                                                  color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'gray',
                                                  shadow: false
                                              }
                                          }
                                      },
                                      xAxis: { categories: seriesName },
                                      series: [{
                                              name: 'ACCIDENT',
                                              data: ac
                                          }, {
                                              name: 'ABNORMAL',
                                              data: ab
                                          }, {
                                              name: 'NC',
                                              data: nc
                                          }]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } // end if 0
                          if (stat == "1") {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      ac.push(parseInt(dataArr2[3]));
                                  }
                                  ;
                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#3399cc'],
                                      plotOptions: {
                                          column: {
                                              dataLabels: {
                                                  enabled: true,
                                                  color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'gray',
                                                  shadow: false
                                              }
                                          }
                                      },
                                      xAxis: { categories: seriesName },
                                      series: [{
                                          name: 'ACCIDENT',
                                          data: ac
                                      }]
                                  }); //end chart
                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } // end if 1

                          //if 2
                          if (stat == "2") {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      ab.push(parseInt(dataArr2[4]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#99cc33'],
                                      xAxis: { categories: seriesName },
                                      plotOptions: {
                                          column: {
                                              dataLabels: {
                                                  enabled: true,
                                                  color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'gray',
                                                  shadow: false
                                              }
                                          }
                                      },
                                      series: [{
                                          name: 'ABNORMAL',
                                          data: ab
                                      }]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } // end if 2

                          //if 3
                          if (stat == "3") {
                              try {
                                  for (i = 0; i < dataArr.length; i++) {
                                      dataStr = String(dataArr[i]);
                                      dataArr2 = dataStr.split("#");

                                      //masukkan ke array  hasil akhir
                                      seriesName.push(String(dataArr2[0]));
                                      ac.push(parseInt(dataArr2[3]));
                                      ab.push(parseInt(dataArr2[4]));
                                      nc.push(parseInt(dataArr2[5]));
                                  }
                                  ;

                                  //generate chart
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#cc3333'],
                                      xAxis: { categories: seriesName },
                                      plotOptions: {
                                          column: {
                                              dataLabels: {
                                                  enabled: true,
                                                  color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'gray',
                                                  shadow: false
                                              }
                                          }
                                      },
                                      series: [{
                                          name: 'NC',
                                          data: nc
                                      }]
                                  }); //end chart

                              } catch(error) {
                                  alert("error " + error.message);
                              }
                          } // end if 3
                          break;
                      case 5:
//============================================================== PM vs BD
                          try {
                              for (i = 0; i < dataArr.length; i++) {
                                  dataStr = String(dataArr[i]);
                                  dataArr2 = dataStr.split("#");

                                  //masukkan ke array  hasil akhir
                                  seriesName.push(String(dataArr2[0]));
                                  pm.push(parseInt(dataArr2[3]));
                                  bm.push(parseInt(dataArr2[4]));
                                  ab.push(parseInt(dataArr2[5]));
                                  nc.push(parseInt(dataArr2[6]));
                              }
                              ;
                          } catch(error) {
                              alert("error " + error.message);
                          }
                          if (req == "0") {
                              var chart1 = new Highcharts.Chart({
                                  chart: { renderTo: 'hChart' },
                                  colors: ['#99cc33', '#cc3333'],
                                  xAxis: { categories: seriesName },
                                  plotOptions: { column: { stacking: 'normal' } },
                                  yAxis: {
                                      title: {
                                          text: 'Downtime (Hour)'
                                      }
                                  },
                                  series: [{
                                          name: 'Preventive',
                                          data: pm
                                      }, {
                                          name: 'Breakdown',
                                          data: bm
                                      }]
                              });
                          } // end if 0
                          else if (req == "1") {
                              var chart1 = new Highcharts.Chart({
                                  chart: { renderTo: 'hChart' },
                                  colors: ['#99cc33'],
                                  xAxis: { categories: seriesName },
                                  plotOptions: { column: { stacking: 'normal' } },
                                  yAxis: {
                                      title: {
                                          text: 'Downtime (Hour)'
                                      }
                                  },
                                  series: [{
                                      name: 'Preventive',
                                      data: pm
                                  }]
                              });
                          } // end if 1
                          else if (req == "2") {
                              if (abnc == "0") {
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#cc3333', '#99cc33'],
                                      xAxis: { categories: seriesName },
                                      plotOptions: { column: { stacking: 'normal' } },
                                      yAxis: {
                                          title: {
                                              text: 'Downtime (Hour)'
                                          }
                                      },
                                      series: [{
                                          name: 'Breakdown (NC)',
                                          data: nc
                                      }]
                                  });
                              } else if (abnc == "1") {
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#99cc33'],
                                      xAxis: { categories: seriesName },
                                      plotOptions: { column: { stacking: 'normal' } },
                                      yAxis: {
                                          title: {
                                              text: 'Downtime (Hour)'
                                          }
                                      },
                                      series: [{
                                          name: 'ABNORMAL',
                                          data: ab
                                      }]
                                  });
                              } else {
                                  var chart1 = new Highcharts.Chart({
                                      chart: { renderTo: 'hChart' },
                                      colors: ['#99cc33', '#cc3333'],
                                      xAxis: { categories: seriesName },
                                      plotOptions: { column: { stacking: 'normal' } },
                                      yAxis: {
                                          title: {
                                              text: 'Downtime (Hour)'
                                          }
                                      },
                                      series: [{
                                              name: 'ABNORMAL',
                                              data: ab
                                          }, {
                                              name: 'NC',
                                              data: nc
                                          }]
                                  });
                              }

                          }
// end if 1
                      }
                      

                  }
              });
          });
      });

  </script>

</asp:Content>

