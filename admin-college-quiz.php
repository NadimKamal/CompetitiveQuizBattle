<?php
require 'admin-header.php';
require 'connection.php';

    //echo "geche";
    //echo "Print";
    if(isset($_POST["submit"])){
    $question = $_POST["question"];
    $a = $_POST["a"];
    $b = $_POST["b"];
    $c = $_POST["c"];
    $d = $_POST["d"];
    $correct = $_POST["correct"];
    $subject = $_GET['subject'];
    
    $sql = "INSERT into college (subject_id,question,a,b,c,d,correct) values($subject,'$question','$a','$b','$c','$d','$correct')";
    $conn->exec($sql);
    $var = 'http://'.$_SERVER['HTTP_HOST'].$_SERVER['REQUEST_URI'];
    //echo $var;
    header("Location: $var");
    /*echo $question;
    echo $a;
    echo $b;
    echo $c;
    echo $d;
    echo $correct;
    //echo $class;
    echo $subject;*/
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
                    top:55%!important;
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
                  box-sizing: border-box;
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
                
                .QuestionContainer {
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
                
                /* Clear floats after the columns */
                .row:after {
                  content: "";
                  display: table;
                  clear: both;
                }
                #QuestionContainerSubmit
                {
                    float:right;
                }
                
                /* Responsive layout - when the screen is less than 600px wide, make the two columns stack on top of each other instead of next to each other */
                @media screen and (max-width: 600px) {
                  .col-25, .col-75, input[type=submit] {
                    width: 100%;
                    margin-top: 0;
                  }
                }
                </style>
                
                <div class="QuestionContainer">
                  <form class="form form-signup" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]).'?subject='.$_GET['subject'];?>">
                    <div class="row">
                      <div class="col-25">
                        <label for="qdesc">Question Desc</label>
                      </div>
                      <div class="col-75">
                        <textarea id="qdesc" name="question" placeholder="Write Question Desc" style="height:100px"></textarea>
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="a">A</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="a" name="a" placeholder="Option A">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="b">B</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="b" name="b" placeholder="Option B">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="c">C</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="c" name="c" placeholder="Option C">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="d">D</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="d" name="d" placeholder="Option D">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="curr">Correct</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="curr" name="correct" placeholder="Correct Option( Ex: A )">
                      </div>
                    </div>
                    <div class="row" id="QuestionContainerSubmit">
                      <input name="submit" type="submit" value="Submit" >
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
require 'admin-footer.php';
?>