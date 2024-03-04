
<?php
function Openconnection(){
    $server = "DESKTOP-A7ACU5B\SQLEXPRESS";
    $options = array(  "UID" => "juba",  "PWD" => "1234",  "Database" => "QB_DB");
    $conn = sqlsrv_connect($server, $options);
    
    if ($conn === false) {
        die("<pre>".print_r(sqlsrv_errors(), true));
    }
        return $conn;
  }
?>