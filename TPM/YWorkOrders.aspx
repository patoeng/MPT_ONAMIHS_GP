<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YWorkOrders.aspx.cs" Inherits="TPM.YWorkOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
    <div class="row-fluid">
        <div class="span6">
            <label for="search">Enter Work Order Number</label>
            <input name="search" type="text" id="txtMWOID" class="required number" style="display:inline"/>&nbsp;
            <hr />  
        </div>
    </div>
    <div class="row-fluid">
        <div class="span8">
            <h3 style="display:<%= Status==""? "none":"block"%>">Work Order Details</h3>
            <asp:Table ID="tblLastStatus" runat="server" ClientIDMode="Static" CssClass="table table-bordered table-condensed"></asp:Table>
        </div>
         <div class="span4">
                
                
                 
             </div>
    </div>
    <div class="row-fluid">
        <div class="span12">
            <hr />
            <h3 style="display:<%= Status==""? "none":"block"%>">Work Order Steps</h3>
            <asp:Table ID="tblHistory" runat="server" ClientIDMode="Static" CssClass="table table-bordered table-striped"></asp:Table>
        </div>
    </div>
   </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
   <!-- <script src="http://api.jquery.com/resources/events.js"></script> -->
    <script type="text/javascript">
        $(document).ready(function () {
            $('#form1').validate({
                submitHandler: function () {
                    window.open("<%= Request.Url.AbsolutePath %>?v=" + $('#txtMWOID').val(), "_self");
                }
            }
            );
            $('#txtMWOID').keypress(function(event) {
                if ( event.which == 13 ) {
                    event.preventDefault();
                    $('#form1').trigger('submit');
                   
                }
               // $.print(event,"html");
               // $.print(event);
            });
            $('.btn').click(function () {
                
                var action = $(this).attr('value');
                var value;
                var id;
                action = action.toUpperCase();
                switch (action)
                {
                    case "REMARKS":
                        value = $('#txtRemarks').val();
                        id = $('#lwoid').val();
                        break;
                    default:
                        value = "";
                        id = $('#mwoid').val();
                        break;
                }
                $.ajax({
                    url: './Methodes/workorder.asmx/action',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ act : action, id:id, val:value }, null, 2),
                    success: function (json) {

                        try {
                            var obj = json.d;
                            if (obj == -1){
                                window.location.reload();
                            }

                        } catch (error) {
                            alert("error " + error.message);
                        }
                        
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                        var obj = $.parseJSON(xhr.responseText);
                        alert(obj.Message);
                    }
                });
            });
            
        });
        
      $('.editable').editable('./Methodes/workorder.asmx/Jeditable', {
          indicator: "<img src='./Images/indicator.gif'>",
          onblur: 'submit',
          submit: "OK",
          cancel: "Cancel",
          tooltip: "Click to edit...",
          style: "inherit",
          type: "select",
          data: "{'(OC)OPERATE IN GOOD CONDITION':'(OC)OPERATE IN GOOD CONDITION'," +
                  "'(OD)OPERATE IN DIRTY CONDITION':'(OD)OPERATE IN DIRTY CONDITION'," +
                  "'(OA)OPERATE IN ABNORMAL CONDITION':'(OA)OPERATE IN ABNORMAL CONDITION'," +
                  "'(BD)BREAK DOWN':'(BD)BREAK DOWN'," +
                  "'(UR)STILL UNDER REPAIR/SERVICE':'(UR)STILL UNDER REPAIR/SERVICE'}",
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
      $('.editable2').editable('./Methodes/workorder.asmx/Jeditable', {
          indicator: "<img src='./Images/indicator.gif'>",
          onblur: 'submit',
          submit: "OK",
          cancel: "Cancel",
          tooltip: "Click to edit...",
          style: "inherit",
          type: "select",
          data: "{'Preventive Maintenance' :'Preventive Maintenance',"+
      "'Breakdown Maintenance' : 'Breakdown Maintenance',"+
      "'Service &  Inspection' :'Service &  Inspection',"+
      "'No Action Required' :'No Action Required'}"
      ,
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
      $('.text_editable').editable('./Methodes/workorder.asmx/Jeditable', {
          indicator: "<img src='./Images/indicator.gif'>",
          onblur: 'submit',
          submit: "OK",
          //cancel: "Cancel",
          height: '100px',
          type: "textarea",
          tooltip: "Click to edit...",
          style: "inherit",
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
    </script>
</asp:Content>
