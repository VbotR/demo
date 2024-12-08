using Microsoft.Maui.Controls;
using demo.Data;
using demo.Models;
using Microsoft.Maui.Storage;

namespace demo.Pages;

public partial class SurveysPage : ContentPage
{
    public SurveysPage()
    {
        InitializeComponent();
        LoadSurveys();
    }

    private async void LoadSurveys()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                // Получаем список всех опросов из базы данных
                var surveys = db.Surveys.ToList();
                SurveysCollectionView.ItemsSource = surveys;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnSurveyTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Survey selectedSurvey) // Проверяем, что параметр - это объект Survey
        {
            Console.WriteLine($"Navigating to survey: {selectedSurvey.Title}");

            // Получаем ID текущего пользователя
            int userId = GetCurrentUserId();

            if (userId == 0)
            {
                // Если пользователь не авторизован, показываем сообщение
                await DisplayAlert("Error", "User not logged in. Please log in to vote.", "OK");
                return;
            }

            // Переход на страницу голосования с surveyId и userId
            await Navigation.PushAsync(new VotePage(selectedSurvey.Id, userId));
        }
        else
        {
            Console.WriteLine("Tapped event did not receive a valid Survey object.");
            await DisplayAlert("Error", "Unable to open the survey. Please try again.", "OK");
        }
    }

    private int GetCurrentUserId()
    {
        // Получаем ID текущего пользователя
        int userId = Preferences.Get("CurrentUserId", 0); // Возвращает 0, если пользователь не найден
        Console.WriteLine($"Retrieved CurrentUserId: {userId}");
        return userId;
    }
}
