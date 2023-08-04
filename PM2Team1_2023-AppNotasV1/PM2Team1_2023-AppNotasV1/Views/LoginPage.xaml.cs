using Acr.UserDialogs;
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
    public partial class LoginPage : ContentPage
    {
        private IUserDialogs _userDialogService;
        private IFirebaseAuthService _firebaseService;
        public LoginPage()
        {
            InitializeComponent();
            //this.BindingContext = new LoginViewModel();
           // _userDialogService = userDialogsService;
            _firebaseService = DependencyService.Get<IFirebaseAuthService>();
            MessagingCenter.Subscribe<String, String>(this, _firebaseService.getAuthKey(), async (sender, args) =>
            {
               await LoginGoogle(args);

            });

        }

        private async Task LoginCommandExecute(string Username, string Password)
        {
            try
            {
                if (_firebaseService.IsUserSigned())
                {
                    await Navigation.PushAsync(new Dashboard());
                }
                else {
                    if (await _firebaseService.SignIn(Username, Password))
                    {
                        await DisplayAlert("Aviso", "Bienvenido", "Ok");
                        await Navigation.PushAsync(new Dashboard());

                    }
                    else
                    {
                        // _userDialogService.Toast();
                        await DisplayAlert("Aviso", "Usuario o contraseña incorrectos", "Ok");

                    }
                }
               
            }
            catch (Exception ex) {
               await DisplayAlert("Error", $"{ex}", "ok");
            }
            

        }
        private async Task LoginGoogleCommandExecute()
        {
            _firebaseService.SignInWithGoogle();

        }

        private async Task LoginGoogle(String token)
        {
            if (await _firebaseService.SignInWithGoogle(token))
            {
                await Navigation.PushAsync(new Dashboard());
            }

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Regisro());
        }

        private async void btnInSecion_Clicked(object sender, EventArgs e)
        {

             await LoginCommandExecute(txtusuario.Text, txtcontraseña.Text);
            
        }

    }
}