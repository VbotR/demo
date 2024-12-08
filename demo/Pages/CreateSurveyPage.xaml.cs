using System.Diagnostics;
using Microsoft.Maui.Controls;
using demo.Data;
using demo.Models;

namespace demo.Pages;

public partial class CreateSurveyPage : ContentPage
{
    private string _username = string.Empty; // Инициализация поля для устранения предупреждения CS8618

    public CreateSurveyPage(string username)
    {
        try
        {
            InitializeComponent(); // Инициализация компонентов XAML
            _username = username ?? throw new ArgumentNullException(nameof(username)); // Проверка, что username не равен null
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing CreateSurveyPage: {ex.Message}");
            Application.Current.MainPage.DisplayAlert("Error", "Failed to load the Create Survey page.", "OK");
        }
    }

    private async void OnCreateSurveyClicked(object sender, EventArgs e)
    {
        try
        {
            // Проверка на null для элементов TitleEntry и DescriptionEditor
            if (TitleEntry?.Text == null || DescriptionEditor?.Text == null)
            {
                await DisplayAlert("Error", "Please fill in both fields", "OK");
                return;
            }

            var title = TitleEntry.Text.Trim();
            var description = DescriptionEditor.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            {
                await DisplayAlert("Error", "Please fill in both fields", "OK");
                return;
            }

            using (var db = new AppDbContext())
            {
                // Создаём объект Survey
                var survey = new Survey
                {
                    Title = title,
                    Description = description,
                    CreatedBy = _username,
                    CreatedAt = DateTime.Now
                };

                db.Surveys.Add(survey); // Добавляем новый опрос
                await db.SaveChangesAsync(); // Сохраняем изменения в базу данных
            }

            await DisplayAlert("Success", "Survey created successfully", "OK");
            await Navigation.PopAsync(); // Возвращаемся назад в меню
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating survey: {ex.Message}");
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
