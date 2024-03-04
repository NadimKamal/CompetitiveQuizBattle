<?php
require 'connection.php';
session_start();
if($_GET['user']==1)
{
    session_unset();
    session_destroy();
    header('Location: index.php');
    exit;
}

?>