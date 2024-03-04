<?php
require 'user-header.php';

    $email = $_SESSION['user_email'];
      $result="";
      $name="";
      $email="";
      $ins="";
      $pass="";
      $details="";
    try{
      $get_data = "SELECT * FROM CM_ACCOUNT_LIST WHERE EMAIL='" . $email . "'";
      $conn = Openconnection();
      $qry = sqlsrv_query($conn,$get_data);
      $result="";
      $name="";
      $email="";
      $ins="";
      $pass="";
      while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
        {
          $name = $result['ACCNT_NAME'];
          $pass = $result['PASS'];
          $ins=$result['INSTITUTE_NAME'];
          $details=$result['INSTITUTE_NAME'];
        }
        sqlsrv_free_stmt($qry);
        sqlsrv_close($conn);
      } catch (\Throwable $th) {
        
      }
    try{
	
   
    if(isset($_POST["add-admin"])){
    	$name = $_POST["name"];
    	$email = $_POST["email"];
    	$pass = $_POST["password"];
        $ins = $_POST["username"];
		
		$sql = "update CM_ACCOUNT_LIST SET ACCNT_NAME = '".$name."',INSTITUTE_NAME= '".$ins."',PASS= '".$pass."' WHERE EMAIL='" . $_SESSION['user_email'] . "'";
		ExecuteNonQuery($sql);
		
	    }
    }
    catch(Exception $e)
    {
        echo $e;
    }
   
?>

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
                  <form method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" >
                    <div class="row">
                      <div class="col-25">
                        <label for="a">Name</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="a" name="name" value="<?php echo $name ?>" placeholder="Enter Full Name">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="b">Institute</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="b" name="username" value="<?php echo $details ?>" placeholder="Enter Institute Name">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="c">Email</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="c" name="email" value="<?php echo $_SESSION['user_email'] ?>" readonly placeholder="Enter Unique Email">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="d">Password</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="d" name="password" value="<?php echo $pass ?>" placeholder="Enter Password">
                      </div>
                    </div>
                    <div class="row" id="QuestionContainerSubmit">
                      <input name="add-admin" type="submit" value="Submit" >
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