<%@ Page Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="Report2.aspx.cs" Inherits="TPM.Report2" %>
<%@ Import Namespace="TPM.Classes" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
        <div id="divMain" class="span10">
            <form id="form1" runat="server">
                 <div class="row-fluid">
                     <div class="span3">
                        <label for="ddlDepartment">Department </label>
                            <asp:DropDownList runat="server" ID="ddlDepartment" ClientIDMode="Static" cssClass="ddl inline"></asp:DropDownList>        
                    </div>   
                     <div class="span3">
                         <asp:Label ID="lblddlFlexy" runat="server" ClientIDMode="Static" cssClass="lbl">Status :</asp:Label>
                            <asp:DropDownList runat="server"  ID="ddlFlexy" ClientIDMode="Static" CssClass="ddl">
                            </asp:DropDownList>
                    </div> 
                      <div class="span3" <%= ReportType!="4" ? "style=\"display: none;\"" :""  %>>
                         <asp:Label ID="Label1" runat="server" ClientIDMode="Static" cssClass="lbl">Status Machine Condition </asp:Label>
                            <asp:DropDownList runat="server"  ID="ddlReport4" ClientIDMode="Static" CssClass="ddl">
                            </asp:DropDownList>
                    </div> 
                  </div>
                 <div class="row-fluid">
                    <div class="span3">
                         <label for="txtStartdate">From Date </label>
                        <input runat="server" ClientIDMode="Static" type="text" id="txtStartdate" class="datepicker required" />           
                    </div>
                    <div class="span3">
                        <label for="txtEnddate">To Date </label>
                            <input runat="server" ClientIDMode="Static" type="text" id="txtEnddate" class="datepicker"/>         
                    </div>
       
                 </div>
            </form>
        </div>
      
    </div>
    <div class="row-fluid">
                    <div class="span12">
                        <asp:Label ID="lblTitle" runat="server" ClientIDMode="Static" cssClass="lblTitle" Font-Size="24px" Font-Bold="true"></asp:Label><br /><br />
                    </div>
                </div>
    <div class="row-fluid">
      
        <div id="divMain2"class="span12">
            <div id="hChart" style="width:100%; height:500px;"></div>
        </div>
        
    </div>
    <div class="row-fluid">
         <div id="div3"class="span1">
          
        </div>
        <div id="tableContainer"class="span10">
           
        </div>
         <div id="divHidden"class="span1" style="display: none;">
          
        </div>
    </div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
