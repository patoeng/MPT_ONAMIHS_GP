<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YBDSummary.aspx.cs" Inherits="TPM.YSummary" %>
<%@ Import Namespace="TPM.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
      <form runat="server" id="form1" name="form1">
      <div class="row-fluid">
         <div class="span3">
            <label for="serialcode">Serial Code </label>
                <input runat="server" ClientIDMode="Static" type="text" id="serialcode"/>        
        </div>   
         <div class="span3">
            <label for="Department">Department  </label>
                <asp:DropDownList runat="server"  ID="Department" ClientIDMode="Static" CssClass="ddl">
                </asp:DropDownList>
        </div> 
        <div class="span3">
             <label for="status_id">Status  </label>
                <asp:DropDownList ID="status_id" ClientIDMode="Static" runat="server" CssClass="ddl">                 
                </asp:DropDownList>
        </div>     
    </div>
     <div class="row-fluid">
        <div class="span3">
             <label for="startdate">From Date </label>
            <input runat="server" ClientIDMode="Static" type="text" id="startdate" class="datepicker required"/>
           
        </div>
        <div class="span3">
            <label for="enddate">To Date </label>
                <input runat="server" ClientIDMode="Static" type="text" id="enddate" class="datepicker required"/>
          
        </div>
       <div class="span3">
             <label for="adhoc">WO Type </label>
                <asp:DropDownList ID="adhoc" ClientIDMode="Static" runat="server" CssClass="ddl">
                                     
                </asp:DropDownList>
          
        </div>
        <div class="span3">
             <label for="safety">Category</label>
                <asp:DropDownList ID="safety" ClientIDMode="Static" runat="server" CssClass="ddl">
                                     
                </asp:DropDownList>
          
        </div>
     </div>
     
       
     </form>
    <div class="row-fluid">
        <div class="span12" style="text-align:center">

             <h3>Breakdown Maintenance and Preventive Requisition Reports (PRO-QR-07A)</h3>

        </div>
    </div>
 
    <div class="row-fluid">
        <div class="span12">
           
             <div runat="server" ClientIDMode="Static" id="tableContainer"></div>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">

    <script type="text/javascript">
        var isdownload = 0;
        var m = '<%= m%>';
        if (m != '') {
            $('#jsonTable').dataTable({
                "sPaginationType": "full_numbers"
            });
            $('.btn').click(function () {
                isdownload = 1;
                $('#form1').trigger('submit');
            });
        }
        $('#startdate').Zebra_DatePicker({
            format: 'd-M-Y',
            direction: false,
            inside: true,
            start_date : '<%= tanggal%>',
            pair: $('#enddate'),
            onSelect: function (date) {
                $('#form1').trigger('submit');
            }
        });
        $('#enddate').Zebra_DatePicker({
            format: 'd-M-Y',
            start_date: '<%= tanggal%>',
            direction: 0,
            inside: true,
            onSelect: function(date) {
                $('#form1').trigger('submit');
            }
        });
        $('#serialcode').keypress(function(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                $('#form1').trigger('submit');
                e.preventDefault();
            }
        });
        $('.ddl').change(function(){
                $('#form1').trigger('submit');
        });
       
        $('#form1').validate({
            submitHandler: function () {
                $.ajax({
                    url: './Methodes/downtime.asmx/summary',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ serialcode: $('#serialcode').val(), isdownload: isdownload, department: $('#Department option:selected').val(), status_id: $('#status_id option:selected').val(), startdate: $('#startdate').val(), enddate: $('#enddate').val(),safety: $('#safety option:selected').val(), adhoc: $('#adhoc option:selected').val() }, null, 2),
                    success: function (json) {
                        try {
                            var obj = null;
                            if (isdownload == 0) {
                                obj = json.d;
                                $('#tableContainer').html("");
                                $('#tableContainer').append(obj);

                                $('#jsonTable').dataTable({
                                    "sPaginationType": "full_numbers"
                                });
                                $('.btn').click(function () {
                                    isdownload = 1;
                                    $('#form1').trigger('submit');
                                });

                            } else {
                                obj = json.d;
                                isdownload = 0;
                                window.open("../<%=TPMHelper.WebDirectory%>/StreamDownload.aspx?u=http://<%= Request.Url.Authority +Request.ApplicationPath  %>/" + obj, "_self");
                            }
                        }
                        catch (ex)
                        {
                            isdownload = 0;
                        }

                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        var obj = $.parseJSON(xhr.responseText);
                        $('#tableContainer').html("");
                        $('#tableContainer').append("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                            "Stack Trace : \r\n" + obj.StackTrace + "\r\n<h3>Trying to use page reload instead... Please wait...</h3>"
                            );
                        window.open("YBDSummary.aspx?m=1&sc=" + $('#serialcode').val() + "&dp=" + $('#Department option:selected').val() + "&st=" + $('#status_id option:selected').val() + "&sd=" + $('#startdate').val() + "&ed=" + $('#enddate').val() + "&wo=" + $('#adhoc option:selected').val(), "_self");
                    }
                });
                
            }
        });
    </script>
</asp:Content>
