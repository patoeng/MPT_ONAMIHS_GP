<%@ Page Title="Check List Images" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YCheckListImg.aspx.cs" Inherits="TPM.YCheckListImg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
        <div class="row-fluid">
        <div class="span12">
            <asp:Label ID="ddlLable" runat="server" Text="Select A Checklist"></asp:Label>
            
        <asp:DropDownList ID="ddlChecklist" runat="server" ClientIDMode="Static" style="display: none"></asp:DropDownList>
        <br />
        <asp:FileUpload ID="File1"  runat="server" ClientIDMode="Static"></asp:FileUpload>
        <br />
            <asp:HiddenField ID="ddlvalues" runat="server" />
        <asp:Button ID="Submit1" value="Upload" runat="server" Text="Upload" OnClick="Submit1_ServerClick" CssClass="btn btn-primary"></asp:Button>
           </div> </div>
         <div class="row-fluid">
        <div class="span6">
        <asp:Table ID="TblImage" runat="server" ClientIDMode="Static" CssClass="table table-bordered"></asp:Table></div></div>
</form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $('#TblImage').ready(function () {
            var oTable = $('#TblImage').dataTable({

            });
            $('#ddlChecklist').change(function () {

                window.open("<%= Request.Url.AbsolutePath %>?i="+ $("#ddlChecklist option:selected").val(), "_self");
            });
        });

    </script>
</asp:Content>