<script src="./Scripts/highcharts-3d.js"></script>
  <script type="text/javascript">
      $(document).ready(function() {
          var reportType = '<%= ReportType %>';
          var chartTittle = '<%= Tittle %>';
          
          var isdownload = 0;
         
          var colors =[];
          switch (reportType) {
              case "1":
                  colors = ['red','blue'];
                  break;
              case "2":
                  colors = ['red', 'blue'];
                  break;
              case "3":
                  colors = ['green', 'red', 'cyan'];
                  break;
              case "4":
                  colors = ['green','red'];
                  break;
              case "5":
                  colors = ['blue', 'red', 'green', 'purple', '#FFA500', 'cyan', 'lime','blue','violet'];
                  break;
          }
          
          // datetime picker
          $('#txtStartdate').Zebra_DatePicker({
              direction: false,
              inside: true,
              yearRange: "-1:+0",
              onSelect: function (date) {
                  $('#form1').trigger('submit');
              }
          });
          $('#txtEnddate').Zebra_DatePicker({
              direction: false,
              inside: true,
              yearRange: "-1:+0",
              onSelect: function (date) {
                  $('#form1').trigger('submit');
              }
          });
          $('.btn').click(function() {
              $('#form1').trigger('submit');
          });
          $('.ddl').change(function () {
              $('#form1').trigger('submit');
          });
          //department
          $('#ddlDepartment').click(function(event) {
              var ddl = $(this);

              if ((document.getElementById("ddlDepartment").length <= 1)) {
                  $.ajax({
                      url: './Methodes/report2.asmx/getddl',
                      cache: false,
                      type: 'POST',
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      data: JSON.stringify({ id: "" }, null, 2),
                      success: function(json) {
                          try {
                              var obj = json.d;
                              var data = $.parseJSON(obj);
                              ddl.empty();
                              ddl.append("<option value='' selected>ALL</option>");
                              $.each(data, function(val, text) {
                                  ddl.append("<option value='" + val + "'>" + text + "</option>");
                              });

                          } catch(error) {
                              alert("error " + error.message);
                          }
                          $('#formHolder').hide(1000);
                      },
                      error: function(xhr, ajaxOptions, thrownError) {
                          var obj = $.parseJSON(xhr.responseText);
                          alert(obj.Message);
                      }
                  });
              }
          });
          
          $('#form1').validate({
              submitHandler: function() {
                  $.ajax({
                      url: './Methodes/Report2.asmx/summary',
                      cache: false,
                      type: 'POST',
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      data: JSON.stringify({
                          isdownload: isdownload,
                          department: $('#ddlDepartment option:selected').val(),
                          startdate: $('#txtStartdate').val(),
                          enddate: $('#txtEnddate').val(),
                          rType: reportType,
                          ddlFlexy: $('#ddlFlexy option:selected').val(),
                          report4 : $('#ddlReport4 option:selected').val()
                      }, null, 2),
                      success: function(json) {
                          try {
                              var obj = null;
                              if (isdownload == 0) {
                                  obj = json.d;
                                  $('#tableContainer').html("");
                                  $('#tableContainer').append(obj);
                                 
                                  //$('#jsonTable').dataTable({
                                  //    "sPaginationType": "full_numbers"
                                  //});
                                  $('.btn').click(function() {
                                      isdownload = 1;
                                      $('#form1').trigger('submit');
                                  });
                                  
                                  $('#jsonTable tr').each(function () {
                                      $(this).find('td').each(function () {
                                          if ($(this).html() == '') {
                                              $(this).html('0');
                                          }
                                        
                                      });
                                  });
                                  var jsTEmp = $("#jsonTable").clone(true);
                                  //jsTEmp.setAttribute('id', 'jsTemp1');
                                  $('#jsTemp1').remove();
                                  jsTEmp.appendTo("#divHidden");                                  
                                  jsTEmp.attr('id', 'jsTemp1');
                                  var js = $('#jsTemp1');
                                  if (reportType == '5') {
                                      js = $('#jsonTable');
                                  }
                                      js.each(function() {
                                          var $this = $(this);
                                          var newrows = [];
                                          $this.find("tr").each(function() {
                                              var i = 0;
                                              $(this).find("th").each(function() {
                                                  i++;
                                                  if (newrows[i] === undefined) {
                                                      newrows[i] = $("<tr></tr>");

                                                  }
                                                  if (i > 1) {
                                                      var l = $("<td></td>");
                                                      l.text($(this).html());
                                                      newrows[i].append(l);
                                                  } else {
                                                  
                                                  newrows[i].append($(this));
                                                }
                                              });
                                          });
                                          $this.find("thead").remove();
                                          $this.find("tr").each(function() {
                                              var i = 0;

                                              $(this).find("td").each(function() {
                                                  i++;
                                                  if (newrows[i] === undefined) {
                                                      newrows[i] = $("<tr></tr>");

                                                  }
                                                  if (i == 1) {
                                                      var l = $("<th></th>");
                                                      l.text($(this).html());
                                                      newrows[i].append(l);
                                                  } else {
                                                  
                                                      newrows[i].append($(this));
                                                  }
                                              });
                                          });
                                          $this.find("tr").remove();
                                       
                                          $.each(newrows, function () {
                                             
                                                  $this.append(this);
                                            
                                          });
                                      });
                                  
                                


                                      var tr = $('#jsonTable tr:last').clone();
                                      $('#jsonTable tr:last').after(tr);
                                      $("#jsonTable tr:last td:first").text("Total");
                                      $("#jsonTable tr:last td:not(:first)").text(function (i) {
                                          var t = 0;
                                          var y = 0;
                                          $(this).parent().prevAll().find("td:nth-child(" + (i + 2) + ")").each(function () {
                                              
                                             var k = $(this).parent().find("td:first").text();
                                              if ((k.indexOf('-') != -1) || (reportType != '5')) {
                                                  t += parseInt($(this).text(), 10) || 0;
                                                if ((y == 2) && (reportType == '3')) {
                                                    t -= parseInt($(this).text(), 10) || 0;
                                                }
                                                 
                                              }
                                              y++;

                                          });
                                          return t;
                                      });

                                  
                                  
                                  var x = 0;
                                  $('#jsonTable tr:not(:first,:last)').each(function() {
                                      if ((x != 2) || (reportType != '3')) {
                                          $(this).css('color', colors[x % colors.length]);
                                      }


                                      x++;
                                  });
                                  $('#jsonTable tr,thead').each(function () {                                    
                                      $(this).find('td,th').each(function () {
                                         
                                          $(this).css('font-family', 'arial');
                                          $(this).css('font-weight', 'bold');
                                          $(this).css('font-size', '16px');
                                      });

                                     
                                  });
                                  $('#hChart').highcharts({
                                      colors:colors,
                                      data: {
                                          table: document.getElementById('jsTemp1')
                                          
                                      },
                                      chart: {
                                          type: 'column',
                                          backgroundColor: '#F3F3F3',
                                          margin: 100,
                                          options3d: {
                                              enabled: false,
                                              alpha: 15,
                                              beta:15,
                                              depth: 50
                                          }
                                      },
                                      plotOptions: {
                                          column: {
                                              stacking: (reportType == 3) || (reportType == 5) ? 0 : 'normal',
                                              depth: 25
                                          }
                                      },
                                      title: {
                                          text: chartTittle,
                                          style: {"color": "black",
                                              "fontSize": "28px",
                                              "fontWeight": "bold"
                                              
                                      }
                                  },
                                      yAxis: {
                                          allowDecimals: false,
                                          title: {
                                             text: '',
                                              style: {
                                                  "color": "black",
                                                  "fontSize": "16px",
                                                  "fontWeight": "bold"
                                              }
                                          },
                                          labels: {
                                              // text: 'Units',
                                              style: {
                                                  "color": "black",
                                                  "fontSize": "14px",
                                                  "fontWeight": "bold"
                                              }
                                          }
                                      },
                                      xAxis: {
                                          allowDecimals: false,
                                          labels: {
                                              // text: 'Units',
                                              style: {
                                                  "color": "black",
                                                  "fontSize": "14px",
                                                  "fontWeight": "bold"
                                              }
                                          }
                                      },
                                      tooltip: {
                                          formatter: function () {
                                              return '<b>' + this.series.name + '</b><br/>' +
                                                  this.point.y + ' ' + this.point.name.toLowerCase();
                                          }
                                      }
                                 
                                  });
                                  
                              } else {
                                  obj = json.d;
                                  isdownload = 0;
                                  window.open("../<%=TPMHelper.WebDirectory%>/StreamDownload.aspx?u=http://<%= Request.Url.Authority +Request.ApplicationPath  %>/" + obj, "_self");
                              }
                          } catch(ex) {
                              isdownload = 0;
                          }

                      },
                      error: function(xhr, ajaxOptions, thrownError) {
                          var obj = $.parseJSON(xhr.responseText);
                          $('#tableContainer').html("");
                          $('#tableContainer').append("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                              "Stack Trace : \r\n" + obj.StackTrace + "\r\n<h3>Trying to use page reload instead... Please wait...</h3>"
                          );
                          //window.open("YBDSummary.aspx?m=1&sc=" + $('#serialcode').val() + "&dp=" + $('#Department option:selected').val() + "&st=" + $('#status_id option:selected').val() + "&sd=" + $('#startdate').val() + "&ed=" + $('#enddate').val() + "&wo=" + $('#adhoc option:selected').val(), "_self");
                      }
                  });
              }
          });
          //
         
         
      });

  </script>

</asp:Content>

