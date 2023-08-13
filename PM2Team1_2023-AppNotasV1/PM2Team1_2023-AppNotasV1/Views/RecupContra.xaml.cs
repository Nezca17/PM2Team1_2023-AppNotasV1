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
public partial class RecupContra : ContentPage
    {
        private IFirebaseAuthService _firebaseService;
        public RecupContra()
    {
        InitializeComponent();
            _firebaseService = DependencyService.Get<IFirebaseAuthService>();
        }

        private async void btnEnviar_Clicked(object sender, EventArgs e)
        {
           var correo =  txtCorreo.Text;

            await RecuperarContrasenaCommandExecute(correo);

        }


        private async Task RecuperarContrasenaCommandExecute(string Username)
        {
            try
            {
                if (await _firebaseService.ResetPassword(Username))
                {
                    await DisplayAlert("Aviso", "Se ha enviado a Evaluación, si pasa se enviara un correo", "Ok");
                    await Navigation.PushAsync(new LoginPage());

                }
                else
                {
                    // _userDialogService.Toast();
                    await DisplayAlert("Aviso", "Error al enviar", "Ok");

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex}", "ok");
            }


        }

    }
}