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

	<style>
		/* Full-width input fields */
		input[type=text], input[type=password] {
		width: 100%;
		padding: 12px 20px;
		margin: 8px 0;
		display: inline-block;
		border: 1px solid #ccc;
		box-sizing: border-box;
		}

		/* Set a style for all buttons */
		button {
		background-color: #4CAF50;
		color: white;
		padding: 14px 20px;
		margin: 8px 0;
		border: none;
		cursor: pointer;
		width: 50%;
		}

		button:hover {
		opacity: 0.8;
		}

		span.psw {
		float: right;
		padding-top: 16px;
		}

		/* The Modal (background) */
		.modal {
		position: fixed; /* Stay in place */
		z-index: 1; /* Sit on top */
		left: 0;
		top: 0;
		width: 100%; /* Full width */
		height: 100%; /* Full height */
		overflow: auto; /* Enable scroll if needed */
		background-color: rgb(0,0,0); /* Fallback color */
		background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
		}

		/* Modal Content/Box */
		.modal-content {
		background-color: #fefefe;
		margin-right: auto;
		margin-left: 2.5%;
		margin-top: 7.5%;
		border: 1px solid #888;
		width: 40%; /* Could be more or less, depending on screen size */
		height: 30%;
		}

		/* Add Zoom Animation */
		.animate {
		-webkit-animation: animatezoom 0.6s;
		animation: animatezoom 0.6s
		}

		@-webkit-keyframes animatezoom {
		from {-webkit-transform: scale(0)} 
		to {-webkit-transform: scale(1)}
		}
		
		@keyframes animatezoom {
		from {transform: scale(0)} 
		to {transform: scale(1)}
		}
	</style>
</head>

<body>
	<div id="outerContainer">
		<div id="mainContainer">
				<!-- <table>
					<tr>
						<th>Date</th>
						<th>Temperature</th>
						<th>Summary</th>
					</tr>
					<tbody id="todos"></tbody>
				</table> -->
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