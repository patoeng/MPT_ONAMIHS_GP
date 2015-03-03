<?php
include "link.php";
include "explode.php";
$batchid=$_GET["batchid"];
$ask ="SELECT Ovenid,startdate,enddate,status FROM heatingbatch WHERE batchid='".$batchid."'";
$cur = odbc_exec($link,$ask);
if (odbc_fetch_row($cur)){
	$ovenid=odbc_result( $cur, 1 );
	$startdate=format_date(odbc_result( $cur, 2));
	$enddate=format_date(odbc_result( $cur, 3 ));
	$status=odbc_result( $cur, 4 );
	$ww=$status;
		$classs="other";
		if ($ww=='FIN'){$classs="FINISH";};
		if ($ww=='RUN'){$classs="RUNNING";};
		if ($ww=='READY'){$classs="READY";};
}

odbc_close($link);
?>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title><?php echo $ovenid." Batch ".$batchid;?></title>
    <link href="./flot/examples/layout.css" rel="stylesheet" type="text/css">
    <!--[if lte IE 8]><script language="javascript" type="text/javascript" src="./flot/excanvas.min.js"></script><![endif]-->
    <script language="javascript" type="text/javascript" src="./flot/jquery.js"></script>
    <script language="javascript" type="text/javascript" src="./flot/jquery.flot.js"></script>
 </head>
<body >
<?php
include "body.php";
?>
<script type="text/javascript">

$(function () {
$("#status").text("<?php echo $classs;?>");
$("#lastlabel").text("End: <?php echo $enddate;?> in : ");

 var options = {
        lines: { show: true }, 
        series: { shadowSize: 0 }, // drawing is faster without shadows
        yaxis: { min: 0, max: 200, tickSize: 10 },
        xaxis: { show: true, tickDecimals: 0, tickSize: 10  },
        points: { show: false },
        grid: { hoverable: true, clickable: true, backgroundColor :"rgb(245,245,245)" }
    };
    var data = [[0,null],[null,0],[null,200],[300,null],[0,0]];

var datasets = {
<?php
include "link.php";
        $ask7 = "SELECT cast (fetchtime as float)/60 as 'datax',Temperature as 'datay',Fetchtime FROM batchtemperature where (batchID='".$batchid."')and (fetchtime<'11450')  ORDER BY Fetchtime ASC"; 
		$cur = odbc_exec($link,$ask7);
$last="0";		
$s="[300,null]";
echo "\"BATCH\":{\n";
echo "\"label\": \"Batch ".$batchid."\",\n";
echo "\"color\": \"green\",\n";




echo "\"data\": [";
while( odbc_fetch_row( $cur )) 
  {
  if ($s!='') {$s=$s.",";}
  $row=odbc_result( $cur, 1 );
  if ($row=='')
  {
    $s=$s . "[ null";
  }else{
  $s= $s . "[ " . $row;
  }
  $row=odbc_result( $cur, 2 );
  if ($row=='')
  {
    $s=$s . ", null] ";
  }else{
  $s= $s . ", " . $row . "] ";
  } 
  $last=odbc_result( $cur, 1 );
  }
  $last=intval($last); 
 echo $s;
echo "]\n},";
odbc_close( $link); 
?>
	"UL": {
		    color : "red",
            label: "Upper Limit",
            data: [[0,null],[null,0],[300,null],[0,160],[300,160]]
        },        
        "TGT": {
			color : "orange",
           label: "Target",
            data: [[0,null],[null,0],[300,null],[0,155],[300,155]]
        },
        "LL": {
			color : "blue",
            label: "Lower Limit",
            data: [[0,null],[null,0],[300,null],[0,150],[300,150]]
        }
	};	
    
	//var i = 0;
    //$.each(datasets, function(key, val) {
    //    val.color = i;
    //    ++i;
    //});
    
    // insert checkboxes 
    var choiceContainer = $("#choices");
    $.each(datasets, function(key, val) {
	   
        choiceContainer.append('<input type="checkbox" name="' + key +
                               '" checked="checked" id="id' + key + '">' +
                               '<label for="id' + key + '">'
                                + val.label + '&nbsp;</label>');
    });
    choiceContainer.find("input").click(plotAccordingToChoices);

    
    function plotAccordingToChoices() {
        var data = [];

        choiceContainer.find("input:checked").each(function () {
            var key = $(this).attr("name");
            if (key && datasets[key])
                data.push(datasets[key]);
        });

        if (data.length > 0)
            $.plot($("#placeholder"), data,options);
    }

    plotAccordingToChoices();
//
//-----
function showTooltip(x, y, contents) {
        $('<div id="tooltip">' + contents + '</div>').css( {
            position: 'absolute',
            display: 'none',
            top: y + 5,
            left: x + 5,
            border: '1px solid #fdd',
            padding: '2px',
            'background-color': '#fee',
            opacity: 0.80
        }).appendTo("body").fadeIn(200);
    }
   
 $("#last").text("<?php echo $last;?> mins");   
    

$("#placeholder").bind("plothover", function (event, pos, item) {
        $("#x").text(pos.x.toFixed(0)+" minutes = ");
        $("#y").text(pos.y.toFixed(2)+" Celcius");

        if ($("#enableTooltip:checked").length > 0) {
            if (item) {
                if (previousPoint != item.dataIndex) {
                    previousPoint = item.dataIndex;
                    
                    $("#tooltip").remove();
                    var x = item.datapoint[0].toFixed(0),
                        y = item.datapoint[1].toFixed(2);
                    
                    showTooltip(item.pageX, item.pageY,
                                item.series.label + " of " + x + " minutes = " + y +" C");
                }
            }
            else {
                $("#tooltip").remove();
                previousPoint = null;            
            }
        }
    });


 // initiate a recurring data update
});
</script>

</body>
</html>