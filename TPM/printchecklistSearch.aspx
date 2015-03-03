<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="printchecklistSearch.aspx.cs" Inherits="TPM.printchecklistSearch" %>
<%@ Import Namespace="TPM.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
     <form id="form1" runat="server">
        <div>
            <div class="row-fluid">
                <div class="span12">
                    <h2>Print Daily Machine Checklist</h2>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span6">
                    <label for="search">Enter Machine Serial Number</label>
                    <input name="search" type="text" id="txtSerialNumber" class="required number" style="display:inline"/>&nbsp;
                </div>
            </div>
        </div>
     </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
     <script type="text/javascript">
         $('#form1').validate({
             submitHandler: function () {
                 window.open("/<%= TPMHelper.WebDirectory %>/printchecklist.aspx?v=" + $('#txtSerialNumber').val(), "_blank");
            }
        });
    </script>
</asp:Content>
