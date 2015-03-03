<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="MasterChecklist.aspx.cs" Inherits="TPM.MasterChecklist" %>
<%@ Import Namespace="TPM.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
   <form id="form1" action="FileUploader.aspx" runat="server">
  
    <div class="row-fluid">
            <div class="span3"><h4>PT. SHIMANO BATAM</h4></div>
            <div class="span6"></div>
            <div class="span3">FORM NO : <h5 style="display:inline"><asp:Label ID="LblForm" ClientIDMode="Static" runat="server" CssClass = "editable_cl"></asp:Label></h5></div>
    </div>
     <div class="row-fluid">
            <div class="span12 text-center"><h3 style="display:inline"><asp:Label ID="LblTitle" ClientIDMode="Static" runat="server"></asp:Label> Machine Check List</h3></div>           
    </div>
     <div class="row-fluid">
            <div class="span4"><asp:Table ID="TblAtasKiri" ClientIDMode="Static" cssClass="table table-condensed" runat="server"></asp:Table></div>
            <div class="span5 text-center"></div>
            <div class="span3"><asp:Table ID="TblAtasKanan" ClientIDMode="Static" cssClass="table table-condensed" runat="server"></asp:Table></div>
     </div>
    <div class="row-fluid">
            <div <%= ChecklistTypeid==1? "class=\"span5\"" : "style=\"display:none;\"" %> id="TblImageContainer"><asp:Table ID="TblImage" ClientIDMode="Static" cssClass="table table-bordered" runat="server"></asp:Table></div>
            <div class=<%= ChecklistTypeid==1? "\"span7\"" :"\"span11\"" %> id="TblListItemContainer"><asp:Table ID="TblListItems" ClientIDMode="Static" cssClass="table table-striped table-bordered" runat="server"></asp:Table></div>
    </div>
    <div class="row-fluid">
            <div class="span4"><h5>Cara Pengisian Hasil Pengecheckan</h5>
                <ul>
                    <li>Select "OK" jika sesuai standard</li>
                    <li>Select "NC" jika tidak sesuai standard</li>
                    <li>Select "ABNORMAL" jika kondisinya tidak normal</li>
                </ul>
            </div>
            <div class="span8"><asp:Table ID="TblConfirm" ClientIDMode="Static" cssClass="table table-bordered" runat="server"></asp:Table></div>          
        </div>
        </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        var checklist_type = '<%= ChecklistTypeid %>';
        var checklistId = '';
        var newitem = 0;
        var assetmodelId = '';
        var departmentId = '';
        var serialnumber = '';
        function ddlChange(id,index) {
            //$('#LblForm').text("");
            //$('#LblRevision_No').text("");
            //$('#LblPrepared_By').text("");
            //$('#LblApproved_By').text("");
            //$('#TblListItemContainer').html("");
           // $('#TblImageContainer').html("");
            var ddl = $('#ddlMachine');
            $.ajax({
                url: './Methodes/MasterChecklist.asmx/GetMachineModel',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ deptId: id }, null, 2),
                success: function (json) {

                    try {
                        var obj = json.d;
                        var data = $.parseJSON(obj);
                        ddl.empty();
                        $.each(data, function (val, text) {
                            ddl.append("<option value='" + val + "'>" + text + "</option>");
                        });
                        if (index == null) {
                            ddl.append("<option value='' selected>Please Select...</option>");
                        } else {
                            ddl.val(index);
                        }

                    } catch (error) {
                        alert("error " + error.message);
                    }
                    //$('#formHolder').hide(1000);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });
        }
        $('#ddlDepartment').change(function(event) {
            event.preventDefault();
            $('.tb').val('');
            var id = $(this).val();
            departmentId = id;
            //reset all
            ddlChange(id);
        });

          function mlistitem() {
              $('.editable_cli').editable('./Methodes/MasterChecklist.asmx/ChecklistItemUpdate', {
                  indicator: "<img src='./Images/indicator.gif'>",
                  onblur: 'submit',
                  // submit: "OK",
                  //cancel: "Cancel",
                  tooltip: "Click to edit...",
                  style: "inherit",
                  // type: "select",
                  // data: "{'1':'OK','0':'NC'}",
                  submitdata: function (value, settings) {
                      return {
                          "orig": this.revert
                      };
                  },
                  ajaxoptions: { contentType: "application/json; charset=utf-8" },
                  intercept: function (jsondata) {
                      try {
                          var obj = jQuery.parseJSON(jsondata);

                          return (obj.d);
                      }
                      catch (err) {
                          alert(err.message);
                      }
                      return "";

                  }
              });
          }
         /* $('td').click(function () {
              var col = $(this).parent().children().index($(this));
              var row = $(this).parent().parent().children().index($(this).parent());
              alert('Row: ' + row + ', Column: ' + col);
          });*/
        function Delete() {
            $('.delete').click(function () {
                var id = this.id;
                var s = id.split('_');
                var tr;
                switch (s[0]) {
                    case "image": tr = $("#" + s[1] + "-row2");                 
                       // tr.remove();
                        break;
                    case "listitem": tr = $("#" + s[1] + "-row");
                       // tr.remove();
                        break;
                default:
                }
                $.ajax({
                    url: './Methodes/MasterChecklist.asmx/deleteItem',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ table: s[0], id: s[1] }, null, 2),
                    success: function (json) {

                        try {
                            var obj = json.d;
                            if (obj == '') {
                                tr.remove();
                            }
                            //
                        } catch (error) {
                            alert("error " + error.message);
                        }
                        //$('#formHolder').hide(1000);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        var obj = $.parseJSON(xhr.responseText);
                        alert(obj.Message);
                    }
                });
              
                //console.log(s+ "   "+tr.attr("id"));
               
                //alert("Delete Click!"+ this.id);
            });
        }
        function AfterAjax() {
            mchecklist();
            mlistitem();
            Delete();
            $('.editable_image').click(function() {

                alert("guew");
            });
            $('.btn').click(function() {
                var id = this.id;
                id = id.toLowerCase();
                if (checklistId == "0") {
                    alert("Please Update the Header First!");
                    return false;
                }
                switch (id) {
                    case 'add_list':
                        
                        newitem++;
                        var temp = "<tr id='"+newitem+"-rownew' class='rownew'>";
                        $('#TblListItems th').each(function () {
                                temp += "<td id='" + newitem + "-" + $(this).text() + "-new' class='newitem'></td>";
                        });
                        temp += "</tr>";
                        
                        $('#TblListItems').append(temp);
                        $('.newitem').editable('./Methodes/MasterChecklist.asmx/ChecklistItemInsert', {
                            indicator: "<img src='./Images/indicator.gif'>",
                            onblur: 'submit',
                            // submit: "OK",
                            //cancel: "Cancel",
                            tooltip: "Click to edit...",
                            style: "inherit",
                            // type: "select",
                            // data: "{'1':'OK','0':'NC'}",
                            submitdata: function (value, settings) {
                                return {
                                    "orig": this.revert,
                                    "cid":checklistId
                                };
                            },
                            ajaxoptions: { contentType: "application/json; charset=utf-8" },
                            intercept: function (jsondata) {
                                try {
                                    var obj = jQuery.parseJSON(jsondata);
                                    var kk = jQuery.parseJSON(obj.d);
                                   
                                    $('#TblListItems th').each(function () {
                                        var field = $(this).text();
                                        //console.log(field + '\r\n');
                                        var td = $("#"+kk.newListId + "-" + field + "-new");
                                        if (field != "DELETE") {
                                            td.removeClass("newitem");
                                            td.unbind();
                                            td.addClass("editable_cli");
                                            td.attr("id", kk.InsertedId + "-" + field);

                                        } else {
                                            td.removeClass("newitem");
                                            td.unbind();
                                            td.html('<a href="javascript:void(0)"  id="listitem_' + kk.InsertedId + '" class="btn delete btn-primary" >DELETE</a>');
                                            Delete();
                                        }
                                        var tr = $("#" + kk.newListId + "-rownew");
                                        tr.attr("id", kk.InsertedId + "-row");
                                    });
                                    mlistitem();
                                    return (kk.value);
                                }
                                catch (err) {
                                    alert(err.message);
                                }
                                return "";

                            }
                        });
                        break;
                    case 'add_image':
                        window.open("/<%=TPMHelper.WebDirectory%>/YCheckListImg.aspx?i=" + checklistId, "_self");
                        break;
                }
                
                return false;
            });
           
        }
        function search() {
            var deptid = '';
            var sn = $('.tb').val();
            //$('#serialcode').text('');
            $('.ddl').val('');
            var id = '0';
            assetmodelId = '0';
            deptid = '0';
            $.ajax({
                url: './Methodes/MasterChecklist.asmx/GetCheckList',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ deptId: deptid, id: id, cid: checklist_type, sn: sn }, null, 2),
                success: function (json) {

                    try {
                        var obj = json.d;
                        var checklist = $.parseJSON(obj);
                        checklistId = checklist.Id.toString();
                        $('#LblForm').text(checklist.FormNumber);
                        $('#LblRevision_No').text(checklist.Revision);
                        $('#LblPrepared_By').text(checklist.PreparedBy);
                        $('#LblApproved_By').text(checklist.ApprovedBy);
                        $('#TblListItemContainer').html("");
                        $('#TblListItemContainer').append(checklist.ChecklistItemsTbl);
                        $('#TblImageContainer').html("");
                        $('#TblImageContainer').append(checklist.ChecklistImagesTbl);
                        $('#ddlDepartment').val(parseInt(checklist.DeptID));
                        ddlChange(parseInt(checklist.DeptID), parseInt(checklist.AMI));
                        //$('#ddlMachine').val(parseInt(checklist.AMI));

                        //
                        AfterAjax();
                        //
                    } catch (error) {
                        alert("error " + error.message);
                    }
                    //$('#formHolder').hide(1000);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });
        }

        $('.tb').blur(function() {
           // search();
        });
        $('.tb').keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                //alert('dfsd');
                e.preventDefault();
                serialnumber = $(this).val();
                search();
            }
        });
        $('#ddlMachine').change(function(event) {
            event.preventDefault();
            $('.tb').val('');
            var deptid = $('#ddlDepartment').val();
            var id = $(this).val();
            var sn = $('.tb').val();
            assetmodelId = id;
           // alert('ddlmachine changed!');
            $.ajax({
                url: './Methodes/MasterChecklist.asmx/GetCheckList',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ deptId: deptid,id:id,cid: checklist_type ,sn:sn}, null, 2),
                success: function (json) {

                    try {
                        var obj = json.d;
                        var checklist = $.parseJSON(obj);
                        checklistId = checklist.Id.toString();
                        $('#LblForm').text(checklist.FormNumber);
                        $('#LblRevision_No').text(checklist.Revision);
                        $('#LblPrepared_By').text(checklist.PreparedBy);
                        $('#LblApproved_By').text(checklist.ApprovedBy);
                        $('#TblListItemContainer').html("");
                        $('#TblListItemContainer').append(checklist.ChecklistItemsTbl);
                        $('#TblImageContainer').html("");
                        $('#TblImageContainer').append(checklist.ChecklistImagesTbl);
                        //
                        AfterAjax();
                        //
                    } catch (error) {
                        alert("error " + error.message);
                    }
                    //$('#formHolder').hide(1000);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });
        });

        function mchecklist() {
            $('.editable_cl').editable('./Methodes/MasterChecklist.asmx/ChecklistUpdate', {
                indicator: "<img src='./Images/indicator.gif'>",
                onblur: 'submit',
                // submit: "OK",
                //cancel: "Cancel",
                tooltip: "Click to edit...",
                style: "inherit",
                // type: "select",
                // data: "{'1':'OK','0':'NC'}",
                submitdata: function(value, settings) {
                    return {
                        "orig": this.revert,
                        "cid": checklistId,
                        "ctid": checklist_type,
                        "amid": $('#ddlMachine option:selected').val(),
                        "dpid": $('#ddlDepartment option:selected').val(),
                        "sn": serialnumber
                    };
                },
                ajaxoptions: { contentType: "application/json; charset=utf-8" },
                intercept: function(jsondata) {
                    try {
                        var obj = jQuery.parseJSON(jsondata);
                        var kk = jQuery.parseJSON(obj.d);
                        checklistId = kk.CID;
                        return (kk.VAL);
                    } catch(err) {
                        alert(err.message);
                    }
                    return "";

                }
            });
        }
    </script>
</asp:Content>
