<?php
	require 'connection.php';
	if($_POST["login"])
	{
		$login_email = $_POST["login-email"];
		$pass = $_POST["login-pass"];
		$get_data = "Select name,email,password from AmdinTable where email='$login_email' && password='$pass'";
		$qry = $conn->prepare($get_data);
		$qry->execute();
		//echo $get_data;
		$cnt = $qry->rowCount();
		$result = $qry->fetchAll();
		//echo $result[0][0];
		if($cnt==1)
		{
			session_start();
			$_SESSION["admin_name"] = $result[0][0];
			$_SESSION["admin_email"] = $result[0][1];
			//echo "geche";
			header('Location: admin-home.php');
			exit;
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
			<div class="forms">
			  <div class="form-wrapper is-active">
				<button type="button" class="switcher switcher-login">
				  Login
				  <span class="underline"></span>
				</button>
				<form class="form form-login" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]);?>">
				  <fieldset>
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
			</div>
		  </section>

</body>
<script type="text/javascript" src="login-registration.js"></script>
</html>