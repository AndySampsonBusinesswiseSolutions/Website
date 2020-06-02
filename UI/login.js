const uri = 'http://localhost:5000/Website';

function login(event) {
  errorMessage.style.display = 'none';
  event.preventDefault();

  window.location.href = "http://energyportal/Internal/Dashboard/";
    return true;

  // return postData({ PageGUID: "6641A1BF-84C8-48F8-9D79-70D0AB2BB787", Process: "Login", Data: {EmailAddress: "test", Password: "test"} })
  // .then(response => {
  //   processResponse(response);
  // });
}

function showLoader(show) {
  overlay.style.display = show ? '' : 'none';
}

function processResponse(response) {
  if(response && response.ok) {
    window.location.href = "http://energyportal/Internal/Dashboard/";
    return true;
  }
  
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