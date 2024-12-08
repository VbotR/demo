using demo.Data;
using demo.Models;
using Microsoft.Maui.Controls;
using System.Linq;

namespace demo.Pages
{
    public partial class VotePage : ContentPage
    {
        private readonly int _surveyId; // ID опроса
        private readonly int _userId; // ID пользователя

        public VotePage(int surveyId, int userId)
        {
            InitializeComponent();
            _surveyId = surveyId;
            _userId = userId;

            // Загружаем детали опроса при загрузке страницы
            LoadSurveyDetails();
        }

        private void LoadSurveyDetails()
        {
            using (var db = new AppDbContext())
            {
                var survey = db.Surveys.FirstOrDefault(s => s.Id == _surveyId);

                if (survey != null)
                {
                    SurveyTitle.Text = survey.Title;
                    SurveyDescription.Text = survey.Description;

                    // Если пользователь уже голосовал, сразу показываем результаты
                    if (HasUserVoted(survey))
                    {
                        ShowResults(survey);
                    }
                }
            }
        }

        private async void OnVoteYesClicked(object sender, EventArgs e)
        {
            await RecordVote(true);
        }

        private async void OnVoteNoClicked(object sender, EventArgs e)
        {
            await RecordVote(false);
        }

        private async Task RecordVote(bool isYes)
        {
            using (var db = new AppDbContext())
            {
                var survey = db.Surveys.FirstOrDefault(s => s.Id == _surveyId);

                if (survey != null)
                {
                    // Проверяем, голосовал ли пользователь
                    if (HasUserVoted(survey))
                    {
                        await DisplayAlert("Info", "You have already voted in this survey.", "OK");
                        return;
                    }

                    // Обновляем данные голосования
                    if (isYes)
                    {
                        survey.VotesYes++;
                    }
                    else
                    {
                        survey.VotesNo++;
                    }

                    // Добавляем ID пользователя в список проголосовавших
                    survey.VotedUserIds += $"{_userId},";

                    // Сохраняем изменения в базе данных
                    db.SaveChanges();

                    // Показываем результаты голосования
                    ShowResults(survey);
                }
            }
        }

        private bool HasUserVoted(Survey survey)
        {
            if (string.IsNullOrEmpty(survey.VotedUserIds))
            {
                return false;
            }

            var votedUserIds = survey.VotedUserIds.Split(',').Where(id => !string.IsNullOrEmpty(id)).Select(int.Parse).ToList();
            return votedUserIds.Contains(_userId);
        }

        private void ShowResults(Survey survey)
        {
            int totalVotes = survey.VotesYes + survey.VotesNo;

            if (totalVotes == 0)
            {
                ResultsLabel.Text = "No votes yet.";
            }
            else
            {
                double yesPercentage = (double)survey.VotesYes / totalVotes * 100;
                double noPercentage = (double)survey.VotesNo / totalVotes * 100;

                ResultsLabel.Text = $"Yes: {survey.VotesYes} ({yesPercentage:F1}%) | No: {survey.VotesNo} ({noPercentage:F1}%)";
            }

            // Скрываем кнопки после голосования
            YesButton.IsVisible = false;
            NoButton.IsVisible = false;
            ResultsLabel.IsVisible = true;
        }
    }
}
