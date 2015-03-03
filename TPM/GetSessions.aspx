<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetSessions.aspx.cs" Inherits="TPM.GetSessions" %>
<%
    Response.Write(Session["EmployeeNo"] != null ? Session["EmployeeNo"].ToString() : "");
   
    foreach (var crntSession in Session)
    {
        Response.Write(string.Concat(crntSession, "=", Session[crntSession.ToString()]) + "<br />");
    }
    Response.AddHeader("Connection", "close");
%>