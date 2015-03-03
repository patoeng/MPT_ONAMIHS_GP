<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/TPM.Master" CodeBehind="TpmEmailNew.aspx.cs" Inherits="TPM.TpmEmailNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
         <div class="row-fluid">
            <div class="span12">
                <h2>TPM Auto Email Details</h2>  
                </div>
             </div>
         <div class="row-fluid">
            <div class="span2">
                <asp:HiddenField runat="server" ID="EmailId" ClientIDMode="Static"/>
                    <asp:Label runat="server" >Email Subject</asp:Label>
               </div>
            <div class="span4">
                   <asp:Label ID="lblDescription" runat="server" ClientIDMode="Static" CssClass="editableTxt"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span2">
                <asp:HiddenField runat="server" ID="HiddenField1" ClientIDMode="Static"/>
                    <asp:Label ID="Label6" runat="server" >Remarks</asp:Label>
               </div>
            <div class="span4">
                   <asp:Label ID="lblRemarks" runat="server" ClientIDMode="Static"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span2">
                    <asp:Label ID="Label4" runat="server" >Active</asp:Label>
               </div>
            <div class="span4">
                   <asp:Label ID="lblActive" ClientIDMode="Static" runat="server" CssClass="editableYesNo"></asp:Label>
            </div>
         </div>
        <div class="row-fluid">
            <div class="span2">
                    <asp:Label ID="Label1" runat="server" >Repeat</asp:Label>
             </div>
            <div class="span4">
                    <asp:Label ID="lblRepeat" ClientIDMode="Static" runat="server" CssClass="editableYesNo"></asp:Label>
            </div>
         </div>
        <div class="row-fluid">
         <div class="span2">
                    <asp:Label ID="Label3" runat="server" >Emailing Type</asp:Label>
               </div>
            <div class="span4">
                    <asp:Label ID="lblEmailing" runat="server" ClientIDMode="Static" ></asp:Label>
            </div>
         </div>
         <div class="row-fluid">
         <div class="span2">
                    <asp:Label ID="Label5" runat="server" >Send per Department</asp:Label>
               </div>
            <div class="span4">
                    <asp:Label ID="lblEachDept" runat="server" ClientIDMode="Static" CssClass="editableYesNo" ></asp:Label>
            </div>
         </div>
        <div>    
        <div class="row-fluid">
            
            <div class="span2">
                <asp:Label ID="Label2" runat="server" >Frequency</asp:Label>
            </div>
            <div class="span2">
                <input type="checkbox" id="day_1" value="1" clientidmode="Static" runat="server"/>Sunday
                </div>
            <div class="span2">
                <input type="checkbox" id="day_2" value="2" clientidmode="Static" checked runat="server"/>Monday
            </div>
             <div class="span2">
                <input type="checkbox" id="day_3" value="4" clientidmode="Static" checked runat="server"/>Tuesday
            </div>
        </div>
        <div class="row-fluid">
             <div class="span2">
             </div>
             <div class="span2">
                <input type="checkbox" id="day_4" value="8" clientidmode="Static" checked runat="server"/>Wednesday
             </div>
             <div class="span2">
                <input type="checkbox" id="day_5" value="16" clientidmode="Static" checked runat="server"/>Thursday
            </div>
             <div class="span2">
                <input type="checkbox" id="day_6" value="32" clientidmode="Static" checked runat="server"/>Friday
            </div>
            <div class="span2">
                <input type="checkbox" id="day_7" value="64" clientidmode="Static" checked runat="server"/>Saturday
            </div>
         </div>
        </div>
        <div class="row-fluid">
             <div class="span12">
                 Recipient
             </div>
        </div>
        <div class="row-fluid">
             <div class="span11" id="recipientHolder">
                <asp:Table runat="server" ID="tblRecipient" ClientIDMode="Static" cssClass="table table-bordered"></asp:Table>
             </div>
        </div>
        <div class="row-fluid">
             <div class="span11" id="Div1">
                <a href="#" class="btn btn-primary btn_adduser" id="btn_adduser-<%=EmailingId %>">Add Recipient</a>
             </div>
        </div>
    </form>   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        function isValidEmailAddress(emailAddress) {
            var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
            return pattern.test(emailAddress);
        };
        $('.editableYesNo').editable('./Methodes/Emailing.asmx/UpdateEmail', {
            indicator: "<img src='./Images/indicator.gif'>",
            onblur: 'submit',
            // submit: "OK",
            //cancel: "Cancel",
            tooltip: "Click to edit...",
            style: "inherit",
            type: "select",
            data: "{'0' : 'No','1' :'Yes'}",
            submitdata: function (value, settings) {
                return {
                    "origValue": this.revert,
                    "EmailId": $('#EmailId').val()
                };
            },
            ajaxoptions: { contentType: "application/json; charset=utf-8" },
            intercept: function (jsondata) {
                try {
                    obj = jQuery.parseJSON(jsondata);
                    return (obj.d);
                } catch (err) {
                    alert(err.message);
                }
                return this.revert;

            }
        });
        $('.editableYesNo2').editable('./Methodes/Emailing.asmx/UpdateEmail2', {
            indicator: "<img src='./Images/indicator.gif'>",
            onblur: 'submit',
            // submit: "OK",
            //cancel: "Cancel",
            tooltip: "Click to edit...",
            style: "inherit",
            type: "select",
            data: "{'0' : 'Regular','1' :'Event Triggered'}",
            submitdata: function (value, settings) {
                return {
                    "origValue": this.revert,
                    "EmailId": $('#EmailId').val()
                };
            },
            ajaxoptions: { contentType: "application/json; charset=utf-8" },
            intercept: function (jsondata) {
                try {
                    obj = jQuery.parseJSON(jsondata);
                    return (obj.d);
                } catch (err) {
                    alert(err.message);
                }
                return this.revert;

            }
        });
        $('.editableTxt').editable('./Methodes/Emailing.asmx/UpdateEmail', {
            indicator: "<img src='./Images/indicator.gif'>",
            onblur: 'submit',
            // submit: "OK",
            //cancel: "Cancel",
            width: "400px",
            tooltip: "Click to edit...",
            style: "inherit",
            //type: "select",
            //data: "{'0' : 'Regular','1' :'Event Triggered'}",
            submitdata: function (value, settings) {
                return {
                    "origValue": this.revert,
                    "EmailId": $('#EmailId').val()
                };
            },
            ajaxoptions: { contentType: "application/json; charset=utf-8" },
            intercept: function (jsondata) {
                try {
                    obj = jQuery.parseJSON(jsondata);
                    return (obj.d);
                } catch (err) {
                    alert(err.message);
                }
                return this.revert;

            }
        });
        function Initialize() {
            $('.dept_editable').editable('./Methodes/Emailing.asmx/Update', {
                indicator: "<img src='./Images/indicator.gif'>",
                onblur: 'submit',
                // submit: "OK",
                //cancel: "Cancel",
                tooltip: "Click to edit...",
                style: "inherit",
                type: "select",
                data: "<%= Department %>",
                submitdata: function(value, settings) {
                    return {
                        "origValue": this.revert
                    };
                },
                ajaxoptions: { contentType: "application/json; charset=utf-8" },
                intercept: function(jsondata) {
                    try {
                        obj = jQuery.parseJSON(jsondata);
                        return (obj.d);
                    } catch(err) {
                        alert(err.message);
                    }
                    return this.revert;

                }
            });
            
            $('.text_editable').editable('./Methodes/Emailing.asmx/Update', {
                indicator: "<img src='./Images/indicator.gif'>",
                onblur: 'submit',
                // submit: "OK",
                //cancel: "Cancel",
                tooltip: "Click to edit...",
                style: "inherit",
                // type: "select",
                // data: "{'1':'OK','0':'NC'}",
                onsubmit: function(settings, value) {
                    var hasil=true;
                    var id = $(value).attr('id');
                    id = id.split('_');
                    if (id[0] == 'EmailAddress') {
                        var input = $(value).find('input');
                        //alert($(input).val());
                        hasil = isValidEmailAddress($(input).val());
                        if (hasil) {
                            $(input).after("");
                        } else {
                            $(input).after("<br><span>Invalid Email Address</span>");
                        }
                    }

                    return hasil;
                },

                submitdata: function(value, settings) {
                    return {
                        "origValue": this.revert
                    };
                },
                ajaxoptions: { contentType: "application/json; charset=utf-8" },
                intercept: function(jsondata) {
                    try {
                        obj = jQuery.parseJSON(jsondata);

                        return (obj.d);
                    } catch(err) {
                        alert(err.message);
                    }
                    return "";

                }
            });
            $('.btnremove').click(function() {
                var id = $(this).attr('id');

                id = id.split('_');
                var idd = id[1] + '-row';
                $('#' + idd).remove();
                //alert('#' + idd);

                $.ajax({
                    url: './Methodes/Emailing.asmx/Remove',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ id: id[1] }, null, 2),
                    success: function(json) {

                        try {
                            var obj = json.d;
                            if (obj == "OK")
                                $('#' + idd).remove();

                        } catch(error) {
                            alert("error " + error.message);
                        }
                        //$('#formHolder').hide(1000);
                    },
                    error: function(xhr, ajaxOptions, thrownError) {
                        var obj = $.parseJSON(xhr.responseText);
                        alert(obj.Message);
                    }
                });
            });
        };
        $('.btn_adduser').click(function () {
            var id = $(this).attr('id');
            id = id.split('-');

            $.ajax({
                url: './Methodes/Emailing.asmx/Insert',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: id[1] }, null, 2),
                success: function (json) {

                    try {
                        var obj = json.d;
                        if (obj != "ER")
                            $('#tblRecipient tr:last').after('<tr id="' + obj + '-row"><td class="text_editable" id="EmailAddress_' + obj + '"></td><td class="text_editable" id="EmployeeName_' + obj + '"></td><td class="dept_editable" id="Department_' + obj + '"></td><td><a class="btn btnremove btn-primary" id="btn_' + obj + '">REMOVE</a></td></tr>');
                            Initialize();

                    } catch (error) {
                        alert("error " + error.message);
                    }
                    //$('#formHolder').hide(1000);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });
           
        });
        $('input:checkbox').change(function() {
            var id = $(this).attr('id');
            var checked = $('#' + id).is(':checked') ? "1" : "0";
           
            $.ajax({
                url: './Methodes/Emailing.asmx/UpdateFrequency',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: $('#EmailId').val(), check: checked,value:$('#' + id).val() }, null, 2),
                success: function (json) {
                    try {
                        var obj = json.d;
                       // if (obj != "ER")
                          //  $('#tblRecipient tr:last').after('<tr id="' + obj + '-row"><td class="text_editable" id="EmailAddress_' + obj + '"></td><td class="text_editable" id="EmployeeName_' + obj + '"></td><td class="dept_editable" id="Department_' + obj + '"></td><td><a class="btn btnremove" id="btn_' + obj + '">REMOVE</a></td></tr>');
                        //Initialize();

                    } catch (error) {
                        alert("error " + error.message);
                    }
                    //$('#formHolder').hide(1000);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });
            
        });
        Initialize();
    </script>
</asp:Content>
