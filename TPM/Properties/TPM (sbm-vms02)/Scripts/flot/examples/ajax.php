<?php
$device=$_GET["dev"];
$batchid=$_GET["BatchId"];

echo "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n";
echo "<html>\n";
echo "<head>\n";
echo "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n";
echo "<title>".$device."</title>\n";
echo "<link href=\"layout.css\" rel=\"stylesheet\" type=\"text/css\">\n";
echo "<!--[if lte IE 8]><script language=\"javascript\" type=\"text/javascript\" src=\"../excanvas.min.js\"></script><![endif]-->\n";
echo "<script language=\"javascript\" type=\"text/javascript\" src=\"../jquery.js\"></script>\n";
echo "<script language=\"javascript\" type=\"text/javascript\" src=\"../jquery.flot.js\"></script>\n";
echo "</head>\n";
echo "<body>\n";
echo "<h1>Headline</h1>\n";
echo "\n";
echo "<div id=\"placeholder\" style=\"width:1000px;height:600px;\"></div>\n";
echo "\n";
echo "<h2>Model Loaded in this Oven Batch</h2>\n";
echo "<div id=\"modelholder\"></div>\n";
echo "\n";
echo "  <p>";
echo " <input class=\"fetchSeries\" type=\"button\" value=\"First dataset\"> -\n";
echo " <a href=\"getuser.php?q=1\">data</a> - \n";
echo " <span></span>\n";
echo " </p> \n";

    

echo "<p>\n";
echo "<input class=\"dataUpdate\" type=\"button\" value=\"Poll for data\">\n";
echo "</p>\n";

echo "<script type=\"text/javascript\">\n";
echo "\$(function () {\n";
echo "var options = {\n";
echo "lines: { show: true },\n"; 
echo "  series: { shadowSize: 0 },\n"; // drawing is faster without shadows
echo "  yaxis: { min: 0, max: 200 },\n";
echo "  xaxis: { show: true, tickDecimals: 0, tickSize: 10  },\n";
echo "  points: { show: false }\n";
                

echo"    };\n";
echo"    var data = [[0,null],[null,0],[null,200],[300,null],[0,0]];\n";
echo"    var placeholder = \$(\"#placeholder\");\n";
    
echo"    $.plot(placeholder, data, options);\n";

    
    // fetch one series, adding to what we got
echo"    var alreadyFetched = {};\n";
   
  
echo"    \$(\"input.fetchSeries\").click(function () {\n";
echo"        var button = \$(this);\n";
        
        // find the URL in the link right next to us 
echo"        var dataurl = button.siblings('a').attr('href');\n";

        // then fetch the data with jQuery
echo"        function onDataReceived(series) {\n";
            // extract the first coordinate pair so you can see that
            // data is now an ordinary Javascript object
echo"            var firstcoordinate = '(' + series.data[0][0] + ', ' + series.data[0][1] + ')';\n";

echo"            button.siblings('span').text('Fetched ' + series.label + ', first point: ' + firstcoordinate);\n";

            // let's add it to our current data
echo"            if (!alreadyFetched[series.label]) {\n";
echo"                alreadyFetched[series.label] = true;\n";
echo"                data.push(series);\n";
echo"            }\n";
            
            // and plot all we got
echo"            $.plot(placeholder, data, options);\n";
echo"         }\n";
        
echo"        $.ajax({\n";
echo"            url: dataurl,\n";
echo"           method: 'GET',\n";
echo"            dataType: 'json',\n";
echo"            success: onDataReceived\n";
echo"        });\n";
echo"    });\n";


    // initiate a recurring data update
echo"    $(\"input.dataUpdate\").click(function () {\n";
        // reset data
echo"        data = [[],[]];\n";
echo"		mbel =[];\n";
echo"        alreadyFetched = {};\n";
        
echo"       $.plot(placeholder, data, options);\n";

echo"        var iteration = 0;\n";
echo"		var stepper = 100;\n";
        
echo"        function fetchData() {\n";
echo"           ++iteration;\n";

echo"            function onDataReceived(series) {\n";
                // we get all the data in one go, if we only got partial
                // dhata, we could merge it with what we already got
echo"              var plot = \$.plot($(\"#placeholder\"), data, options);\n";
echo"			if (iteration==1){\n";
echo"				mbel=series.data;\n";
echo"				data =[series];\n";
				
echo"				}else{\n";
echo"				$.merge(mbel,series.data);\n";
echo"				data.data =[mbel];\n";
echo"				data.label = series.label\n";
				
echo"				}\n";
				
echo"                plot.setData(data);\n";
echo"				plot.setupGrid();\n";
echo"				plot.draw();\n";
              //  $.plot($("#placeholder"), data, options);\n";
echo"            }\n";
echo"			stepper = 99+iteration;\n";
echo"            $.ajax({\n";
                // usually, we'll just call the same URL, a script
                // connected to a database, but in this case we only
                // have static example files so we need to modify the
                // URL
                //url: "data-eu-gdp-growth-" + iteration + ".json",
//				url: "getuser_1.php?q="+iteration,
				
echo"				url: \"getuser.php?q=\"+iteration+\"&&batchid=76\",\n";
echo"                method: 'GET',\n";
echo"                dataType: 'json',\n";
echo"                success: onDataReceived\n";
echo"            });\n";
            
         //   if (iteration < 8)
echo"                setTimeout(fetchData, 1000);\n";
         //   else {
                //data = [];
                //alreadyFetched = {};
          //  }
echo"        }\n";

echo"        setTimeout(fetchData, 1000);\n";
echo"    });\n";
echo"});\n";
echo "</script>\n\n";

echo "</body>\n";
echo "</html>\n";
?>