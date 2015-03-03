<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="TPMOnlyChangePwd.aspx.cs" Inherits="TPM.TPMOnlyChangePwd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
    <div class="row-fluid" <%= loggedIn==true?"":"style='display:none'" %>>
        <div class="span12">
           <h3>Change Password!</h3>
                <h4>User Name : <%=session.EmployeeName+" ("+session.EmployeeNo+")" %></h4>
                <label  for="oldpassword" >Old Password</label>
                    <asp:TextBox ID="oldpassword" runat="server" TextMode="Password">
                    </asp:TextBox>
                
                <label  for="newpassword">New Password</label>
                    <asp:TextBox ID="newpassword"  runat="server" TextMode="Password">
                    </asp:TextBox><br />
                <asp:Button ID="Save" runat="server" Text="Save" OnClick="Save_Click" CssClass="btn btn-primary"/>
            </div>    
    </div>
    <div class="row-fluid">
        <div class="span12">
            <asp:Label ID="info" runat="server"></asp:Label>
         </div> 
    </div>
   </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
</asp:Content>
