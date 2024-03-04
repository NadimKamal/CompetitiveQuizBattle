<?php
require 'connection.php';
require 'php_function.php';
session_start();


if($_SESSION["user_name"]=="")
{
    header('Location: index.php');
    exit;
}
    

    if(isset($_POST['notification_id']))
    {
        

    $notification_id = $_POST['notification_id'];
    $_SESSION['temp_notification'] = $notification_id;
    echo $notification_id;
    }
    //echo $notification_id;

    if(isset($_POST['ChallangeAccept']))
    {
        //echo "dhukeche";
        $_SESSION['temp_notification'] = $notification_id;
        $id = $_SESSION['user_id'];
        $sql ="UPDATE CM_BATTLE_REQUEST SET REQUEST_STATE='A' WHERE REQUEST_ID='$notification_id'";
		//echo $sql;
        ExecuteNonQuery($sql);
        try{
            $get_data = "INSERT INTO CM_BATTLE_HISTORY (REQUEST_ID,USER_ID,OPPONENT_ID)  (SELECT REQUEST_ID,SENDER_PARTY,RECEPIENT_PARTY FROM CM_BATTLE_REQUEST WHERE REQUEST_ID='$notification_id')";
            //echo $get_data;
            ExecuteNonQuery($get_data);
            
            } catch (\Throwable $th) {
                
            }
            $result = array();
            try{
                $get_data = "SELECT * FROM CM_BATTLE_REQUEST CR,CM_SYSTEM_SUBJECT SS,CM_SYSTEM_QUESTION CQ WHERE CR.REQUEST_TEXT=SS.SYS_SUB_ID AND SS.SYS_SUB_ID=CQ.SYS_SUB_ID AND REQUEST_ID='$notification_id'";
                $conn = Openconnection();
                $qry = sqlsrv_query($conn,$get_data);
                //echo $get_data;
                $resultQ="";
                while($resultQ = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
                    {
                        $key = $resultQ['SYS_QUES_ID'];
                        array_push($result,$key);
                    }
                    sqlsrv_free_stmt($qry);
                    sqlsrv_close($conn);
                } catch (\Throwable $th) {
                    
                }
                $cnt=count($result);
                $question="";
                //print_r ($result);
                //echo sizeof(array_unique($final_array));
                $FinalQ=array();
                $FinalQ=UniqueRandomNumbersWithinRange(0,$cnt,5);
                
                for($i=0;$i<count($FinalQ);$i++){      
                $tmp=$result[$FinalQ[$i]];
                $question.=$tmp;
                if($i<count($FinalQ))
                {
                    $question.=',';
                }
            }
                        
                    
            $sql ="UPDATE CM_BATTLE_HISTORY SET QUESTIONS_LIST='$question' WHERE REQUEST_ID='$notification_id'";
		    //echo $sql;
		    ExecuteNonQuery($sql);

        header('Location: challenge-page.php');
        exit;
    }

    if(isset($_POST['ChallangeReject']))
    {
        //echo "dhukeche";
        $id = $_SESSION['user_id'];
        
		$sql ="UPDATE CM_BATTLE_REQUEST SET REQUEST_STATE='R' WHERE REQUEST_ID='$notification_id'";
		//echo $sql;
		ExecuteNonQuery($sql);

        header('Location: home.php');
        exit;
    }
    else if(isset($_GET['ChallangeReject']))
    {
        $id = $_SESSION['user_id'];
        $sql ="UPDATE CM_BATTLE_REQUEST SET REQUEST_STATE='R' WHERE SENDER_PARTY='$id' and REQUEST_DATE=(SELECT MAX(REQUEST_DATE) FROM [QB_DB].[dbo].[CM_BATTLE_REQUEST] WHERE SENDER_PARTY='$id')";
		//echo $sql;
		ExecuteNonQuery($sql);

        header('Location: home.php');
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
    <link href="https://fonts.googleapis.com/css?family=Raleway:100,200,300,400,500,600,700,800,900&display=swap"
        rel="stylesheet">

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
                        <a href="home.php" class="logo">
                            qb
                        </a>
                        <!-- ***** Logo End ***** -->
                        <!-- ***** Menu Start ***** -->
                        <ul class="nav">
                            <li class="scroll-to-section"><a href="home.php" class="active">Home</a></li>
                            <li class="scroll-to-section"><a href="about-us.php">About</a></li>
                            <li class="scroll-to-section"><a href="contact-us.php">Contact Us</a></li>
                            <li class="scroll-to-section"><a href="rank_list.php">Rank-List</a></li>
                            <li class="submenu">
                                
                                <style>
                                    #showNCount
                                    {
                                          font-size:18px;
                                          float:left;
                                          margin:-15px;
                                    }
                                </style>

                                <a href="#">Notifications <div id="showNCount">0</div></a>
                                <style>
                                    .notifications
                                    {
                                        width:275px!important;
                                        background-color:#ffffff;
                                        
                                    }
                                    .notifications li
                                    {
                                        padding:12px!important;
                                        border-bottom:1.5px solid #eee;
                                    }
                                    .notifications li input[type=submit]
                                    {
                                        background-color: #4CAF50;
                                        border: none;
                                        color: white;
                                        padding: 6px 15px;
                                        text-decoration: none;
                                        margin: 2px 2px;
                                        cursor: pointer;
                                        width: 48%;
                                    }
                                </style>
                                <ul class="notifications">
                                
                                <div id="userNotification"></div>
                                
                                <script>
                                    var t = 1000;
                                    setInterval(user_notification,t);
                                    function user_notification()
                                    {
                                        jQuery.ajax({
                                            url:'notification.php',
                                            type:'post',
                                            success:function(result)
                                            {
                                                document.getElementById("userNotification").innerHTML = result;
                                            }
                                        });
                                        let x = document.getElementById("nCount").innerHTML;
                                        document.getElementById("showNCount").innerHTML = x;
                                    }
                                   // user_notification();
                                </script>
                                
                                    
                                <?php
                                   /* $email = $_SESSION["user_email"];
                                    //echo $email;
                                    $get_data = "SELECT id from UserTable where email='$email'";
                                    $qry = $conn->prepare($get_data);
                                    $qry->execute();
                                    //echo $get_data;
                                    $cnt = $qry->rowCount();
                                    $result = $qry->fetchAll();
                                    $u_id = $result[0][0];
                                    //echo $u_id;
                                    $again_qry = "SELECT * from Notifications where opponent_id=$u_id";
                                    //echo $again_qry;
                                    $m_qry = $conn->prepare($again_qry);
                                    $m_qry->execute();
                                    $m_cnt = $m_qry->rowCount();
                                    $m_result = $m_qry->fetchAll();
                                    //echo $m_result[0][2];
                                    //echo "<br>";
                                    //echo $m_result[0][2];
                                    
                                    for($i = $m_cnt-1; $i>=0; $i--)
                                    {
                                        if($m_result[$i][6]==0){
                                        $id = $m_result[$i][0];
                                        echo "<li>".$m_result[$i][4]."
                                        <br />
                                        <form method='post'>
                                            <input type = 'text' name='notification_id' value='$id' style = 'display: none'/>
                                            <input type='submit' value='Accept' name='ChallangeAccept' />
                                            <input type='submit' value='Reject
' name='ChallangeReject' style='background-color: red; float: right' />
                                        </form>
                                        </li>";
                                        //echo "<br>";
                                    }
                                    }
                                    //echo $get_data;
                                    */
                                ?>
                                </ul>
                            </li>

                        
                            <li class="submenu">
                                <a>
                                    <?php echo $_SESSION["user_name"]; ?>
                                </a>
                                <ul>
                                    <li><a href="user-profile.php">Profile</a></li>
                                    <li><a href="user-logout.php?user=1">Logout</a></li>
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