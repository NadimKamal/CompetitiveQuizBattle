<?php
    
    require 'user-header.php';
    require_once 'php_function.php';
    
    if(isset($_POST["submit"]))
    {
       
        $tname = $_POST["tname"];
        $pass = $_POST["pass"];
        $startTime = $_POST["startTime"];
        $dateS = strtotime($startTime);
        $dateSS=date('d/M/Y h:i:s', $dateS);
        $endTime = $_POST["endTime"];
        $dateE = strtotime($endTime);
        $dateEE=date('d/M/Y h:i:s', $dateE);
        $qnum = $_POST["qnum"];
       
        $createdBy = $_SESSION["user_id"];
        
        $sql = "INSERT into CreateTournament (private_name,password,start_time,end_time,question_num,created_by)values('$tname','$pass','$dateSS','$dateEE','$qnum','$createdBy')";
        //echo $sql;
        ExecuteNonQuery($sql);
        header('Location: add_private_quiz.php');
		    exit;

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
                    top:50%!important;
                    //left:50%!important;
                    margin:auto!important;
                    //background:white;
                    width:80%!important;
                    text-align:left;
                }
            </style>
            <div class="text-content">
                <style>
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
                
                <div class="CreateTournamentContainer">
                  <form method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" >
                    <div class="row">
                      <div class="col-25">
                        <label for="tname">Name</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="tname" name="tname" placeholder="Enter Tournament Name">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="pass">Password</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="pass" name="pass" placeholder="Enter Tournament Password">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="startTime">Start Time</label>
                      </div>
                      <div class="col-75">
                        <input type="datetime-local" id="startTime" name="startTime">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="endTime">End Time</label>
                      </div>
                      <div class="col-75">
                        <input type="datetime-local" id="endTime" name="endTime">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="qnum">No. of Question</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="qnum" name="qnum" placeholder="Enter Number of Question">
                      </div>
                    </div>
                    <div class="row" id="CreateTournamentContainerSubmit">
                      <input type="submit" value="submit" name="submit" >
                    </div>
                  </form>
                  <br />
                </div>

            </div>
        </div>
      </div>
      <!-- // Item -->
    </div>
</div>
<!-- ***** Main Banner Area End ***** -->

<?php
    require 'user-footer.php';
?>