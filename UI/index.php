<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Login";
	$errorMessage = "";
	include($_SERVER['DOCUMENT_ROOT']."/includes/_navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>
	<link rel="stylesheet" href="/index.css">
</head>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
	<div class="login-body">                    
		<?php if ($errorMessage != '') { ?>
			<div id="login-alert" class="alert alert-danger col-sm-12"><?php echo $errorMessage; ?></div>                            
		<?php } ?>
		<form method="POST" action='/Dashboard/'>                                    
			<div style="margin-bottom: 25px">
				<input type="text" class="form-control" id="loginId" name="loginId"  value="<?php if(isset($_COOKIE["loginId"])) { echo $_COOKIE["loginId"]; } ?>" placeholder="email">                                        
			</div>                                
			<div style="margin-bottom: 25px">
				<input type="password" class="form-control" id="loginPass" name="loginPass" value="<?php if(isset($_COOKIE["loginPass"])) { echo $_COOKIE["loginPass"]; } ?>" placeholder="password">
			</div>
			<table class="form-control">
				<tr>
					<td>
						<label>
							<input type="checkbox" id="remember" name="remember" <?php if(isset($_COOKIE["loginId"])) { ?> checked <?php } ?>> Remember me
						</label>
					</td>
					<td style="width:30%"></td>
					<td style="width:20%" rowspan=2 >
						<input style="width:100%" type="submit" name="login" value="Login" class="btn">
					</td>
				</tr>
				<tr>
					<td valign=middle>
						<a href="#">Forgot Password</a>
					</td>
					<td style="width:30%"></td>
				</tr>
			</table>                           
		</form>
	</div>
	<br>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/_footer/footer.php");?>