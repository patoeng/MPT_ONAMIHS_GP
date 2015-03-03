<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="ASMODAttribute.aspx.cs" Inherits="TPM.ASMODAttribute" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
        <div class="span12">
            <asp:Table ID="tblContainer" ClientIDMode="Static" runat="server" CssClass="table table-bordered"></asp:Table>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var bTable = $('#tblContainer').dataTable(
           {
               "bProcessing": true,
               "bLengthChange": false,
               "bFilter": true,
               "bSort": true,
               "bInfo": false,
               "bAutoWidth": true
           });
        function btables() {
            bTable.$('tr').hover(function () {
                $(this).addClass("hell");
            });
            bTable.$('tr').mouseout(function () {
                $(this).removeClass("hell");

            });
         
        }
        btables();
       
        });
    </script>
</asp:Content>
