<%@ Page Title="Table Bulk Application" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YBulk.aspx.cs" Inherits="TPM.YBulk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
        <div class="span12">
            <h4>Table <%= TableName %> Bulk Data Manipulation</h4>
            <hr />
        </div>
        <div class="span12">
            <form id="form1" runat="server">
                <asp:Table ID="FormTable" ClientIDMode="Static"   runat="server" CssClass="table table-bordered"></asp:Table>
            </form>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <asp:Table ID="LogTbl" ClientIDMode="Static" runat="server"></asp:Table> 
             </div>
        </div>
        <div class="row-fluid">
            <div class="span12" id="csvimporthint">
                <span></span>
            </div>
        </div>
     </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
        <script type='text/javascript' src='./Scripts/ui/jquery-ui.min.js'></script>
    <script type="text/javascript" src="./Scripts/swfobject.js"></script>
    <script type='text/javascript' src='./Scripts/jquery.FileReader.js'></script>
    <script type='text/javascript'>
        var RowExecuted = 0;
        var RowFailed = 0;
        var RowSucceeded = 0;
        var RowIgnored = 0;
       
        $('#BtnDownloadTemplate').click(function () {
           
            $.ajax({
                url: './Methodes/Bulk.asmx/CreateTemplate',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ TableName: $('#TableName').val(), prepare: $('#DdlPrepareRowInsert').val() }, null, 2),
                success: function (json) {
                    var obj = json.d;
                    window.open("../TPM/StreamDownload.aspx?u=http://<%= Request.Url.Authority +Request.ApplicationPath  %>/" + obj, "_self");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                   
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);
                }
            });

        });
        $('#BtnDownloadData').click(function () {
           
            $.ajax({
                url: './Methodes/Bulk.asmx/Export',
                cache: false,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ TableName: $('#TableName').val(), prepare: $('#DdlPrepareRowInsert').val() }, null, 2),
                success: function (json) {
                    var obj = json.d;   
                    window.open("../TPM/StreamDownload.aspx?u=http://<%= Request.Url.Authority +Request.ApplicationPath  %>/" + obj, "_self");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                   
                    var obj = $.parseJSON(xhr.responseText);
                    alert(obj.Message);

                }
            });

        });
        var loadedfile = null;
        $("#filename").fileReader();
        $("#filename").on('change', function (evt) {
            /*var ext = $("input#filename").val().split(".").pop().toLowerCase();
            if ($.inArray(ext, ["csv"]) == -1) {
                alert('Upload CSV only');
                return false;
            }*/
            //alert("afdasdas");

            if (evt.target.files != undefined) {
                loadedfile = evt;
                //console.log("set loaded file ");

            };

            return false;
        });
        $("#BtnUploadExecute").click(function () {
            $("#csvimporthint span").html("");
            if (loadedfile == null) { alert("Please Load a CSV file!"); return }
            var reader = new FileReader();

            reader.onload = function (loadedfile) {

                var csvval2 = CSV.parse(loadedfile.target.result);
                //console.log(csvval2.toString());

                var csvline = csvval2.length;
                var inputrad = "";
                if (csvline > 3) {
                    //get table field
                    //j is the head row
                    var tablename = csvval2[0][1];
                    //console.log(tablename);
                    if (tablename != $("#TableName").val()) {
                        alert("Wrong Table Template!");
                        return false;

                    };
                    var fieldnumber = csvval2[1][1];
                    var foreignnumber = csvval2[2][1];
                    var j = fieldnumber - foreignnumber + 3;
                    var field_type = [];
                    for (i = 0; i < fieldnumber - foreignnumber; i++) {
                        field_type[i] = csvval2[i + 3][1];
                    }
                    var field = csvval2[j];
                    j++;
                    var error = false;
                    next = 0;
                    function AjaxLoop() {
                       
                        $("#RowExecuted").text(RowExecuted);
                        $("#RowSucceeded").text(RowSucceeded);
                        $("#RowFailed").text(RowFailed);
                        $("#RowIgnored").text(RowIgnored);
                        var csvvalue = csvval2[j];
                        var option = parseInt(csvvalue[parseInt(fieldnumber) + 1]);
                        inputrad = "{\"data\":[";
                        inputrad += "{\"name\":\"0option\",\"value\":\"" + option + "\"}";
                        inputrad += ",{\"name\":\"0tablename\",\"value\":\"" + tablename + "\"}";
                        inputrad += ",{\"name\":\"0done_by\",\"value\":\"12312\"}";
                        inputrad += ",{\"name\":\"0fieldnumber\",\"value\":\"" + fieldnumber + "\"}";
                        inputrad += ",{\"name\":\"0foreignfield\",\"value\":\"" + foreignnumber + "\"}";
                        for (var i = 0; i < fieldnumber - foreignnumber; i++) {
                            var temp = csvvalue[i];
                            temp = temp.replace(/\"/g, "\\\"");
                            temp = ",{\"name\":\"" + field[i] + ":" + field_type[i] + "\",\"value\":\"" + temp + "\"}";
                            inputrad += temp;
                            //build the json string
                        }
                        inputrad += "]}"
                        RowExecuted++;
                        if (option > 0) {

                            $.ajax({
                                url: './Methodes/Bulk.asmx/Execute',
                                cache: false,
                                type: 'POST',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: inputrad,
                                success: function (json) {
                                    RowSucceeded++;
                                    var obj = json.d;
                                    //$("#csvimporthint").html(obj);
                                    j++;
                                    if (j < csvval2.length) { AjaxLoop(); }
                                    else { }
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    RowFailed +
                                    j++;
                                    var obj = $.parseJSON(xhr.responseText);
                                    $("#csvimporthint span").append("<p>Row Number : " + RowExecuted + " : " + obj.Message + "\r\n");
                                    if (j < csvval2.length) { AjaxLoop(); }
                                    else { }
                                }
                            });
                        }
                        else {
                            j++;
                            RowIgnored++;
                           
                            if (j < csvval2.length) { AjaxLoop(); }
                        }
                    }//end of dunction definision;

                    AjaxLoop();//execute it;
                }
            }

            RowExecuted = 0;
            RowFailed = 0;
            RowSucceeded = 0;
            RowIgnored = 0;
            $("#RowExecuted").text(RowExecuted);
            $("#RowSucceeded").text(RowSucceeded);
            $("#RowFailed").text(RowFailed);
            $("#RowIgnored").text(RowIgnored);
            reader.readAsText(loadedfile.target.files.item(0));

            //console.log(loadedfile.target.files.item(0));
        });
    </script>

</asp:Content>
