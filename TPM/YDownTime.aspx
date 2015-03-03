<%@ Page Title="" Language="C#" MasterPageFile="~/TPM.Master" AutoEventWireup="true" CodeBehind="YDownTime.aspx.cs" Inherits="TPM.YDownTime" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentHolder" runat="server">
    <div class="row-fluid">
        <div class="span12" >
         <h3>SUMMARY WORK ORDER BY DEPARTMENT</h3>
        </div>
    </div>
    <form runat="server" id="form1" name="form1">
    <div class="row-fluid">
        <div class="span4">
             <label for="ddlYear">Year
                <asp:DropDownList ID="ddlYear" ClientIDMode="Static" runat="server" CssClass="required ddl"></asp:DropDownList>
            </label>
        </div>
        <div class="span4">
             <label for="ddlMonth">Month
                <asp:DropDownList ID="ddlMonth" ClientIDMode="Static" runat="server" CssClass="required  ddl"></asp:DropDownList>
            </label>
        </div>
        
        <div class="span4">
             <label for="ddlDepartment">Department
                <asp:DropDownList ID="ddlDepartment" ClientIDMode="Static" runat="server" CssClass="required  ddl"></asp:DropDownList>
            </label>
        </div>
    </div>
     <div class="row-fluid">
 
        <div class="span8" >
           
            <div id="graphplaceholder" style="width:100%;height:450px;" ></div>
        </div>
         <div class="span4" >
            <div id="tableContainer"></div>
        </div>
    </div>
    <div class="row-fluid">
       
    </div>
    
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScriptsHolder" runat="server">
    <!--[if IE]> <script type="text/javascript" src="Scripts/flot/excanvas.min.js"></script><![endif]-->
<script type="text/javascript" src="Scripts/flot/jquery.flot.js"></script>
<script type="text/javascript" src="Scripts/flot/jquery.flot.axislabels.js"></script>
<script  type="text/javascript" src="Scripts/flot/jquery.flot.resize.js"></script>
<script  type="text/javascript" src="Scripts/flot/jquery.flot.stack.js"></script>
<script  type="text/javascript" src="Scripts/flot/jquery.flot.barnumbers.js"></script>
<%-- ReSharper disable UnusedLocals --%>
    <script type="text/javascript">

        $('.ddl').change(function () {
           
            $('#form1').trigger('submit');
            
        });
        $('#form1').validate({
            submitHandler: function () {
                var id = 'bom_1';
                $.ajax({
                    url: './Methodes/downtime.asmx/Showtable',
                    cache: false,
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ dept_id: $('#ddlDepartment option:selected').val(), year: $('#ddlYear option:selected').val(), month: $('#ddlMonth option:selected').val() }, null, 2),
                    success: function (json) {
                        try {
                            var obj = $.parseJSON(json.d);
                            $('#tableContainer').html("<hr/>");
                            $('#tableContainer').append(obj.htmltable);

                            $('#jsonTable').dataTable({
                                'bPaginate':false,
								'bInfo':false,
								'bFilter': false
                            });

                            //plot the flot
                            options.xaxis.ticks = obj.flotchart.ticks;
                            total = obj.flotchart.total;
                            
                            var plot = $.plot($('#graphplaceholder'), obj.flotchart.chart, options);
                           // $.each(p.getData()[0].data, function(i, el) {
                              //  var o = p.pointOffset({ x: el[0], y: el[1] });
                               // var totals = p.getData()[1].data[i][1] + el[1];
                               // $('<div class="data-point-label" style="font-size:0.7em;align:center;">' + total + '<br/></div>').css({
                               //     position: 'absolute',
                               //     left: o.left - 25,
                               //     top: o.top - 40,
                               //     display: 'none'
                               // }).appendTo(p.getPlaceholder()).fadeIn('slow');
                            //});
                            var animate = function() {
                                $('#graphplaceholder').animate({ tabIndex: 0 }, {
                                    duration: 1000,
                                    step: function(now, fx) {

                                        var r = $.map(init.data, function(o) {
                                            return [[o[0], o[1] * fx.pos]];
                                        });

                                        plot.setData([{ data: r }]);
                                        plot.draw();

                                    }
                                });
                            };
                            animate();
                        } catch(ex) {
                            
                        }
                    },
                    error: function (xhr) {
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
                lines: { show: false, steps: false },
                bars: {
                    show: true, barWidth: 0.5, align: 'center', numbers: {
                        show: true,
                        xAlign: function (x) { return x; }
                    }
                },
                yaxis: { min: 0 },
                xaxis: { tickDecimals: 0 }
                

            },
            xaxis: {
                ticks: [[1, 'One'], [2, 'Two'], [3, 'Three'], [4, 'Four'], [5, 'Five']],
                axisLabel: 'Weeks',
                axisLabelUseCanvas: true,
                labelAngle: 60,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
                axisLabelPadding: 5,
                tickDecimals: 0
            },
            yaxis: {
                axisLabel: 'Count',
                axisLabelUseCanvas: true,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
                axisLabelPadding: 5,
                min: 0,
                autoscaleMargin: 0.2 // value from highest data +
              
            },

            grid: { hoverable: true }
        };
 
    </script>
<%-- ReSharper restore UnusedLocals --%>
</asp:Content>
