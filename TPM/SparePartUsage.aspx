<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="SparePartUsage.aspx.cs" Inherits="TPM.SparePartUsage" %>
<%@ Import Namespace="TPM.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
        <form runat="server" id="form1" name="form1">
      <div class="row-fluid">
         <div class="span3">
            <label for="serialcode">Machine Serial Code</label>
                <input runat="server" ClientIDMode="Static" type="text" id="serialcode" class="input2"/>         
        </div> 
         <div class="span3">
            <label for="sparepartcode">Spare Part Code</label>
                <input runat="server" ClientIDMode="Static" type="text" id="sparepartcode" class="input2"/>         
        </div>  
         <div class="span3">
            <label>Department  </label>
                <asp:DropDownList runat="server"  ID="Department" ClientIDMode="Static" CssClass="ddl">
                </asp:DropDownList>
        </div> 
        <div class="span3">
             <label>Usage Type  </label>
                <asp:DropDownList ID="usageType" ClientIDMode="Static" runat="server" CssClass="ddl">                 
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
                     
        </div>  
          <div class="span3">
            <label for="id2">PMID / WO Number</label>
                <input runat="server" ClientIDMode="Static" type="text" id="id2" class="input2"/>         
        </div>  
       
     </div>  
     </form>
    <div class="row-fluid">
        <div class="span12" style="text-align:center">
             <h3>Spare Part Usage</h3>
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
            btnActivate();
        }
        $('#startdate').Zebra_DatePicker({
            format: 'd-M-Y',
            direction: false,
            inside: true,
            start_date: '<%= tanggal%>',
               pair: $('#enddate'),
               onSelect: function (date) {
                   $('#form1').trigger('submit');
               }
           });
           $('#enddate').Zebra_DatePicker({
               format: 'd-M-Y',
               direction: 0,
               inside: true,
               start_date: '<%= tanggal%>',
               onSelect: function (date) {
                   $('#form1').trigger('submit');
               }
           });
           var keypress = $('.input2').keypress(function (e) {
               var code = (e.keyCode ? e.keyCode : e.which);
               if (code == 13) {
                   $('#form1').trigger('submit');
                   e.preventDefault();
               }
           });
           $('.ddl').change(function () {
               $('#form1').trigger('submit');
           });
           function btnActivate() {
               $('.btn').click(function() {
                   $.ajax({
                       url: './Methodes/bom.asmx/UsageDownload2',
                       cache: false,
                       type: 'POST',
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       data: JSON.stringify({
                           sn: $('#serialcode').val(),
                           spc: $('#sparepartcode').val(),
                           depid: $('#Department option:selected').val(),
                           utype: $('#usageType option:selected').val(),
                           fdate: $('#startdate').val(),
                           tdate: $('#enddate').val(),
                           id: $('#id2').val()
                       }, null, 2),
                       success: function (json) {
                           try {

                               var obj = json.d;
                               isdownload = 0;
                               window.open("../<%=TPMHelper.WebDirectory%>/StreamDownload.aspx?u=http://<%= Request.Url.Authority +Request.ApplicationPath  %>/" + obj, "_self");

                       }
                       catch (ex) {
                           alert(ex);
                       }

                   },
                         error: function (xhr, ajaxOptions, thrownError) {
                             var obj = $.parseJSON(xhr.responseText);
                             alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                                   "Stack Trace : \r\n" + obj.StackTrace
                                   );
                         }
                     });

               });
             
           }
           $('#form1').validate({
               submitHandler: function () {
                   $.ajax({
                       url: './Methodes/bom.asmx/Usage2',
                       cache: false,
                       type: 'POST',
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       data: JSON.stringify({
                           sn: $('#serialcode').val(),
                           spc: $('#sparepartcode').val(),
                           depid: $('#Department option:selected').val(),
                           utype: $('#usageType option:selected').val(),
                           fdate: $('#startdate').val(),
                           tdate: $('#enddate').val(),
                           id : $('#id2').val()
                       }, null, 2),
                       success: function (json) {
                           try {
                            
                                   //var obj = $.parseJSON(json);
                                   var obj = json.d;
                                   $('#tableContainer').html("");
                                   $('#tableContainer').append(obj);

                                   $('#jsonTable').dataTable({
                                       "sPaginationType": "full_numbers"
                                   });
                               btnActivate();
                                  
                              
                           }
                           catch (ex) {
                               alert(ex);
                           }

                       },
                       error: function (xhr, ajaxOptions, thrownError) {
                           var obj = $.parseJSON(xhr.responseText);
                           $('#tableContainer').html("");
                           $('#tableContainer').append("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                               "Stack Trace : \r\n" + obj.StackTrace + "\r\n<h3>Trying to use page reload instead... Please wait...</h3>"
                               );
                           window.open("SparePartUsage.aspx?m=1"
                               + "&sn=" + $('#serialcode').val()
                               + "&sp="+ $('#sparepartcode').val()
                               +"&dp="+ $('#Department option:selected').val()
                               +"&ut="+ $('#usageType option:selected').val()
                               +"&sd="+ $('#startdate').val()
                               +"&ed="+ $('#enddate').val()
                               +"&id="+ $('#id2').val()
                               , "_self");
                       }
                   });

               }
           });
    </script>
</asp:Content>
