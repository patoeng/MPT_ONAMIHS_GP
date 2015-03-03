<table border='0' width='100%' bgcolor=#FFFFFF style='table-layout:fixed' cellspacing='0'>
	<col width=8>
	<col width=2>
	<col width=20>
	<col width=30>
	<col width=30>
	<col width=30>
	<col width=30>
	<col width=20>
	<col width=10>
	
	
	<tr>
		<td  rowspan="2" colspan="3" align="center" bgcolor=#2DA5D2>&nbsp;<!--OvenID-->
			<font color=#FFFFFF face="arial" size="6"><?php echo $ovenid ?><span id="ovenid"></span></font>
		</td>
		
		<td rowspan="2" align="center" bgcolor="black"><!--STATUS-->
			<font color="red" face="arial" size="6" ><span id="status"></span></font>
		</td>
		<td colspan=2 bgcolor=#2DA5D2><font color=#FFFFFF face="arial" size="4" >Started :&nbsp;<?php echo $startdate;?></font></td>
		<td rowspan=2 colspan=2 align="center" bgcolor=#2DA5D2>&nbsp;<!--&nbsp;SUHU--> 
		    
			<font color=#FFFFFF face="arial" size="6"><span id="suhu"></span><span id="derajat"></span></font>
		</td>
		<td rowspan=2 bgcolor=#2DA5D2 align="center">&nbsp;<font color="silver" face="arial" size="3"><span id="countup"></span></font></td>
	</tr>
	    <td colspan=2 bgcolor=#2DA5D2><!--ENDED-->
			<font face="arial" size="4" color=#FFFFFF><span id="lastlabel"></span>&nbsp;</font>
			<font face="arial" size="4"color="black"><span id="last"></span></font>&nbsp;
		</td>
	<tr>
	   <td>&nbsp;</td>
	   <td align="center"  valign="center" bgcolor="white"><!--labely--><font color="blue" face="sans-serif" size="1.5"><div id="verticaltext" class="box_rotate">Temperature&nbsp;(&#186;C)</div></font></td>
	   <td colspan=6 align="center" bgcolor="white">
			<font face="verdana" size="2px" color="blue"><!--checkbox-->
			<p id="choices" align="right">
			<input id="enableTooltip" type="checkbox" checked="true">Tooltip</p>
			</font><!-- chart--->
			<font face="times" size="3" color="blue"><!--checkbox-->
			<br><div id="placeholder" style="width:1000px;height:600px;"></div>
			</font>
	   </td>
	   <td >&nbsp;<!--checkbox-->
			
			
	   </td>
	</tr >
	<tr>
	   <td>&nbsp;</td>
	   <td  bgcolor="white">&nbsp;</td>
	   <td colspan=6 align="center"  bgcolor="white"><font color="blue" face="arial" size="1.5">Times (Minutes)</font></td>
	   <td>&nbsp;</td>
	</tr>
	
</table>


