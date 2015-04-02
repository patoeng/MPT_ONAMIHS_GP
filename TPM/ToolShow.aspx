<%@ Page Language="C#"  MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="ToolShow.aspx.cs" Inherits="TPM.ToolShow" %>
<%@ Import Namespace="TPM.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
     
 <div class="row-fluid"> 
        <div class="span12">
        <asp:Table runat="server" ID="tblTool" ClientIDMode="Static" cssClass="table table-bordered"></asp:Table>
       </div>
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    
  <script type="text/javascript">
      function isNumber(s) {
          var hasil = true;
          var isWhole_re = /^\s*\d+\s*$/;
          if (String(s).search(isWhole_re) == -1) hasil = false;
          return hasil;
      }
      function isValidSerialCode(s) {
          var hasil = true;
          if (s.length != 12) hasil = false;
          hasil = isNumber(s);
          return hasil;
      }
      $(document).ready(function () {

          $('#tblTool').dataTable();
          $('.editableMachine').editable('./Methodes/tool.asmx/EditableMachine', {
              indicator: "<img src='./Images/indicator.gif'>",
              onblur: 'submit',
              // submit: "OK",
              //cancel: "Cancel",
              tooltip: "Click to edit...",
              style: "inherit",
              //type: "select",
              //data: "{'1':'OK','0':'NC'}",
              onsubmit: function (settings, value) {
                  var hasil = true;
                  var input = $(value).find('input');
                  //alert($(input).val());
                  hasil = isValidSerialCode($(input).val());
                  if (hasil) {
                      $(input).after("");
                  } else {
                      $(input).after("<br><span><i>Invalid Machine Serial Number</i></span>");
                  }
                  return hasil;
              },
              ajaxoptions: { contentType: "application/json; charset=utf-8" },
              intercept: function (jsondata) {
                  try {
                      var id = $(this).attr('id').split('__');
                      
                      obj = $.parseJSON(jsondata);
                      obj = $.parseJSON(obj.d);
                      // do something with obj.status and obj.other{
                      $('#lblMachine_Descriptions__' + id[1]).html(obj[1]);
                      return (obj[0]);
                  }
                  catch (err) {
                      alert(err.message);
                  }
                  return "";

              }
          });
          $('.editableToolLifeSpec').editable('./Methodes/tool.asmx/EditableToolLifeSpec', {
              indicator: "<img src='./Images/indicator.gif'>",
              onblur: 'submit',
              // submit: "OK",
              //cancel: "Cancel",
              tooltip: "Click to edit...",
              style: "inherit",
              //type: "select",
              //data: "{'1':'OK','0':'NC'}",
              onsubmit: function (settings, value) {
                  var hasil = true;
                  var input = $(value).find('input');
                  //alert($(input).val());
                  hasil = isNumber($(input).val());
                  if (hasil) {
                      $(input).after("");
                  } else {
                      $(input).after("<br><span><i>Invalid Number</i></span>");
                  }
                  return hasil;
              },
              ajaxoptions: { contentType: "application/json; charset=utf-8" },
              intercept: function (jsondata) {
                  try {
                      obj = $.parseJSON(jsondata);
                     
                      return (obj.d);
                  }
                  catch (err) {
                      alert(err.message);
                  }
                  return "";

              }
          });
          
          $('.editablePosition').editable('./Methodes/tool.asmx/EditablePosition', {
              indicator: "<img src='./Images/indicator.gif'>",
              onblur: 'submit',
              // submit: "OK",
              //cancel: "Cancel",
              tooltip: "Click to edit...",
              style: "inherit",
              type: "select",
              data: "{'Machine':'Machine'," +
                    "'Stock Before Sharpen':'Stock Before Sharpen'," +
                    "'Supplier':'Supplier'," +
                    "'Stock After Sharpen':'Stock After Sharpen'" +
                    "}",
              ajaxoptions: { contentType: "application/json; charset=utf-8" },
              intercept: function (jsondata) {
                  try {
                      obj = $.parseJSON(jsondata);

                      return (obj.d);
                  }
                  catch (err) {
                      alert(err.message);
                  }
                  return "";

              }
          });
          $('.editableDescription').editable('./Methodes/tool.asmx/EditableDescription', {
              indicator: "<img src='./Images/indicator.gif'>",
              onblur: 'submit',
              // submit: "OK",
              //cancel: "Cancel",
              tooltip: "Click to edit...",
              style: "inherit",
             // type: "select",
             /* data: "{'Machine':'Machine'," +
                    "'Stock Before Sharpen':'Stock Before Sharpen'," +
                    "'Supplier':'Supplier'," +
                    "'Stock After Sharpen':'Stock After Sharpen'" +
                    "}",*/
              ajaxoptions: { contentType: "application/json; charset=utf-8" },
              intercept: function (jsondata) {
                  try {
                      obj = $.parseJSON(jsondata);

                      return (obj.d);
                  }
                  catch (err) {
                      alert(err.message);
                  }
                  return "";

              }
          });
      });
  </script>
        
</asp:Content>
