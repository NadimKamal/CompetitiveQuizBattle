<?php
require 'connection.php';
require 'php_function.php';

session_start();
$notification_id = $_SESSION['temp_notification'];
//echo $notification_id;
$own_id=$_SESSION['user_id'];
$qstns="";
$usr_id="";
$opp_id="";
try{
    $get_data = "SELECT * FROM [QB_DB].[dbo].[CM_BATTLE_HISTORY]  WHERE REQUEST_ID='$notification_id'";
    $conn = Openconnection();
    $qry = sqlsrv_query($conn,$get_data);
    while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
        {
            $qstns = $result['QUESTIONS_LIST'];
            $usr_id=$result['USER_ID'];
            $opp_id=$result['OPPONENT_ID'];
        }
        sqlsrv_free_stmt($qry);
        sqlsrv_close($conn);
    } catch (\Throwable $th) {
        //echo $th;
        //echo '<script>alert("Error")</script>';
    }





$questions = explode(",",$qstns);

if(isset($_POST['submit']))
{
    
    $right = 0;
    for($i=0; $i<count($questions);$i++)
    {
        $name = "radio".$questions[$i];
        if(!empty($_POST["$name"])) {
            $option = $_POST["$name"];
            $get_data = "SELECT * FROM CM_SYSTEM_QUESTION WHERE SYS_QUES_ID='$questions[$i]'";
            $conn = Openconnection();
            $qry = sqlsrv_query($conn,$get_data);
            while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
            {
                $COR = $result['CORRECT_OPTION'];
                if($option==$COR)
                {
                    $right+=1;
                }
                
            }
        }
    }

    if($own_id==$opp_id)
    {
        try{
        $qry = "UPDATE CM_BATTLE_HISTORY SET OPPONENT_SCORE='$right' WHERE REQUEST_ID='$notification_id'";
        ExecuteNonQuery($qry);
        }
        catch(Exception $e)
        {
            //echo $e;
        }
        //echo $notification_id;
    }
    else if($own_id==$usr_id)
    {
        try{
            $qry = "UPDATE CM_BATTLE_HISTORY SET USER_SCORE='$right' WHERE REQUEST_ID='$notification_id'";
            ExecuteNonQuery($qry);
        }
        catch(Exception $e)
        {
            //echo $e;
        }
    }
    
    //check if both finished battle and select winner start
    
    $usr_scr="";
    $opp_scr="";
    $get_data = "SELECT * FROM [QB_DB].[dbo].[CM_BATTLE_HISTORY]  WHERE REQUEST_ID='$notification_id'";
    $conn = Openconnection();
    $qry = sqlsrv_query($conn,$get_data);
    while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
        {
            $usr_scr = $result['USER_SCORE'];
            $opp_scr = $result['OPPONENT_SCORE'];
            
        }
        sqlsrv_free_stmt($qry);
        sqlsrv_close($conn);
    
    if($usr_scr==null || $opp_scr==null)
    {
        ob_start();
        header('Location: waiting-for-result.php');
        exit;
    }
    
    if($own_id==$opp_id)
      {
          if($opp_scr<$usr_scr)
          {
            try{
              $qry = "UPDATE CM_BATTLE_HISTORY SET WINNER_ID='$usr_id' WHERE REQUEST_ID='$notification_id'";
              ExecuteNonQuery($qry);
          }
          catch(Exception $e)
          {
              //echo $e;
          }
              ob_start();
              header('Location: loser-page.php');
              exit;
          }
          else if($opp_scr>$usr_scr)
          {
              try{
                $qry = "UPDATE CM_BATTLE_HISTORY SET WINNER_ID='$opp_id' WHERE REQUEST_ID='$notification_id'";
                ExecuteNonQuery($qry);
            }
            catch(Exception $e)
            {
                //echo $e;
            }
              ob_start();
              header('Location: winner-page.php');
              exit;
          }
          else {
              ob_start();
              header('Location: draw-page.php');
              exit;
          }
      }
      else
      {
          if($opp_scr>$usr_scr)
          {
            try{
              $qry = "UPDATE CM_BATTLE_HISTORY SET WINNER_ID='$usr_id' WHERE REQUEST_ID='$notification_id'";
              ExecuteNonQuery($qry);
          }
          catch(Exception $e)
          {
              //echo $e;
          }
              ob_start();
              header('Location: loser-page.php');
              exit;
          }
          else if($opp_scr<$usr_scr)
          {
            try{
              $qry = "UPDATE CM_BATTLE_HISTORY SET WINNER_ID='$opp_id' WHERE REQUEST_ID='$notification_id'";
              ExecuteNonQuery($qry);
          }
          catch(Exception $e)
          {
              //echo $e;
          }
              ob_start();
              header('Location: winner-page.php');
              exit;
          }
          else {
              ob_start();
              header('Location: draw-page.php');
              exit;
          }
      }
    
}





