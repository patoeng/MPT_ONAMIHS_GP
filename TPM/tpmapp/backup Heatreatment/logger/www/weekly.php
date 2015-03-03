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

<style type="text/css">
#content-containertable.override{
  width:100%;
  border-color:Silver;
  border-style:Double;
  background-color: #FFF; 
}
#content-containertable.override div{
  height:100px;
  }
body {
  font-family: sans-serif;
  font-size: 16px;
  margin: 10px;
  max-width: 2000px;
}  
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
	.even { background-color:#E8E8E8; }
	.odd { background-color:#C0DCFD; }

</style> 

<script type="text/javascript" >
function showuser(str2,str3)
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
xmlhttp.open("GET","./getweekly.php?q="+str2+"&&z="+str3);
xmlhttp.send();
}		

window.onload = function() {

          //Get a reference to the link on the page
          // with an id of "mylink"
          var a = document.getElementById("mylink");
		  var b = document.getElementById("inputField");
		  var c = document.getElementById("idweek");
		  var d = document.getElementById("mylink1");
          //Set code to run when the link is clicked
          // by assigning a function to "onclick"
          a.onclick = function() {

            // Your code here...
			showuser(b.value,c.value);	
            //If you don't want the link to actually 
            // redirect the browser to another page,
            // "google.com" in our example here, then
            // return false at the end of this block.
            // Note that this also prevents event bubbling,
            // which is probably what we want here, but won't 
            // always be the case.
            return false;
          }
		  d.onclick = function (){
		    window.open("./xlswriter/weeklywriter.php?q="+b.value+"&&z="+c.value);
		    return false;
		  }
        }

$(document).ready(function() {

    // assuming the controls you want to attach the plugin to
    // have the "datepicker" class set
    $('input.datepicker').Zebra_DatePicker({
    view:'days',
	format:'M-Y'
	});
	

});

</script>


</head>
<body margin="10px" bgcolor="black" background="background-12.jpg">
<form>
	<table border="0"  width="100%" bgcolor=#2DA5D2 align="center">
	<tr bgcolor=#FFFFFF>
		<td colspan=4><img src="logo.png"><font size=6 face=arial color=ORAnge>&nbsp;BM-HEATREATMENT</font></td>
	</tr>
	<tr height=80px>
		
		<td colspan=1 ><b>Month : </b><input type="text" size="12" name="start" id="inputField" class="datepicker"></td>
		<td><b>Week&nbsp;: </b>
			<select name=week id='idweek'>
			<option value='wk1'>WEEK 1</option>
			<option value='wk2'>WEEK 2</option>
			<option value='wk3'>WEEK 3</option>
			<option value='wk4'>WEEK 4</option>
			</select>
		</td>
		<td><a href='#' id="mylink" class='button title'>SHOW</a></td>
		<td><a href='#' id="mylink1" class='button title'>DOWNLOAD</a></td>
	</tr>
	<tr>
		
	</tr>
	</table>
	<td colspan="4" id="content-containertable" class="override" style="overflow:scroll;"><font size='2' face='arial'><div id="txtHint" width="80%";></div></font></td>
</form>
</body>
</html>
