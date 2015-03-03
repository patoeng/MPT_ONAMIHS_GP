<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>jsDatePick Javascript example</title>
<!-- 

	Copyright 2009 Itamar Arjuan
	jsDatePick is distributed under the terms of the GNU General Public License.
	
	****************************************************************************************

	Copy paste these 2 lines of code to every page you want the calendar to be available at
-->
<link rel="stylesheet" type="text/css" media="all" href="jsDatePick_ltr.min.css" />
<!-- 
	OR if you want to use the calendar in a right-to-left website
	just use the other CSS file instead and don't forget to switch g_jsDatePickDirectionality variable to "rtl"!
	
	<link rel="stylesheet" type="text/css" media="all" href="jsDatePick_ltr.css" />
-->
<script type="text/javascript" src="jsDatePick.min.1.3.js"></script>
<!-- 
	After you copied those 2 lines of code , make sure you take also the files into the same folder :-)
    Next step will be to set the appropriate statement to "start-up" the calendar on the needed HTML element.
    
    The first example of Javascript snippet is for the most basic use , as a popup calendar
    for a text field input.
-->
<script type="text/javascript">
	window.onload = function(){
		new JsDatePick({
			useMode:2,
			target:"inputField",
			dateFormat:"%Y-%m-%d",
			cellColorScheme:"beige"
			/*selectedDate:{				This is an example of what the full configuration offers.
				day:5,						For full documentation about these settings please see the full version of the code.
				month:9,
				year:2006
			},
			yearsRange:[1978,2020],
			limitToToday:false,
			cellColorScheme:"beige",
			dateFormat:"%m-%d-%Y",
			imgPath:"img/",
			weekStartDay:1*/
		});
		new JsDatePick({
			useMode:2,
			target:"inputField2",
			dateFormat:"%Y-%m-%d",
			cellColorScheme:"beige"
			/*selectedDate:{				This is an example of what the full configuration offers.
				day:5,						For full documentation about these settings please see the full version of the code.
				month:9,
				year:2006
			}/*,
			yearsRange:[1978,2020],
			limitToToday:false,
			cellColorScheme:"beige",
			dateFormat:"%m-%d-%Y",
			imgPath:"img/",
			weekStartDay:1*/
		});

};
function showuser(str1,str2,str3,str4)
{
str ="anoman";
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
xmlhttp.open("GET","../getbatch.php?q="+str2+"&&r="+str3+"&&o="+str1+"&&f="+str4);
xmlhttp.send();
}		

</script>
<style type="text/css">
#content-containertable.override{
  width:100%;
  border-color:Silver;
  border-style:Double;
  background-color: buttonface; 
}
#content-containertable.override div{
  height:200px;
  }
body {
  font-family: sans-serif;
  font-size: 16px;
  margin: 150px;
  max-width: 2000px;
}  
.even { background-color:cyan; }
.odd { background-color:white; }
a {
text-decoration: none;
}
</style>
</head>
<body margin="100px" bgcolor="black">
<form>
	<table border="1" bgcolor="windows">
	<tr>
		<td>
		<b>Oven ID : </b>
				<select name="users" onchange="showuser(users.value,start.value,end.value,minimal.value);">
					<option value="Oven%231" selected>OVEN1</option>
					<option value="Oven%232">OVEN2</option>
					<option value="OVEN%233">OVEN3</option>
					<option value="OVEN%234">OVEN4</option>
				</select>
		</td>
		<td><b>Start Date :</b><input type="text" size="12" name="start" id="inputField" onblur="timeselect();"/></td>
		<td><b>End Date :</b><input type="text" size="12" name="end" id="inputField2"onblur="timeselect();"/></td>
		<td><b>Min Elapsed Time :</b>
				<select name="minimal" onchange="showuser(users.value,start.value,end.value,minimal.value);">
					<option value="0"></option>
					<option value="0">All</option>
					<option value="30">30 minutes</option>
					<option value="60">1 hours</option>
					<option value="90">1.5 hours</option>
					<option value="120">2 hours</option>
					<option value="150">2.5 hours</option>
					<option value="180">3 hours</option>
				</select>
		</td>
	</tr>
	<tr>
		<td colspan="4" id="content-containertable" class="override" style="overflow:scroll;"><div id="txtHint" width="80%";></div></td>
	</tr>
	</table>
</form>
</body>
</html>
