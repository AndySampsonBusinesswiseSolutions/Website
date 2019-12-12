function initializeMap(address) {
    var geocoder = new google.maps.Geocoder();
    var mapOptions = {
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.SATELLITE,
        disableDefaultUI: true
    }
    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    
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