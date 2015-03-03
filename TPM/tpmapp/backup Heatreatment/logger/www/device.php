<?php
$ovenid=$_GET["dev"];
if ($ovenid==''){$ovenid = $_POST["ovenid"];}
//$ovenid=urldecode($ovenid);
//echo "<p>Device : ".$ovenid;
include "link.php";
//form prosessor
if (isset($_POST["save"])){
if ($_POST['apply']=='all'){
$update="UPDATE OvenGraphSetting SET \n
   		max_y='".$_POST["max_y"]."',
		max_x='".$_POST["max_x"]."',
		limit='".$_POST["limit"]."',
		ticksize='".$_POST["ticksize"]."',
		graph_color='".$_POST["graph_color"]."',
		upper_limit_color='".$_POST["upper_limit_color"]."',
		lower_limit_color='".$_POST["lower_limit_color"]."',
		target_color='".$_POST["target_color"]."',
		limit_tolerance='".$_POST["limit_tolerance"]."',
		background_color='".$_POST["background_color"]."'";
}else{
$update="UPDATE OvenGraphSetting SET \n
        alias='".$_POST["alias"]."',
		max_y='".$_POST["max_y"]."',
		max_x='".$_POST["max_x"]."',
		limit='".$_POST["limit"]."',
		ticksize='".$_POST["ticksize"]."',
		graph_color='".$_POST["graph_color"]."',
		upper_limit_color='".$_POST["upper_limit_color"]."',
		lower_limit_color='".$_POST["lower_limit_color"]."',
		target_color='".$_POST["target_color"]."',
		limit_tolerance='".$_POST["limit_tolerance"]."',
		background_color='".$_POST["background_color"]."' 
		 Where ovenid='".$ovenid."'";
		}

$exes=odbc_exec($link,$update);
if ($exes) {echo "Succes";}else{echo "gagal";}

}
///end vform prosessor

echo "<script type=\"text/javascript\" src=\"jscolor/jscolor.js\"></script>";
$ask="select ovenid,alias,max_x,max_y,limit,limit_tolerance,ticksize,graph_color,upper_limit_color,lower_limit_color,target_color,background_color from ovengraphsetting where ovenid='".$ovenid."'";
$exe= odbc_exec($link,$ask);
$field=odbc_num_fields($exe)-5;
echo "\n<form name='OvenForm' method='POST' action='".$_SERVER['PHP_SELF']."'>";
while (odbc_fetch_row($exe)){
echo "\n<p>".odbc_field_name($exe,1)."&nbsp;<input disabled type='text' name='".odbc_field_name($exe,1)."dis' value='".odbc_result($exe,1)."'>";
echo "\n<input  type='hidden' name='".odbc_field_name($exe,1)."' value='".odbc_result($exe,1)."'>";
for ($i=2;$i<=$field;$i++){
echo "\n<p>".odbc_field_name($exe,$i)."&nbsp;<input type='text' name='".odbc_field_name($exe,$i)."' value='".odbc_result($exe,$i)."'>";
}
for ($i=$field+1;$i<=$field+5;$i++){
echo "\n<p>".odbc_field_name($exe,$i)."&nbsp;<input class=\"color{required:false}\" type='text' name='".odbc_field_name($exe,$i)."' value='".odbc_result($exe,$i)."'>";
}
}
echo "\n<p><input type='checkbox' name='apply' value='all'>Apply This To All Device.";
echo "\n<p><input type='submit' name='save' value='SAVE'>";

echo "\n</form>";
odbc_close($link);
?>