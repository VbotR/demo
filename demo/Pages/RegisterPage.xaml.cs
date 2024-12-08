using demo.Data;
using demo.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace demo.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            using (var db = new AppDbContext())
            {
                if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text))
                {
                    await DisplayAlert("Error", "All fields are required.", "OK");
                    return;
                }

                if (db.Users.Any(u => u.Username == UsernameEntry.Text))
                {
                    await DisplayAlert("Error", "This username is already taken.", "OK");
                    return;
                }

                var user = new User
                {
                    Username = UsernameEntry.Text,
                    Password = PasswordEntry.Text,
                    Email = EmailEntry.Text
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                Preferences.Set("CurrentUserId", user.Id);

                await DisplayAlert("Success", "Account created successfully.", "OK");
                Application.Current.MainPage = new NavigationPage(new MenuPage(user.Username));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during registration: {ex.Message}");
            await DisplayAlert("Error", "An error occurred during registration.", "OK");
        }
    }
}
