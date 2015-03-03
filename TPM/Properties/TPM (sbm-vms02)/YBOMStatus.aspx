<%@ Page Title="Review Asset BOM Status" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YBOMStatus.aspx.cs" Inherits="TPM.YBOMStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" name="form1" runat="server">

    <div class="row-fluid">
        <div class="span3">
            <asp:ListBox ID="lbAssetModels" ClientIDMode="Static" runat="server" Rows="20"></asp:ListBox>
        </div>
        <div class="span9">
            <h4 id="lblCat"><%= asset_model_id == "" ? "Please Select Asset Model" : asset_model_name %></h4>
            <asp:Table ID="tblBOM" ClientIDMode="Static" runat="server" CssClass="table table-bordered"></asp:Table>
        </div>
       
    </div> 
   </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
           
            var browindex = -1;
            var AssetModel;
            var AssetModelName;
            var ssData;
            var ssName;
            var ssQty;
            var bTable = $('#tblBOM').dataTable(
            {
                "bProcessing": true,
                "bLengthChange":true,
                "bFilter": true,
                "bSort": true,
                "bInfo": false,
                "bAutoWidth": false
            });
            

            function btables() {
                bTable.$('tr').hover(function () {
                    $(this).addClass("hell");
                });
                bTable.$('tr').mouseout(function () {
                    $(this).removeClass("hell");

                });
                bTable.$('td').click(function () {
                    var biData = bTable.fnGetPosition($(this).closest('tr')[0]);
                    browindex = biData;
                    var biiData = bTable.fnGetPosition($(this).closest('td')[0]);
                    var bcData = biiData[1];

                    ssData = bTable.fnGetData(biData)[0]; //index coloumn of DBID
                    ssName = bTable.fnGetData(biData)[1];
                    ssQty = bTable.fnGetData(biData)[2];
                });
            }
            $('#lbAssetModels').change(function () {
                AssetModel = $(this).val();
                AssetModelName = $('#lbAssetModels option:selected').text();
                $('#lblCat').text(AssetModelName);
                browindex = -1;
                bTable.dataTable().fnClearTable();
                $.ajax({
                    url: './Methodes/bom.asmx/GetBOMInfobyasmod2',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ id: AssetModel }, null, 2),
                    success: function (json) {

                        try {
                            var obj = json.d;
                            var data = $.parseJSON(obj);
                            bTable.dataTable().fnAddData(data, true);
                            btables();

                        } catch (error) {
                            alert("error " + error.message);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        var obj = $.parseJSON(xhr.responseText);
                        alert(obj.Message);
                    }
                });
                // alert(AssetModel);

            });



         

            $('.btn').click(function (event) {
                event.preventDefault();
                var action = $(this).val();
                ssQty = $('#txtQty').val();
                $.ajax({
                    url: './Methodes/bom.asmx/action',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ code: ssData, desc: ssName, qty: ssQty, asmod: AssetModel, act: action }, null, 2),
                    success: function (json) {

                        try {
                            var obj = json.d;
                            var data = $.parseJSON(obj);
                            if (data.length != 0) {
                                if (action == "Insert") {
                                    bTable.dataTable().fnAddData(data, true);
                                } else {
                                    bTable.dataTable().fnUpdate(data, browindex); // Row
                                    browindex = -1;
                                }
                            } else {
                                bTable.dataTable().fnDeleteRow(browindex);
                            }
                            $('.pop').slideFadeHide();
                            btables();

                        } catch (error) {
                            alert("error " + error.message);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        var obj = $.parseJSON(xhr.responseText);
                        alert(obj.Message);
                    }
                });
            });
        });
    </script>
</asp:Content>
