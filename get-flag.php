<?php

//echo "waiting page";

require 'connection.php';

//echo "<script>alert('Opponent didn't responed')</script>";
//header('Refresh: 10; URL=home.php');

ob_start();

//$var = 'http://'.$_SERVER['HTTP_HOST'].$_SERVER['REQUEST_URI'];
//header("Refresh: 3; URL='$var'");

session_start();

$user_id = $_SESSION['user_id'];

//echo $user_id;
//echo "geche";

$qry_select = "SELECT * FROM Notifications WHERE user_id=$user_id";
$qry = $conn->prepare($qry_select);
$qry->execute();
//echo $get_data;
$cnt = $qry->rowCount();
$result = $qry->fetchAll();
$temp_notification = $result[$cnt-1][0];
$_SESSION['temp_notification'] = $temp_notification;

$flag = $result[$cnt-1][6];
if($flag == 1)
{
  
}
else if($flag==2)
{
  
}


?>