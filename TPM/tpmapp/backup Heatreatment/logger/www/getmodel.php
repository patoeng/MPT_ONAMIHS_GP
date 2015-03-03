<?php
//$q=$_GET["q"];
//$batchid= $_GET["batchid"];
include "link.php";
$askjoin = " SELECT batchmodel.model as SAP,model.mainrel as MANDREL, model.description as MODEL, batchmodel.qty AS QTY \n
             FROM batchmodel \n
			 INNER JOIN model \n
			 ON model.modelid = batchmodel.model \n
			 WHERE batchmodel.batchid='".$batchid."' \n
			 ORDER BY batchmodel.model";
$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}			 
$jumlahrow=18;
$cur = odbc_exec($link,$askjoin);	
$Fields = odbc_num_fields($cur); 
$c=0;
for ($k=1; $k <= 3; $k++){
        $c=0;   
		$j=($k-1) *$jumlahrow;
		$anoman[$k]="<table border='0' width='100%' cellspacing='0' style='table-layout:fixed' >";
		$anoman[$k].="<col align=left width=16><col align=left width=16><col align=left width=76><col align=center width=8>";
		for ($i=1; $i <= $Fields; $i++){ 
			$anoman[$k].="<th bgcolor=#2DA5D2 align='center'>". odbc_field_name( $cur,$i)."</th>"; 
			} 
		while ($j<= ($k *$jumlahrow)) {
		$j++;
				$mmm=($c++%2==1) ? 'odd' : 'even';
				if ( odbc_fetch_row( $cur )){
				$anoman[$k] .=  "<tr class=".$mmm.">"; 
					for($i=1; $i <= $Fields; $i++){ 
					$anoman[$k] .="<td>".odbc_result( $cur, $i )."</td>"; 
						} 
				$anoman[$k] .= "</tr>";
				}else{
				$anoman[$k] .= "<tr class=".$mmm.">"; 
					for($i=1; $i <= $Fields; $i++){ 
					$anoman[$k] .="<td>&nbsp;</td>"; 
						} 
				$anoman[$k] .= "</tr>";
				}
		}
		$anoman[$k] .= "</table>";
}

$sisa="";
$c=0;
$sisa[$k]="<table border='0' width='100%' style='table-layout:fixed' ><col align=right><col align=right><col align=center><tr>\n
		<col align=left width=16><col align=left width=76><col align=center width=8>";
while (odbc_fetch_row($cur)){
		
		for ($i=1; $i <= $Fields; $i++){ 
			$sisa[$k].="<th bgcolor=#87CEEB align='center'>". odbc_field_name( $cur,$i)."</th>"; 
			}
		$mmm=($c++%2==1) ? 'odd' : 'even';
		$sisa .= "<tr class=".$mmm.">"; 
		for($i=1; $i <= $Fields; $i++){ 
					$sisa .="<td>".odbc_result( $cur, $i )."</td>"; 
		$sisa .="</tr>";			} 
}
if ($s>0){
$anoman[$k] .= "\n".$sisa."\n<table>";
}
//echo $anoman[$q];
odbc_close( $link);
?>