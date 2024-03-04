<?php
    require 'user-header.php';
    session_start();
    $user_id = $_SESSION["user_id"];
    $id = $_GET["id"];
    $qry = "SELECT count(*) FROM private_result WHERE private_id=".$id." and user_id=".$user_id."";
    //echo $qry;
    $check = $conn->prepare($qry);
    $check->execute();
    $check_cnt = $check->rowCount();
    
    //echo $check_cnt;
    $results = $check->fetchAll();
    if($results[0][0]>0)
    {
        header("Location: private-countdown.php?id=$id");
    	exit;
    }
    
    if(isset($_POST["submit"]))
    {
       
        $pass = $_POST["tname"];
        
        $id = $_GET["id"];
        
        
        $qry = "SELECT password FROM CreateTournament WHERE private_id=".$id."";
        //echo $qry;
        $check = $conn->prepare($qry);
        $check->execute();
        $check_cnt = $check->rowCount();
        $results = $check->fetchAll();
        
        $con_pass = $results[0][0];
        
        if($pass==$con_pass)
        {
            
            $sql = "INSERT into private_result (private_id,user_id)values($id,$user_id)";
            //echo $sql;
            $conn->exec($sql);
            header('Location: private-countdown.php');
    		exit;
        }
        else
        {
            echo '<script>alert("Please Enter Correct Password")</script>';
        }
        
        

    }
    
    
?>
    
<!-- ***** Search Area ***** -->

<!-- ***** Main Banner Area Start ***** -->
<div class="main-banner header-text" id="top">
    <div class="Modern-Slider">
      <!-- Item -->
      <div class="item">
        <div class="img-fill">
            <img src="assets/images/slide-03.jpg" alt="">
            <style>
                .text-content
                {
                    top:30%!important;
                    //left:50%!important;
                    margin:auto!important;
                    //background:white;
                    width:80%!important;
                    text-align:left;
                }
                
                * {
                  //box-sizing: border-box;
                }
                
                input[type=text], select, textarea {
                  width: 100%;
                  padding: 12px;
                  border: 1px solid #ccc;
                  border-radius: 4px;
                  resize: vertical;
                }
                
                label {
                  padding: 12px 12px 12px 0;
                  display: inline-block;
                }
                
                input[type=submit] {
                  background-color: #04AA6D;
                  color: white;
                  padding: 12px 20px;
                  border: none;
                  border-radius: 4px;
                  cursor: pointer;
                  float: right;
                }
                
                input[type=submit]:hover {
                  background-color: #45a049;
                }
                
                .CreateTournamentContainer {
                  border-radius: 5px;
                  background-color: #f2f2f2;
                  padding: 40px;
                }
                
                .col-25 {
                  float: left;
                  width: 25%;
                  margin-top: 6px;
                }
                
                .col-75 {
                  float: left;
                  width: 75%;
                  margin-top: 6px;
                }
                #CreateTournamentContainerSubmit
                {
                    float:right;
                }
                
                /* Clear floats after the columns */
                .row:after {
                  content: "";
                  display: table;
                  clear: both;
                }
                
                /* Responsive layout - when the screen is less than 600px wide, make the two columns stack on top of each other instead of next to each other */
                @media screen and (max-width: 600px) {
                  .col-25, .col-75, input[type=submit] {
                    width: 100%;
                    margin-top: 0;
                  }
                }
            </style>
            <div class="text-content">
                    <div class="CreateTournamentContainer">
                    <form method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]) . "?id=". $_GET['id']; ?>" >
                        <div class="row">
                          <div class="col-25">
                            <label for="tname">Password</label>
                          </div>
                          <div class="col-75">
                            <input type="text" id="tname" name="tname" placeholder="Enter Tournament Password">
                          </div>
                        </div>
                        <div class="row" id="CreateTournamentContainerSubmit">
                          <input type="submit" value="submit" name="submit" >
                        </div>
                    </form>
                    <br>
                    </div>
            
            </div>
    </div>
</div>
</div>
</div>
<!-- ***** Main Banner Area End ***** -->

<?php
    require 'user-footer.php';
?>