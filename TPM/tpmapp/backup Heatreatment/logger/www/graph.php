<?php
include "link.php";
include "explode.php";
$mode=$_GET["mode"];
if ($mode=="his"){
$batchid=$_GET["batchid"];
$auto="0";
//$jjj ="a";
//get batch informations jika mode is 'his'
$ask="SELECT BatchId,Ovenid,startdate,enddate,status FROM heatingbatch where batchid='".$batchid."'";
$cur = odbc_exec($link,$ask);
if (odbc_fetch_row($cur)){
	$ovenid=odbc_result( $cur, 2 );
	$startdate=format_date(odbc_result( $cur, 3));
	$enddate=format_date(odbc_result( $cur, 4));
	$status=odbc_result( $cur, 5 );

}
}
else{
//jika mode selain 'his'
$ovenid=$_GET["ovenid"];
$auto=$_GET["auto"];
$ask ="SELECT TOP 1 BatchId,Ovenid,startdate,enddate,status FROM heatingbatch WHERE ovenid='".$ovenid."' ORDER BY startdate DESC";
//$ask ="SELECT TOP 1 BatchId,Ovenid,startdate,enddate,status FROM heatingbatch WHERE batchid='76' ORDER BY startdate DESC";
$cur = odbc_exec($link,$ask);
if (odbc_fetch_row($cur)){
	$batchid=odbc_result( $cur, 1 );
	$startdate=format_date(odbc_result( $cur, 3));
	$enddate=format_date(odbc_result( $cur, 4));
	$status=odbc_result( $cur, 5 );

}
//find next oven
$ask="SELECT TOP 1 OVENID from OvenDB where OVENID > '".$ovenid."' and OVENID like '%OVEN#%' order by OVENID ASC ";
$cur = odbc_exec($link,$ask);
if (odbc_num_rows($cur)<=0){
$ask="SELECT TOP 1 OVENID from OVENDB where OVENID like '%OVEN#%' order by OVENID ASC ";
$cur = odbc_exec($link,$ask);
}
$nextoven = odbc_result ($cur,1);
}
////
//$jjj ="B";
// lihat graphoven setting;
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
//$jjj =$limit;
if ($limit==0){
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
}//limit
odbc_close($link);
?>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<meta http-equiv="PRAGMA" CONTENT="NO-CACHE">
	<meta http-equiv="expires" CONTENT="-1">
    <title><?php echo $alias." Batch ".$batchid;?></title>
    <link href="./flot/examples/layout.css?<?php echo time();?>" rel="stylesheet" type="text/css">
    <!--[if lte IE 8]><script language="javascript" type="text/javascript" src="./flot/excanvas.min.js"></script><![endif]-->
    <script language="javascript" type="text/javascript" src="./flot/jquery.js?<?php echo time();?>"></script>
    <script language="javascript" type="text/javascript" src="./flot/jquery.flot.js?<?php echo time();?>"></script>
<style type="text/css">
.kepala{ bgcolor: rgb(70,178,238);}
td {overflow:hidden;}
table{border-style:none}

</style>
	
 </head>
