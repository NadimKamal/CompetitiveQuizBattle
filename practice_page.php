<?php
require 'connection.php';

session_start();

$own_id = $_SESSION["user_id"];

$qstns = $_SESSION["question"];
$cat = $_SESSION["cat"];

// Select Question from table start


//echo $qstns;

$questions = explode(",",$qstns);

//echo $qstns_qry;
                

// Select Question from table end


//Result Evalution start

if(isset($_POST['submit']))
{
    $correct = 0;
    $database = "";
    if($cat[0]=='s')
    {
        $correct = 8;
        $database = "school";
    }
    if($cat[0]=='u')
    {
        $correct = 8;
        $database = "university";
    }
    if($cat[0]=='c')
    {
        $correct = 7;
        $database = "college";
    }
    if($cat[0]=='g')
    {
        $correct = 6;
        $database = "gk";
    }
    $right = 0;
    for($i=0; $i<count($questions);$i++)
    {
        $name = "radio".$questions[$i];
        if(!empty($_POST["$name"])) {
            $option = $_POST["$name"];
            
            $correct_option = "SELECT * FROM $database WHERE q_id=$questions[$i]";
            $qryy = $conn->prepare($correct_option);
            $qryy->execute();
            $cntt = $qryy->rowCount();
            $results = $qryy->fetchAll();

            if($option==$results[0][$correct])
            {
                $right+=1;
            }

        }
    }
    
    $_SESSION["right"] = $right;
    ob_start();
    header('Location: practice_result.php');
    exit;

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
            if($cat[0]=='u')
            {
                $new_query = "SELECT * FROM university WHERE q_id = $questions[$i]";
                $qry = $conn->prepare($new_query);
                $qry->execute();
                $cnt = $qry->rowCount();
                $result = $qry->fetchAll();
            
                $qstn = $result[0][3];
                $AA = $result[0][4];
                $BB = $result[0][5];
                $CC = $result[0][6];
                $DD = $result[0][7];
            }
            if($cat[0]=='c')
            {
                $new_query = "SELECT * FROM college WHERE q_id = $questions[$i]";
                $qry = $conn->prepare($new_query);
                $qry->execute();
                $cnt = $qry->rowCount();
                $result = $qry->fetchAll();
            
                $qstn = $result[0][2];
                $AA = $result[0][3];
                $BB = $result[0][4];
                $CC = $result[0][5];
                $DD = $result[0][6];
            }
            if($cat[0]=='s')
            {
                $new_query = "SELECT * FROM school WHERE q_id = $questions[$i]";
                $qry = $conn->prepare($new_query);
                $qry->execute();
                $cnt = $qry->rowCount();
                $result = $qry->fetchAll();
            
                $qstn = $result[0][3];
                $AA = $result[0][4];
                $BB = $result[0][5];
                $CC = $result[0][6];
                $DD = $result[0][7];
            }
            if($cat[0]=='g')
            {
                $new_query = "SELECT * FROM gk WHERE q_id = $questions[$i]";
                $qry = $conn->prepare($new_query);
                $qry->execute();
                $cnt = $qry->rowCount();
                $result = $qry->fetchAll();
                
                $qstn = $result[0][1];
                $AA = $result[0][2];
                $BB = $result[0][3];
                $CC = $result[0][4];
                $DD = $result[0][5];
            }
            
            //echo "<script>alert('Asche')</script>";
            //echo $qstn;
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
    <?php }?>

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