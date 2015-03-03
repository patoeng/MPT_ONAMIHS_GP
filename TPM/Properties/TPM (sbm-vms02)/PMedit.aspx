<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="PMedit.aspx.cs" Inherits="TPM.PMedit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hsonuc {
            position:absolute;
            top:20px;
            right:50%; /* Positions 50% from right (right edge will be at center) */
            margin-right:200px; /* Positions 200px to the left of center */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
   <form id="form1" name="form1" runat="server" style="display:inline;">
      <div class="row-fluid">
        <div class="span12">
           <div class="messagepop pop" id="FormHolder"> 
                    <asp:Table ID="FormTbl" runat="server" ClientIDMode="Static" CssClass="table">
                    </asp:Table>   
           </div>
        </div>
    </div>

          <div class="row-fluid">
             <div class="span12 text-center" ><h3>Master Plan of Machine <%= checktype==2? "Monthly" : "Daily" %> Preventive Maintenance</h3></div>
        </div>
         <div class="row-fluid">
             <div class="span12 text-center"><h3>For the Month of <asp:Label ID="lblMonth" ClientIDMode="Static" runat="server"> <%= saiki.ToString("MMMM yyyy") %></asp:Label></h3></div>
        </div>
        
         <div class="row-fluid">
                    <div class="span4" style="display:inline;">
                     Department
                     <asp:DropDownList runat="server" ID="ddlDepartment" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                        </div>
                    <div class="span4" style="display:inline;">
                     Month
                     <asp:DropDownList runat="server" ID="ddlMonth" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
                     </div>
                        <div class="span4" style="display:inline;">
                        Year
                     <asp:DropDownList runat="server" ID="ddlYear" ClientIDMode="Static" cssClass="ddl"></asp:DropDownList>
               
                            </div>
            </div>
       
       </form>
         <div class="row-fluid">
             <div class="span12 text-center"><asp:Table ID="tblSchedule" ClientIDMode="Static" runat="server" CssClass="table table-striped table-condensed table-bordered"></asp:Table></div>
        </div>
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var id = "";
            var pmid = "";
        $('.pop').hide();
        $(".ddl").change(function () {
            window.open("<%= Request.Url.AbsolutePath %>?dept=" + $("#ddlDepartment").val() + "&mon=" + $("#ddlMonth").val() + "&year=" + $("#ddlYear").val() + "&ty=<%=checktype.ToString() %>", "_self");
        });
          
        $('.sched_date').click(function () {
            id= $(this).attr('id');
            $.ajax({
                url: './Methodes/pm.asmx/getPMbyAssetCode',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id:id}, null, 2),
                success: function (json) {

                    try {
                        var obj = json.d;
                        var data = $.parseJSON(obj);
                        if (data.length != 0) {
                            pmid = data[0];
                            $('#BtnSubmit1').val("Delete");
                            $('.rowtxt').hide();
                            $(".pop").slideFadeShow(function () {
                              
                                
                            });
                            
                        } else {

                            $('.rowtxt').show();
                            $('input:text').val("");
                           
                            $('#BtnSubmit1').val("Insert");
                            $(".pop").slideFadeShow(function () {
                               

                            });
                        }
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
        
        $.fn.slideFadeShow = function (easing, callback) {
            
            this.animate({ opacity: 'show', left: '30%', top: '50%',position:'absolute' }, "fast", easing, callback);
        
            return true;
        }
        $.fn.slideFadeHide = function (easing, callback) {
           
            this.animate({ opacity: 'hide' }, "fast", easing, callback);
          
            return true
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
            });}
            $('#BtnCloseForm').click(function () { $('.pop').slideFadeHide(); $('#form1').resetForm() });
            $('#FormHolder').drags({ handle: "#trhead", div: "FormHolder" });
            $('.tombol').click(function (event) {
                event.preventDefault();
                var action = $(this).val();
                var txtPic = $('#txtPIC').val();
                var txtAppr = $('#txtApprovedby').val();
                
                $.ajax({
                    url: './Methodes/pm.asmx/action2',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ id:id,pmid:pmid,act:action,pic:txtPic,appr:txtAppr }, null, 2),
                    success: function (json) {

                        try {
                            var obj = json.d;
                            
                            if (obj != "") {
                                if (action == "Insert") {
                                    
                                    $("#"+obj).text("X");
                                } else {
                                    $("#"+obj).text("-");
                                }
                            } else {
                                
                            }
                            $('.pop').slideFadeHide();
                            

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
