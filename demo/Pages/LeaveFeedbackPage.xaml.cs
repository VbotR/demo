using demo.Data;
using demo.Models;

namespace demo.Pages;

public partial class LeaveFeedbackPage : ContentPage
{
    private string _username;

    public LeaveFeedbackPage(string username)
    {
        InitializeComponent();
        _username = username ?? throw new ArgumentNullException(nameof(username));
    }

    private async void OnSubmitFeedbackClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FeedbackEditor.Text))
        {
            await DisplayAlert("Error", "Feedback cannot be empty.", "OK");
            return;
        }

        try
        {
            using (var db = new AppDbContext())
            {
                var feedback = new Feedback
                {
                    Username = _username,
                    Content = FeedbackEditor.Text
                };

                db.Feedbacks.Add(feedback);
                await db.SaveChangesAsync();

                await DisplayAlert("Success", "Feedback submitted successfully!", "OK");
                FeedbackEditor.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
