<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YPMMaster2.aspx.cs" Inherits="TPM.YPMMaster2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         .newclass{ width: 200px!important;}        
        .tdf {font-size:6px}
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
         <div class="row-fluid">
         <div class="span8"></div>
            <div class="span2"></div>
            <div class="span2" style="text-align: right;display: none"><h5 id="buttonExport"class="btn btn-primary">Export To Excel</h5></div>

         </div>
    <div class="row-fluid">
            <div class="span4"><h4>PT SHIMANO BATAM</h4></div>
            <div class="span4"></div>
            <div class="span4" style="text-align: right;">FORM NO : PRO-QR-10/A </div>
    </div>
    <div class="row-fluid">
        <div class="span12 text-center">
            <h2>Yearly Maintenance Schedule of Year <%= year %></h2>
            <hr />
        </div>
    </div>
     <div class="row-fluid">
        <div class="span4 text-center">
            Department&nbsp;<asp:DropDownList ID="ddlDept" ClientIDMode="Static" runat="server" CssClass="ddl"></asp:DropDownList>
        </div>
         <div class="span4 text-center">
            Year&nbsp;<asp:DropDownList ID="ddlYear" ClientIDMode="Static" runat="server" CssClass="ddl"></asp:DropDownList>
        </div>
         <div class="span4 text-center">
        </div>
    </div>
    
       
            <hr />                      
                <asp:Table runat="server" ID="tblSchedule" ClientIDMode="Static" CssClass="table table-bordered table-condensed table-striped"></asp:Table>
   
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">

        $(document).ready(function() {

            $('.ddl').change(function() {
                window.location = "<%= Request.Url.AbsolutePath %>?d=" + $('#ddlDept').val() + "&y=" + $('#ddlYear').val();
            });
            
            $('#tblSchedule').dataTable({
                "bPaginate": false,
                "aoColumnDefs": [
                    { 'bSortable': false, 'aTargets': [2,3,4,5,6,7,8,9,10,11,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49] }
                ],
                "bAutoWidth": false,
                "aoColumns": [
                      { "sWidth": "100px"},
                      { "sWidth": "200px" }
                    
                ],
                "aaSorting": [[1, "desc"]]

            });
            $('#tblSchedule').each(function() {
                var i = 0;
                var $this = $(this);
                $this.find("tr").each(function() {
                    i = 0;
                    $(this).find("td").each(function () {
                        i++;
                        if (i == 1) { jQuery(this).addClass('newclass'); }
                        if (i == 2) { jQuery(this).addClass('newclass'); }
                    });
                });

            });
        });
    </script>
</asp:Content>
