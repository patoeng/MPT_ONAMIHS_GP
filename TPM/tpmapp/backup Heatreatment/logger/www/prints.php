<?php
include "link.php";
include "explode.php";
$batchid=$_GET["batchid"];
$ask ="SELECT Ovenid,startdate,enddate,status FROM heatingbatch WHERE batchid='".$batchid."'";
$cur = odbc_exec($link,$ask);
if (odbc_fetch_row($cur)){
	$ovenid=strtoupper(odbc_result( $cur, 1 ));
	$startdate=format_date(odbc_result( $cur, 2));
	$enddate=format_date(odbc_result( $cur, 3 ));
	$status=odbc_result( $cur, 4 );
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
<body bgcolor="black">
<table border='0' width='1280px'  bgcolor="white" style='table-layout:fixed'>
		<col width=2>
		<col width=2>
		<col width=80>
		<col width=10>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td><img src="logo.jpg" align="left"></img></td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td><h1>POST MOLD CURE <?php echo $ovenid ?> CHART</h1></td>
		<td></td>
	</tr>
	
	<tr>
		<td>&nbsp;</td>
		<td><font color="blue" face="arial" size="1.5" align='right' valign=bottom><div id="verticaltext" class="box_rotate">Temperature&nbsp;(&#186;C)</div></td>
		<td><font face="arial" size="2" color="blue"><!--checkbox--><div id="placeholder" style="width:1000px;height:600px;"></div></font></td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td align='center'><font color="blue" face="arial" size="1.5">Times (Minutes)</font></td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td>&nbsp;</td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td>The Chart of Batch <?php echo $batchid ?></td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td>Device :&nbsp;<?php echo $ovenid ?></td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td>Started&nbsp;:&nbsp;<?php echo $startdate;?></td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td>Ended&nbsp;:&nbsp;<?php echo $enddate;?></td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td>Running for&nbsp;:&nbsp;<span id="last"></span></td>
		<td></td>
	</tr>
	<tr height='50px'>
		<td>&nbsp;</td>
		<td></td>
		<td>&nbsp;</td>
		<td></td>
	</tr
</table>
<script type="text/javascript">

$(function () {



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
        $ask7 = "SELECT CAST(fetchtime as float)/60 'datax',Temperature as 'datay',Fetchtime FROM batchtemperature where (batchID='".$batchid."') and (fetchtime<'11450') ORDER BY Fetchtime ASC"; 
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
        /*"TGT": {
			color : "orange",
           label: "Target",
            data: [[0,null],[null,0],[300,null],[0,40],[20,80],[50,80],[80,135],[170,135],[190,90]]
        },*/
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
$("#last").text("<?php echo $last;?> minutes");    
    $.each(datasets, function(key, val) {
	   
        if (key && datasets[key])
                data.push(datasets[key]);

    });
var plot = $.plot($("#placeholder"), data, options);

 // initiate a recurring data update
});
</script>

</body>
</html>