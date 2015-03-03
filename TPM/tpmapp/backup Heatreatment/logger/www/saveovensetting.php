<?php
$ovenid = $_POST["ovenid"];
include "link.php";
/*$ask="select ovenid,alias,status,ip,port,pattern from ovendb where ovenid='".$ovenid."'";
$exe= odbc_exec($link,$ask);
$field=odbc_num_fields($exe);
echo "SDS";
//echo "<form name='OvenForm' method='POST' action='.\saveovensetting.php'>";
while (odbc_fetch_row($exe)){
for ($i=1; $i <= $field; $i++){
echo "\n<p>".odbc_field_name($exe,$i)."&nbsp;<input disabled type='text' name='".odbc_field_name($exe,$i)."' value='".$_POST[odbc_field_name($exe,$i)]."'>";}
}
//echo "\n<p><input type='submit' value='Submit'>";
//echo "\n</form>"*/
if (isset($_POST["Submit"])){
if (isset($_POST['apply'])){
$update="UPDATE OvenDB SET \n
        port='".$_POST["port"]."',
		max_y='".$_POST["max_y"]."',
		max_x='".$_POST["max_x"]."',
		limit='".$_POST["limit"]."',
		ticksize='".$_POST["ticksize"]."',
		pattern='".$_POST["pattern"]."',
		allow_open='".$_POST["allow_open"]."',
		upper_limit_color='".$_POST["upper_limit_color"]."',
		lower_limit_color='".$_POST["lower_limit_color"]."',
		target_color='".$_POST["target_color"]."',
		limit_tolerance='".$_POST["limit_tolerance"]."',
		background_color='".$_POST["background_color"]."'";
}else{
$update="UPDATE OvenDB SET \n
        alias='".$_POST["alias"]."',
		status='".$_POST["status"]."',
		ip='".$_POST["ip"]."',
		port='".$_POST["port"]."',
		max_y='".$_POST["max_y"]."',
		max_x='".$_POST["max_x"]."',
		limit='".$_POST["limit"]."',
		ticksize='".$_POST["ticksize"]."',
		allow_open='".$_POST["allow_open"]."',
		pattern='".$_POST["pattern"]."',
		upper_limit_color='".$_POST["upper_limit_color"]."',
		lower_limit_color='".$_POST["lower_limit_color"]."',
		target_color='".$_POST["target_color"]."',
		limit_tolerance='".$_POST["limit_tolerance"]."',
		background_color='".$_POST["background_color"]."'
		Where ovenid='".urldecode($_POST["ovenid"])."'";
		}

$exe=odbc_exec($link,$update);
if ($exe) {echo "Succes";}else{echo "gagal";}
odbc_close($link);
}
?>

