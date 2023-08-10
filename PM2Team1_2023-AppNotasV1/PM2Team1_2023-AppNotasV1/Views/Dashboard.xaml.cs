using PM2Team1_2023_AppNotasV1.Interfaces;
using PM2Team1_2023_AppNotasV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2Team1_2023_AppNotasV1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Dashboard : ContentPage
{
        private IFirebaseAuthService _firebaseService;
        int start = 0;

        public Dashboard()
        {
        InitializeComponent();
            _firebaseService = DependencyService.Get<IFirebaseAuthService>();
           // start = 0;
        }
        protected override async void OnAppearing()
        {

            base.OnAppearing();
            if (_firebaseService.IsUserSigned()==false)
            {
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                if (start == 0)
                {
                  //  BindingContext = new DashBoardViewModel();
                    start = 1;
                }
                return;
            }

        }

        private async void BtnAddNotasView_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            btn.BackgroundColor = new Color(0.5, 0.5, 0.5, 0.5);
            await Navigation.PushAsync(new IngresarNotas());

        }
        private async void BtnMisNotas_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            btn.BackgroundColor = new Color(0.5, 0.5, 0.5, 0.5);
            await Navigation.PushAsync(new VerNotas());
        }
        private async void BtnRecordatorio_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            btn.BackgroundColor = new Color(0.5, 0.5, 0.5, 0.5);
            //   await Navigation.PushAsync(new IngresarNotas());
        }


        private async void btnSalir_Clicked(object sender, EventArgs e)
        {
          await  LogoutCommandExecute();
        }

        private async Task LogoutCommandExecute()
        {
            try
            {
                if (await _firebaseService.Logout())
                {
                    await DisplayAlert("Aviso", "Salio", "Ok");
                    await Navigation.PushAsync(new LoginPage());

                }
                else
                {
                  
                    await DisplayAlert("Aviso", "Error al cerrar Sesion", "Ok");

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex}", "ok");
            }


        }


    }
}