<?php
    //require 'user-header.php';
    require 'connection.php';
    session_start();
    $user_id = $_SESSION["user_id"];
    $id = $_GET["id"];
    $qnum=0;
    try{
        $qry = "SELECT * FROM CreateTournament WHERE private_id=".$id."";
        $check = $conn->prepare($qry);
        $check->execute();
        $check_cnt = $check->rowCount();
        $results = $check->fetchAll();
        
        $con_end = $results[0][4];
        $qnum= $results[0][2];
        
        
        //echo $con_end;
        
        echo "<div id='end_date' style='display: none'>$con_end</div>";
        echo "<div id='id' style='display: none'>?id=$id</div>";
        
        if(isset($_POST['submit']))
        {
            $right = 0;
            $correct_option = "SELECT * FROM private_question WHERE private_id=".$id."";
            $qryy = $conn->prepare($correct_option);
            $qryy->execute();
            $cntt = $qryy->rowCount();
            $results = $qryy->fetchAll();
            for($i=0; $i<$qnum;$i++)
            {
                $name = "radio".$results[$i][0];
                //echo $name;
                if(!empty($_POST["$name"])) {
                    $option = $_POST["$name"];
                    //echo $results[$i][0];
                    if($option==$results[$i][7])
                    {
                        $right+=1;
                    }
        
                }
            }
            //echo $right;
            
            $qry = "UPDATE private_result SET score=$right WHERE private_id=".$id." and user_id=".$user_id."";
            $conn->exec($qry);
            header('Location: home.php');
		    exit;
            
        }
    
    }
    catch(Exception $e)
    {
        header('Location: home.php');
		exit;
    }
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
    <form class="form form-signup" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]). "?id=". $_GET['id'];?>">
    <?php
        //echo count($questions);
        
        $new_query = "SELECT * FROM private_question WHERE private_id = ".$id."";
        $qry = $conn->prepare($new_query);
        $qry->execute();
        $cnt = $qry->rowCount();
        $result = $qry->fetchAll();
        for($i = 0; $i<$qnum; $i++)
        {
            $q_id = $result[$i][0];
            $qstn = $result[$i][2];
            $AA = $result[$i][3];
            $BB = $result[$i][4];
            $CC = $result[$i][5];
            $DD = $result[$i][6];
    
    
    ?>

    

    <div class="container mt-sm-5 my-1">
        <div class="question ml-sm-5 pl-sm-5 pt-2">
            <div class="py-2 h5"><b>Q. <?php echo $qstn;?></b></div>
            <div class="ml-md-3 ml-sm-3 pl-md-5 pt-sm-0 pt-3" id="options"> <label class="options"><?php echo $AA;?> <input type="radio" name="radio<?php echo $q_id;?>" value="A"> <span class="checkmark"></span> </label> <label
                    class="options"><?php echo $BB;?> <input type="radio" name="radio<?php echo $q_id;?>" value="B"> <span
                        class="checkmark"></span> </label> <label class="options"><?php echo $CC;?> <input
                        type="radio" name="radio<?php echo $q_id;?>" value="C"> <span class="checkmark"></span> </label> <label class="options"><?php echo $DD;?>
                    <input type="radio" name="radio<?php echo $q_id;?>" value="D"> <span class="checkmark"></span> </label> </div>
        </div>
    </div>
    <?php echo "<br>"; ?>
    <?php }?>

        <div style="text-align:center;">
            <input id="submit" type="submit" class="btn-submit my-btn" name="submit" value="Submit"/>
        </div>    
    </form>
    <br /><br />


    

    <!-- ***** Main Banner Area End ***** -->
</body>

</html>



