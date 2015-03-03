<?php
$q=$_GET["q"];
$next=$q+100;
$batchid=$_GET["batchid"];

//$con = mysql_connect('localhost', 'root', '');

//if (!$con)
//  {
//  die('Could not connect: ' . mysql_error());
//  }
$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
//$link2 = odbc_connect('SQLI', 'sa', 'passwordsa','');

if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}
//echo ('berhasil');
if ($q=='1') {
$ask7 = "SELECT fetchtime/60 as 'datax',Temperature as 'datay' FROM batchtemperature where (batchID='".$batchid."') and (fetchtime/60<100) ORDER BY Fetchtime ASC"; 
$s="[300,null]";
}
else {
$ask7="SELECT TOP 1 fetchtime/60 as 'datax',Temperature as 'datay' FROM batchtemperature where (batchID='".$batchid."') and (fetchtime/60 >'".$next."') ORDER BY Fetchtime ASC"; 
$s="";
};


$cur = odbc_exec($link,$ask7); 

//$askbatch = "SELECT ovenid,status,startdate,enddate from heatingbatch WHERE batchid='".$batchid."'";


echo "{\n";
echo "\"label\": \"Batch ".$batchid."\",\n";
echo "\"color\": \"blue\",\n";




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
echo "]\n}";

odbc_close( $link); 
//odbc_close( $link2);

?>