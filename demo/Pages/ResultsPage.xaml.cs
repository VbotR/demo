using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using demo.Data;
using demo.Models;
using System;

namespace demo.Pages;

public partial class ResultsPage : ContentPage
{
    private int _surveyId;

    public ResultsPage(int surveyId)
    {
        InitializeComponent();
        _surveyId = surveyId;

        LoadSurveyDetails();
    }

    private void LoadSurveyDetails()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                var survey = db.Surveys.FirstOrDefault(s => s.Id == _surveyId);

                if (survey != null)
                {
                    SurveyTitle.Text = survey.Title;
                    SurveyDescription.Text = survey.Description;

                    // Обновляем диаграмму
                    ChartCanvas?.InvalidateSurface();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading survey details: {ex.Message}");
        }
    }

    private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
    {
        try
        {
            using (var db = new AppDbContext())
            {
                var survey = db.Surveys.FirstOrDefault(s => s.Id == _surveyId);

                if (survey == null) return;

                var totalVotes = survey.VotesYes + survey.VotesNo;
                if (totalVotes == 0) totalVotes = 1;

                var yesPercentage = (float)survey.VotesYes / totalVotes;
                var noPercentage = (float)survey.VotesNo / totalVotes;

                var canvas = e.Surface.Canvas;
                canvas.Clear(SKColors.White);

                var rect = new SKRect(50, 50, e.Info.Width - 50, e.Info.Height - 50);
                var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 50
                };

                // Рисуем сегмент "Да"
                paint.Color = SKColors.Green;
                canvas.DrawArc(rect, -90, 360 * yesPercentage, false, paint);

                // Рисуем сегмент "Нет"
                paint.Color = SKColors.Red;
                canvas.DrawArc(rect, -90 + 360 * yesPercentage, 360 * noPercentage, false, paint);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error drawing chart: {ex.Message}");
        }
    }
}
