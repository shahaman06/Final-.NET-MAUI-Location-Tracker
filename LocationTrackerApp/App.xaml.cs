using Microsoft.Maui.Controls;

namespace LocationTrackerApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Set the MainPage of the app
        MainPage = new NavigationPage(new Pages.MainPage());
    }
}
