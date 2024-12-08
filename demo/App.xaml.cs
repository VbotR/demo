using demo.Pages;

namespace demo;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Устанавливаем StartPage как начальную страницу
        MainPage = new NavigationPage(new StartPage());
    }
}
