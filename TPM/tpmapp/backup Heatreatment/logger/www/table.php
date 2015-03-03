<?php
include "link.php";
include "explode.php";
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
$ask="SELECT TOP 1 OVENID from OvenDB where OVENID > '".$ovenid."' and OVENID like '%HEATER%' order by OVENID ASC ";
$cur = odbc_exec($link,$ask);
if (odbc_num_rows($cur)<=0){
$ask="SELECT TOP 1 OVENID from OVENDB order by OVENID ASC ";
$cur = odbc_exec($link,$ask);
}
$nextoven = odbc_result ($cur,1);

odbc_close($link);
?>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<meta http-equiv="PRAGMA" CONTENT="NO-CACHE">
	<meta http-equiv="expires" CONTENT="-1">
    <title><?php echo $ovenid." Batch ".$batchid;?></title>
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
<?php 
include "body.php";
?>
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
        yaxis: { min: 0, max: 200, tickSize:10 },
        xaxis: { show: true, tickDecimals: 0, tickSize: 10  },
        points: { show: false},
        grid: { hoverable: true, clickable: true, backgroundColor :'rgb(245,245,245)' }
    };
    var data = [[0,null],[null,0],[null,200],[300,null],[0,0]];




//var data = [[0,null],[null,0],[null,200],[300,null],[0,0]];
    var placeholder = $("#placeholder");
    
    $.plot(placeholder, data, options);

    
    
    // fetch one series, adding to what we got
    var alreadyFetched = {};
   
  
function showlimit(){

var datasets = {
        "UL": {
		    color : "red",
            label: "Upper Limit",
            data: [[0,null],[null,0],[300,null],[0,160],[300,160]]
        },        
       // "TGT": {
	//		color : "orange",
        //    label: "Target",
       //     data: [[0,null],[null,0],[300,null],[0,40],[20,80],[50,80],[80,135],[170,135],[190,90]]
       // },
        "LL": {
			color : "blue",
            label: "Lower Limit",
            data: [[0,null],[null,0],[300,null],[0,150],[300,150]]
        }
    };
	data =[];
    $.each(datasets, function(key, val) {
	   
       //if ((!alreadyFetched[key])&&(iteration>0)) {
         //      if (iteration >1) { alreadyFetched[key] = true;}
                data.push(val);
           // }
 
    });         
};
	
	
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
				url: "getuser.php?q="+iteration+"&&batchid=<?php echo $batchid;?>"+"&&last="+lastsss+"&&ovenid=<?php echo urlencode($ovenid);?>",
                method: 'GET',
                dataType: 'json',
                success: onDataReceived
            });
		 $("#countup").text(iteration);
		 if (iteration>=30){iteration==2}
		 if ((iteration>=10)&&(auto=="1")){window.location.href="table.php?ovenid="+nextoven+"&z=<?php echo time();?>"+"&auto=1";}
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
				if (statusbaru=="FIN") {$("#lastlabel").text("End : <?php echo $enddate;?> in : " );}else{$("#lastlabel").text("Time elapsed :")}
				//
				}else{
					last=series.last;
				    //statusbaru="";
					statusbaru = series.status;
					if (statusbaru=='FIN'){ statusbarudis='FINISH';}
					if (statusbaru=='RUN'){ statusbarudis='RUNNING';}
					if (statusbaru=='READY'){ statusbarudis='READY';}
					
					$("#status").text(statusbarudis);
					if (last!=-2){
						$.merge(mbel,series.data);
						datass["data"] =mbel;
						datass["label"] = series.label;
						datass["color"] = series.color;
						showlimit();
						data.push(datass);
						lastsss =last;
						minute = series.minute;
						$("#last").text(minute +" mins");
						if (statusbaru=="FIN") {$("#lastlabel").text("End : <?php echo $enddate;?> in : " );}else{$("#lastlabel").text("Time elapsed :")}
						suhu = series.suhu;
						if (statusbaru=="RUN") {
							$("#suhu").text(suhu);
							document.getElementById("derajat").innerHTML="&nbsp;&#186;C";
							}else{$("#suhu").text("");}
						
					//datasets["batch"]=[data]; 
					}
				//if (statusbaru=="FIN") {$("#lastlabel").text("End : <?php echo $enddate;?> in :" );}else{$("#lastlabel").text("Time elapsed :")}
				//if (status=="FIN") {$("#suhu").text("");
				var plot1 = $.plot($("#placeholder"), data, options);
				if ((statusbaru !=status)&&(iteration>3)){window.location.href="table.php?ovenid=<?php echo urlencode($ovenid);?>&z=<?php echo time();?>"+"&auto="+auto}
				}
				setTimeout(fetchData, 10000);
                
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