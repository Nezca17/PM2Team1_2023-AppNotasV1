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

        public Dashboard()
        {
        InitializeComponent();
            _firebaseService = DependencyService.Get<IFirebaseAuthService>();
   
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
              //  BindingContext = new DashBoardViewModel();
            }

        }

        private async void BtnAddNotasView_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngresarNotas());
        }
        private async void BtnMisNotas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerNotas());
        }
        private async void BtnRecordatorio_Clicked(object sender, EventArgs e)
        {
         //   await Navigation.PushAsync(new IngresarNotas());
        }
        private async void BtnImportantes_Clicked(object sender, EventArgs e)
        {
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