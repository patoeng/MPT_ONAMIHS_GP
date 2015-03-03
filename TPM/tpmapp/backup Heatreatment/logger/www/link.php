<?php
$link = odbc_connect('SQLI', 'sa', 'passwordsa','');
if (!$link) {
    die('Something went wrong while connecting to MSSQL');
}

?>