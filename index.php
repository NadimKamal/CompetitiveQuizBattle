<?php
	require 'connection.php';
	if(isset($_POST["signup"])){
	$fullname = $_POST["full_name"];
	$signup_email = $_POST["signup-email"];
	$signup_pass = $_POST["signup-pass"];
	$signup_confirm_pass = $_POST["signup-confirm-pass"];
	$institute = $_POST["institute"];
	if($fullname!="" && $signup_email!="" && $signup_pass!=""){
	if($signup_pass == $signup_confirm_pass)
	{
		//echo "matched";
		try {
			$sql = "INSERT into CM_ACCOUNT_LIST (ACCNT_NAME,EMAIL,PASS,INSTITUTE_NAME) values('$fullname', '$signup_email', '$signup_pass','$institute')";
		//echo $sql;
		$conn = Openconnection();
		$qry = sqlsrv_query($conn,$sql);
		sqlsrv_free_stmt($qry);
        sqlsrv_close($conn);
		$fullname="";
		$signup_email="";
		$signup_pass="";
		$signup_confirm_pass ="";
		$var = 'http://'.$_SERVER['HTTP_HOST'].$_SERVER['REQUEST_URI'];
        header("Location: $var");
		} catch (\Throwable $th) {
			echo '<script>alert("Email Already Exists")</script>';
		}
		
		//exit;
	}
}
	}

	else if(isset($_POST["login"]))
	{
		
		//echo $get_data;
		try{
		$login_email = $_POST["login-email"];
		$pass = $_POST["login-pass"];
		$get_data = "Select ACCNT_NAME,EMAIL,ACCNT_ID from CM_ACCOUNT_LIST where EMAIL='$login_email' and PASS='$pass'";
		$conn = Openconnection();
		$qry = sqlsrv_query($conn,$get_data);
		while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
            {
                
				session_start();
				$_SESSION["user_name"] = $result['ACCNT_NAME'];
				$_SESSION["user_email"] = $result['EMAIL'];
				$_SESSION["user_id"] = $result['ACCNT_ID'];
				//echo "geche";
				header('Location: home.php');
				exit;
            }
            sqlsrv_free_stmt($qry);
            sqlsrv_close($conn);
		} catch (\Throwable $th) {
			echo '<script>alert("Invalid UserId or Password")</script>';
		}
		
	}

?>

<html>
	<head>
		<meta name="viewport" content="width=device-width, initial-scale=1">	
		<link rel="stylesheet" type="text/css" href="login-registration.css"/>
		
	</head>
	<body>
		<section class="forms-section">
			<h1 class="section-title">Enter Information</h1>
			<div class="forms">
			  <div class="form-wrapper is-active">
				<button type="button" class="switcher switcher-login">
				  Login
				  <span class="underline"></span>
				</button>
				<form class="form form-login" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]);?>">
				  <fieldset>
					<legend>Please, enter your email and password for login.</legend>
					<div class="input-block">
					  <label for="login-email">E-mail</label>
					  <input name="login-email" id="login-email" type="email" required>
					</div>
					<div class="input-block">
					  <label for="login-password">Password</label>
					  <input name="login-pass" id="login-password" type="password" required>
					</div>
				  </fieldset>
				  <input type="submit" class="btn-login" name="login" value="Login"/>
				</form>
			  </div>
			  <div class="form-wrapper">
				<button type="button" class="switcher switcher-signup">
				  Sign Up
				  <span class="underline"></span>
				</button>
				<form class="form form-signup" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]);?>">
				  <fieldset>
					<legend>Please, enter your email, password and Institute name for sign up.</legend>
					<div class="input-block">
						<label for="signup-email">Full Name</label>
						<input name="full_name" id="signup-email" type="text" required>
					  </div>
					<div class="input-block">
					  <label for="signup-email">E-mail</label>
					  <input name="signup-email" id="signup-email" type="email" required>
					</div>
					<div class="input-block">
					  <label for="signup-password">Password</label>
					  <input name="signup-pass" id="signup-password" type="password" required>
					</div>
					<div class="input-block">
					  <label for="signup-password-confirm">Confirm Password</label>
					  <input name="signup-confirm-pass" id="signup-password-confirm" type="password" required>
					</div>
					<div class="input-block">
					  <label for="institute">Institute Name</label>
					  <input name="institute" id="institute" type="text" required>
					</div>
				  </fieldset>
				  <input type="submit" class="btn-signup" name="signup" value="SignUp"/>
				</form>
			  </div>
			</div>
		  </section>

</body>
<script type="text/javascript" src="login-registration.js"></script>
</html>