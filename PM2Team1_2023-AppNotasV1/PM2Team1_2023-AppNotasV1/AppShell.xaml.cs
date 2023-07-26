using PM2Team1_2023_AppNotasV1.ViewModels;
using PM2Team1_2023_AppNotasV1.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PM2Team1_2023_AppNotasV1
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Dashboard), typeof(Dashboard));
            
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
