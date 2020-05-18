<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Login";
	$errorMessage = "";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>
	<link rel="stylesheet" href="login.css">
</head>

<body>
	<div id="outerContainer">
		<div id="mainContainer">
			<img src="/images/EMaaS2.PNG" style="width: 100%;">
			<div id="id01" class="modal">
				<form class="modal-content animate roundborder" action="/Internal/Dashboard/" method="post">
					<div class="container">
						<label for="uname"><b>Email Address</b></label>
						<input type="text" placeholder="Enter Email Address" name="uname" required>

						<label for="psw"><b>Password</b></label>
						<input type="password" placeholder="Enter Password" name="psw" required>
							
						<button type="submit">Login</button>
						<span class="psw"><a href="/Internal/ForgottenPassword/">Forgot password?</a></span>
					</div>
				</form>
			</div>
		</div>
	</div>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>