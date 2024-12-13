// Ensure the Google Maps API is loaded before executing this script
function initializeHeatmap(map, locations) {
    // Convert locations into a format suitable for heatmap
    const heatmapData = locations.map(location => {
        return new google.maps.LatLng(location.latitude, location.longitude);
    });

    // Create a heatmap layer
    const heatmap = new google.maps.visualization.HeatmapLayer({
        data: heatmapData,
        map: map, // The map where the heatmap will be rendered
        radius: 20, // Adjust the radius of the heat points
        opacity: 0.6, // Set the opacity level for the heatmap
    });

    heatmap.setMap(map);
}

// Function to be called from .NET MAUI to trigger heatmap initialization
function renderHeatmap(map, locations) {
    initializeHeatmap(map, locations);
}
