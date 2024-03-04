<?php
require 'connection.php';
session_start();
if($_GET['admin']==1)
{
    session_unset();
    session_destroy();
    header('Location: admin-login-form-for-selected.php');
    exit;
}

?>