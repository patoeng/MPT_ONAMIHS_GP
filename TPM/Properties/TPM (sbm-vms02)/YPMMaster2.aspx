<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YPMMaster2.aspx.cs" Inherits="TPM.YPMMaster2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         body
         {
            background-image: url('/TPM/Images/UnderConstruction.jpg');
            background-repeat:no-repeat;
            background-attachment:fixed;
            background-position:center;
         }
        .newclass{width:200px!important}
        td
        {
            overflow:hidden;
        }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
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
    <div class="row-fluid">
        <div class="span12 text-center">
            <hr />
            <asp:Table runat="server" ID="tblSchedule" ClientIDMode="Static" CssClass="table table-fixed table-bordered table-condensed table-striped"></asp:Table>
        </div>
    </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $('.ddl').change(function () {
            window.location = "<%= Request.Url.AbsolutePath %>?d=" + $('#ddlDept').val()+"&y="+$('#ddlYear').val();
        });
    </script>
</asp:Content>
