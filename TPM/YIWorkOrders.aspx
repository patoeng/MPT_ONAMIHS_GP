<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YIWorkOrders.aspx.cs" Inherits="TPM.YIWorkOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
    <div class="row-fluid">
        <div class="span6">
            <label for="search">Enter Improvement Work Order Number</label>
            <input name="search" type="text" id="txtMWOID" class="required" style="display:inline"/>&nbsp;
            <hr />  
        </div>
        <div class="span6 text-right"><h4>Form No: RE-OR-23</h4></div>
    </div>
    <div class="row-fluid">
        <div class="span8">
            <h3 style="display:<%= Status==""? "none":"block"%>">Improvement Work Order Details</h3>
            <asp:Table ID="tblLastStatus" runat="server" ClientIDMode="Static" CssClass="table table-bordered table-condensed"></asp:Table>
        </div>
         <div class="span4"></div>
    </div>
    <div class="row-fluid">
        <div class="span12">
            <hr />
            <h3 style="display:<%= Status==""? "none":"block"%>">Improvement Work Order Steps</h3>
            <asp:Table ID="tblHistory" runat="server" ClientIDMode="Static" CssClass="table table-bordered table-striped"></asp:Table>
        </div>
    </div>
   </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
     <!-- <script src="http://api.jquery.com/resources/events.js"></script> -->
    
    <script src="./Scripts/jquery.formatCurrency-1.4.0.min.js"></script>
    <script src="./Scripts/jquery.formatCurrency.id-ID.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#form1').validate({
                submitHandler: function () {
                    window.open("<%= Request.Url.AbsolutePath %>?v=" + $('#txtMWOID').val(), "_self");
                }
            }
            );
            $('#txtMWOID').keypress(function (event) {
                if (event.which == 13) {
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
                switch (action) {
                    case "REMARKS":
                        value = $('#txtRemarks').val();
                        id = $('#lwoid').val();
                        break;
                    case "CANCEL":
                        value = '8';
                        id = $('#mwoid').val();
                        break;
                    default:
                        value = "";
                        id = $('#mwoid').val();
                        break;
                }
                $.ajax({
                    url: './Methodes/iwo.asmx/action',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ act: action, id: id, val: value }, null, 2),
                    success: function (json) {

                        try {
                            var obj = json.d;
                            if (obj == -1) {
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

       
        $('.editable2').editable('./Methodes/iwo.asmx/Jeditable', {
            indicator: "<img src='./Images/indicator.gif'>",
            onblur: 'submit',
            submit: "OK",
            cancel: "Cancel",
            tooltip: "Click to edit...",
            style: "inherit",
            type: "select",
            data: "{'1':'PROJECT','2':'IMPROVE','3':'MODIFICATION','4':'PART REPAIR','5':'OTHER'}",
            ajaxoptions: { contentType: "application/json; charset=utf-8" },
            intercept: function (jsondata) {
                try {
                    var obj = jQuery.parseJSON(jsondata);
                    // do something with obj.status and obj.other{
                    var hasil = "";
                    switch(obj.d) {
                        case "1":
                            hasil = "PROJECT";
                            break;
                        case "2":
                            hasil = "IMPROVE";
                            break;
                        case "3":
                            hasil = "MODIFICATION";
                            break;
                        case "4":
                            hasil = "PART REPAIR";
                            break;
                        case "5":
                            hasil = "OTHER";
                            break;
                    }
                    return (hasil);
                }
                catch (err) {
                    alert(err.message);
                }
                return "";

            }
        });
        $('.text_editable').editable('./Methodes/iwo.asmx/Jeditable', {
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
        $('.cost').toNumber({ colorize: true, region: 'id-ID' }).formatCurrency({ colorize: true, region: 'id-ID' });
        $('.editable_cost').editable('./Methodes/iwo.asmx/Jeditable_Cost', {
                indicator: "<img src='./Images/indicator.gif'>",
                onblur: 'submit',
                submit: "OK",
                //cancel: "Cancel",
                tooltip: "Click to edit...",
                style: "inherit",
                // type: "select",
                // data: "{'1':'OK','0':'NC'}",
                submitdata: function (value, settings) {
                    var input = $('#' + $(this).attr('id')).find('input');
                    input.toNumber();
                    var changedVal = input.toNumber().val();
                    //changedVal = changedVal.replace(',', '');
                    //changedVal = changedVal.replace('.', '');
                    return {
                        "id": $(this).attr('id'),
                        "value": changedVal     
                    };
                },
                ajaxoptions: { contentType: "application/json; charset=utf-8" },
                intercept: function (jsondata) {
                    try {
                        var obj = jQuery.parseJSON(jsondata);
                        window.location.reload();
                       return obj.d;
                    } catch (err) {
                        alert(err.message);
                    }
                    return "";

                }
            });
        
    </script>
</asp:Content>
