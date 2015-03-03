<%@ Page Title="PM Master Schedule" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YPMMaster.aspx.cs" Inherits="TPM.YPMMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
             <div class="span12 text-center" ><h3>Master Plan of Machine <%= checktype==2? "Monthly" : "Daily" %> Preventive Maintenance</h3></div>
        </div>
         <div class="row-fluid">
             <div class="span12 text-center"><h3>For the Month of <asp:Label ID="lblMonth" ClientIDMode="Static" runat="server"> <%= saiki.ToString("MMMM yyyy") %></asp:Label></h3></div>
        </div>
         <form id="form1" runat="server" class="form-inline">
         <div class="row-fluid">
             <div class="span12">
                <fieldset>
                     <label>Department</label>
                     <asp:DropDownList runat="server" ID="ddlDepartment" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                     <label>Month</label>
                     <asp:DropDownList runat="server" ID="ddlMonth" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                     <label>Year</label>
                     <asp:DropDownList runat="server" ID="ddlYear" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
               </fieldset>
            </div>
        </div>
       </form>
         <div class="row-fluid">
             <div class="span12 text-center"><asp:Table ID="tblSchedule" ClientIDMode="Static" runat="server" CssClass="table table-striped table-condensed table-bordered"></asp:Table></div>
        </div>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script>
        $(".ddl").change(function () {
            window.open("<%= Request.Url.AbsolutePath %>?dept=" + $("#ddlDepartment").val() + "&mon=" + $("#ddlMonth").val() + "&year=" + $("#ddlYear").val() + "&ty=<%=checktype.ToString() %>", "_self");
       });
   </script>
</asp:Content>
