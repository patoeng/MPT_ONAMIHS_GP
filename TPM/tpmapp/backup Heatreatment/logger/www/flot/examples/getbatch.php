<?php
$q =$_GET["q"];
$r =$_GET["r"];
$o =$_GET["o"];
$f =$_GET["f"];
$f = $f * 60;
$askjoin = "SELECT BATCHID,startdate,enddate FROM HeatingBatch WHERE (ovenid='".$o."') and (startdate>'".$q."') and (enddate<'".$r."') ORDER BY batchid DESC"; 
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
	$j=odbc_result ($cur,1);
	
	$k="<a href=\"history.php?batchid=".$j."\" target=\"_blank\">View CHART</a>";
	
	$ask2 = "SELECT TOP 1 * FROM batchtemperature Where (batchid='".$j."') and (fetchtime >'".$f."') ORDER BY Fetchtime DESC";
	$cur2 = odbc_exec($link,$ask2);
	if (odbc_num_rows($cur2)>0){
    print "<tr>"; 
    for($i=1; $i <= $Fields; $i++){ 
        printf("<td>%s</td>", odbc_result( $cur, $i )); 
        } 
		
		printf("<td>%s</td>",$k ); 
    print "</tr>"; 
    } 
	}
print "</table>"; 

echo "</div>\n";
echo "\n";
echo "  <p>";
?>