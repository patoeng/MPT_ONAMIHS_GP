<?php
// Server in the this format: <computer>\<instance name> or 
// <server>,<port> when using a non default port number
$server = 'SQLI';

// Connect to MSSQL
$link = odbc_connect($server, 'sa', 'passwordsa','');
echo "<a href=\"anoman%23\">dfasd</a>";

if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}
echo ('berhasil');
$ask7 = "SELECT BATCHID,startdate,enddate FROM HeatingBatch WHERE (ovenid='OVEN#1') and (startdate>'2011-12-31') and (enddate<'2012-1-2')"; 
$cur = odbc_exec($link,$ask7); 
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
print "<b> Your request returned $Outer rows!</b>"; 
odbc_close( $link); 
?>