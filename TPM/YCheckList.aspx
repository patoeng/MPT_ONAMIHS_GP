<%@ Page Title="TPM System Checklist" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YCheckList.aspx.cs" Inherits="TPM.YCheckList" %>
<%@ Import Namespace="TPM.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
  
    <div class="row-fluid">
            <div class="span3"><h4>PT. SHIMANO BATAM</h4></div>
        <div class="span6 text-center"><asp:PlaceHolder runat="server" ID="barcodeHolder"/></div>
            <div class="span3">FORM NO : <h5 style="display:inline"><asp:Label ID="LblForm" ClientIDMode="Static" runat="server"></asp:Label></h5></div>
    </div>
     <div class="row-fluid">
            <div class="span12 text-center"><h3 style="display:inline"><asp:Label ID="LblTitle" ClientIDMode="Static" runat="server"></asp:Label> Machine Checklist</h3></div>           
    </div>
     <div class="row-fluid">
            <div class="span4"><asp:Table ID="TblAtasKiri" ClientIDMode="Static" cssClass="table table-condensed" runat="server"></asp:Table></div>
            <div class="span5 text-center"></div>
            <div class="span3"><asp:Table ID="TblAtasKanan" ClientIDMode="Static" cssClass="table table-condensed" runat="server"></asp:Table></div>
     </div>
    <div class="row-fluid">
            <div <%= Checklisttypeid==1? "class=\"span5\"" : "style=\"display:none;\"" %>><asp:Table ID="TblImage" ClientIDMode="Static" cssClass="table table-bordered" runat="server"></asp:Table></div>
            <div class=<%= Checklisttypeid==1? "\"span7\"" :"\"span11\"" %>><asp:Table ID="TblListItems" ClientIDMode="Static" cssClass="table table-striped table-bordered" runat="server"></asp:Table></div>
    </div>
    <div class="row-fluid">
            <div class="span3"><h5>Cara Pengisian Hasil Pengecheckan</h5>
                <ul>
                    <li>Select "OK" jika sesuai standard</li>
                    <li>Select "ABNORMAL" jika kondisi Tidak normal</li>
                    <li>Select "NC" jika tidak sesuai standard</li>
                </ul>
            </div>
            <div class="span9"><asp:Table ID="TblConfirm" ClientIDMode="Static" cssClass="table table-bordered" runat="server"></asp:Table></div> 
        </div>
        </form>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
     <script type="text/javascript">

         $('.editable').editable('./Methodes/crud.asmx/Jeditable', {
             indicator: "<img src='./Images/indicator.gif'>",
             onblur: 'submit',
             // submit: "OK",
             //cancel: "Cancel",
             tooltip: "Click to edit...",
             style: "inherit",
             type: "select",
             data: "{'1':'OK','2':'ABNORMAL',3:'NC'}",
             ajaxoptions: { contentType: "application/json; charset=utf-8" },
             intercept: function (jsondata) {
                 try {
                     obj = jQuery.parseJSON(jsondata);
                     // do something with obj.status and obj.other{
                     return (obj.d);
                 }
                 catch (err) {
                     alert(err.message);
                 }
                 return "";

             }
         });
         $('.editable2').editable('./Methodes/crud.asmx/Jeditable2', {
             indicator: "<img src='./Images/indicator.gif'>",
             onblur: 'submit',
             // submit: "OK",
             //cancel: "Cancel",
             tooltip: "Click to edit...",
             style: "inherit",
             // type: "select",
             // data: "{'1':'OK','0':'NC'}",
             ajaxoptions: { contentType: "application/json; charset=utf-8" },
             intercept: function (jsondata) {
                 try {
                     obj = jQuery.parseJSON(jsondata);
                     // do something with obj.status and obj.other{
                     $("#userid").val(obj.d);
                     if ($("#userid").val() == "") { alert("User Not Registered. Please Contact TPM administrator") }
                     return (obj.d);
                 }
                 catch (err) {
                     alert(err.message);
                 }
                 return "";

             }
         });
         $('.mesin_run_editable').editable('./Methodes/crud.asmx/checklistRunning', {
             indicator: "<img src='./Images/indicator.gif'>",
             onblur: 'submit',
             // submit: "OK",
             //cancel: "Cancel",
             tooltip: "Click to edit...",
             style: "inherit",
             type: "select",
             data: "{'1':'YES','0':'NO'}",
             ajaxoptions: { contentType: "application/json; charset=utf-8" },
             intercept: function (jsondata) {
                 try {
                     var obj = jQuery.parseJSON(jsondata);
                     var outputs = obj.d == 1 ? 'YES' : 'NO';
                     window.location.reload();
                     return (outputs);
                 }
                 catch (err) {
                     alert(err.message);
                 }
                 return "";

             }
         });
         $('.rem_editable').editable('./Methodes/crud.asmx/rem_editable', {
             indicator: "<img src='./Images/indicator.gif'>",
             onblur: 'submit',
             // submit: "OK",
             //cancel: "Cancel",
             tooltip: "Click to edit...",
             style: "inherit",
             // type: "select",
             // data: "{'1':'OK','0':'NC'}",
             submitdata: function (value, settings) {
                 return {
                     "origValue": this.revert
                 };
             },
             ajaxoptions: { contentType: "application/json; charset=utf-8" },
             intercept: function (jsondata) {
                 try {
                     obj = jQuery.parseJSON(jsondata);

                     return (obj.d);
                 }
                 catch (err) {
                     alert(err.message);
                 }
                 return "";

             }
         });
         $('.btn').click(function (event) {
             event.preventDefault();
             var id = this.id;
             if (id == "btnNotConfirm") {
                 $('#mode').val("notconfirmed");
             }
             if (id == "btnConfirm") {
                 $('#mode').val("confirmed");
             }
             if (id == "btnSubmit") {
                 $('#mode').val("checked");
             }
             if (id == "btnImg") {

                 window.location = "/<%=TPMHelper.WebDirectory%>/YCheckListImg.aspx?v=" + $('#selectChecklist').val();
                return true;
            }
            $.ajax({
                url: './Methodes/crud.asmx/Confirm',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ anoman: $('#form1').serializeArray() }, null, 2),
                success: function (json) {
                    var obj = $.parseJSON(json.d);

                    alert(obj.message);
                    location.reload();

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                          "Stack Trace : \r\n" + obj.StackTrace
                          );
                }
            });

        });
        $("#selectChecklist").change(function () {
            if ($('#selectChecklist').val() != "") {
                window.location = "<%= Request.Url.AbsolutePath %>?m=p&id=" + $(this).val();
            }
        });
    </script>
</asp:Content>
