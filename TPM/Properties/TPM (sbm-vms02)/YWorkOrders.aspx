<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YWorkOrders.aspx.cs" Inherits="TPM.YWorkOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         body
         {
            background-image: url('/TPM/Images/UnderConstruction.jpg');
            background-repeat:no-repeat;
            background-attachment:fixed;
            background-position:center;
         }
  </style>
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
        <div class="span6">
            <h3 style="display:<%= status==""? "none":"block"%>">Work Order Details</h3>
            <asp:Table ID="tblLastStatus" runat="server" ClientIDMode="Static" CssClass="table table-bordered table-condensed"></asp:Table>
        </div>
        <div class="span6">
            <asp:Table ID="tblInfo" runat="server" ClientIDMode="Static" ></asp:Table>
        </div>
    </div>
    <div class="row-fluid">
        <div class="span6">
            <hr />
            <h3 style="display:<%= status==""? "none":"block"%>">Work Order Steps</h3>
            <asp:Table ID="tblHistory" runat="server" ClientIDMode="Static" CssClass="table table-bordered table-striped"></asp:Table>
        </div>
    </div>
   </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script src="http://api.jquery.com/resources/events.js"></script>
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
                        $('#formHolder').hide(1000);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                        var obj = $.parseJSON(xhr.responseText);
                        alert(obj.Message);
                    }
                });
            });
            
        });
    </script>
</asp:Content>
