<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="HT_Summary.aspx.cs" Inherits="TPM.WebForm1" %>
<%@ Import Namespace="TPM.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
     <form runat="server" id="form1" name="form1">
      <div class="row-fluid">
         <div class="span3">
            <label for="order_number">Order Number</label>
               <input runat="server" ClientIDMode="Static" type="text" id="order_number"/>      
        </div>   
         <div class="span3">
            <label for="bm1code">SAP CODE  </label>
                <input runat="server" ClientIDMode="Static" type="text" id="bm1code"/>     
        </div> 
        <div class="span3">
             <label for="OvenID">Heater  </label>
                <asp:DropDownList ID="OvenID" ClientIDMode="Static" runat="server" CssClass="ddl" />
        </div>  
        <div class="span3">
             <label for="batchid">Batch ID</label>
                <input runat="server" ClientIDMode="Static" type="text" id="batchid"/>
        </div>   
    </div>
     <div class="row-fluid">
        <div class="span3">
             <label for="startdate">From Date </label>
            <input runat="server" ClientIDMode="Static"  type="text" id="startdate" class="datepicker"/>
           
        </div>
        <div class="span3">
            <label for="enddate">To Date </label>
                <input runat="server" ClientIDMode="Static" type="text" id="enddate" class="datepicker"/>
          
        </div>
       <div class="span3">
             <label for="stage">Proccess Stage </label>
                <asp:DropDownList ID="stage" ClientIDMode="Static" runat="server" CssClass="ddl"></asp:DropDownList>
                                     
          
       </div>
     </div>
     
       
     </form>
    <div class="row-fluid">
        <div class="span12" style="text-align:center">

             <h3>BM-Heatreatment Summary</h3>

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
            start_date: '<%= Tanggal%>',
            pair: $('#enddate'),
            onSelect: function (date) {
                $('#form1').trigger('submit');
            }
        });
        $('#enddate').Zebra_DatePicker({
            format: 'd-M-Y',
            start_date: '<%= Tanggal%>',
            direction: 0,
            inside: true,
            onSelect: function (date) {
                $('#form1').trigger('submit');
            }
        });
        $('#order_number').keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                $('#form1').trigger('submit');
                e.preventDefault();
            }
        });
        $('#bm1code').keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                $('#form1').trigger('submit');
                e.preventDefault();
            }
        });
        $('#batchid').keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                $('#form1').trigger('submit');
                e.preventDefault();
            }
        });
        $('.ddl').change(function () {
            $('#form1').trigger('submit');
        });

        $('#form1').validate({
            submitHandler: function () {
                $.ajax({
                    url: './Methodes/bmht.asmx/summary',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //summary(string batchid,string bm1code, string order_number, int isdownload, string ovenid, string stage, string startdate, string enddate = null)
                    data: JSON.stringify({ batchid: $('#batchid').val(), bm1code: $('#bm1code').val(), order_number: $('#order_number').val(), isdownload: isdownload, ovenid: $('#OvenID option:selected').val(), stage: $('#stage option:selected').val(), startdate: $('#startdate').val(), enddate: $('#enddate').val() }, null, 2),
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
                        catch (ex) {
                            isdownload = 0;
                        }

                    },
             error: function (xhr, ajaxOptions, thrownError) {
                 var obj = $.parseJSON(xhr.responseText);
                 $('#tableContainer').html("");
                 $('#tableContainer').append("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                     "Stack Trace : \r\n" + obj.StackTrace + "\r\n<h3>Trying to use page reload... Please wait...</h3>"
                     );
                 window.open("HT_Summary.aspx?m=1"
                     + "&bi=" + $('#batchid').val()
                     + "&bc=" + $('#bm1code').val()
                     + "&on=" + $('#order_number').val()
                     + "&od=" + $('#OvenID option:selected').val()
                     + "&sg=" + $('#stage option:selected').val()
                     + "&sd=" + $('#startdate').val()
                     + "&ed=" + $('#enddate').val()
                     ,"_self");
             }
         });

            }
        });
    </script>
</asp:Content>
