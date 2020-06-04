const uri = 'http://localhost:5000/Website';

function login(event) {
  errorMessage.style.display = 'none';
  event.preventDefault();

  var emailAddress = document.getElementById('userName').value;
  var password = document.getElementById('password').value;

  if(emailAddress == "andy.sampson@businesswise.co.uk"
    && password == "URLTesting") {
      postData(
        {
          QueueGUID: CreateGUID(), 
          PageGUID: "6641A1BF-84C8-48F8-9D79-70D0AB2BB787", 
          Process: "Login", 
          EmailAddress: emailAddress, 
          Password: password
        }
      ).then(response => {
        processResponse(response);
      });;

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
  // if(response && response.ok) {
  //   window.location.href = "/Internal/Dashboard/";
  //   return true;
  // }
  
  errorMessage.style.display = '';
  showLoader(false);
  return false;
}

async function postData(data) {
  try {
    const response = await fetch(uri + '/Validate', {
      method: 'POST',
      mode: 'cors',
      cache: 'no-cache',
      credentials: 'same-origin',
      headers: {
        'Content-Type': 'application/json',
      },
      redirect: 'follow',
      referrerPolicy: 'no-referrer',
      body: JSON.stringify(data)
    });
  
    return response;
  }
  catch {
    return null;
  }
}