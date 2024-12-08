using demo.Data;
using demo.Models;

namespace demo.Pages;

public partial class ViewFeedbacksPage : ContentPage
{
    public ViewFeedbacksPage()
    {
        InitializeComponent();
        LoadFeedbacks();
    }

    private async void LoadFeedbacks()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                var feedbacks = db.Feedbacks
                                  .OrderByDescending(f => f.CreatedAt)
                                  .ToList();

                FeedbackCollectionView.ItemsSource = feedbacks;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
