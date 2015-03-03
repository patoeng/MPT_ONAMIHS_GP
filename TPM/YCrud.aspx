<%@ Page Title="Table CRUD" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YCrud.aspx.cs" Inherits="TPM.YCrud" %>
<%@ Import Namespace="TPM.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
        <div class="span12">
           <div class="messagepop pop" id="FormHolder">
                <form id="form1" runat="server" >
                    <asp:Table ID="FormTbl" runat="server" ClientIDMode="Static" CssClass="table table-condensed">
                    </asp:Table>    
                </form>
           </div>
        </div>
    </div>
    <div class="row-fluid">
            <div class="span12" style="text-align: right">
                <h5><%= form=="" ? "":"Form NO : "+ form %></h5>
            </div>
        </div>
    <div class="row-fluid">
        <div class="span8">
            <h3><%=tittle==string.Empty? "Table Of"+ TableName : tittle %></h3>
            <hr/>
        </div>
        <div class="span4" style="text-align: right">
            <a href="/<%=TPMHelper.WebDirectory%>/YBulk.aspx?table=<%= TableName%>&t=<%= tittle %>" class="btn btn-primary">Export To Excel / Import From CSV</a>
        </div>
    </div>
     <div class="row-fluid">
        <div class="span12">
            <asp:Table ID="CrudTbl" runat="server" ClientIDMode="Static" CssClass="table"></asp:Table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">

        function fnAddRow() {
            $('#FormHeader').text('Add New')
            $('#form1').resetForm();
            $('#id').val("0");
            $(".pop").slideFadeShow(function () {
            });
            $("#BtnSubmit").val("Insert");
            rowindex = -1;
        };
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
        var rowindex = -1;
        var ForeignColNumber =<%= ForeignColNumS %>

         $(document).ready(function () {
             $('.onchange').change(function () {
                 $('.onchange').val($(this).val());
             });
             $('input.datepicker').Zebra_DatePicker({
                 format: 'd-M-Y'
             });
             var oTable = $('#<%= CrudTbl.ClientID%>').dataTable({
                 "sDom": '<"top"lf<"tools">>rt<"actions"<"actions-left"i><"actions-right"p>>',
                 "aoColumnDefs": [

                             { "bSearchable": false, "bVisible": false, "aTargets": [0, 1 <% for (int j = 0; j < ForeignColNumS; j++) { Response.Write(","+ (j + 2)); } %>] },
                            { "bVisible": false, "aTargets": [] }],
                 "bSortClasses": false,
                 "sPaginationType": "full_numbers"
             });
             $('div.tools').html("&nbsp;&nbsp;<button id='btnAdd' value='Test' class='btn btn-primary' onclick='fnAddRow()'>Add New</button>");

             $('#form1').validate({
                 submitHandler: function (form) {
                     //alert("sasdas"),
                     // console.log(JSON.stringify($(form).serializeArray(),null,2)),
                  
                     $.ajax({
                         url: './Methodes/crud.asmx/InsertUpdate',
                         cache: false,
                         type: 'POST',
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         data: JSON.stringify({ anoman: $(form).serializeArray() }, null, 2),
                         success: function (json) {
                             
                             try {
                                 var obj = json.d;
                                 var data = $.parseJSON(obj);
                                 if (data.length != 0) {
                                     if (rowindex == -1) {
                                         $('#<%= CrudTbl.ClientID%>').dataTable().fnAddData(data);
                                         otables();
                                    } else {
                                        $('#<%= CrudTbl.ClientID%>').dataTable().fnUpdate(data, rowindex); // Row
                                        rowindex = -1;
                                    }
                                } else {
                                    $('#<%= CrudTbl.ClientID%>').dataTable().fnDeleteRow(rowindex);
                                }
                                $('.pop').slideFadeHide();
                                alert("Data Successfuly Saved.");

                            } catch (error) {
                                alert("error " + error.message);
                            }
                            $('#formHolder').hide(1000);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //alert(xhr.status);
                            //alert(thrownError);
                            //alert(xhr.responseText);
                            
                            var obj = $.parseJSON(xhr.responseText);
                            alert(obj.Message);


                        }
                    })
                }
             });
             otables();
             function otables() {
                 oTable.$('tr').hover(function () {
                     $(this).addClass("hell");
                 });
                 oTable.$('tr').mouseout(function () {
                     $(this).removeClass("hell");

                 });
                 oTable.$('td').click(function () {
                     $("#BtnSubmit").val("Update");
                     $(".pop").slideFadeShow(function () {
                     });
                     var sData = oTable.fnGetData(this);
                     iData = oTable.fnGetPosition($(this).closest('tr')[0]);
                     rowindex = iData;
                     iiData = oTable.fnGetPosition($(this).closest('td')[0]);
                     cData = iiData[1];
                     ssData = oTable.fnGetData(iData)[0]; //index coloumn of DBID
                     $('#FormHeader').text('Edit');
                     $('#form1').resetForm();
                     $('#Id').val(ssData);
                     $('#formHolder').show(1000);

                     $.ajax({
                         url: './Methodes/crud.asmx/GetOneRow',
                         cache: false,
                         type: 'POST',
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         data: JSON.stringify({ id: ssData, tablename: $("#TableName").val() }, null, 2),
                         success: function (json) {

                             try {
                                 var obj = json.d;
                                 //obj = obj.replace(",]", "]");
                                 //alert(obj);
                                 //console.log(obj);
                                 var data = $.parseJSON(obj);
                                 $('#form1').resetForm();
                                 <%
                                 int i = 0;
                                 for (i = 0; i < Field.Count;i++ )
                                 {
                                     Response.Write("$('#" + Field[i] + "').val(data[" + i + "]);\r\n");
                                 }
                                
                            %>

                             } catch (error) {
                                 alert("error " + error.message);
                             }

                         },
                         error: function (xhr, ajaxOptions, thrownError) {
                             var obj = $.parseJSON(xhr.responseText);
                             alert(obj.Message);


                         }

                     });


                     // alert('The cell clicked on had the value of ' + sData + ' TBrowindex ' + iData + ' TBcolindex ' + iiData[1] + ' DBid ' + ssData);
                 });
             }
             $('#BtnCloseForm').click(function () { $('.pop').slideFadeHide(); $('#form1').resetForm() });
             $('#FormHolder').drags({ handle: "#trhead", div: "FormHolder" });
             <%  Response.Write(defaultv == "" ? "" : "$('.dataTables_filter').children().children().attr('disabled', 'disabled')");%>
             oTable.fnFilter("<%= defaultv %>");
         });
     </script>
</asp:Content>