//Result Evalution End




?>

<html>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Montserrat&display=swap');

        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box
        }

        body {
            background-color: #FFF;
        }

        .container {
            background-color: #555;
            color: #ddd;
            border-radius: 10px;
            padding: 20px;
            font-family: 'Montserrat', sans-serif;
            max-width: 700px
        }

        .container>p {
            font-size: 32px
        }

        .question {
            width: 75%
        }

        .options {
            position: relative;
            padding-left: 40px
        }

        #options label {
            display: block;
            margin-bottom: 15px;
            font-size: 14px;
            cursor: pointer
        }

        .options input {
            opacity: 0
        }

        .checkmark {
            position: absolute;
            top: -1px;
            left: 0;
            height: 25px;
            width: 25px;
            background-color: #555;
            border: 1px solid #ddd;
            border-radius: 50%
        }

        .options input:checked~.checkmark:after {
            display: block
        }

        .options .checkmark:after {
            content: "";
            width: 10px;
            height: 10px;
            display: block;
            background: white;
            position: absolute;
            top: 50%;
            left: 50%;
            border-radius: 50%;
            transform: translate(-50%, -50%) scale(0);
            transition: 300ms ease-in-out 0s
        }

        .options input[type="radio"]:checked~.checkmark {
            background: #21bf73;
            transition: 300ms ease-in-out 0s
        }

        .options input[type="radio"]:checked~.checkmark:after {
            transform: translate(-50%, -50%) scale(1)
        }

        .btn-primary {
            background-color: #555;
            color: #ddd;
            border: 1px solid #ddd
        }

        .btn-primary:hover {
            background-color: #21bf73;
            border: 1px solid #21bf73
        }

        .btn-success {
            padding: 5px 25px;
            background-color: #21bf73
        }

        @media(max-width:576px) {
            .question {
                width: 100%;
                word-spacing: 2px
            }
        }
    </style>
</head>

<body>
    <br><br>

    <!-- ***** Main Banner Area Start ***** -->
    <style>
        .my-btn
        {
          background-color: #4CAF50; /* Green */
          border: none;
          color: white;
          padding: 10px 52px;
          text-align: center;
          text-decoration: none;
          display: inline-block;
          font-size: 16px;
        }
    </style>
    <form class="form form-signup" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]);?>">
    <?php
        //echo count($questions);
        for($i = 0; $i<count($questions); $i++)
        {
            //echo $questions[$i];
            //$qstns_qry .=" q_id = $questions[$i]";
            $get_data = "SELECT * FROM CM_SYSTEM_QUESTION WHERE SYS_QUES_ID='$questions[$i]'";
            $conn = Openconnection();
            $qry = sqlsrv_query($conn,$get_data);
            while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
            {
                $qstn = $result['SYS_QUES_NAME'];
                $AA = $result['SYS_OPTION_A'];
                $BB = $result['SYS_OPTION_B'];
                $CC = $result['SYS_OPTION_C'];
                $DD = $result['SYS_OPTION_D'];
            }
    ?>

    

    <div class="container mt-sm-5 my-1">
        <div class="question ml-sm-5 pl-sm-5 pt-2">
            <div class="py-2 h5"><b>Q. <?php echo $qstn;?></b></div>
            <div class="ml-md-3 ml-sm-3 pl-md-5 pt-sm-0 pt-3" id="options"> <label class="options"><?php echo $AA;?> <input type="radio" name="radio<?php echo $questions[$i];?>" value="A"> <span class="checkmark"></span> </label> <label
                    class="options"><?php echo $BB;?> <input type="radio" name="radio<?php echo $questions[$i];?>" value="B"> <span
                        class="checkmark"></span> </label> <label class="options"><?php echo $CC;?> <input
                        type="radio" name="radio<?php echo $questions[$i];?>" value="C"> <span class="checkmark"></span> </label> <label class="options"><?php echo $DD;?>
                    <input type="radio" name="radio<?php echo $questions[$i];?>" value="D"> <span class="checkmark"></span> </label> </div>
        </div>
    </div>
    <?php echo "<br>"; ?>
    <?php
     }
        sqlsrv_free_stmt($qry);
        sqlsrv_close($conn);
    ?>

        <div style="text-align:center;">
            <input id="submit" type="submit" class="btn-submit my-btn" name="submit" value="Submit"/>
        </div>    
    </form>
    <br /><br />


    

    <!-- ***** Main Banner Area End ***** -->
</body>

</html>

<script>

    setTimeout(() => {
        document.getElementById("submit").click();
    }, 1800000);

</script>