<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YUser.aspx.cs" Inherits="TPM.YUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <form name="form1" id="form1" runat="server"> 
    <div class="row-fluid">
        <div class="span12">
            
                <div class="container-fluid">
                     <div class="row-fluid">
                        <div class="span8">
                            <div class="container-fluid">
                             <div class="row-fluid">
                                <div class="span12">
                
                                    <h4 >User by Category</h4>
                                    <hr />
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span3">
          
                                     <asp:ListBox ID="lbCat" ClientIDMode="Static" runat="server" Rows="10" CssClass="table table-bordered">

                                    </asp:ListBox>
           
                                </div>
                                <div class="span9">
                                    <h4 id="lblCategory"></h4>
                                      <asp:Table ID="tblUserByCat" ClientIDMode="Static" runat="server" CssClass="table table-bordered">

                                    </asp:Table>
                                </div>
                            </div>      
                            <div class="row-fluid">
                                <div class="span12">
                                    <hr />                        
                                    <h4 id="Categorylbl"class="btn btn-primary">Show User Master Data</h4>
                                    
                            </div>
                            </div>   
                             <div class="row-fluid"  id="divCategory" style="display:none">
                                        <div class="span12">
                                        <hr />
                                        <asp:Table ID="tblUserList" ClientIDMode="Static" runat="server" CssClass="table table-bordered"></asp:Table>
                                    </div>
                                </div>
                               </div>
                        </div>

                     
                          <div class="span4">
                            <asp:Table ID="tblInfo" ClientIDMode="Static" runat="server" CssClass="table">
                                <asp:TableRow >
                                    <asp:TableCell ColumnSpan="2">
                                        <h4 id="lblusername"></h4>
                                     </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow >
                                    <asp:TableCell RowSpan="2">
                                        <asp:ListBox ID="listAvailableRole" ClientIDMode="Static" Rows="6" runat="server">
                                            
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        Available Roles
                                    </asp:TableCell>
                                </asp:TableRow>
                                 <asp:TableRow >
                                    <asp:TableCell>
                                        <asp:Button  OnClientClick="return false;" ID="btnAddRole" UseSubmitBehavior="false" ClientIDMode="Static" runat="server" CssClass="btn btn-primary tombol"  Text="Add Role" />
                                  </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow >
                                    <asp:TableCell RowSpan="2">
                                        <asp:ListBox ID="listRole" ClientIDMode="Static" Rows="6" runat="server">
                                           
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                       User's Roles  
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow >
                                 <asp:TableCell>
                                        <asp:Button OnClientClick="return false;" ID="btnRemoveRole" UseSubmitBehavior="false" ClientIDMode="Static" runat="server" CssClass="btn btn-primary tombol" Text="Remove Role"/>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="2">
                                        <asp:CheckBox ID="cbEmail" ClientIDMode="static" CssClass="inline" Text="Email Receiver" runat="server"/>
                                    </asp:TableCell>
                                </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell ColumnSpan="2" >
                                        <asp:CheckBox ID="cbMobile" ClientIDMode="static" CssClass="inline" Text="SMS Receiver" runat="server"/>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                         
                    </div>
                </div>
             
        </div>
    </div>
   
     </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <script type="text/javascript">
        function ajaxUser(table,  value, act, successfunction) {
            $.ajax({
                url: './Methodes/user.asmx/Update'+table+'Info',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({val:value, action: act }, null, 2),
                success: function (json) {

                    try {
                        var obj = json.d;
                        var data = $.parseJSON(obj);
                        
                            successfunction(data);
                        
                        
                    } catch (error) {
                        alert("error " + error.message);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });

        }
        $(document).ready(function () {
            var togle = false;
           
            var ssData = " ";
            var ssName = " ";
            var ssEmail = "";
            var ssMobile = "";
            var bssData = "";
            var browindex = -1;

            $('.tombol').click(function (event) {
                event.preventDefault();
                var s = this.id;
               
                var selected = $('#listRole').val() == null ? " " : $('#listRole').val();
              
                if (s == "btnRemoveRole") {
                    ajaxUser("Role", ssData + "$" + selected + "$" + ssName + "$" + $('#listAvailableRole option:selected').text(), "delete", function (data) {
                      
                        $("#listRole option[value='" + selected + "']").remove();
                        if ((browindex > -1)) {
                            bTable.dataTable().fnDeleteRow(browindex);
                        }
                    });
                
                } else {
                    var selected = $('#listAvailableRole').val() == null ? " " : $('#listAvailableRole').val();
                    ajaxUser("Role", ssData + "$" + selected + "$" + ssName + "$" + $('#listAvailableRole option:selected').text(), "insert", function (data) {
                       
                        $.each(data, function (val, text) {
                            splited = val.split('_');
                            $('#listRole').append("<option value='" + splited[1] + "'>" + text + "</option>");
                        });
                    });
                }
            });
            $("input[type='checkbox']").change(function () {
                var cbid = this.id=="cbEmail" ? "Email":"Mobile";
                var action = this.checked == true ? "insert" : "delete";
                var iskosong = (cbid == "Email" ? (ssEmail == "") : (ssMobile == ""))&&(action=="insert");
                if (iskosong) {

                    alert(ssName + "'s " + cbid + " is not supplied");
                    $('#cb' + cbid).prop("checked", false);
                }
                else
                {
                    ajaxUser(cbid, ssData + "$" + (cbid == "Email" ? ssEmail : ssMobile) + "$" + ssName, action, function (data) {
                        $.each(data, function (val, text) {
                            var c = text == "False" ? false : true;
                            $('#cb' + cbid).prop("checked", c);
                            if ((browindex > -1) && (c == false)) {
                                //bTable.dataTable().fnDeleteRow(browindex);
                            }
                        });
                    });
                }
                    
            });
            var bTable = $('#tblUserByCat').dataTable(
            {
                "bProcessing": true,
                
                "bLengthChange": false,
                "bFilter": true,
                "bSort": false,
                "bInfo": false,
                "bAutoWidth": false
            });
            var oTable = $('#tblUserList').dataTable({});
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
                    ssData = oTable.fnGetData(iData)[0]; //index coloumn of DBID
                    ssName = oTable.fnGetData(iData)[1];
                    ssEmail = oTable.fnGetData(iData)[2];
                    ssMobile = oTable.fnGetData(iData)[3];
                    $('#lblusername').text(ssName);
                    bTable.dataTable().fnClearTable();
                    $('#lbCat').val('');
                    browindex = -1;
                    $.ajax({
                        url: './Methodes/user.asmx/GetUserInfo',
                        cache: false,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ id: ssData }, null, 2),
                        success: function (json) {

                            try {
                                var obj = json.d;
                                var data = $.parseJSON(obj);
                                $('#listRole').empty();
                                $.each(data, function (val, text) {
                                    splited = val.split('_');
                                    if (splited[0] == "role") {
                                        $('#listRole').append("<option value='" + splited[1] + "'>" + text + "</option>");

                                    }
                                    if (splited[0] == "sms") {
                                        $('#cbMobile').prop("checked", splited[1] == 'False' ? false : true);
                                    }
                                    if (splited[0] == "email") {
                                        $('#cbEmail').prop("checked", splited[1] == 'False' ? false : true);
                                    }
                                });

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

                    ssData = bTable.fnGetData(biData)[2]; //index coloumn of DBID
                    ssName = bTable.fnGetData(biData)[1];
                    ssEmail = $('#lbCat').val()=="email_"? bTable.fnGetData(biData)[3]:"";
                    ssMobile = $('#lbCat').val() == "mobile_" ? bTable.fnGetData(biData)[3] : "";
                    $('#lblusername').text(ssName);
                    
                    $.ajax({
                        url: './Methodes/user.asmx/GetUserInfo',
                        cache: false,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ id: ssData }, null, 2),
                        success: function (json) {

                            try {
                                var obj = json.d;
                                var data = $.parseJSON(obj);
                                $('#listRole').empty();
                                $.each(data, function (val, text) {
                                    splited = val.split('_');
                                    if (splited[0] == "role") {
                                        $('#listRole').append("<option value='" + splited[1] + "'>" + text + "</option>");
                                    }
                                    if (splited[0] == "sms") {
                                        $('#cbMobile').prop("checked", splited[1] == 'False' ? false : true);
                                    }
                                    if (splited[0] == "email") {
                                        $('#cbEmail').prop("checked", splited[1] == 'False' ? false : true);
                                    }
                                });

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
            }
            //
            $('#lbCat').click(function () {
                bTable.dataTable().fnClearTable();
                browindex = -1;
                $.ajax({
                    url: './Methodes/user.asmx/selectbyCAT',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ param: $(this).val() }, null, 2),
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

            });

            $('#Categorylbl').click(function () {
                oTable.dataTable().fnClearTable();
               
                $.ajax({
                    url: './Methodes/user.asmx/getmasteruser',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({param: $(this).val() }, null, 2),
                    success: function (json) {

                        try {
                            var obj = json.d;
                            var data = $.parseJSON(obj);
                            oTable.dataTable().fnAddData(data, true);
                            otables();
                            $('#divCategory').show();
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
