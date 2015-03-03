<%@ Page Language="C#"  MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="EmailNew.aspx.cs" Inherits="TPM.EmailNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
     <form id="form1" runat="server">
         <div class="row-fluid">
            <div class="span12">
                <h2>TPM Auto Email Lists</h2>  
            </div>
         </div>
         <div class="row-fluid">
            <div class="span11">
                <asp:Table runat="server" ID="tblList" ClientIDMode="Static" CssClass="table table-bordered"></asp:Table>
            </div>
         </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
<script type="text/javascript">
    $(document).ready(function() {

        $('.editableYesNo').editable('./Methodes/Emailing.asmx/UpdateEmail', {
            indicator: "<img src='./Images/indicator.gif'>",
            onblur: 'submit',
            // submit: "OK",
            //cancel: "Cancel",
            tooltip: "Click to edit...",
            style: "inherit",
            type: "select",
            data: "{'0' : 'No','1' :'Yes'}",
            submitdata: function(value, settings) {
                return {
                    "origValue": this.revert,
                    "EmailId": $(this).attr('id').split('_')[1],
                    "id": $(this).attr('id').split('_')[0]
                };
            },
            ajaxoptions: { contentType: "application/json; charset=utf-8" },
            intercept: function(jsondata) {
                try {
                    obj = jQuery.parseJSON(jsondata);
                    return (obj.d);
                } catch(err) {
                    alert(err.message);
                }
                return this.revert;

            }
        });
    });
</script>
</asp:Content>