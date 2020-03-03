<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Forgotten Password";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="forgottenpassword.css">
</head>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div style="text-align: center;">
		<br>
		<div id="EnterEmailAddress" onclick="sendVerificationCode()">
			<div>
				<label for="emailAddress">Enter email address:</label>
				<input id="emailAddress" placeholder="Email Address"></input><br><br>
				<button>Submit email address</button><br>
			</div>
		</div>
		<div id="EnterVerificationCode" style="display: none;" onclick="resetPassword()">
			<div>
				<label for="verificationCode">Enter verification code:</label>
				<input id="verificationCode1"></input><span>-</span><input id="verificationCode2"></input><br><br>
				<button>Submit verification code</button><br>
			</div>
		</div>
		<div id="ResetPassword" style="display: none;">
			<div>
				<label for="password" style="padding-left: 18px;">Enter password:</label>
				<input id="password" placeholder="Set Password"></input><br>
				<label for="confirmPassword">Confirm password:</label>
				<input id="confirmPassword" placeholder="Confirm Password"></input><br><br>
				<button>Reset password and Login</button><br>
			</div>
		</div>
	</div>
	<br>
</body>

<script>
	function sendVerificationCode() {
		var emailAddressDiv = document.getElementById('EnterEmailAddress');
		emailAddressDiv.style.display = "none";

		var verificationCodeDiv = document.getElementById('EnterVerificationCode');
		verificationCodeDiv.style.display = "";
	}

	function resetPassword() {
		var verificationCodeDiv = document.getElementById('EnterVerificationCode');
		verificationCodeDiv.style.display = "none";

		var resetPasswordDiv = document.getElementById('ResetPassword');
		resetPasswordDiv.style.display = "";
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>