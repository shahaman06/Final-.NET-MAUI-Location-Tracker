using SQLite;

namespace LocationTrackerApp.Data;

public class LocationDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public LocationDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Models.LocationModel>().Wait();
    }

    public Task<List<Models.LocationModel>> GetLocationsAsync() =>
        _database.Table<Models.LocationModel>().ToListAsync();

    public Task<int> SaveLocationAsync(Models.LocationModel location) =>
        _database.InsertAsync(location);
}
