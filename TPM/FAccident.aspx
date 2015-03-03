<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="FAccident.aspx.cs" Inherits="TPM.FAccident" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="./Styles/jquery.timeentry.css"> 
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
         <div class="span3">
           
         </div>
        <div class="span6">
          <div class="row-fluid">
              <div class="span2">
              <img alt="K3_logo" src="Images/k3.png"></img></div><div class="span10">
            <h3>Accident Report</h3></div>
          </div>
            <form runat="server" id="form1">
                <asp:Table ID="tblForm" runat="server" ClientIDMode="Static" CssClass="table table-condensed"></asp:Table>
            </form>
        </div><div class="span3 text-right">Form No :EHS-SOP-27/01A</div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript" src="./Scripts/jquery.plugin.min.js"></script> 
    <script type="text/javascript" src="./Scripts/jquery.timeentry.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.validator.addMethod("time", function (value, element) {
                return this.optional(element) || /^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$/i.test(value);
            }, "Please enter a valid time.");
            
            $('#txtEmployeeNumber').keypress(function (e) {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {

                    $.ajax({
                        url: './Methodes/accident.asmx/getEmployee',
                        cache: false,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ id: $('#txtEmployeeNumber').val() }, null, 2),
                        success: function(json) {
                            try {
                                var obj = json.d;
                                $('#txtEmployeeName').val(obj);
                            } catch(error) {
                                alert("error " + error.message);
                            }

                        },
                        error: function(xhr, ajaxOptions, thrownError) {
                            var obj = $.parseJSON(xhr.responseText);
                            alert(obj.Message);
                        }
                    });
                    e.preventDefault();
                } else {
                    $('#txtEmployeeName').val("");
                }
            });
            $('#ddlDepartment').click(function(event) {
                var ddl = $(this);

                if (document.getElementById("ddlDepartment").length == 1) {
                    $.ajax({
                        url: './Methodes/accident.asmx/getddl',
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
                                $.each(data, function(val, text) {
                                    ddl.append("<option value='" + val + "'>" + text + "</option>");
                                });
                                ddl.append("<option value='' selected>Please Select...</option>");
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
            $('#ddlDepartment').change(function() {
               
                var id = $(this).val();

                $.ajax({
                    url: './Methodes/accident.asmx/getddl',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ id: id }, null, 2),
                    success: function(json) {

                        try {
                            var obj = json.d;
                            var data = $.parseJSON(obj);
                            $('#ddlAsset').empty();
                            $.each(data, function(val, text) {
                                $('#ddlAsset').append("<option value='" + val + "'>" + text + "</option>");
                            });
                            $('#ddlAsset').append("<option value='' selected>Please Select...</option>");
                            $('#txtmachineNumber').val("");
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

            });
            $('.btn').click(function() {
                $('#form1').trigger('submit');
            });
            $('#txtDate').Zebra_DatePicker();
           
            $('#form1').validate({
                submitHandler: function () {
                    $.ajax({
                        url: './Methodes/accident.asmx/generate',
                        cache: false,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            ai:$('#AccessKey').val(),
                            eno: $('#txtEmployeeNumber').val(),
                            enam: $('#txtEmployeeName').val(),
                            dep: $('#ddlDepartment option:selected').val(),
                            dt: $('#txtDate').val() + ' ' + $('#txtTime').val() + ':00',
                            jb: $('#txtJob').val(),
                            lc: $('#txtLocation').val(),
                            mcno: $('#txtmachineNumber').val(),
                            mcdis: $('#ddlAsset option:selected').text(),
                            inj: $('#txtInjuries').val(),
                            dmc: $('#txtDaysMC').val(),
                            at: $('#ddlDAccidentType option:selected').val(),
                            others: $('#txtOthers').val(),
                           
                        }, null, 2),
                        success: function (json) {

                            try {
                                var obj = json.d;
                                if (obj > 0) {
                                    window.location = "./YAccAttachment.aspx?v=" + obj;
                                }

                            } catch (error) {
                                alert("error " + error.message);
                            }

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //alert(xhr.status);
                            //alert(thrownError);
                            //alert(xhr.responseText);

                            var obj = $.parseJSON(xhr.responseText);
                            alert(obj.Message);
                        }
                    });
                }
            });
            $('#ddlAsset').change(function() {
                $('#txtmachineNumber').val($('#ddlAsset option:selected').val());
            });
            $('#txtmachineNumber').keypress(function (e) {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    if ($('#ddlAsset option[value="' + $('#txtmachineNumber').val() + '"]').text() == "") {
                        alert("Machine number does not exist in the current Department!");
                    } else {
                        $('#ddlAsset option').prop('selected', false)
                            .filter('[value="' + $('#txtmachineNumber').val() + '"]')
                            .prop('selected', true);
                    }
                    e.preventDefault();
                }
            });
        });
    </script>
</asp:Content>
