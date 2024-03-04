
<?php
require 'connection.php';
ob_start();

session_start();


if($_SESSION["admin_name"]=="")
{
    header('Location: admin-login-form-for-selected.php');
    exit;
}

?>
<!DOCTYPE html>
<html lang="en">

  <head>
     <meta name="viewport" content="width=device-width, initial-scale=1">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link href="https://fonts.googleapis.com/css?family=Raleway:100,200,300,400,500,600,700,800,900&display=swap" rel="stylesheet">

    <title>qb</title>

<!--

Breezed Template

https://templatemo.com/tm-543-breezed

-->
    <!-- Additional CSS Files -->
    <link rel="stylesheet" type="text/css" href="assets/css/bootstrap.min.css">

    <link rel="stylesheet" type="text/css" href="assets/css/font-awesome.css">

    <link rel="stylesheet" href="assets/css/templatemo-breezed.css">

    <link rel="stylesheet" href="assets/css/owl-carousel.css">

    <link rel="stylesheet" href="assets/css/lightbox.css">

    </head>
    
    <body>
    
    <!-- ***** Preloader Start ***** -->
    <div id="preloader">
        <div class="jumper">
            <div></div>
            <div></div>
            <div></div>
        </div>
    </div>  
    <!-- ***** Preloader End ***** -->
    
    
    <!-- ***** Header Area Start ***** -->
    <header class="header-area header-sticky">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <nav class="main-nav">
                        <!-- ***** Logo Start ***** -->
                        <a href="admin-home.php" class="logo">
                            qb
                        </a>
                        <!-- ***** Logo End ***** -->
                        <!-- ***** Menu Start ***** -->
                        <ul class="nav">
                            <li class="scroll-to-section"><a href="admin-home.php" class="active">Home</a></li>
                            <li class="submenu">
                                <a href="">Competition</a>
                                <ul>
                                    <li><a href="">Quiz</a></li>
                                    <li><a href="">Coding</a></li>
                                    <li><a href="">GK</a></li>
                                    <li><a href="">IQ</a></li>
                                </ul>
                            </li>
                            <li class="submenu">
                                <a href="">
                                    <?php echo $_SESSION["admin_name"]; ?>
                                </a>
                                <ul>
                                    <li><a href="">Profile</a></li>
                                    <li><a href="admin-logout.php?admin=1">Logout</a></li>
                                </ul>
                            </li>
                        </ul>        
                        <a class='menu-trigger'>
                            <span>Menu</span>
                        </a>
                        <!-- ***** Menu End ***** -->
                    </nav>
                </div>
            </div>
        </div>
    </header>
    <!-- ***** Header Area End ***** -->