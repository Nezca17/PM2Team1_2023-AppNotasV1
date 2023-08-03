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
public partial class IngresarNotas : ContentPage
{
    public IngresarNotas()
    {
        InitializeComponent();
    }

        private async void AgregarFotografia_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngresarNotas());
        }
        private async void AgregarAudio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngresarNotas());
        }
        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngresarNotas());
        }
    }
}