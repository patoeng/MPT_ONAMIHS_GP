<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="PrintChecklist.aspx.cs" Inherits="TPM.PrintChecklist" %>
<!DOCTYPE html>
<meta http-equiv="X-UA-Compatible" content="IE=8" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
        <meta http-equiv="PRAGMA" CONTENT="NO-CACHE">
        <meta http-equiv="expires" CONTENT="-1">
    <title></title>
    <link href='./Styles/print.css' media="print" rel='stylesheet' type='text/css' />
<style>
    body {
        font-family: arial;
    }
      table.items td{
      border: 1px solid black;
      font-size: 14px;
      padding: 5px;
      vertical-align: middle;
      padding: 3px;
      height: 40px;
      width : 8%
     
  }
  td.first {
      width: 30px;
  }
  .first {
      width: 30px;
  }
  table.header td {
      font-size: 14px;
      vertical-align: middle;
      padding: 6px;  
  }
  table.containerku td {
      width: 120px;
  }
  table.containerku{
       table-layout: fixed;
   }
  table.items{
          table-layout: fixed;
         width: 95%;
     }
  table.items th
  {
      border: 2px solid black;
      font-size: 16px;
      height: 30px;
      vertical-align: middle;
      font-weight: bold;
      padding: 10px; 
  }
  h3 {
      font-size: 30px;
      font-weight: bold;
  }
</style>    
</head>
<body>
    <form id="form1" runat="server">
        <div <%= SerialNumber==null ? "" : "style=\"display:none;\"" %>>
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
    
        <table style="width: 1440px; border-collapse: collapse;<%= SerialNumber==null ? "display:none;" :"" %>" class="containerku" >
            <tr>
                <td colspan="3"><h4>PT. SHIMANO BATAM</h4></td>
                <td colspan="6"></td>
                <td colspan="3" style="text-align: right;">FORM NO : <h5 style="display:inline"><asp:Label ID="LblForm" ClientIDMode="Static" runat="server"></asp:Label></h5></td>
            </tr>
            <tr><td colspan="3"><asp:PlaceHolder runat="server" ID="barcodeHolder"/></td>
                <td colspan="6" style="text-align: center"><h3 style="display:inline;text-align: center"><asp:Label ID="LblTitle" ClientIDMode="Static" runat="server"></asp:Label> Machine Checklist</h3></td>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td colspan="3"><asp:Table ID="TblAtasKiri" ClientIDMode="Static" cssClass="header" HorizontalAlign="Center" Width="100%"  runat="server"></asp:Table></td>
                <td colspan="6"></td>
                <td colspan="3"><asp:Table ID="TblAtasKanan" ClientIDMode="Static" HorizontalAlign="Center" Width="100%" cssClass="header" runat="server"></asp:Table></td>
            </tr>
            <tr>
                <td colspan="5" style="vertical-align: middle"><asp:Table ID="TblImage" Width="100%" ClientIDMode="Static" cssClass="" runat="server"></asp:Table></td>
                <td colspan="7" style="vertical-align: top;text-align: left"><asp:Table ID="TblListItems"  ClientIDMode="Static" cssClass="items" BorderStyle="Solid" HorizontalAlign="right" BorderWidth="1" runat="server"></asp:Table></td>
            </tr>
            <tr>
                <td colspan="4"><h5>Cara Pengisian Hasil Pengecheckan</h5>
                    <ul>
                        <li>Select "OK" jika sesuai standard</li>
                        <li>Select "ABNORMAL" jika kondisi Tidak normal</li>
                        <li>Select "NC" jika tidak sesuai standard</li>
                    </ul>
                </td>
                <td colspan="5"></td>
                <td colspan="3"><asp:Table ID="TblConfirm" ClientIDMode="Static" cssClass="" runat="server"></asp:Table></td>
           
            </tr>
        </table>
        
    </form>

    <script type="text/javascript">
        $('#form1').validate({
            submitHandler: function () {
                window.open("<%= Request.Url.AbsolutePath %>?v=" + $('#txtSerialNumber').val(), "_self");
            }
        });
    </script>
 </body>
</html>
