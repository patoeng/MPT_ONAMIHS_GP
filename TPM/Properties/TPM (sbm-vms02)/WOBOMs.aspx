<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="WOBOMs.aspx.cs" Inherits="TPM.WOBOMs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form id="form1" runat="server">
      <div class="row-fluid">
        <div class="span12">
           <div class="messagepop pop" id="FormHolder" style="display:inline-block;margin: auto 1.5em;"> 
                    <asp:Table ID="FormTbl" runat="server" ClientIDMode="Static" CssClass="table">
                    </asp:Table>   
           </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="span11">
            <h4 id="lblCat">Spare Part Replacement WO ID : #</h4>
             <hr />
            <asp:Table ID="tblBOM" ClientIDMode="Static" runat="server" CssClass="table table-bordered"></asp:Table>
        </div>
        <div class="span1">
            
        </div>

    </div>
    <div class="row-fluid">
        <div class="span11">
            <hr />
            <h4 id="lblInv" class="btn btn-primary">Master Inventory Stock</h4>
        </div>
        <div class="span1">
            
        </div>
    </div>
    <div class="row-fluid" id="Inventory" style="display:none;">
        <div class="span11">
             <hr />
            <asp:Table ID="tblInventory" ClientIDMode="Static" runat="server" CssClass="table table-bordered"></asp:Table>
        </div>

    </div>
   </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
   <script type="text/javascript">
       $(document).ready(function () {
           var woid = "<%= WOId %>";
           $('.pop').hide();
           var browindex = -1;
           var AssetModel;
           var AssetModelName;
           var ssData;
           var ssName;
           var ssQty;
           var ssReason;
           var bTable = $('#tblBOM').dataTable(
           {
               "bProcessing": true,
               "bLengthChange": false,
               "bFilter": true,
               "bSort": false,
               "bInfo": false,
               "bAutoWidth": false
           });
           var oTable = $('#tblInventory').dataTable(
           {
               "bProcessing": true

           });
           btables();
           $('#lblInv').click(function () {
               oTable.dataTable().fnClearTable();
               $.ajax({
                   url: './Methodes/bom.asmx/GetInventory',
                   cache: false,
                   type: 'POST',
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   data: JSON.stringify({ id: AssetModel }, null, 2),
                   success: function (json) {

                       try {
                           var obj = json.d;
                           var data = $.parseJSON(obj);
                           oTable.dataTable().fnAddData(data, true);
                           otables();
                           $('#Inventory').show();
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
           function otables() {
               oTable.$('tr').hover(function () {
                   $(this).addClass("hell");
               });
               oTable.$('tr').mouseout(function () {
                   $(this).removeClass("hell");

               });
               oTable.$('td').click(function () {

                   var iData = oTable.fnGetPosition($(this).closest('tr')[0]);
                   var rowindex = iData;
                   var iiData = oTable.fnGetPosition($(this).closest('td')[0]);
                   var cData = iiData[1];
                   ssData = oTable.fnGetData(iData)[1]; //index coloumn of DBID
                   ssName = oTable.fnGetData(iData)[2];
                   ssQty = 0;
                   ssReason = "";

                   $('#FormHeader').text('Add Item To BOM');
                   $(".pop").slideFadeShow(function () {
                       $('#BtnSubmit1').val("Insert");
                       $('#txtCode').val(ssData);
                       $('#txtName').val(ssName);
                       $('#txtQty').val(ssQty);
                       $('#txtReason').val(ssReason);
                   });
               });
           }
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

                   ssData = bTable.fnGetData(biData)[1]; //index coloumn of DBID
                   ssName = bTable.fnGetData(biData)[2];
                   ssQty = bTable.fnGetData(biData)[3];
                   ssReason = bTable.fnGetData(biData)[4];


                   $('#FormHeader').text('Edit BOM Item');
                   $(".pop").slideFadeShow(function () {
                       $('#BtnSubmit1').val("Update");
                       $('#BtnSubmit2').val("Delete");
                       $('#BtnSubmit2').show();
                       $('#txtCode').val(ssData);
                       $('#txtReason').val(ssReason);
                       $('#txtName').val(ssName);
                       $('#txtQty').val(ssQty);
                   });
               });
           }
         



           $.fn.slideFadeShow = function (easing, callback) {
               return this.animate({ opacity: 'show', left: 'auto' }, "fast", easing, callback);
           }
           $.fn.slideFadeHide = function (easing, callback) {
               return this.animate({ opacity: 'hide' }, "fast", easing, callback);
           }
           $.fn.drags = function (opt) {

               opt = $.extend({
                   handle: "",
                   cursor: "move",
                   draggableClass: "draggable",
                   activeHandleClass: "active-handle",
                   div: ""
               }, opt);

               var $selected = null;
               var $elements = (opt.handle === "") ? this : this.find(opt.handle);

               return $elements.css('cursor', opt.cursor).on("mousedown", function (e) {
                   if (opt.handle === "") {
                       $selected = $(this);
                       $selected.addClass(opt.draggableClass);
                   } else {
                       $selected = $('#' + opt.div);
                       $selected.addClass(opt.draggableClass).find(opt.handle).addClass(opt.activeHandleClass);
                   }
                   var drg_h = $selected.outerHeight(),
                       drg_w = $selected.outerWidth(),
                       pos_y = $selected.offset().top + drg_h - e.pageY,
                       pos_x = $selected.offset().left + drg_w - e.pageX;
                   $(document).on("mousemove", function (e) {
                       $selected.offset({
                           top: e.pageY + pos_y - drg_h,
                           left: e.pageX + pos_x - drg_w
                       });
                   }).on("mouseup", function () {
                       $(this).off("mousemove"); // Unbind events from document
                       $selected = $('#' + opt.div);
                       $selected.removeClass(opt.draggableClass);
                       $selected = null;
                   });
                   e.preventDefault(); // disable selection
               }).on("mouseup", function () {
                   if (opt.handle === "") {
                       $selected = $('#' + opt.div);
                       $selected.removeClass(opt.draggableClass);
                   } else {
                       $selected.removeClass(opt.draggableClass)
                           .find(opt.handle).removeClass(opt.activeHandleClass);
                   }
                   $selected = null;
               });

           }
           $('#BtnCloseForm').click(function () { $('.pop').slideFadeHide(); $('#form1').resetForm() });
           $('#FormHolder').drags({ handle: "#trhead", div: "FormHolder" });

           $('.tombol').click(function (event) {
               event.preventDefault();
               var action = $(this).val();
               ssQty = $('#txtQty').val();
               ssReason = $('#txtReason').val();
               $.ajax({
                   url: './Methodes/bom.asmx/action3',
                   cache: false,
                   type: 'POST',
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   data: JSON.stringify({ woid: woid, code: ssData, qty: ssQty, act: action, reason: ssReason, name: ssName }, null, 2),
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
