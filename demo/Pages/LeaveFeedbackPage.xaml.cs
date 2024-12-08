using Microsoft.Maui.Controls;

namespace demo.Pages;

public partial class LeaveFeedbackPage : ContentPage
{
    private string _username = string.Empty; // Инициализация

    public LeaveFeedbackPage(string username)
    {
        InitializeComponent();
        _username = username ?? throw new ArgumentNullException(nameof(username)); // Проверка аргумента
    }
}
