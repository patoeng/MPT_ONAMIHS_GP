<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YAccAttachment.aspx.cs" Inherits="TPM.YAccAttachment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
      <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
      <div runat="server" ID="containerku" class="container-fluid">
       <div class="row-fluid">
        <div class="span2">
           <asp:Label ID="Label1" runat="server" Text="Remarks"></asp:Label>
         </div> 
        <div class="span8">
            <asp:TextBox ID="txtRemarks" ClientIDMode="Static" runat="server" Width="493px" ></asp:TextBox>
        </div> 
       </div>
         <div class="row-fluid">
           <div class="span2">
            <asp:Label ID="ddlLable" runat="server" Text="Select A Checklist" ></asp:Label>
            
            <asp:DropDownList ID="ddlChecklist" runat="server" ClientIDMode="Static" style="display: none"></asp:DropDownList>
         </div> 
        <div class="span8">
             <asp:FileUpload ID="File1"  runat="server" ClientIDMode="Static" Width="497px"></asp:FileUpload>
       
            <asp:HiddenField ID="ddlvalues" runat="server" />
        </div>
       </div>
         <div class="row-fluid">
              <div class="span2">
           
          </div>
           <div class="span8">
            <asp:Button ID="Submit1" value="Upload" runat="server" Text="Upload" OnClick="Submit1_ServerClick" CssClass="btn btn-primary"></asp:Button>
          </div>
          

         </div> 

   </div>
          <div class="row-fluid">
            <div class="span12">
                <hr/>
                <h3 runat="server" id="lblTittle"></h3>
            </div>
        </div>
          <div class="row-fluid">
            <div class="span6">
                <asp:Button ID="Button1" value="Improvement Work Order Status" runat="server" Text="Go To Accident Report Details" OnClick="GotoClick" CssClass="btn btn-primary"></asp:Button>
            </div>
        </div>
          <div class="row-fluid">
            <div class="span6">
               &nbsp;
            </div>
        </div>
        <div class="row-fluid">
            <div class="span10">
                <asp:Table ID="TblImage" runat="server" ClientIDMode="Static" CssClass="table table-bordered">
                </asp:Table>
            </div>
        </div>
</form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $('.delete').click(function () {
            var id = $(this).attr('id');
           
            $.ajax({
                url: './Methodes/accident.asmx/delete',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: id }, null, 2),
                success: function (json) {
                    $('#'+json.d).hide();
                },
                error: function (xhr) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });
        });
    </script>
</asp:Content>
