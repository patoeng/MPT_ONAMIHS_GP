<?php
$batchid=$_GET["batchid"];
$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}
$ask ="SELECT Ovenid,startdate,enddate FROM heatingbatch WHERE batchid='".$batchid."'";
$cur = odbc_exec($link,$ask);
if (odbc_fetch_row($cur)){
	$ovenid=odbc_result( $cur, 1 );
	$startdate=odbc_result( $cur, 2);
	$enddate=odbc_result( $cur, 3 );
}
odbc_close( $link);
echo "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n";
echo "<html>\n";
echo "<head>\n";
echo "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n";
echo "<title>".$ovenid." Batch ".$batchid."</title>\n";
echo "<link href=\"layout.css\" rel=\"stylesheet\" type=\"text/css\">\n";
echo "<!--[if lte IE 8]><script language=\"javascript\" type=\"text/javascript\" src=\"../excanvas.min.js\"></script><![endif]-->\n";
echo "<script language=\"javascript\" type=\"text/javascript\" src=\"../jquery.js\"></script>\n";
echo "<script language=\"javascript\" type=\"text/javascript\" src=\"../jquery.flot.js\"></script>\n";
echo "</head>\n";
echo "<body>\n";
echo "<h1>".$ovenid."</h1>\n";
echo "<h3>Started : ".$startdate."</h3>\n";
echo "<h3>Ended : ".$enddate."</h3>\n";
echo "\n";
echo "<div id=\"placeholder\" style=\"width:1000px;height:600px;\"></div>\n";
echo "\n";

echo "<p id=\"choices\">Show:</p>";
echo "<h2>Model Loaded in this Oven Batch</h2>\n";
echo "<div id=\"modelholder\">\n";
$askjoin = " SELECT batchmodel.model, model.description, batchmodel.qty AS Quantity \n
             FROM batchmodel \n
			 INNER JOIN model \n
			 ON model.modelid = batchmodel.model \n
			 WHERE batchmodel.batchid='".$batchid."' \n
			 ORDER BY batchmodel.model";
$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}			 
$cur = odbc_exec($link,$askjoin);	
$Fields = odbc_num_fields($cur); 
print "<table border='1' width='100%'><tr>"; 
// Build Column Headers 
    for ($i=1; $i <= $Fields; $i++){ 
    printf("<th bgcolor='silver'>%s</th>", odbc_field_name( $cur,$i)); 
    }    
// Table Body 
    $Outer=0; 
    while( odbc_fetch_row( $cur )){ 
    $Outer++; 
    print "<tr>"; 
    for($i=1; $i <= $Fields; $i++){ 
        printf("<td>%s</td>", odbc_result( $cur, $i )); 
        } 
    print "</tr>"; 
    } 
print "</table>"; 

echo "</div>\n";
echo "\n";
echo "  <p>";

?>    
    

<script type="text/javascript">
$(function () {
 var options = {
        lines: { show: true }, 
        series: { shadowSize: 0 }, // drawing is faster without shadows
        yaxis: { min: 0, max: 200 },
        xaxis: { show: true, tickDecimals: 0, tickSize: 10  },
        points: { show: false }
                

    };
    var data = [[0,null],[null,0],[null,200],[300,null],[0,0]];

var datasets = {
<?php
		$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
		if (!$link) {
			die('Something went wrong while connecting to MSSQL');
		}
        $ask7 = "SELECT fetchtime/60 as 'datax',Temperature as 'datay' FROM batchtemperature where (batchID='".$batchid."')  ORDER BY Fetchtime ASC"; 
		$cur = odbc_exec($link,$ask7); 
$s="[300,null]";
echo "\"BATCH\":{\n";
echo "\"label\": \"Batch ".$batchid."\",\n";
echo "\"color\": \"red\",\n";




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

  }
   
 echo $s;
echo "]\n},";
odbc_close( $link); 
?>

        "UL": {
		    color : "green",
            label: "Upper Limit",
            data: [[0,null],[null,0],[300,null],[20,90],[40,90],[70,145],[160,145],[180,100]]
        },        
        "TGT": {
			color : "yellow",
            label: "Target",
            data: [[0,null],[null,0],[300,null],[20,80],[40,80],[70,135],[160,135],[180,90]]
        },
        "LL": {
			color : "green",
            label: "Lower Limit",
            data: [[0,null],[null,0],[300,null],[20,70],[40,70],[70,125],[160,125],[180,80]]
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
	   
        choiceContainer.append('<br/><input type="checkbox" name="' + key +
                               '" checked="checked" id="id' + key + '">' +
                               '<label for="id' + key + '">'
                                + val.label + '</label>');
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

   
    
    


    // initiate a recurring data update
});
</script>

 </body>
</html>
	