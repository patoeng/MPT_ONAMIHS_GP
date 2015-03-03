<?php
$q=$_GET["q"];

$con = mysql_connect('localhost', 'root', '');
if (!$con)
  {
  die('Could not connect: ' . mysql_error());
  }

mysql_select_db("datas", $con);

$sql="SELECT * FROM datass WHERE id='".$q."'";

$result = mysql_query($sql);
//if ($q==1){
echo "{\n";
echo "\"label\": \"Batch\",\n";
//echo "[";
//}
$s="";
echo "\"data\" :[";

while($row = mysql_fetch_array($result))
  {
  if ($s!='') {$s=$s.",";}
  if ($row['datax']=='')
  {
    $s=$s . "[ null";
  }else{
  $s= $s . "[ " . $row['datax'];
  }
  if ($row['datay']=='')
  {
    $s=$s . ", null] ";
  }else{
  $s= $s . ", " . $row['datay'] . "] ";
  }
  
  

  }
   
   echo $s;
//if ($q==1){
echo "]}";
//}//lse{
//echo "]\n";
//}
mysql_close($con);
?>