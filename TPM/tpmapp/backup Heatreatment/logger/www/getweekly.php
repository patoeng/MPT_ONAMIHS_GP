<?php

include "explode.php";
$a=$_GET["q"];
$c=strtoupper($a);
$b=$_GET["z"];
$starttime='00:00:00';
$endtime='23:59:59';
$data= explode('-',$a);
$month=array("Jan"=>1,"Feb"=>2,"Mar"=>3,"Apr"=>4,"May"=>5,"Jun"=>6,"Jul"=>7,"Aug"=>8,"Sep"=>9,"Oct"=>10,"Nov"=>11,"Dec"=>12);
$month_angkat=array(1=>"Jan",2=>"Feb",3=>"Mar",4=>"Apr",5=>"May",6=>"Jun",7=>"Jul",8=>"Aug",9=>"Sept",10=>"Oct",11=>"Nov",12=>"Dec");
$month_angka=$month[$data[0]];
$year_angka=intval($data[1]);
if (($c=='UNDEFINED')||($c=='')) {die();}
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
$askjoin = "SELECT Batchid as Batch,startdate as Started ,enddate as Ended,Ovenid,
				(select alias from ovengraphsetting where ovengraphsetting.ovenid= heatingbatch.ovenid)as alias 
				FROM HeatingBatch WHERE (((startdate>'".$start."') and
				(enddate<'".$end."'))or((startdate<'".$start."') and (enddate>'".$start."'))) and status='FIN' ORDER BY batchid DESC";
//
$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}			 
//
//print "<p>Weekly Report of BM HEATREATMENT";
//Print  "<p>".$week." of ".$a;
//
$c=0;
$cur = odbc_exec($link,$askjoin);	
$Fields = odbc_num_fields($cur); 
print "<table border='0' width='100%'  cellspacing='0'><font face='arial' size='7px'>
<col align=center><col align=center><col align=center><col align=center><col align=center><col align=center>
<tr height=40px>"; 
// Build Column Headers 
    for ($i=1; $i <= $Fields-1; $i++){ 
    printf("<th bgcolor='silver'>%s</th>", odbc_field_name( $cur,$i)); 
    }    
	printf("<th bgcolor='silver'>CHART</th>");
	printf("<th bgcolor='silver'>DOWNLOAD</th>");
	printf("<th bgcolor='silver'>MANDREL</th>");
	printf("<th bgcolor='silver'>MODEL</th>");
	printf("<th bgcolor='silver'>QTY</th>");
	printf("<th bgcolor='silver'>STATUS</th>");
// Table Body 
    $Outer=0; 
    while( odbc_fetch_row( $cur )){ 
    $Outer++; 
	$j=odbc_result ($cur,1);
	$z=odbc_result ($cur,4);
	$k="<a href=\"graph.php?batchid=".$j."&mode=his\" target=\OVEN".$j."\">BATCH ".$j."</a>";
	$hh="<a href=\"xlswriter\writer.php?batchid=".$j."\"><IMG SRC='excel-icon24.png' border='0'>&nbsp;MS EXCEL FILE</a>";
	//$l="<a href=\"realtime2.php?ovendid=".$z."\" target=\"_blank\">View R</a>";
	
	//$bb=($c++%2==1) ? 'odd' : 'even';
     
    /*for($i=1; $i <= $Fields; $i++){ 
        if (($i>1)&&($i<4)){
		printf("<td>%s</td>", format_date(odbc_result( $cur, $i ))); }
		
		else{
		printf("<td>%s</td>", odbc_result( $cur, $i )); }
        } 
		
		printf("<td>%s</td>",$k ); 
		printf("<td>%s</td>",$hh );*/
		
		$batchid = odbc_result( $cur, 1 );
		$startdate =format_date(odbc_result( $cur, 2 ));
		$enddate =format_date(odbc_result( $cur, 3 ));
		$ovenid = odbc_result( $cur, 4 );
		$alias = odbc_result( $cur, 5 );
		if ($alias==''){$alias=$ovenid;}
		$ask3 =" SELECT model.mainrel as MANDREL, model.description as MODEL, batchmodel.qty AS QTY \n
             FROM batchmodel \n
			 INNER JOIN model \n
			 ON model.modelid = batchmodel.model \n
			 WHERE batchmodel.batchid='".$j."' \n
			 ORDER BY batchmodel.model";
		$cur3= odbc_exec($link,$ask3);
		$r=0;
		
        while (odbc_fetch_row($cur3)){
		    if ($r==0){$bb=($c++ % 2==1) ? 'odd' : 'even';}
			$r++;
		    printf( "<tr class='%s'>",$bb);
			printf("<td>%s</td>", $batchid);
		    printf("<td>%s</td>", $startdate);
			printf("<td>%s</td>", $enddate);
			printf("<td>%s</td>", $alias);
			printf("<td>%s</td>",$k ); 
			printf("<td>%s</td>",$hh );
			printf("<td>%s</td>",odbc_result($cur3,1) ); 
			printf("<td>%s</td>",odbc_result($cur3,2) );
			printf("<td>%s</td>",odbc_result($cur3,3) ); 
			$ask4="select * from alarmmonitor where batchid='".$batchid."' and type='E'";
			$cur4=odbc_exec($link,$ask4);
			$status="OK";
			if (odbc_fetch_row($cur4)){
			$status="Error";
			}
			printf("<td>%s</td>",$status);
			print "</tr>"; 
		} 		
		
 //   print "</tr>"; 
  //  } 
	}
print "</font></table>"; 

echo "</div>\n";
echo "\n";
echo "  <p>";
//
?>
