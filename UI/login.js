const uri = 'https://localhost:5000/api/WeatherForecast';
let todos = [];

function getItems() {
  fetch(uri)
    .then(response => response.json())
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.', error));
}

function _displayItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    data.forEach(item => {
      let tr = tBody.insertRow();
      let textNode1 = document.createTextNode(item.date);
      let textNode2 = document.createTextNode(item.temperatureC);
      let textNode3 = document.createTextNode(item.summary);
      
      let td1 = tr.insertCell(0);
      td1.appendChild(textNode1);
  
      let td2 = tr.insertCell(1);
      td2.appendChild(textNode2);
  
      let td3 = tr.insertCell(2);
      td3.appendChild(textNode3);
    });
  
    todos = data;
  }