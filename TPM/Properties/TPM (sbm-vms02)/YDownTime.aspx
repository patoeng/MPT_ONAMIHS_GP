<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YDownTime.aspx.cs" Inherits="TPM.YDownTime" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
        <div class="span12" >
         <h3>Asset Down Time</h3>
        </div>
    </div>
    <form runat="server" id="form1" name="form1">
    <div class="row-fluid">
        <div class="span3" >
            <label for="startdate">From
                <input type="text" id="startdate" class="datepicker required"/>
            </label>
        </div>
        <div class="span3">
             <label for="enddate">To
                <input type="text" id="enddate" class="datepicker required" />
            </label>
        </div>
         <div class="span3">
             <label for="ddlAsset">Asset
                <asp:DropDownList ID="ddlAsset" ClientIDMode="Static" runat="server" CssClass="required"></asp:DropDownList>
            </label>
        </div>
    </div>
    <div class="row-fluid">
        <div class="span12" >
            <hr />
            <div id="graphplaceholder" style="width:100%;height:450px;" ></div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="span12" >
            <div id="tableContainer"></div>
        </div>
    </div>
    
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <!--[if IE]> <script type="text/javascript" src="Scripts/flot/excanvas.min.js"></script><![endif]-->
<script type="text/javascript" src="Scripts/flot/jquery.flot.js"></script>
<script type="text/javascript" src="Scripts/flot/jquery.flot.axislabels.js"></script>
<script  type="text/javascript" src="Scripts/flot/jquery.flot.resize.js"></script>
<script  type="text/javascript" src="Scripts/flot/jquery.flot.stack.js"></script>
    <script type="text/javascript">
        var btnclicked = "";
        $('#startdate').Zebra_DatePicker({
            format: 'd-M-Y',
            direction: false,
            inside: true,
            pair :$('#enddate'),
            onSelect: function (date) {
                $('#form1').trigger('submit');
            }
        });
        $('#enddate').Zebra_DatePicker({
            format: 'd-M-Y',
            direction: 0,
            inside: true,
            onSelect: function (date) {
                $('#form1').trigger('submit');
            }
        });
        $('#ddlAsset').change(function () {
            btnclicked = "showtable";
            $('#form1').trigger('submit');
        });
        $('.btn').click(function () {
            btnclicked = $(this).attr('id');
            $('#form1').trigger('submit');
            
        });
        $('#form1').validate({
            submitHandler: function () {
                var id = "bom_1";
                $.ajax({
                    url: './Methodes/downtime.asmx/'+btnclicked,
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ asset_id: $('#ddlAsset option:selected').val(), startdate: $('#startdate').val(), enddate: $('#enddate').val() }, null, 2),
                    success: function (json) {
                        var obj = $.parseJSON(json.d);
                        $('#tableContainer').html("<hr/>");
                        $('#tableContainer').append(obj.htmltable);
                       
                        $('#jsonTable').dataTable({
                            "sPaginationType": "full_numbers"
                        });

                        //plot the flot
                        options.xaxis.ticks = obj.flotchart.ticks;
                        total = obj.flotchart.total;
                        var p = $.plot($('#graphplaceholder'), obj.flotchart.chart, options);
                        $.each(p.getData()[0].data, function (i, el) {
                            var o = p.pointOffset({ x: el[0], y: el[1] });
                            var totals = p.getData()[1].data[i][1] + el[1];
                            $('<div class="data-point-label" style="font-size:0.7em;align:center;">' + totals + '<br/>(' + (totals * 100 / total).toFixed(2) + '%)</div>').css({
                                position: 'absolute',
                                left: o.left - 25,
                                top: o.top - 40,
                                display: 'none'
                            }).appendTo(p.getPlaceholder()).fadeIn('slow');
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        var obj = $.parseJSON(xhr.responseText);
                        alert("Exception Type:\r\n" + obj.ExceptionType + "\r\nMessage:\r\n" + obj.Message + "\r\n" +
                              "Stack Trace : \r\n" + obj.StackTrace
                              );
                    }
                });
            }
        });
        ///
        var total = 700;
        var options = {
            series: {
                stack: true,
                lines: { show: false, fill: true, steps: false },
                bars: { show: true, barWidth: 0.5, align: 'center' }

            },
            xaxis: {
                ticks: [[1, 'One'], [2, 'Two'], [3, 'Three'], [4, 'Four'], [5, 'Five']],
                axisLabel: 'Asset Name',
                axisLabelUseCanvas: true,
                labelAngle: 60,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
                axisLabelPadding: 5
            },
            yaxis: {
                axisLabel: 'Minutes',
                axisLabelUseCanvas: true,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
                axisLabelPadding: 5,
                autoscaleMargin: 0.2 // value from highest data +
            },

            grid: { hoverable: true }
        };
 
    </script>
</asp:Content>
