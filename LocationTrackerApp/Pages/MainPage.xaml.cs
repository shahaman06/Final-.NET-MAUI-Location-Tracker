using Microsoft.Maui.Controls.Maps;
using LocationTrackerApp.Data;
using LocationTrackerApp.Services;

namespace LocationTrackerApp.Pages;

public partial class MainPage : ContentPage
{
    private readonly LocationDatabase _database;
    private readonly LocationService _locationService;

    public MainPage(LocationDatabase database, LocationService locationService)
    {
        InitializeComponent();
        _database = database;
        _locationService = locationService;
    }

    private async void OnTrackLocationClicked(object sender, EventArgs e)
    {
        var location = await _locationService.GetCurrentLocationAsync();
        if (location != null)
        {
            await _database.SaveLocationAsync(new Models.LocationModel
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Timestamp = DateTime.Now
            });

            await DisplayAlert("Success", "Location saved!", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Unable to get location.", "OK");
        }
    }

    private async void OnShowHeatMapClicked(object sender, EventArgs e)
    {
        HeatMap.IsVisible = true;

        // Get all saved locations from the database
        var locations = await _database.GetLocationsAsync();
        if (locations.Count == 0)
        {
            await DisplayAlert("No Locations", "No locations to display for the heatmap.", "OK");
            return;
        }

        // Create a list of location objects for heatmap rendering
        var locationList = new List<object>();
        foreach (var loc in locations)
        {
            locationList.Add(new
            {
                latitude = loc.Latitude,
                longitude = loc.Longitude
            });
        }

        // Call JavaScript to render the heatmap
        await Browser.EvaluateJavaScriptAsync("renderHeatmap(arguments[0], arguments[1]);", HeatMap, locationList);
    
    }
}
