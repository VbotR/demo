using Microsoft.Maui.Controls;
using demo.Pages;

namespace demo.Pages;

public partial class StartPage : ContentPage
{
    public StartPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            Console.WriteLine("Navigating to LoginPage...");
            if (Navigation != null)
            {
                // Переход на страницу входа
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                Console.WriteLine("Navigation is null. Cannot navigate to LoginPage.");
                await DisplayAlert("Error", "Navigation system is not initialized.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error navigating to LoginPage: {ex.Message}");
            await DisplayAlert("Error", "An error occurred while navigating to the Login page.", "OK");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            Console.WriteLine("Navigating to RegisterPage...");
            if (Navigation != null)
            {
                // Переход на страницу регистрации
                await Navigation.PushAsync(new RegisterPage());
            }
            else
            {
                Console.WriteLine("Navigation is null. Cannot navigate to RegisterPage.");
                await DisplayAlert("Error", "Navigation system is not initialized.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error navigating to RegisterPage: {ex.Message}");
            await DisplayAlert("Error", "An error occurred while navigating to the Register page.", "OK");
        }
    }
}
