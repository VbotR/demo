using demo.Data;
using demo.Models;
using demo.Pages;
using Microsoft.Maui.Controls;

namespace demo.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please fill in both fields.", "OK");
            return;
        }

        try
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    Preferences.Set("CurrentUserId", user.Id);
                    Application.Current.MainPage = new NavigationPage(new MenuPage(user.Username));
                }
                else
                {
                    await DisplayAlert("Error", "Invalid username or password.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
            await DisplayAlert("Error", "An error occurred during login.", "OK");
        }
    }
}
