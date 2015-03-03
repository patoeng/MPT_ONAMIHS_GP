<?php
include "link.php";
$q=$_GET["q"];
$next=$_GET["last"];
//$next=100+$;
$batchid=$_GET["batchid"];
$ovenid=$_GET["ovenid"];
$lim=$_GET["lim"];
if ($lim!=''){
$limit=intval($lim) * 60;
}else{$limit=0;}
//$con = mysql_connect('localhost', 'root', '');

//if (!$con)
//  {
//  die('Could not connect: ' . mysql_error());
//  }
//$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
//$link2 = odbc_connect('SQLI', 'sa', 'passwordsa','');
if ($ovenid=='') {
$bb="select graph_color from ovengraphsetting where ovenid=(select ovenid from heatingbatch where batchid='".$batchid."')";
}else{
$bb="select graph_color from ovengraphsetting where ovenid='".$ovenid."'";
}
$bbb= odbc_exec($link,$bb);
$graph_color= odbc_result($bbb,"graph_color");

if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}
//echo ('berhasil');
if ($q=='1') {
if ($limit<=0){
$ask7 = "SELECT cast(fetchtime as float)/60 as 'datax',Temperature as 'datay',fetchtime FROM batchtemperature where (batchID='".$batchid."')  ORDER BY Fetchtime ASC"; 
}else{
$ask7 = "SELECT cast(fetchtime as float)/60 as 'datax',Temperature as 'datay',fetchtime FROM batchtemperature where (batchID='".$batchid."') and fetchtime<='".$limit."'  ORDER BY Fetchtime ASC"; 
}
$s="[300,null]";
}
else {
if ($limit<=0){
$ask7="SELECT TOP 1 cast(fetchtime as float)/60 as 'datax',Temperature as 'datay',fetchtime FROM batchtemperature where (batchID='".$batchid."') and (fetchtime >'".$next."') ORDER BY Fetchtime ASC"; 
}else{
$ask7="SELECT TOP 1 cast(fetchtime as float)/60 as 'datax',Temperature as 'datay',fetchtime FROM batchtemperature where (batchID='".$batchid."') and (fetchtime >'".$next."') and fetchtime<='".$limit."' ORDER BY Fetchtime ASC"; 
}
$s="";
};
//$ask2 = "SELECT  TOP 1 OvenID,Status,startdate FROM heatingbatch where batchid='".$batchid."' ORDER BY startdate DESC";
if ($ovenid==""){
$ask2 ="SELECT batchid,status FROM heatingbatch WHERE batchid='".$batchid."' ORDER BY startdate DESC";
}else{
$ask2 ="SELECT TOP 1 batchid,status FROM heatingbatch WHERE ovenid='".$ovenid."' ORDER BY startdate DESC";
}
$cur = odbc_exec($link,$ask7); 
$cur2 = odbc_exec($link,$ask2);
//$askbatch = "SELECT TOP 1 ovenid,status,startdate,enddate from heatingbatch WHERE batchid='".$batchid."'";
$suhu="0";

echo "{\n";

echo 				"\"label\": \"Batch ".$batchid."\",\n";
echo 				"\"color\": \"#".$graph_color."\",\n";
//echo 				"\"color\": \"green\",\n";




echo "\"data\": [";
while( odbc_fetch_row( $cur )) 
  {
  if ($s!='') {$s=$s.",";}
  $row=odbc_result( $cur, 1 );
  $minute=$row;
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
    if ($row=='.0'){
	$s=$s . ", 0] ";
	}else{
  	$s= $s . ", " . $row . "] ";}
  }
  $suhu =$row;
  $last = odbc_result ($cur,3);

  }
  $last=intval($last);   
$minute=intval($minute);  
   echo $s;
echo "],\n";

$status = odbc_result($cur2,2);
echo "\"status\" : \"".$status."\",";
echo "\"suhu\" : \"".$suhu."\",";
echo "\"minute\" : \"".$minute."\",";
echo "\"batchid\" : \"".$batchid."\",";
if ($last==""){
$last = "-2";
}
echo "\"last\" : \"".$last."\"";

echo "\n}";

odbc_close( $link); 
//odbc_close( $link2);

?>