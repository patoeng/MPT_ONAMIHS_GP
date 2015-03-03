<?php
function format_date($s)
{
	$first = explode(" ",$s);
	$getdate = explode("-",$first[0]);
	$second = explode(".",$first[1]);
	//$gettime =explode(":",$second[0]);
	$month=array("01"=>"Jan","02"=>"Feb","03"=>"Mar","04"=>"0Apr","05"=>"May","06"=>"Jun","07"=>"Jul","08"=>"Aug","09"=>"Sept","10"=>"Oct","11"=>"Nov","12"=>"Dec");
	//$sm=date("D d-M-Y G:i:s",mktime($gettime[0],$gettime[1],$gettime[2],$getdate[1],$getdate[2],$getdate[0]));
	$sm = $getdate[2]."-".$month[$getdate[1]]."-".$getdate[0]." ".$second[0];
	return $sm;
}

?>