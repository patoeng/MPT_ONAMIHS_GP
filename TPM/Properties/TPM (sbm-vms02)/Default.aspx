<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TPM.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/TPM/Styles/accordion.css" type="text/css">
     <style type="text/css">
         body
         {
            background-image: url('/TPM/Images/UnderConstruction.jpg');
            background-repeat:no-repeat;
            background-attachment:fixed;
            background-position: bottom right ;
         }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    
 
    <div class="row-fluid"> 
       <div class="span3">

        <section id="accordion">
             <ul>
                <li>
                    <div class="handle">Work Orders</div>
                    <div class="panel">
                        <ul>
                                <li><a href="javascript:void(0)"  id="wo_1"  class="link" >Need Engineering Confirmation</a></li>
                                <li><a href="javascript:void(0)"  id="wo_2"  class="link" >Waiting For Work Execution</a></li>
                                <li><a href="javascript:void(0)"  id="wo_3"  class="link" >On Work</a></li>
                                <li><a href="javascript:void(0)"  id="wo_4"  class="link" >Waiting For Leader Review</a></li>
                            </ul>
                    </div>
                </li>
                <li>
                    <div class="handle">Daily Prompt</div>
                    <div class="panel">
                        <ul>
                                <li><a href="javascript:void(0)"  id="dp_1"  class="link" >Checklist Filling</a></li>
                                <li><a href="javascript:void(0)"  id="dp_2"  class="link" >Waiting for Leader Confirmation</a></li>
                                <li><a href="javascript:void(0)"  id="dp_3"  class="link" >Over Due</a></li>
                        </ul>
                    </div>
                </li>
                <li>
                <div class="handle">PM Schedule</div>
                <div class="panel">
                        
                            <ul>
                                <li><a href="javascript:void(0)"  id="pm_1"  class="link" >PM Due</a></li>
                                <li><a href="javascript:void(0)"  id="pm_2"  class="link" >Asset On PM</a></li>
                                <li><a href="javascript:void(0)"  id="pm_3"  class="link" >PM Due on The Next Day</a></li>
                                <li><a href="javascript:void(0)"  id="pm_4"  class="link" >PM Due in the next 2 months</a></li>
                            </ul>
                        
                    </div>
                </li>
                 <li>
                <div class="handle">BOM</div>
                <div class="panel">                        
                            <ul>
                                <li><a href="javascript:void(0)"  id="bom_1"  class="link" >BOM Availibity</a></li>
                                <li><a href="javascript:void(0)"  id="bos_1"  class="link" >BOM Low Stock All</a></li>
                            </ul>
                        
                    </div>
                </li>
                 <li>
                 <div class="handle">Summary</div>
                 <div class="panel">
                        <ul>
                            <li><a href="javascript:void(0)"  id="sum_1"  class="link" >Asset Down Time Today</a></li>
                            <li><a href="javascript:void(0)"  id="asd"  class="link" >Asset Down Time</a></li>
                        </ul>
                    </div>
                </li>
            </ul>
        </section>
      
        </div>
        <div class="span9">
            <div id="tableContainer">
                <h2>Welcome <% TPM.Classes.MySessions sessions = new TPM.Classes.MySessions(); Response.Write( sessions.EmployeeName); %></h2>
                <h4>The TPM Application is still Under Construction. We'll be back soon.&nbsp;</h4>
            </div>
       </div>
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
   
  <script type="text/javascript" src="/TPM/Scripts/jquery.accordion.js" charset="utf-8"></script>
  <script type="text/javascript">
      $('#accordion ul').accordion({ multiple: true });
      $('.link').click(function () {
          var id = $(this).attr('id');
          var text = ($(this).text());
          if (id == "asd") {
              window.open("YDownTime.aspx?v=0", "_self");
              return true;
          }
          $.ajax({
              url: './Methodes/mainpage.asmx/getlist',
              cache: false,
              type: 'POST',
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              data: JSON.stringify({id:id }, null, 2),
              success: function (json) {
                  var obj = (json.d);
                  $('#tableContainer').html('<h3>'+text+'</h3>');
                  $('#tableContainer').append(obj);
                  
                  $('.link2').click(function () {
                      var id = $(this).attr('id');
                      var text = ($(this).attr('value'));

                      $.ajax({
                          url: './Methodes/mainpage.asmx/getlist',
                          cache: false,
                          type: 'POST',
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          data: JSON.stringify({ id: id }, null, 2),
                          success: function (json) {
                              var obj = (json.d);
                              $('#tableContainer').html('<h3>' + text + '</h3>');
                              $('#tableContainer').append(obj);
                              $('#jsonTable').dataTable({
                                  "sPaginationType": "full_numbers"
                              });
                          },
                          error: function (xhr, ajaxOptions, thrownError) {
                              var obj = $.parseJSON(xhr.responseText);
                              alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                                    "Stack Trace : \r\n" + obj.StackTrace
                                    );
                          }
                      });

                  });
                  $('#jsonTable').dataTable({
                      "sPaginationType": "full_numbers"
                  });
              },
              error: function (xhr, ajaxOptions, thrownError) {
                  var obj = $.parseJSON(xhr.responseText);
                  alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                        "Stack Trace : \r\n" + obj.StackTrace
                        );
              }
          });
          
      });
  </script>
        
</asp:Content>
