<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testSession.aspx.cs" Inherits="TPM.testSession" %>
<%@ Import Namespace="TPM.Classes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <%= new MySessions().EmployeeNo %>
    </div>
    </form>
</body>
</html>
