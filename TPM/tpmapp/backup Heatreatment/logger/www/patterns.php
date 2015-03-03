<?php
$command=$_GET["command"];
$pattern=$_GET["pattern"];
include "link.php";


if ($command=='add'){
$ask="select * from pattern where pattern='".$pattern."'";
$exec = odbc_exec($link,$ask);
$fields=odbc_num_fields($exec);
echo "\n<form name=patternaddform method=post action='".$_SERVER['PHP_SELF']."'>";
while (odbc_fetch_row($exec)){
for ($i=1;$i<=$fields;$i++){
echo "\n<p>".odbc_field_name($exec,$i)."&nbsp;<input type=text name='".odbc_field_name($exec,$i)."' value=''>";
}}
echo "\n<p><input type=submit name=addsubmit value=Save>";
echo "\n</form>";
}


if ($command=='edit'){
$ask="select * from pattern where pattern='".$pattern."'";
$exec = odbc_exec($link,$ask);
$fields=odbc_num_fields($exec);
echo "\n<form name=patterneditform method=post action='".$_SERVER['PHP_SELF']."'>";
while (odbc_fetch_row($exec)){
for ($i=1;$i<=$fields;$i++){
echo "\n<p>".odbc_field_name($exec,$i)."&nbsp;<input type=text name='".odbc_field_name($exec,$i)."' value='".odbc_result($exec,$i)."'>";
}}
echo "\n<input type='hidden' name='patternid' value='".$pattern."'>";
echo "\n<p><input type=submit name=editsubmit value=Save>";
echo "\n</form>";
}


if ($command=='delete'){
echo "Delete";
$ask="delete pattern where pattern='".$pattern."'";
$exec=odbc_exec($link,$ask);
}
// form processor
if ((isset($_POST["addsubmit"]))||(isset($_POST["editsubmit"]))){
$askk="select * from pattern";
$exec=odbc_exec($link,$askk);
$field=odbc_num_fields($exec);
 if (odbc_fetch_row($exec)){
	if (isset($_POST["addsubmit"])){
	$ask="Insert into pattern values(";
	for ($i=1;$i<$field;$i++){
	    $ask .= "'".$_POST[odbc_field_name($exec,$i)]."',";
		}
		$ask .= "'".$_POST[odbc_field_name($exec,$i)]."')";
	}
	if (isset($_POST["editsubmit"])){
	$ask="Update pattern set ";
	for ($i=1;$i<$field;$i++){
	    $ask .= odbc_field_name($exec,$i)."='".$_POST[odbc_field_name($exec,$i)]."',";
		}
		$ask .= odbc_field_name($exec,$i)."='".$_POST[odbc_field_name($exec,$i)]."' ";
		$ask .= "where pattern='".$_POST["patternid"]."'";
	}
 }

$exe=odbc_exec($link,$ask);

}


// end form processor

echo "<hr>";
$ask="select * from pattern order by pattern";
$exe=odbc_exec($link,$ask);
$Fields = odbc_num_fields($exe);

echo "\n<table border='1'>"; 
//create Header
echo "\n<th bgcolor=#2DA5D2 align='center'>COMMAND</th>";
for ($i=1;$i<=$Fields;$i++){
echo "\n<th bgcolor=#2DA5D2 align='center'>". odbc_field_name( $exe,$i)."</th>";
}

while (odbc_fetch_row($exe)){
echo "\n<tr>";
echo "<td>&nbsp;<a href='.\patterns.php?command=edit&pattern=".urlencode(odbc_result($exe,1))."'>edit</a>&nbsp;
		<a href='.\patterns.php?command=delete&pattern=".urlencode(odbc_result($exe,1))."'>delete</a>&nbsp;</td>";
for ($i=1;$i<=$Fields;$i++){
echo "<td>".odbc_result($exe,$i)."</td>";
}


echo "</tr>";
}
echo "</table>";
echo "&nbsp;<a href='.\patterns.php?command=add&pattern=".urlencode(odbc_result($exe,1))."'>Add</a>";
?>