using System;
using ToDo.Helpers;
using ToDo.PageModels;
using ToDo.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            IoC.Build();

            var listsPage = IoC.ResolvePage<ListsPage, ListsPageModel>();

            MainPage = new NavigationPage(listsPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
