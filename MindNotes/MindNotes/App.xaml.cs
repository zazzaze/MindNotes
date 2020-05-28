using System;
using MindNotes.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MindNotes
{
    public partial class App : Application
    {
        public const String ServerUrl = "http://62.113.116.228/";
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainTabbedPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}