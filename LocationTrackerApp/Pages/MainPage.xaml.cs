using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps; // For Map and MapSpan
using Microsoft.Maui.Maps;
using SQLite;

namespace LocationTrackerApp.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnTrackLocationClicked(object sender, EventArgs e)
        {
            var deviceLocation = await GetCurrentLocationAsync(); // Using Microsoft.Maui.Devices.Sensors.Location
            if (deviceLocation != null)
            {
                // Show the location on the map
                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Microsoft.Maui.Controls.Maps.Position(deviceLocation.Latitude, deviceLocation.Longitude),
                    Distance.FromMiles(1)));

                // Save the location to the database
                var locationDatabase = new LocationDatabase("locations.db");
                await locationDatabase.SaveLocationAsync(new Location
                {
                    Latitude = deviceLocation.Latitude,
                    Longitude = deviceLocation.Longitude,
                    Timestamp = DateTime.Now
                });
            }
            else
            {
                await DisplayAlert("Error", "Unable to get location.", "OK");
            }
        }

        public async Task<Microsoft.Maui.Devices.Sensors.Location> GetCurrentLocationAsync()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High));
            }
            return location;
        }
    }

    // Location class to represent each location
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
    }

    // Database helper class to interact with SQLite
    public class LocationDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public LocationDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Location>().Wait();
        }

        public Task<List<Location>> GetLocationsAsync() => _database.Table<Location>().ToListAsync();
        public Task<int> SaveLocationAsync(Location location) => _database.InsertAsync(location);
    }
}
