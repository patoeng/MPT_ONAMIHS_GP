<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Trsitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>OVEN CHART</title>
<meta charset="utf-8">

        <title>Shimano Batam BM HEATREATMENT</title>
        <link rel="stylesheet" href="./css/zebra_datepicker.css" type="text/css">
        <script type="text/javascript" src="./jquery-1.7.1.min.js"></script>
        <script type="text/javascript" src="./zebra_datepicker.js"></script>
        

<script type="text/javascript" >
$(document).ready(function() {

    // assuming the controls you want to attach the plugin to
    // have the "datepicker" class set
    $('input.datepicker').Zebra_DatePicker({
    format: 'd-M-Y'
	});
	$('input.datepicker2').Zebra_DatePicker({
    format: 'd-M-Y'
	});

});

window.onload = function() {

          //Get a reference to the link on the page
          // with an id of "mylink"
          var a = document.getElementById("mylink");
		  var b = document.getElementById("inputField");
		  var c = document.getElementById("inputField2");
		  var d = document.getElementById("oven");
          //Set code to run when the link is clicked
          // by assigning a function to "onclick"
          a.onclick = function() {

            // Your code here...
			showuser(d.value,b.value,c.value);	
            //If you don't want the link to actually 
            // redirect the browser to another page,
            // "google.com" in our example here, then
            // return false at the end of this block.
            // Note that this also prevents event bubbling,
            // which is probably what we want here, but won't 
            // always be the case.
            return false;
          }
		
        }

function showuser(str1,str2,str3)
{
str ="anoman";
str4='170';
if (str=="")
  {
  document.getElementById("txtHint").innerHTML="";
  return;
  } 
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
    document.getElementById("txtHint").innerHTML=xmlhttp.responseText;
    }
  }
xmlhttp.open("GET","./getbatch.php?q="+str2+"&&r="+str3+"&&o="+str1+"&&f="+str4);
xmlhttp.send();
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
	.button.ready { color: yellow; }
	.button.other { color: red }
	.button.title { background-color: gray}
	.even { background-color:#B4CDCD; }
.odd { background-color:#E8E8E8; }

</style>
</head>
<body margin="10px" bgcolor="black" background="background-12.jpg">
<form>s
	<table border="0"  width="100%" bgcolor=#2DA5D2 align="center">
	<tr bgcolor=#FFFFFF>
		<td colspan=4><img src="logo.png"><font size=6 face=arial color=ORAnge>&nbsp;BM-HEATREATMENT</font></td>
	</tr>
	<tr height=80px>
		
		<td><b>Start Date : </b><input type="text" size="12" name="start" id="inputField" class="datepicker"></td>
		<td><b>End Date : </b><input type="text" size="12" name="end" id="inputField2" class="datepicker2" ></td>
		<td><!--<b>Min Elapsed Time : </b>
				<select name="minimal" onchange="showuser(users.value,start.value,end.value);">
					<option value="0" selected></option>
					<option value="0">All</option>
					<option value="30">30 minutes</option>
					<option value="60">1 hours</option>
					<option value="90">1.5 hours</option>
					<option value="120">2 hours</option>
					<option value="150">2.5 hours</option>
					<option value="180">3 hours</option>
				</select>
                 <input type='button' value='REFRESH'onclick="showuser(users.value,start.value,end.value);"> </input> -->
			<b>Oven ID : </b>
				<select id="oven" name="users" onchange="showuser(users.value,start.value,end.value);">
					<?php 
					include "link.php";
					$askjoin = "Select ovenid,(select alias from OvenGraphSetting where OvenGraphSetting.OvenId=OvenDB.OvenID) as alias From Ovendb where ovenid not like '%ALARM%' order by ovenid";
					$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
					if (!$link) {
					die('Something went wrong while connecting to MSSQL');
						}			 
					$cur = odbc_exec($link,$askjoin);
					$i=0;
					while (odbc_fetch_row($cur)){
					   $woe=urlencode(odbc_result($cur,1));
					   if ($i==0){echo "<option value=\"".$woe."\" selected>".odbc_result($cur,2)."</option>";}
					   else{echo "<option value=\"".$woe."\">".odbc_result($cur,2)."</option>";}
					   $i++;
					}
					echo "<option value=\"all\">ALL</option>";
					odbc_close($link);
					?>
				</select>

		</td>
		<td><a href='#' id="mylink" class='button title'>SHOW</a></td>
	</tr>
	<tr>
		
	</tr>
	</table>
	<td colspan="4" id="content-containertable" class="override" style="overflow:scroll;"><div id="txtHint" width="80%";></div></td>
</form>
</body>
</html>
