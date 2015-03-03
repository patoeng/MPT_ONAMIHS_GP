<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="FIWOrkOrder.aspx.cs" Inherits="TPM.FIWOrkOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
     <div class="row-fluid">
        <div class="span6">
            <h3>New Improvement Work Order Request</h3>
            <form runat="server" id="form1">
                <asp:Table ID="tblForm" runat="server" ClientIDMode="Static" CssClass="table table-condensed"></asp:Table>
            </form>
        </div><div class="span6 text-right">Form No : RE-OR-23</div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ddlDepartment').click(function (event) {
                var ddl = $(this);

                if (document.getElementById("ddlDepartment").length == 1) {
                    $.ajax({
                        url: './Methodes/workorder.asmx/getddl',
                        cache: false,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ id: "" }, null, 2),
                        success: function (json) {
                            try {
                                var obj = json.d;
                                var data = $.parseJSON(obj);
                                ddl.empty();
                                $.each(data, function (val, text) {
                                    ddl.append("<option value='" + val + "'>" + text + "</option>");
                                });
                                ddl.append("<option value='' selected>Please Select...</option>");
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
                }
            });
          
            $('.btn').click(function () {
                $('#form1').trigger('submit');
            });
            $('#form1').validate({
                submitHandler: function () {
                    $.ajax({
                        url: './Methodes/iwo.asmx/NewIwo',
                        cache: false,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            deptid: $('#ddlDepartment option:selected').val(),
                            reqid: $('#ddlRequestType option:selected').val(),
                            req: $('#txtRequest').val(),
                            cause: $('#txtCause').val()
                        }, null, 2),
                        success: function(json) {

                            try {
                                var obj = json.d;

                                // window.location = "./YIWOrkOrders.aspx?v=" + obj;
                                window.location = "./YIWOAttachments.aspx?v=" + obj;


                            } catch(error) {
                                alert("error " + error.message);
                            }

                        },
                        error: function(xhr, ajaxOptions, thrownError) {
                            //alert(xhr.status);
                            //alert(thrownError);
                            //alert(xhr.responseText);

                            var obj = $.parseJSON(xhr.responseText);
                            alert(obj.Message);


                        }
                    });
                }
            });
        });
    </script>
</asp:Content>