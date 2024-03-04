<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmWellcomeMessage.aspx.cs" Inherits="Forms_frmWellcomeMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1 {
            font-size: xx-large;
        }

        .style2 {
            text-align: center;
        }

        .notification-style {
            width: 100%;
        }

        .not-Table {
            width: 100%;
            border-top: groove #D4E0F3;
        }

            .not-Table td:first-child {
                border-right: thin solid #000000;
            }

        .images {
            height: 25px;
            display: inline;
            margin-top: -20px;
        }

        #divnotice a {
            font-family: Copperplate Gothic Bold;
        }

        #header {
            margin-top: -35px;
        }

        #noticepic {
            margin-top: -41px;
            height: 81px;
        }

        .noticetd {
            width: 50%;
        }

            .noticetd:hover {
                background-color: #D0DDF1;
                color: #000000;
                border-radius: 8%;
                box-shadow: 0px 0px 10px #ff0000;
                -webkit-box-shadow: 0px 0px 10px #ff0000;
                -moz-box-shadow: 0px 0px 10px #ff0000;
            }

        .icon {
            width: 48px;
            height: 48px;
            background: url(Images/icon.png);
            float: left;
            text-decoration: none !important;
            cursor: pointer;
        }

        .count {
            background: #ff0000;
            min-width: 18px;
            height: 14px;
            color: #000000;
            border-radius: 50%;
            padding: 5px;
            text-align: center;
            font-size: 19px;
        }

        .noticetd p {
            font-family: Copperplate Gothic Bold;
        }

        .notifications {
            width: 400px;
            float: left;
            display: block;
        }

        body {
            display: none;
        }

        #pendings {
            display: block;
            float: left;
            background: #ff0000;
            min-width: 18px;
            height: 18px;
            color: #000000;
            border-radius: 50%;
            padding: 5px;
            text-align: center;
            vertical-align: middle;
            font-size: 14px;
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script src="js/script.js" type="text/javascript"> </script>
    <link rel="Stylesheet" href="js/jquery-ui.css" />
     
    <script type="text/javascript" src="resources/ExampleTab.js"></script>
<style>
    label {
        display: inline-block;
        width: 5em;
    }
</style>
</head>
<body onload="totalfunc();">

    
    

     
</body>
    
</html>
 