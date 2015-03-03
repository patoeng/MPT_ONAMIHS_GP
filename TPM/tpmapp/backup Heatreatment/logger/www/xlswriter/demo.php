<?php
//documentation on the spreadsheet package is at:
//http://pear.php.net/manual/en/package.fileformats.spreadsheet-excel-writer.php
chdir('phpxls');
require_once 'Writer.php';
chdir('..');

$sheet1 =  array(
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

$workbook = new Spreadsheet_Excel_Writer();

$format_und =& $workbook->addFormat();
$format_und->setBottom(2);//thick
$format_und->setBold();
$format_und->setColor('black');
$format_und->setFontFamily('Arial');
$format_und->setSize(8);

$format_reg =& $workbook->addFormat();
$format_reg->setColor('black');
$format_reg->setFontFamily('Arial');
$format_reg->setSize(8);

$arr = array(
      'Calendar'=>$sheet1,
      'Names'   =>$sheet2,
      );

foreach($arr as $wbname=>$rows)
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
?>

