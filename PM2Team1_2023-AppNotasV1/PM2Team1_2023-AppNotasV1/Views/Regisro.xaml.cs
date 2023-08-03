using PM2Team1_2023_AppNotasV1.Interfaces;
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
	public partial class Regisro : ContentPage
	{
        private IFirebaseAuthService _firebaseService;

        public Regisro ()
		{
			InitializeComponent ();
            _firebaseService = DependencyService.Get<IFirebaseAuthService>();
        }

        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
           await RegistrarCommandExecute(txtcorreo.Text, txtcontraseña.Text);
        }

        private async Task RegistrarCommandExecute(string Username, string Password)
        {
            try
            {
                if (await _firebaseService.SignUp(Username, Password))
                {
                    await DisplayAlert("Aviso", "Usuario Creado", "Ok");

                    await _firebaseService.SignIn(Username, Password);
                    await Navigation.PushAsync(new Dashboard());

                }
                else
                {
                    // _userDialogService.Toast();
                    await DisplayAlert("Aviso", "Error Al Crear", "Ok");

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex}", "ok");
            }


        }
    }
}