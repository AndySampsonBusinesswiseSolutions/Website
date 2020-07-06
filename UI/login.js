function login(event) {
  errorMessage.style.display = 'none';
  event.preventDefault();

  var emailAddress = document.getElementById('userName').value;
  var password = document.getElementById('password').value;

  if(emailAddress == "andy.sampson@businesswisesolutions.co.uk"
    && password == "URLTesting") {
      var processQueueGUID = CreateGUID();

      var postSuccessful = postData(
        {
          ProcessQueueGUID: processQueueGUID, 
          PageGUID: "6641A1BF-84C8-48F8-9D79-70D0AB2BB787", 
          ProcessGUID: "AF10359F-FD78-4345-9F26-EF5A921E72FD", 
          EmailAddress: emailAddress, 
          Password: password
        }
      );

      if(postSuccessful) {
        getProcessResponse(processQueueGUID)
        .then(response => {
          processResponse(response);
        })
      }
      else {
        processResponse(null);
      }

      return false;
  }
  else {
    window.location.href = "/Internal/Dashboard/";
    return true;
  }
}

function showLoader(show) {
  var emailAddress = document.getElementById('userName').value;
  var password = document.getElementById('password').value;

  if(!emailAddress && !password) {
    show = false;
  }

  overlay.style.display = show ? '' : 'none';
}

function processResponse(response) {
  errorMessage.style.display = '';

  if(response) {
    if(response.message == "OK") {
      errorMessage.innerText = 'Login OK';
      //window.location.href = "/Internal/Dashboard/";
      // return true;
    }
    else if(response.status == 401) {
      errorMessage.innerText = "Email address/Password combination invalid or account has been locked";
    }
    else {
      errorMessage.innerText = "Sorry, we could not log you in at this time. Please try again later";
    }
  }
  else {
    errorMessage.innerText = 'A system error occurred. Please contact System@BusinessWiseSolutions.co.uk quoting reference ERR7426424';
  }

  showLoader(false);
  return false;
}