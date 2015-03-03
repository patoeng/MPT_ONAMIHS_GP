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
$askss="select alias,max_x,max_y,ticksize,limit,graph_color,upper_limit_color,lower_limit_color,target_color,background_color,limit_tolerance from OvenGraphSetting where ovenid='".$ovenid."'";
$currr= odbc_exec($link,$askss);
$alias= odbc_result($currr,1);
$max_x= odbc_result($currr,2);
$max_y= odbc_result($currr,3);
$ticksize= odbc_result($currr,4);
$limit= intval(odbc_result($currr,5));
$graph_color= odbc_result($currr,6);
$upper_limit_color= odbc_result($currr,7);
$lower_limit_color= odbc_result($currr,8);
$target_color= odbc_result($currr,9);
$background_color= odbc_result($currr,10);
$limit_tolerance= odbc_result($currr,11);

//$jjj ="C";
$askpattern = "Select * from batchpattern where batchid='".$batchid."'";
$kurap = odbc_exec($link,$askpattern);
if (odbc_num_rows($kurap)>0){
//$jjj="ada batch";
}else{
$askpattern ="select * from ovenpattern where ovenid='".$ovenid."'";
$kurap =odbc_exec($link,$askpattern);
//$jjj="nggak ada batch";
}

$stepnumber= odbc_result($kurap,"stepnumber");
$level_tolerancy = odbc_result($kurap,"level_tolerancy");
$start_level =odbc_result($kurap,"start_level");

for ($i=1;$i<=$stepnumber;$i++){
 $pattern[$i]["step"]= odbc_result($kurap,'step'.$i);
 $pattern[$i]["level"]= odbc_result($kurap,'level'.$i);
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
		<td><img src="logo.png" align="left"></img></td>
		<td></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td></td>
		<td><h1>BM HEATREATMENT <?php  if ($alias==''){ echo $ovenid;}else{echo $alias."'s";} ?> CHART</h1></td>
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
		<td>Machine :&nbsp;<?php if ($alias==''){ echo $ovenid;}else{echo $alias;} ?></td>
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

"UL": {
		    color : "#<?php echo $upper_limit_color;?>",
            label: "Upper Limit",
<?php
		  if ($limit<=0){
		  $datas ="data:[[0,null],[null,0],[$max_x,null],[0,$start_level+$level_tolerancy]";
		  $temp =0;
		  for ($i=1;$i<=$stepnumber;$i++){
		        $temp +=$pattern[$i]["step"];
				$datas .= ",[".$temp.",".($pattern[$i]["level"]+$level_tolerancy)."]";
				}
		  $datas .= "]";
		  echo $datas;
		  }else{
          echo  "data: [[0,null],[null,0],[$max_x,null],[0,$limit+$limit_tolerance],[$max_x,$limit+$limit_tolerance]]";
		  }
		if ($limit<=0)
		{
		  $ask7 = "SELECT CAST(fetchtime as float)/60 'datax',Temperature as 'datay',Fetchtime FROM batchtemperature where (batchID='".$batchid."') and (fetchtime<'".($temp*60)."') ORDER BY Fetchtime ASC"; 
		}else
		{
		  $ask7 = "SELECT CAST(fetchtime as float)/60 'datax',Temperature as 'datay',Fetchtime FROM batchtemperature where (batchID='".$batchid."')  ORDER BY Fetchtime ASC"; 
		}
       echo" },";        
include "link.php";
        
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
		
      
        "LL": {
			color : "#<?php echo $lower_limit_color;?>",
            label: "Lower Limit",
        <?php
		 if ($limit<=0){
		  $datas ="data:[[0,null],[null,0],[$max_x,null],[0,$start_level-$level_tolerancy]";
		  $temp =0;
		  for ($i=1;$i<=$stepnumber;$i++){
		        $temp +=$pattern[$i]["step"];
				$datas .= ",[".$temp.",".($pattern[$i]["level"]-$level_tolerancy)."]";
				}
		  $datas .= "]";
		  echo $datas;
		  }else{
		echo "data: [[0,null],[null,0],[$max_x,null],[0,$limit-$limit_tolerance],[$max_x,$limit-$limit_tolerance]]";
		}
		?>
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