<body>
<table border='0' width='100%' bgcolor=#FFFFFF style='table-layout:fixed' cellspacing='0'>
	<col width=8>
	<col width=2>
	<col width=20>
	<col width=30>
	<col width=30>
	<col width=30>
	<col width=30>
	<col width=20>
	<col width=10>
	
	
	<tr>
		<td  rowspan="2" colspan="3" align="center" bgcolor=#2DA5D2>&nbsp;<!--OvenID-->
			<font color=#FFFFFF face="arial" size="6"><?php echo $alias ?><span id="ovenid"></span></font>
		</td>
		
		<td rowspan="2" align="center" bgcolor="black"><!--STATUS-->
			<font color="red" face="arial" size="6" ><span id="status"></span></font>
		</td>
		<td colspan=2 bgcolor=#2DA5D2><font color=#FFFFFF face="arial" size="4" >Start:&nbsp;<?php echo $startdate." ";?></font></td>
		<td rowspan=2 colspan=2 align="center" bgcolor=#2DA5D2>&nbsp;<!--&nbsp;SUHU--> 
		    
			<font color=#FFFFFF face="arial" size="6"><span id="suhu"></span><span id="derajat"></span></font>
		</td>
		<td rowspan=2 bgcolor=#2DA5D2 align="center">&nbsp;<font color="silver" face="arial" size="3"><span id="countup"></span></font></td>
	</tr>
	    <td colspan=2 bgcolor=#2DA5D2><!--ENDED-->
			<font face="arial" size="4" color=#FFFFFF><span id="lastlabel"></span>&nbsp;</font>
			<font face="arial" size="4"color="black"><span id="last"></span></font>&nbsp;
		</td>
	<tr>
	   <td>&nbsp;</td>
	   <td align="center"  valign="center" bgcolor="white"><!--labely--><font color="blue" face="sans-serif" size="1.5"><div id="verticaltext" class="box_rotate">Temperature&nbsp;(&#186;C)</div></font></td>
	   <td colspan=6 align="center" bgcolor="white">
			<font face="verdana" size="2px" color="blue"><!--checkbox-->
			<p id="choices" align="right">
			<input id="enableTooltip" type="checkbox" checked="true">Tooltip</p>
			</font><!-- chart--->
			<font face="times" size="3" color="blue"><!--checkbox-->
			<br><div id="placeholder" style="width:1000px;height:600px;"></div>
			</font>
	   </td>
	   <td >&nbsp;<!--checkbox-->
			
			
	   </td>
	</tr >
	<tr>
	   <td>&nbsp;</td>
	   <td  bgcolor="white">&nbsp;</td>
	   <td colspan=6 align="center"  bgcolor="white"><font color="blue" face="arial" size="1.5">Times (Minutes)</font></td>
	   <td>&nbsp;</td>
	</tr>
	<tr>
	   <td colspan=4 align="left">
			<font color="black" face="arial" size="1px" >
				<div id="modelholder1">
				<?php 
				  include "getmodel.php";
				  echo $anoman[1];
				  ?></div>
			</font>		
	   </td>
	   <td colspan=2 align="left"><!--model1-->
			<font color="black" face="arial"size="1px">
				<div id="modelholder2">
				<?php 
				  echo $anoman[2];
				  ?>
				</div>
			</font>
	   </td>
	    <!--- <td>&nbsp;</td>		 -->
	   <td colspan=3 align="left"><!--model2-->
			<font color="black" face="arial" size="1px">
			<div id="modelholder3"> 
			
				<?php 
				  echo $anoman[3];
				?>
			
			
			</div>
			</font>
	   </td>
	   
	</tr>
</table>
<script type="text/javascript">

$(function () {
//var ovenarray = <?php echo $ovenarray;?>;
//var jumlahoven =<?php echo $i;?>; 
var nextoven = "<?php echo urlencode($nextoven);?>";
var data=[];
//$("#status").text(<?php echo "\"".$status."\""; ?>); 
	var options = {
        lines: { show: true }, 
		series: { shadowSize: 0 }, // drawing is faster without shadows
        yaxis: { min: 0, max: <?php echo $max_y;?>, tickSize:<?php echo $ticksize;?> },
        xaxis: { show: true, tickDecimals: 0, tickSize: <?php echo $ticksize;?>  },
        points: { show: false},
        grid: { hoverable: true, clickable: true, backgroundColor :'#<?php echo $background_color;?>' }
    };
    var data = [[0,null],[null,0],[null,<?php echo $max_y;?>],[<?php echo $max_x;?>,null],[0,0]];




//var data = [[0,null],[null,0],[null,200],[300,null],[0,0]];
    var placeholder = $("#placeholder");
    
    $.plot(placeholder, data, options);

    
    
    // fetch one series, adding to what we got
    var alreadyFetched = {};
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
		?>		
        },        
       "TGT": {
		color : "#<?php echo $target_color;?>",
        label: "Target",
		<?php
		if ($limit<=0){
		  $datas ="data:[[0,null],[null,0],[$max_x,null],[0,$start_level]";
		  $temp =0;
		  for ($i=1;$i<=$stepnumber;$i++){
		        $temp +=$pattern[$i]["step"];
				$datas .= ",[".$temp.",".$pattern[$i]["level"]."]";
				}
		  $datas .= "]";
		  echo $datas;
		  }else{
        echo "data: [[0,null],[null,0],[$max_x,null],[0,$limit],[$max_x,$limit]]";
		}
		?>
         },
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
var choiceContainer = $("#choices");
    $.each(datasets, function(key, val) {
	    if (val.label=="Target"){
        choiceContainer.append('<input type="checkbox" name="' + key +
                               '"  id="id' + key + '">' +
                               '<label for="id' + key + '">'
                                + val.label + '&nbsp;</label>');
		}else{
		choiceContainer.append('<input type="checkbox" name="' + key +
                               '" checked="True" id="id' + key + '">' +
                               '<label for="id' + key + '">'
                                + val.label + '&nbsp;</label>');
		}
    });	
  
function showlimit(){


	var datasetss=[];
	var data_temp=[];
	data.splice(0,data.length-1);
	data_temp=data;
	data=[];
	choiceContainer.find("input:checked").each(function () {
            var key = $(this).attr("name");
            if (key && datasets[key])
                datasetss.push(datasets[key]);
        });
	$.each(datasetss, function(key, val) {
                data.push(val);
    });
	$.each(data_temp, function(key, val) {
	           // val.color = "<?php echo $graph_color;?>";
                data.push(val);
    });
	plot = $.plot($("#placeholder"), data, options);
};

choiceContainer.find("input").click(showlimit);	
    // initiate a recurring data update
//    $("input.dataUpdate").click(function () {
        // reset data
        datass = [[],[]];
		mbel =[];
        alreadyFetched = {};
        
       // $.plot(placeholder, data, options);

        var iteration = 0;
//		var stepper = 100;
        var status= "<?php echo $status; ?>";
		var auto= "<?php echo $auto; ?>";
        var last=0;
		var lastsss=0;
		var statusbaru=status;
		var suhu="";
		var mbek=[];
		var series =[];
		var minute ="";
		var statusbarudis="";
		var batchid= "<?php echo $batchid; ?>";
		var batchid_old = batchid;
		//$("#last").text(lastsss); 
function fetchData() {
		 ++iteration;
		 // $.plot($("#placeholder"), data, options);
			//stepper = 99+iteration;
            $.ajax({
                // usually, we'll just call the same URL, a script
                // connected to a database, but in this case we only
                // have static example files so we need to modify the
                // URL
                //url: "data-eu-gdp-growth-" + iteration + ".json",
//				url: "getuser_1.php?q="+iteration,
				
				//url: "getuser.php?q="+iteration+"&&batchid=<?php echo $batchid;?>",
				<?php
				if ($limit<=0){
				  if ($mode=='his') {
				  echo "url: \"getuser.php?q=\"+iteration+\"&&batchid=".$batchid."&&last=\"+lastsss+\"&&lim=$temp\",";
				 
				  }
				  else{
				  echo "url: \"getuser.php?q=\"+iteration+\"&&batchid=".$batchid."&&last=\"+lastsss+\"&&ovenid=".urlencode($ovenid)."&&lim=$temp\",";
				  }
				  }else{
				  if ($mode=='his') {
				  echo "url: \"getuser.php?q=\"+iteration+\"&&batchid=".$batchid."&&last=\"+lastsss,";
				 
				  }
				  else{
				  echo "url: \"getuser.php?q=\"+iteration+\"&&batchid=".$batchid."&&last=\"+lastsss+\"&&ovenid=".urlencode($ovenid)."\",";
				  }
				  }
				?>
				//url: "getuser.php?q="+iteration+"&&batchid=<?php echo $batchid;?>"+"&&last="+lastsss+"&&ovenid=<?php echo urlencode($ovenid);?>",
                method: 'GET',
                dataType: 'json',
                success: onDataReceived
            });
		 $("#countup").text(iteration);
		 if (iteration>=30){iteration=2}
		 if ((iteration>=10)&&(auto=="1")){window.location.href="./graph.php?mode=real&ovenid="+nextoven+"&z=<?php echo time();?>"+"&auto=1";}
			delete series;
			series=null;
			//var series=[];
            function onDataReceived(series) {
                // we get all the data in one go, if we only got partial
                // dhata, we could merge it with what we already got
              //var plot = $.plot($("#placeholder"), data, options);
			    if (iteration==1){
					last=series.last;
					mbek = series;
					//statusbaru="";
					statusbaru = mbek["status"];
					if (statusbaru=='FIN'){ statusbarudis='FINISH';}
					if (statusbaru=='RUN'){ statusbarudis='RUNNING';}
					if (statusbaru=='READY'){ statusbarudis='READY';}
					$("#status").text(statusbarudis);
					suhu = series.suhu;
					batchid= series.batchid;
					if (last!=-2){
						lastsss =last;
						minute = series.minute;
						$("#last").text(minute +" mins");
						if (statusbaru=="RUN") {
							$("#suhu").text(suhu);
							document.getElementById("derajat").innerHTML="&nbsp;&#186;C";
							}else{$("#suhu").text("");document.getElementById("derajat").innerHTML="";}
						data=[];
						showlimit();
						mbel=series.data;
						data.push(series);
						var plot = $.plot($("#placeholder"), data, options);
					//datasets['batch']=[data];
				}
				if (statusbaru=="FIN") {$("#lastlabel").text("End  : <?php echo $enddate;?> in : " );}else{$("#lastlabel").text("Time elapsed :")}
				//
				}else{
					last=series.last;
					batchid= series.batchid;
				    //statusbaru="";
					statusbaru = series.status;
					if (statusbaru=='FIN'){ statusbarudis='FINISH';}
					if (statusbaru=='RUN'){ statusbarudis='RUNNING';}
					if (statusbaru=='READY'){ statusbarudis='READY';}
					showlimit();
					$("#status").text(statusbarudis);
					if (last!=-2){
						$.merge(mbel,series.data);
						datass["data"] =mbel;
						datass["label"] = series.label;
						datass["color"] = series.color;
						data.splice(data.length-1,1);
						data.push(datass);
						lastsss =last;
						minute = series.minute;
						$("#last").text(minute +" mins");
						if (statusbaru=="FIN") {$("#lastlabel").text("End  : <?php echo $enddate;?> in : " );}else{$("#lastlabel").text("Time elapsed :")}
						suhu = series.suhu;
						if (statusbaru=="RUN") {
							$("#suhu").text(suhu);
							document.getElementById("derajat").innerHTML="&nbsp;&#186;C";
							}else{$("#suhu").text("");}
						
					//datasets["batch"]=[data]; 
					}
				//if (statusbaru=="FIN") {$("#lastlabel").text("End : <?php echo $enddate;?> in :" );}else{$("#lastlabel").text("Time elapsed :")}
				//if (status=="FIN") {$("#suhu").text("");
				   plot = $.plot($("#placeholder"), data, options);
				if (((statusbaru !=status)||(batchid!=batchid_old))&&(iteration>3)){window.location.href="./graph.php?mode=real&ovenid=<?php echo urlencode($ovenid);?>&z=<?php echo time();?>"+"&auto="+auto}
				}
				setTimeout(fetchData, 3000);
                
            }
			
            
         //   if (iteration < 8)
		       // showlimit();
                
         //   else {
                //data = [];
                //alreadyFetched = {};
          //  }
        }

        
//////


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

    var previousPoint = null;
    $("#placeholder").bind("plothover", function (event, pos, item) {
        $("#x").text(pos.x.toFixed(0) +" minutes = ");
        $("#y").text(pos.y.toFixed(2)+" Celcius");

        if ($("#enableTooltip:checked").length > 0) {
            if (item) {
                if (previousPoint != item.dataIndex) {
                    previousPoint = item.dataIndex;
                    
                    $("#tooltip").remove();
                    var x = item.datapoint[0].toFixed(0),
                        y = item.datapoint[1].toFixed(2);
                    
                    showTooltip(item.pageX, item.pageY,
                                item.series.label + " of " + x + " minutes = " + y +" Celcius");
                }
            }
            else {
                $("#tooltip").remove();
                previousPoint = null;            
            }
        }
    });
//////
	//var i = 0;
    //$.each(datasets, function(key, val) {
    //    val.color = i;
    //    ++i;
    //});
    
    // insert checkboxes 
    
//
//-----
fetchData();
//setTimeout(fetchData, 1000);
//////
});
</script>

</body>
</html>