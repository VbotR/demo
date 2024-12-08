using Microsoft.Maui.Controls;
using demo.Data;
using demo.Models;
using System.Linq;

namespace demo.Pages;

public partial class ProfilePage : ContentPage
{
    private string _username;

    public ProfilePage(string username)
    {
        InitializeComponent();
        _username = username ?? throw new ArgumentNullException(nameof(username));

        WelcomeLabel.Text = $"Welcome, {_username}!";
        LoadSurveys(); // Загрузка опросов
    }

    private void LoadSurveys()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                // Получаем список опросов, созданных пользователем
                var surveys = db.Surveys
                                .Where(s => s.CreatedBy == _username)
                                .OrderByDescending(s => s.CreatedAt) // Сортируем по дате создания
                                .ToList();

                // Привязываем данные к CollectionView
                SurveysCollectionView.ItemsSource = surveys;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading surveys: {ex.Message}");
        }
    }
}
