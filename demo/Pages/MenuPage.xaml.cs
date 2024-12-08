using Microsoft.Maui.Controls;
using demo.Pages;
using Microsoft.Maui.Storage;

namespace demo.Pages
{
    public partial class MenuPage : ContentPage
    {
        private readonly string _username;

        public MenuPage(string username) 
        {
            InitializeComponent();
            _username = username; // Сохраняем имя пользователя
        }

        private async void OnViewProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(_username));
        }

        private async void OnCreateSurveyClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateSurveyPage(_username));
        }

        private async void OnLeaveFeedbackClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LeaveFeedbackPage(_username));
        }

        private async void OnViewSurveysClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewSurveysPage());
        }

        private async void OnViewFeedbacksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewFeedbacksPage());
        }

        private async void OnLogOutClicked(object sender, EventArgs e)
        {
            // Удаляем данные о текущем пользователе из Preferences
            Preferences.Remove("CurrentUserId");

            // Перенаправляем пользователя на страницу выбора "Register or Login"
            Application.Current.MainPage = new NavigationPage(new StartPage());
            await DisplayAlert("Logged Out", "You have successfully logged out.", "OK");
        }
    }
}
