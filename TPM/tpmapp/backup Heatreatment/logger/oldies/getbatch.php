<?php
include "explode.php";
include "link.php";
$q =$_GET["q"]." 00:00:00";
$r =$_GET["r"]." 23:59:59.999";
$o =$_GET["o"];
$f =$_GET["f"];
$f = $f * 60;
$c=0;
if ($o=='all') {
$askjoin = "SELECT Batchid as Batch,startdate as Started ,enddate as Ended,Ovenid FROM HeatingBatch WHERE (startdate>'".$q."') and (enddate<'".$r."') and status='FIN' ORDER BY batchid DESC";
}else{
$askjoin = "SELECT Batchid as Batch,startdate as Started ,enddate as Ended,Ovenid FROM HeatingBatch WHERE (ovenid='".$o."') and (startdate>'".$q."') and (enddate<'".$r."') and status='FIN' ORDER BY batchid DESC"; 
}



//$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}			 
$cur = odbc_exec($link,$askjoin);	
$Fields = odbc_num_fields($cur)-1; 
print "<table border='0' width='100%'  cellspacing='0'><font face='arial' size='7px'>
<col align=center><col align=center><col align=center><col align=center><col align=center><col align=center><col align=center>
<tr height=40px>"; 
// Build Column Headers 
    for ($i=1; $i <= $Fields; $i++){ 
    printf("<th bgcolor='silver'>%s</th>", odbc_field_name( $cur,$i)); 
    }    
	printf("<th bgcolor='silver'>DEVICE</th>");
	printf("<th bgcolor='silver'>CHART</th>");
	printf("<th bgcolor='silver'>DOWNLOAD</th>");
// Table Body 
    $Outer=0; 
    while( odbc_fetch_row( $cur )){
		
    $Outer++; 
	$j=odbc_result ($cur,1);
	
	$z=odbc_result ($cur,4);
	$askks= "Select alias from OvenGraphSetting where ovenid='".$z."'";
	$ss=odbc_exec($link,$askks);
	$jj=odbc_result($ss,1);
	if ($jj==''){$jj=$z;}
	$k="<a href=\"graph.php?batchid=".$j."&mode=his\" target=\OVEN".$j."\">BATCH ".$j."</a>";
	$hh="<a href=\"xlswriter\writer.php?batchid=".$j."\"><IMG SRC='excel-icon24.png' border='0'>&nbsp;MS EXCEL FILE</a>";
	//$l="<a href=\"realtime2.php?ovendid=".$z."\" target=\"_blank\">View R</a>";
	$mummet="
	select 
       (case when  (select stepnumber from batchpattern where batchid='".$j."')>=0  
       then 
       (select stepnumber from batchpattern where batchid='".$j."')
       else
       (select stepnumber from ovenpattern where ovenid=
				(select ovenid from heatingbatch where batchid='".$j."'))   
	   end)  as stepnumber";
	$ed =odbc_exec($link,$mummet);
	$f= odbc_result($ed,"stepnumber");
	$ask2 = "SELECT batchid from batchmonitor Where (batchid='".$j."') and (laststep >='".$f."') ";
	$cur2 = odbc_exec($link,$ask2);
	//echo "<p>".$j;
	if (odbc_fetch_row($cur2)){
	$bb=($c++%2==1) ? 'odd' : 'even';
    echo "<tr class=".$bb.">"; 
    for($i=1; $i <= $Fields; $i++){ 
        if (($i>1)&&($i<4)){
		printf("<td>%s</td>", format_date(odbc_result( $cur, $i ))); }
		
		else{
		printf("<td>%s</td>", odbc_result( $cur, $i )); }
        } 
		printf("<td>%s</td>",$jj ); 
		printf("<td>%s</td>",$k ); 
		printf("<td>%s</td>",$hh );
    print "</tr>"; 
	}
  } 
	
print "</font></table>"; 

echo "</div>\n";
echo "\n";
echo "  <p>";
?>