<?php
$a=$_GET["q"];
$b=$_GET["z"];
$c=strtoupper($a);
if (($c=='UNDEFINED')||($c=='')) {die("<p>Please specify the month...");}
chdir('phpxls');
require_once 'Writer.php';
chdir('..');

$workbook = new Spreadsheet_Excel_Writer();

$format_head =& $workbook->addFormat();
//$format_head->setBottom(2);//thick
$format_head->setBold();
$format_head->setColor('black');
$format_head->setFontFamily('Arial');
$format_head->setSize(18);

$format_head2 =& $workbook->addFormat();
//$format_head->setBottom(2);//thick
$format_head2->setBold();
$format_head2->setColor('black');
$format_head2->setFontFamily('Arial');
$format_head2->setSize(12);


$format_und =& $workbook->addFormat();
$format_und->setBottom(2);//thick
$format_und->setBold();
$format_und->setColor('black');
$format_und->setFontFamily('Arial');
$format_und->setSize(10);

$format_reg =& $workbook->addFormat();
$format_reg->setColor('black');
$format_reg->setFontFamily('Arial');
$format_reg->setSize(10);

$worksheet =& $workbook->addworksheet('Weekly report');

include "../link.php";
include "../explode.php";
//dsds

$starttime='00:00:00';
$endtime='23:59:59';
$data= explode('-',$a);
$month=array("Jan"=>1,"Feb"=>2,"Mar"=>3,"Apr"=>4,"May"=>5,"Jun"=>6,"Jul"=>7,"Aug"=>8,"Sep"=>9,"Oct"=>10,"Nov"=>11,"Dec"=>12);
$month_angkat=array(1=>"Jan",2=>"Feb",3=>"Mar",4=>"0Apr",5=>"May",6=>"Jun",7=>"Jul",8=>"Aug",9=>"Sept",10=>"Oct",11=>"Nov",12=>"Dec");
$month_angka=$month[$data[0]];
$year_angka=intval($data[1]);
if ($b=='wk1'){
$start='';
$end ='';
$jan=0;
$week ="Week 1";
if ($month_angka==1) {
$start="26-".$month_angkat[12]."-".($year_angka-1)." ".$starttime;}
else{
$start="26-".$month_angkat[$month_angka-1]."-".($year_angka)." ".$starttime;
}
$end="03-".$month_angkat[$month_angka]."-".($year_angka)." ".$endtime;
//getbatch wk1

}
if ($b=='wk2'){
$start='';
$end ='';
$start="04-".$month_angkat[$month_angka]."-".$year_angka." ".$starttime;
$end="10-".$month_angkat[$month_angka]."-".$year_angka." ".$endtime;
$week ="Week 2";
//getbatch wk2

}
if ($b=='wk3'){
$start='';
$end ='';
$start="11-".$month_angkat[$month_angka]."-".$year_angka." ".$starttime;
$end="17-".$month_angkat[$month_angka]."-".$year_angka." ".$endtime;
$week ="Week 3";
//getbatch wk2
}
if ($b=='wk4'){
$start='';
$end ='';
$start="18-".$month_angkat[$month_angka]."-".$year_angka." ".$starttime;
$end="25-".$month_angkat[$month_angka]."-".$year_angka." ".$endtime;
$week ="Week 4";
//getbatch wk2
}
$askjoin = "SELECT Batchid as Batch,startdate as Started ,enddate as Ended,Ovenid FROM HeatingBatch WHERE (((startdate>'".$start."') and (enddate<'".$end."'))or((startdate<'".$start."') and (enddate>'".$start."'))) and status='FIN' ORDER BY batchid DESC";
//
$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}			 
//
$row=0;
$fmt  =& $format_head;
$worksheet->write($row,1, "Weekly Report of BM HEATREATMENT",$fmt);$row++;
$worksheet->write($row,1, $week." of ".$a,$fmt);$row++;
//print "<p>Weekly Report of BM HEATREATMENT";
//Print  "<p>".$week." of ".$a;
//
$cur = odbc_exec($link,$askjoin);	
$Fields = odbc_num_fields($cur); 
// Build Column Headers 
    $fmt  =& $format_und;
    for ($i=1; $i <= $Fields; $i++){ 
    //printf("<th bgcolor='silver'>%s</th>", odbc_field_name( $cur,$i)); 
	$worksheet->write($row,$i,odbc_field_name( $cur,$i),$fmt);
    }    
	$worksheet->write($row,$i++,"MANDREL",$fmt);
	$worksheet->write($row,$i++,"MODEL",$fmt);
	$worksheet->write($row,$i++,"QTY",$fmt);
	$worksheet->write($row,$i++,"STATUS",$fmt);
	$row++;
// Table Body 
    $Outer=0; 
    while( odbc_fetch_row( $cur )){ 
    $Outer++; 
	$j=odbc_result ($cur,1);
	$z=odbc_result ($cur,4);

	$ask2 = "SELECT TOP 1 * FROM batchtemperature Where (batchid='".$j."') and (fetchtime >'".(170*60)."') ORDER BY Fetchtime DESC";
	$cur2 = odbc_exec($link,$ask2);
	if (odbc_num_rows($cur2)!=0){
	$bb=($c++%2==1) ? 'odd' : 'even';
     
    
		
		$batchid = odbc_result( $cur, 1 );
		$startdate =format_date(odbc_result( $cur, 2 ));
		$enddate =format_date(odbc_result( $cur, 3 ));
		$ovenid = odbc_result( $cur, 4 );
		$ask3 =" SELECT model.mainrel as MANDREL, model.description as MODEL, batchmodel.qty AS QTY \n
             FROM batchmodel \n
			 INNER JOIN model \n
			 ON model.modelid = batchmodel.model \n
			 WHERE batchmodel.batchid='".$j."' \n
			 ORDER BY batchmodel.model";
		$cur3= odbc_exec($link,$ask3);
		$fmt  =& $format_reg;
		
        while (odbc_fetch_row($cur3)){
		 	$worksheet->write($row,1, $batchid,$fmt); 
			$worksheet->write($row,2, $startdate,$fmt); 
		    $worksheet->write($row,3, $enddate,$fmt); 
			$worksheet->write($row,4, $ovenid,$fmt); 
			$worksheet->write($row,5, odbc_result($cur3,1) ,$fmt); 
			$worksheet->write($row,6, odbc_result($cur3,2) ,$fmt); 
			$worksheet->write($row,7, odbc_result($cur3,3) ,$fmt);
			$ask4="select * from alarmmonitor where batchid='".$batchid."' and type='E'";
			$cur4=odbc_exec($link,$ask4);
			$status="OK";
			if (odbc_fetch_row($cur4)){
			$status="Error";
			}
			$worksheet->write($row,8,$status,$fmt); 
			$row++;
		} 		
		
 //   print "</tr>"; 
    } 
	}

//dsds


$workbook->send('HT'.$b.$a.'.xls');
$workbook->close();
odbc_close($link);

?>

