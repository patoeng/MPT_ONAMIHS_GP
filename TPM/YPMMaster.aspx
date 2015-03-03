<%@ Page Title="PM Master Schedule" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YPMMaster.aspx.cs" Inherits="TPM.YPMMaster" %>
<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
            <div class="span4"><h4>PT SHIMANO BATAM</h4></div>
            <div class="span4"></div>
            <div class="span4" style="text-align: right;"> <h5><%= checktype==2? "FORM NO : PRO - QR - 11/A":""%> </h5></div>
    </div>
   <div class="row-fluid">
    <div class="span10">
        <div class="row-fluid">
        <div class="span3 text-center" ></div>
        <div class="span9 text-center" ><h2><%= checktype==2? "Master Plan of Machine Preventive Maintenance" : "Daily Machine Checklist Status" %></h2></div>
       
        </div>
         <div class="row-fluid">
              <div class="span3 text-center" ></div>
             <div class="span9 text-center"><h3>For the Month of <asp:Label ID="lblMonth" ClientIDMode="Static" runat="server"> <%= saiki.ToString("MMMM yyyy") %></asp:Label></h3></div>
        </div>
         <form id="form1" runat="server" class="form-inline">
         <div class="row-fluid">
             
             <div class="span12">
             
                    <div class="container-fluid">
                     <div class="row-fluid">
                         <div class="span6">
                     <label>Serial Number</label>
                     <asp:TextBox  ID="serialcode"  ClientIDMode="Static" runat="server"/></div>
                              <div class="span6">
                     <label>Department</label>
                     <asp:DropDownList runat="server" ID="ddlDepartment" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                       </div>

                     </div>
                    <div class="row-fluid">
                           <div class="span12">
                     <label>Month</label>
                     <asp:DropDownList runat="server" ID="ddlMonth" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                     <label>Year</label>
                     <asp:DropDownList runat="server" ID="ddlYear" ClientIDMode="Static" cssClass="ddl "></asp:DropDownList>
                     </div>

                    </div>
              </div>
            </div>
          </div>
       </form>
        </div>
       <div class="span2">
           Legend:
           <asp:PlaceHolder runat="server" ID="legend">
          
           </asp:PlaceHolder>
       </div>
       </div>
    
         <div class="row-fluid">
             <div class="span12 text-center"><asp:Table ID="tblSchedule" ClientIDMode="Static" runat="server" CssClass="table table-striped table-condensed table-bordered"></asp:Table></div>
        </div>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script>
        $('#serialcode').keypress(function(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                //$('#form1').trigger('submit');
                e.preventDefault();
                window.open("<%= Request.Url.AbsolutePath %>?dept=" + $("#ddlDepartment").val() + "&mon=" + $("#ddlMonth").val() + "&year=" + $("#ddlYear").val() + "&ty=<%=checktype.ToString(CultureInfo.InvariantCulture) %>&sn="+$('#serialcode').val(), "_self");
            }
        });
        $(".ddl").change(function () {
            window.open("<%= Request.Url.AbsolutePath %>?dept=" + $("#ddlDepartment").val() + "&mon=" + $("#ddlMonth").val() + "&year=" + $("#ddlYear").val() + "&ty=<%=checktype.ToString(CultureInfo.InvariantCulture) %>&sn=" + $('#serialcode').val(), "_self");
        });
    </script>
</asp:Content>
