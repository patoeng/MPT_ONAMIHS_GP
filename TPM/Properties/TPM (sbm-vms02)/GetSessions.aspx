<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetSessions.aspx.cs" Inherits="TPM.GetSessions" %>
<%
    Response.Write(Session["EmployeeNo"] != null ? Session["EmployeeNo"].ToString() : "");
    Response.AddHeader("Connection", "close");
%>