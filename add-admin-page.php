<?php
require 'admin-header.php';
require 'connection.php';
	if($_POST["add-admin"]){
	$name = $_POST["name"];
	$email = $_POST["email"];
	$pass = $_POST["password"];
    $username = $_POST["username"];
    $details = $_POST["details"];
		/*echo $name;
        echo $username;
        echo $email;
        echo $pass;
        echo $details;*/
		$sql = "INSERT into AmdinTable (name,username,email,password,details) values('$name', '$username', '$email', '$pass', '$details')";
		$conn->exec($sql);
		$var = 'http://'.$_SERVER['HTTP_HOST'].$_SERVER['REQUEST_URI'];
        header("Location: $var");
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
                        <label for="qdesc">Admin Details</label>
                      </div>
                      <div class="col-75">
                        <textarea id="qdesc" name="details" placeholder="Write Admin Details" style="height:100px"></textarea>
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="a">Name</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="a" name="name" placeholder="Enter Full Name">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="b">User Name</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="b" name="username" placeholder="Enter Unique UserName">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="c">Email</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="c" name="email" placeholder="Enter Unique Email">
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-25">
                        <label for="d">Password</label>
                      </div>
                      <div class="col-75">
                        <input type="text" id="d" name="password" placeholder="Enter Password">
                      </div>
                    </div>
                    <div class="row" id="QuestionContainerSubmit">
                      <input name="add-admin" type="submit" value="Add" >
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