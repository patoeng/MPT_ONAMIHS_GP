<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tunnel.aspx.cs" Inherits="TPM.Tunnel" %>
<%@ Import Namespace="TPM.Classes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h3 class="btn btn-primary">download</h3>
    <% Response.Write(Session["tpm_employeeno"] != null ? Session["tpm_employeeno"].ToString() : "koso"); %>
    <script type="text/javascript">
        $('.btn').click(function(){

            $('#original-data').html('Processing data ...');

            // DATA TAKEN FROM http://192.168.1.2/index/vxhr/information/get-stock-api/format/html

            $.post('http://192.168.1.2/index/vxhr/information/get-stock-api/format/html', function (jsn) {

                $('#original-data').html(jsn).hide();

                $('#control').html('<span class="label label-info" id="to-view">'

                                + 'View/Close Original Data</span>');

                $('#to-view').click(function(){

                    $('#original-data').toggle();

                });

                //CONVERT DATA 

                var o = $.parseJSON(jsn);

                // CHECK WETHER ANY DATA TO PROCESS

                if(o.length > 0){

                    // INITIALIZE TABLE

                    $('#formatting-data')

                        .html('<table class="table table-bordered table-striped table-condensed">'

                            + '<thead><tr><th>No</th><th>Name</th><th>Code</th><th>Category</th>'

                            + '<th>Current</th><th>Min</th><th>Max</th><th>Deficit</th></thead>'

                            + '<tbody></tbody></table>');

                    // LOOPING DATA as ROW OF TABLE

                    $.each(o, function(i, v){

                        $('#formatting-data tbody').append('<tr><th>' + (i + 1) + '</td>'

                            + '<th>' + v.name + '</td><th>' + v.code + '</td>'

                            + '<th>' + v.pname + '</td><th>' + v.stock + '</td>'

                            + '<th>' + v.qtymin + '</td><th>' + v.qtymax + '</td>'

                            + '<th>' + v.def + '</td></tr>');

                    });

                }

            });

        });
        

    </script>
</body>
</html>
