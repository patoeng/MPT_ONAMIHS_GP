<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TPM.Default" %>
<%@ Import Namespace="TPM.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
     
 <div class="row-fluid"> 
        <div class="span12">
            <div runat="server" ClientIDMode="Static" id="tableContainer">
                <p>Loading The Page ...&nbsp;</p>
            </div>
       </div>
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    
  <script type="text/javascript">
      var paramId ='<%= paramId%>';
      var paramText = '<%= paramText%>';
      var m = '<%= m%>';
      if (paramId != '')
      {
          if (m == '') {
              setTimeout(loader, 300);
          } else {
              $('#jsonTable').dataTable({
                  "sPaginationType": "full_numbers"
              });
              enableBtn('0');
              $('.link2').click(function () {
                  var id = $(this).attr('id');
                  var text = ($(this).attr('value'));

                  $.ajax({
                      url: './Methodes/mainpage.asmx/getlist',
                      cache: false,
                      type: 'POST',
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      data: JSON.stringify({ id: id, text: paramText, depid: '0' }, null, 2),
                      success: function (json) {
                          var obj = (json.d);
                          $('#tableContainer').html('<h3>' + text + '</h3>');
                          $('#tableContainer').append(obj);
                          $('#jsonTable').dataTable({
                              "sPaginationType": "full_numbers"
                          });


                          enableBtn($('.ddl option:selected').val() == null ? '0' : $('.ddl option:selected').val());
                      },
                      error: function (xhr, ajaxOptions, thrownError) {
                          var obj = $.parseJSON(xhr.responseText);
                          alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                              "Stack Trace : \r\n" + obj.StackTrace
                          );
                      }
                  });


              });
          }
          
      }
      function enableBtn(deptid) {
          $('.btn').click(function () {
              var id = $(this).attr('id');
              text = ($(this).attr('value'));

              $.ajax({
                  url: './Methodes/mainpage.asmx/downloadlist',
                  cache: false,
                  type: 'POST',
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  data: JSON.stringify({ id: id,depid: deptid}, null, 2),
                  success: function (json) {
                      try {
                          var obj = (json.d);
                          window.open("../<%=TPMHelper.WebDirectory%>/StreamDownload.aspx?u=http://<%= Request.Url.Authority +Request.ApplicationPath  %>/" + obj, "_self");
                                  } catch (ex) {

                                  }
                              },
                              error: function (xhr) {
                                  var obj = $.parseJSON(xhr.responseText);
                                  alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                                      "Stack Trace : \r\n" + obj.StackTrace
                                  );
                              }
                          });
                      });
                    
      }
      function loaderDdl() {
          $('.ddl').change(function () {
              $.ajax({
                  url: './Methodes/mainpage.asmx/getlist',
                  cache: false,
                  type: 'POST',
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  data: JSON.stringify({ id: paramId, text: paramText, depid: $('.ddl option:selected').val() == null ? '0' : $('.ddl option:selected').val() }, null, 2),
                  success: function (json) {
                      var obj = (json.d);
                      $('#tableContainer').html('<h3>' + paramText + '</h3>');
                      $('#tableContainer').append(obj);
                      $('#jsonTable').dataTable({
                          "sPaginationType": "full_numbers"
                      });
                      $('.link2').click(function () {
                          var id = $(this).attr('id');
                          var text = ($(this).attr('value'));

                          $.ajax({
                              url: './Methodes/mainpage.asmx/getlist',
                              cache: false,
                              type: 'POST',
                              contentType: "application/json; charset=utf-8",
                              dataType: "json",
                              data: JSON.stringify({ id: id, text: paramText, depid: '0' }, null, 2),
                              success: function (json) {
                                  var obj = (json.d);
                                  $('#tableContainer').html('<h3>' + text + '</h3>');
                                  $('#tableContainer').append(obj);
                                  $('#jsonTable').dataTable({
                                      "sPaginationType": "full_numbers"
                                  });


                                  enableBtn($('.ddl option:selected').val() == null ? '0' :$('.ddl option:selected').val());
                              },
                              error: function (xhr, ajaxOptions, thrownError) {
                                  var obj = $.parseJSON(xhr.responseText);
                                  alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                                      "Stack Trace : \r\n" + obj.StackTrace
                                  );
                              }
                          });


                      });
                      loaderDdl();
                      enableBtn($('.ddl option:selected').val() == null ? '0' : $('.ddl option:selected').val());
                  },
                  error: function (xhr, ajaxOptions, thrownError) {
                      var obj = $.parseJSON(xhr.responseText);
                      alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                          "Stack Trace : \r\n" + obj.StackTrace
                      );
                  }
              });
          });
      }

      function loader() {
          var id = paramId;//$(this).attr('id');
          var text = paramText;//($(this).text());
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
              data: JSON.stringify({id:id,text:paramText,depid:'0' }, null, 2),
              success: function (json) {
                  var obj = (json.d);
                  $('#tableContainer').html('<h3>'+text+'</h3>');
                  $('#tableContainer').append(obj);
                  loaderDdl();
                  $('.link2').click(function () {
                      var id = $(this).attr('id');
                      var text = ($(this).attr('value'));

                      $.ajax({
                          url: './Methodes/mainpage.asmx/getlist',
                          cache: false,
                          type: 'POST',
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          data: JSON.stringify({ id: id, text: paramText, depid: '0' }, null, 2),
                          success: function (json) {
                              var obj = (json.d);
                              $('#tableContainer').html('<h3>' + text + '</h3>');
                              $('#tableContainer').append(obj);
                              $('#jsonTable').dataTable({
                                  "sPaginationType": "full_numbers"
                              });

                                 
                              enableBtn('0');
                          },
                          error: function (xhr, ajaxOptions, thrownError) {
                              var obj = $.parseJSON(xhr.responseText);
                              alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                                  "Stack Trace : \r\n" + obj.StackTrace
                              );
                          }
                      });


                  });
                  enableBtn($('.ddl option:selected').val() == null ? '0' : $('.ddl option:selected').val());
                  $('#jsonTable').dataTable({
                      "sPaginationType": "full_numbers"
                  });
              },
              error: function (xhr) {
                  var obj = $.parseJSON(xhr.responseText);
                  alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                      "Stack Trace : \r\n" + obj.StackTrace
                  );
              }
          });
          return false;
      }
  </script>
        
</asp:Content>
