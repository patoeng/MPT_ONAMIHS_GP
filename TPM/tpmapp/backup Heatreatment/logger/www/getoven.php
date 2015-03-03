<?php
include "link.php";
include "explode.php";
//$askjoin = "Select alias,status,startdate,ovenid From Ovendb where ovenid not like '%ALARM%' order by id";
$askjoin = "Select ovenid From Ovendb where ovenid not like '%ALARM%' order by ovenid";
//$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}			 
$cur = odbc_exec($link,$askjoin);
$i=0;
$c=0;
echo "<table  width=100% cellspacing='0'><tr bgcolor=#ABCDEF><th></th><th>Device</th><th>Starting</th><th>Status</th><th></th>";
echo "<col width=4%><col width=26% align=center><col width=50% align=center><col width=16% align=center><col width=4% align=center>";
while (odbc_fetch_row($cur)){
   $woe=(odbc_result($cur,1));
    //$status = odbc_result($cur,2);
    $mm=($c++%2==1 )? 'odd':'even';
	
	$askkk="select TOP 1 ovenid,status,startdate,(select alias from ovengraphsetting where ovenid='".$woe."')from heatingbatch where ovenid='".$woe."' order by startdate DESC";
	$currr = odbc_exec($link,$askkk);
	if (odbc_fetch_row($currr)){
		$classs="other";
		$status=odbc_result($currr,2); 
		if ($status=='FIN'){$classs="finished";};
		if ($status=='RUN'){$classs="running";};
		if ($status=='READY'){$classs="ready";};
		if ($status=='OFF'){$classs="shutdown";};
		if ($status=='PM'){$classs="PM";};
		$lll=odbc_result($currr,4);
		if ($lll==''){$lll=odbc_result($currr,1);}
		$kk= "<tr class=".$mm." bgcolor=#DDDDDD ><td></td><td><a href=\".\device.php?dev=".urlencode(odbc_result($currr,1))."\" class=\"\" >".$lll." </a></td><td>".format_date(odbc_result($currr,3))."</td><td><a href='./graph.php?mode=real&ovenid=".urlencode($woe)."&auto=0'  class='button ".$classs."'>".strtoupper($classs)."</a>
		</td><td ></td></tr>\n";
		if (($status=='OFF')||($status=='PM')){
		$kk= "<tr class=".$mm." bgcolor=#DDDDDD  ><td></td><td><a href=\".\device.php?dev=".urlencode(odbc_result($currr,1))."\" class=\"\" >".$lll." </a></td><td>".format_date(odbc_result($currr,3))."</td><td><a href='#'  class='button ".$classs."'>".strtoupper($classs)."</a>
		</td><td ></td></tr>\n";		
		};}
		echo $kk;
	$i++;
}
echo "</table>";
odbc_close($link);
?>