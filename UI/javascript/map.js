var geocoder;
var map;
function initializeMap(postCode) {
    geocoder = new google.maps.Geocoder();
        var latlng = new google.maps.LatLng(-34.397, 150.644);
        var mapOptions = {
        zoom: 16,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.SATELLITE,
        disableDefaultUI: true
        }
        map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
        codeAddress(postCode);
}

function codeAddress(postCode) {
    var address = postCode;
    geocoder.geocode( { 'address': address}, function(results, status) {
    if (status == google.maps.GeocoderStatus.OK) {
        map.setCenter(results[0].geometry.location);
        var marker = new google.maps.Marker({
            map: map,
            position: results[0].geometry.location
        });
    } else {
        alert('Geocode was not successful for the following reason: ' + status);
    }
    });
}