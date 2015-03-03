<%@ Page Title="PM Status" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YPMStatus.aspx.cs" Inherits="TPM.YPMStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
     <div class="row-fluid">
         <div class="span12">
            <h2>PM Status</h2>
           </div>
        </div>
    <div class="row-fluid">
        <div class="span6">
            <label for="search">Enter PM ID</label>
            <input name="search" type="text" id="txtMPMID" class="required number" style="display:inline"/>&nbsp;
        </div>
    </div>
  <div style="<%=status_id<=0 ? "display:none;": "" %>">
    <div class="row-fluid">
            <div class="span12"><asp:Table ID="tblContainer" ClientIDMode="Static" runat="server" CssClass="table table-bordered"></asp:Table></div>
        </div>
     
     <div class="row-fluid">
         <div class="container-fluid">
                    <div class="span5"><h4>Latest Status</h4><hr />
                        <asp:Table ID="tblLayOut" ClientIDMode="Static" runat="server" CssClass="table table-bordered"></asp:Table></div>
                   <div class="span3"><h4>Remarks</h4><hr />
                       <asp:TextBox ID="txtRemarks" ClientIDMode="Static" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                       <br /><h3 class="btn btn-primary tombol" id="btnRemarks"  style="display:<%= session.IsManagement||session.IsEngineering? "block":"none" %>">SAVE</h3>
                   </div>
             <div class="span4">
                 <div class="row-fluid" style="display:<%= status_id < 4 ? "none":"block"%>">
                     <div class="span12" ><hr /><a href="/TPM/YChecklist.aspx?id=<%= lid2.ToString() %>" class="btn btn-primary link"><h5 >Checklist</h5></a></div>
                     
                 </div>
                 <div class="row-fluid" style="display:<%= status_id < 4 ? "none":"block"%>"> 
                     <div class="span12"><hr /><a href="/TPM/PMBoms.aspx?i=<%= schedule_id.ToString() %>" class="btn btn-primary link"><h5 >Part Replacement</h5></a></div>
                 </div>
                 
             </div>
               
         </div>
     </div>
       
    <div class="row-fluid">      
     
                    <div class="span12">
                        <h4>Summary</h4><hr /> 
                        <asp:Table ID="tblHistory" ClientIDMode="Static" runat="server" CssClass="table  table-bordered table-striped"></asp:Table>

                    </div>
     </div>
    </div>
          </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $('#form1').validate({
            submitHandler: function () {
                window.open("<%= Request.Url.AbsolutePath %>?pid=" + $('#txtMPMID').val(), "_self");
            }
        });
            $('#txtMPMID').keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    $('#form1').trigger('submit');

                }
                // $.print(event,"html");
                // $.print(event);
            });
        var pid = "<%= schedule_id.ToString() %>";
        var lid = "<%= lid.ToString() %>";
        var date2 ="<%= sched %>";
        function ajaxAction(ids,action, done_by, value, onSuccess) {
            $.ajax({
                url: './Methodes/pm.asmx/actions',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({id:ids,act:action,dby:done_by,val:value}, null, 2),
                success: function (json) {
                    try {
                        var obj = json.d;
                        //var data = $.parseJSON(obj);
                        onSuccess(obj);
                    } catch (error) {
                        alert("error " + error.message);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });
        };

        $('.datepicker').Zebra_DatePicker({
            format: 'd-M-Y',
            direction: 1,
            inside: false,
            onSelect: function (date) {
                ajaxAction(pid, "schedule", "test", date, function (objs) {
                   
                    window.location.reload();
                });
            }
        });
        $('.tombol').click(function () {
            var action = $(this).attr('id');
            if (action == "btnRemarks") {
                ajaxAction(lid, action, "test", $("#txtRemarks").val(), function (objs){
                    
                    window.location.reload();
                });
            }
            else {
                ajaxAction(pid, action, "test", date2, function (objs) {

                   
                    window.location.reload();
                });
            }
        });
      
    </script>
</asp:Content>
