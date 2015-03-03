<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="PMScheduleUpload.aspx.cs" Inherits="TPM.PMScheduleUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
     <div class="row-fluid">
       <div class="span3">
         <asp:FileUpload ID="fileUpload" runat="server"/>
        </div>
       <div class="span3">
         <asp:Button ID="btnUpload" OnClick="btnUploadClick" runat="server" Text="Import Monthly Schedule From Excel" ClientIDMode="Static" CssClass="btn btn-primary"/>
       </div>
      </div>
   </form>
    <div class="row-fluid">
        <div class="span12">
            <asp:Table runat="server" ID="tblErrors" ClientIDMode="Static"></asp:Table>
        </div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $('#btnUpload').click(function() {
            
        });
    </script>
</asp:Content>
