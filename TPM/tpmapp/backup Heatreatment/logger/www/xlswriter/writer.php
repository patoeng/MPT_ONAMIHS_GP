<?php
$batchid=$_GET["batchid"];
//documentation on the spreadsheet package is at:
//http://pear.php.net/manual/en/package.fileformats.spreadsheet-excel-writer.php
exec ('c:\\PROGRA~1\\iecapt\\IECapt.exe --delay=500 --url="http://localhost/new/print.php?batchid='.$batchid.'" --out="h:\\'.$batchid.'.bmp"',$ar);
chdir('phpxls');
require_once 'Writer.php';
chdir('..');

/*$sheet1 =  array(
  array('eventid','eventtitle'       ,'datetime'           ,'description'      ,'notes'      ),
  array('5'      ,'Education Seminar','2010-05-12 08:00:00','Increase your WPM',''           ),
  array('6'      ,'Work Party'       ,'2010-05-13 15:30:00','Boss\'s Bday'     ,'bring tacos'),
  array('7'      ,'Conference Call'  ,'2010-05-14 11:00:00','access code x4321',''           ),
  array('8'      ,'Day Off'          ,'2010-05-15'         ,'Go to Home Depot' ,''           ),
);

$sheet2 =  array(
  array('eventid','funny_name'   ),
  array('32'      ,'Adam Baum'    ),
  array('33'      ,'Anne Teak'    ),
  array('34'      ,'Ali Katt'     ),
  array('35'      ,'Anita Bath'   ),  
  array('36'      ,'April Schauer'),
  array('37'      ,'Bill Board'   ),
);
*/
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

$worksheet2 =& $workbook->addWorksheet('GRAPH Batch '.$batchid);
//$worksheet2->insertBitmap(0,0,"c:\\logger\\batches\\Batch".$batchid.".bmp",1,1,1,1);
//
$worksheet2->insertBitmap(0,0,"h:\\".$batchid.".bmp",1,1,1,1);
$worksheet =& $workbook->addWorksheet('Model Loaded Batch '.$batchid);

$worksheet->setColumn(0,0, 6.14);//setColumn(startcol,endcol,float)
$worksheet->setColumn(1,3,15.00);
$worksheet->setColumn(4,4, 8.00);

include "../link.php";
include "../explode.php";
$oven = "Select Ovenid,startdate,enddate from heatingbatch where batchid=".$batchid;

$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}
$j=0;
$worksheet3 =& $workbook->addWorksheet('Log of Batch '.$batchid);
$j++;
$cur2= odbc_exec($link,$oven);
$fmt =& $format_head;
$data = 'HEATREATMENT Batch '.$batchid;
$worksheet->write($j, 1, $data, $fmt);
$data = 'HEATREATMENT Batch '.$batchid.' Log';
$worksheet3->write($j, 1, $data, $fmt);

$fmt =& $format_reg;



$fmt =& $format_head2;
if (odbc_fetch_row($cur2)){
$worksheet->write(2,1, "Oven ID", $fmt);
$worksheet->write(3,1, "Start", $fmt);
$worksheet->write(4,1, "End", $fmt);
$worksheet->write(2,2, odbc_result($cur2,1), $fmt);
$worksheet->write(3,2, format_date(odbc_result($cur2,2)), $fmt);
$worksheet->write(4,2, format_date(odbc_result($cur2,3)), $fmt);
///
$worksheet3->write(2,1, "Oven ID", $fmt);
$worksheet3->write(3,1, "Start", $fmt);
//$//worksheet3->write(4,1, "End", $fmt);
$worksheet3->write(2,2, odbc_result($cur2,1), $fmt);
$worksheet3->write(3,2, format_date(odbc_result($cur2,2)), $fmt);
//s$worksheet3->write(4,2, format_date(odbc_result($cur2,3)), $fmt);
    
	
}


$j=5;
$askjoin = " SELECT batchmodel.model as SAP,model.mainrel as MAINREL ,model.description as MODEL, batchmodel.qty AS QTY \n
             FROM batchmodel \n
			 INNER JOIN model \n
			 ON model.modelid = batchmodel.model \n
			 WHERE batchmodel.batchid='".$batchid."' \n
			 ORDER BY batchmodel.model";
				 
$cur = odbc_exec($link,$askjoin);	
$Fields = odbc_num_fields($cur); 
//writeheader
$j++;
$fmt  =& $format_und;
for ($i=1; $i <= $Fields; $i++){ 
			$data=odbc_field_name( $cur,$i);
            $worksheet->write($j, $i, $data, $fmt);
	}
$fmt  =& $format_reg;
while(odbc_fetch_row($cur)){
        $j++;
		for ($i=1; $i <= $Fields; $i++){ 
			$data=odbc_result( $cur,$i);
            $worksheet->write($j, $i, $data, $fmt);
		}

}
////
$j=4;
$askjoin = "SELECT started as Time,duration As Duration ,error as Event From alarmmonitor where batchid='".$batchid."' order by started asc";	 
$cur = odbc_exec($link,$askjoin);	
$Fields = odbc_num_fields($cur); 
//writeheader
$j++;
$fmt  =& $format_und;
for ($i=1; $i <= $Fields; $i++){ 
			$data=odbc_field_name( $cur,$i);
            $worksheet3->write($j, $i, $data, $fmt);
	}
$fmt  =& $format_reg;
while(odbc_fetch_row($cur)){
        $j++;
		for ($i=1; $i <= 2; $i++){ 
			$data=intval(odbc_result( $cur,$i));
			$hour = intval($data / 3600);
			$menit= intval($data % 3600 / 60);
			$second= intval($data %60);
            $worksheet3->write($j, $i, $hour.":".$menit.":".$second, $fmt);
		}
			$data=odbc_result( $cur,3);
			$worksheet3->write($j, 3, $data, $fmt);
}
////


$workbook->send('Batch'.$batchid.'.xls');
$workbook->close();
odbc_close($link);
unlink("c:\\logger\\batches\\Batch".$batchid.".bmp");
/*$arr = array(
      'Calendar'=>$sheet1,
      'Names'   =>$sheet2,
      );
*/
/*foreach($arr as $wbname=>$rows)
{
    $rowcount = count($rows);
    $colcount = count($rows[0]);

    $worksheet =& $workbook->addWorksheet($wbname);

    $worksheet->setColumn(0,0, 6.14);//setColumn(startcol,endcol,float)
    $worksheet->setColumn(1,3,15.00);
    $worksheet->setColumn(4,4, 8.00);
    
    for( $j=0; $j<$rowcount; $j++ )
    {
        for($i=0; $i<$colcount;$i++)
        {
            $fmt  =& $format_reg;
            if ($j==0)
                $fmt =& $format_und;

            if (isset($rows[$j][$i]))
            {
                $data=$rows[$j][$i];
                $worksheet->write($j, $i, $data, $fmt);
            }
        }
    }
}

$workbook->send('test.xls');
$workbook->close();

//-----------------------------------------------------------------------------
*/

?>

