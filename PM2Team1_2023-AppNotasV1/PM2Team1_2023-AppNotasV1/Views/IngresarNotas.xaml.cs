using Plugin.Media.Abstractions;
using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.Services;
using PM2Team1_2023_AppNotasV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Path = System.IO.Path;

namespace PM2Team1_2023_AppNotasV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngresarNotas : ContentPage
    {
        private string filePath; // Variable para almacenar la ruta de la foto capturada
        private MediaFile photo; // Variable para almacenar la foto capturada


        NotasViewModel NotasViewModel = new NotasViewModel();

        public IngresarNotas()
        {
            InitializeComponent();
           // NotasViewModel.LoadData();
            BindingContext = new NotasViewModel();
             
        }

        private async void AgregarFotografia_Clicked(object sender, EventArgs e)
        {
            //    await Navigation.PushAsync(new IngresarNotas());

            try
            {
                // Verificar si se otorgó el permiso de cámara
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Error", "La cámara no está disponible.", "OK");
                    return;
                }

                // Solicitar permisos para acceder a la cámara
                var status = await CrossMedia.Current.Initialize();
                if (!status)
                {
                    await DisplayAlert("Permiso denegado", "No se ha otorgado el permiso para acceder a la cámara.", "OK");
                    return;
                }

                // Tomar una foto
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "CapturedPhotos",
                    Name = "capturedImage.jpg",
                    SaveToAlbum = true // Guardar la foto en el álbum de fotos del dispositivo (opcional)
                });

                if (photo != null)
                {
                    // Obtener la ruta de la foto capturada
                    filePath = photo.Path;
                    imageField.Source = ImageSource.FromFile(filePath);

                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                await DisplayAlert("Error", $"Ha ocurrido un error: {ex.Message}", "OK");
            }


        }
        private async void AgregarAudio_Clicked(object sender, EventArgs e)
        {
          //  await Navigation.PushAsync(new IngresarNotas());
        }
        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new IngresarNotas());
        }
    }
}