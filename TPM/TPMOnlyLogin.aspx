<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="TPMOnlyLogin.aspx.cs" Inherits="TPM.TPMOnlyLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
        <div class="row-fluid" <%= loggedIn==false ? "":"style='display:none;'"%>>
            <div class="span12">
                <h3>Please Log In!</h3>
                <label  for="EmployeeNo">Employee Number</label>
                    <asp:TextBox ID="EmployeeNo" runat="server">
                    </asp:TextBox>
                
                <label  for="password">Password</label>
                    <asp:TextBox ID="password"  runat="server" TextMode="Password">
                    </asp:TextBox><br />
                <asp:Button ID="Login" runat="server" Text="Log In" OnClick="Login_Click" CssClass="btn btn-primary"/>
            </div>
        </div>
        <div class="row-fluid" <%= loggedIn==true ? "":"style='display:none;'"%>>
            <div class="span12">
                <h4>Logged User : <%=session.EmployeeName+" ("+session.EmployeeNo+")" %></h4>
                <br />
                <a href="TPMOnlyLogin.aspx?v=out" class="btn btn-primary">Log Out</a>
                <br />
                <br />
                <a href="TPMOnlyChangePwd.aspx" class="btn btn-primary">Change Password</a>
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
