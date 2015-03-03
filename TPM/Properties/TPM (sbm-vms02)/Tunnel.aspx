<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tunnel.aspx.cs" Inherits="TPM.Tunnel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <% Response.Write(Session["tpm_employeeno"] != null ? Session["tpm_employeeno"].ToString() : "koso"); %>
    <script type="text/javascript">
        $(document).ready(function () {
            var d = "<%= d %>";
            if (d !="") {
                window.location = "../TPM/<%= url %>";
            } else {
            $.ajax({
                url: '../TTPM/getsessions.aspx',
                cache: false,
                type: 'GET',
                contentType: "application/text; charset=utf-8",
                dataType: "text",
                data: "",
                success: function (json) {
                    alert(json);
                    window.location = "../TPM/Tunnel.aspx?d=x&s=" + json + "&u=<%= url %>";
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj =xhr.responseText;
                    alert("Problem occured while accessing parent's sessions");
                }
            });
        }
        });
        

    </script>
</body>
</html>
