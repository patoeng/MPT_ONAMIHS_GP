<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>BM-HEATREATMENT</title>
	<meta name="keywords" content="">
	<meta name="description" content="">
	<script language="javascript" type="text/javascript" src="./flot/jquery.js?<?php echo time();?>"></script>
	<script language="javascript" type="text/javascript" src="./flot/jquery.flot.js?<?php echo time();?>"></script>
	
<!-- dd menu -->
<script type="text/javascript">
$(document).ready(function() {
  fetchData();
 
});

function fetchData()
{

if (window.XMLHttpRequest)
  {// code for IE7+, Firefox, Chrome, Opera, Safari
  xmlhttp=new XMLHttpRequest();
  }
else
  {// code for IE6, IE5
  xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
  }
xmlhttp.onreadystatechange=function()
  {
  if (xmlhttp.readyState==4 && xmlhttp.status==200)
    {
    document.getElementById("container").innerHTML=xmlhttp.responseText;
    }
  }
xmlhttp.open("GET","./getoven.php");
xmlhttp.send();
setTimeout(fetchData,3000);
}		

</script>
<style type="text/css">
a{text-decoration:none;}
.button {
		padding: 5px 10px;
		display: inline;
	
		border: none;
		color: white;
		cursor: pointer;
		font-weight: bold;
		font-size:20px;
		border-radius: 5px;
		-moz-border-radius: 5px;
		-webkit-border-radius: 5px;
		text-shadow: 1px 1px #666;
		}
	.button:hover {
		background-position: 0 center;
		background-color: transparent;
		}
	.button:active {
		background-position: 0 top;
		position: relative;
		top: 1px;
		padding: 6px 10px 4px;
		}
	.button.running { color: yellow; }
	.button.finished {color: green; }
	.button.ready { color: green; }
	.button.other { color: red }
	.button.title { background-color: gray}
	.even { background-color:#E8E8E8; }
	.odd { background-color:#C0DCFD; }

</style>
</head>
<body background="background-12.jpg" >
<table border="0"  width="100%"  align="center" cellspacing=0>
	<tr bgcolor=#FFFFFF>
		<td colspan=4><img src="logo.png"><font size=6 face=arial color=ORAnge>&nbsp;BM-HEATREATMENT</font></td>
	</tr>
	<tr height=80px bgcolor=#2DA5D2>
	   <td align=left><input type=button class='button title' value="VIEW ALL REALTIME" onclick="window.location.assign('./table.php?ovenid=OVEN%231&auto=1')"></input>&nbsp;&nbsp;</td>
	   <td><input type=button class='button title' value="VIEW CHART HISTORY" onclick="window.location.assign('./selector2.php')"></input></td>
	   <td><input type=button class='button title' value="Weekly Summary" onclick="window.location.assign('./weekly.php')"></input></td>
	   <td colspan=1></td>
	<tr>
	<tr>
		<td colspan=4></td>
	</tr>
	<tr>
	<td colspan=4><font face=arial size=5><div id="container"><?php include "./getoven.php";?></div></font></td>
	</tr>
	<tr>
		<td colspan=4></td>
	</tr>
	<tr height=30px bgcolor=#2DA5D2 align=center>
	   <td colspan=4><font face=arial color=#FFFFFF size=1px>SHIMANO BATAM BM-HEATREATMENT</font></td>
	<tr>
</table>
</body>
</html>
