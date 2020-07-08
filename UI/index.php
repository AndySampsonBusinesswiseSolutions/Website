<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Login";
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
				<form name="loginForm" class="modal-content animate" onsubmit="return login(event)">
					<div class="container">
						<label for="userName"><b>Email Address</b></label>
						<input type="text" placeholder="Enter Email Address" id="userName" required>

						<label for="password"><b>Password</b></label>
						<input type="password" placeholder="Enter Password" id="password" required>
							
						<button type="submit" onclick="showOverlay(true); ">Login</button>
						<span class="psw"><a href="/Internal/ForgottenPassword/">Forgot password?</a></span>
						<div id="errorMessage" style="display: none; color: red;"></div>
					</div>
				</form>
			</div>
		</div>
		<div id="overlay" style="display: none;">
			<div class="loader"></div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>
<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>