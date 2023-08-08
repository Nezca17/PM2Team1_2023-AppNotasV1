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
using System.IO;


using PM2Team1_2023_AppNotasV1.Converters;
using System.Runtime.InteropServices;
using Firebase.Storage;
using Plugin.AudioRecorder;
using Android.Net.Wifi.Aware;
using Android;
using Android.Content.PM;
using Xamarin.Essentials;
using System.Data.SqlTypes;

namespace PM2Team1_2023_AppNotasV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngresarNotas : ContentPage
    {
        private string filePath; // Variable para almacenar la ruta de la foto capturada
        private MediaFile photo; // Variable para almacenar la foto capturada
        ConvertStreamToByteArray converter;
        AudioRecorderService recorder;

        NotasViewModel NotasViewModel = new NotasViewModel();

        public IngresarNotas()
        {
            InitializeComponent();
            BindingContext = new NotasViewModel();
            converter = new ConvertStreamToByteArray();
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds
            };
        }
        public async Task<string> TomarFoto(Stream fotoFile, string nombre)
        {
            try
            {

                var photo = fotoFile;

                if (photo != null)
                {
                    var task = new FirebaseStorage("pm2team1-2023.appspot.com", new FirebaseStorageOptions
                    {

                        ThrowOnCancel = true

                    }).Child("Notas").Child(nombre).PutAsync(photo);

                    task.Progress.ProgressChanged += (s, args) =>
                    {
                        progressBar.Progress = args.Percentage;
                    };

                    var dowloadlink = await task;

                    return dowloadlink;
                }
                else
                {
                    return "No se envio";
                }



            }
            catch (Exception e)
            {


                await App.Current.MainPage.DisplayAlert("Aviso", $"{e}", "Ok");
                return "N/A";
            }
        }

        public async Task<string> EnviarAudio(Stream audioFile, string nombre)
        {
            try
            {


                var audio = audioFile;

                if (audio != null)
                {
                    var task = new FirebaseStorage("pm2team1-2023.appspot.com", new FirebaseStorageOptions
                    {

                        ThrowOnCancel = true

                    }).Child("Notas").Child(nombre).PutAsync(audio);

                    task.Progress.ProgressChanged += (s, args) =>
                    {
                        progressBar2.Progress = args.Percentage;
                    };

                    var dowloadlink2 = await task;
                    progressBar2.Progress = 1;
                    return dowloadlink2;

                }
                else
                {
                    return "No se envio";
                }



            }
            catch (Exception e)
            {


                await App.Current.MainPage.DisplayAlert("Aviso", $"{e}", "Ok");
                return "N/A";
            }
        }

        private async void AgregarFotografia_Clicked(object sender, EventArgs e)
        {
            //    await Navigation.PushAsync(new IngresarNotas());

            try
            {
                btnGuardar.IsEnabled = false;
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
                var stream = photo.GetStream();

                //  NotasViewModel.StreamFoto = stream;
                lbRutaFirebase.Text = await TomarFoto(stream, photo.OriginalFilename);
                btnGuardar.IsEnabled = true;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                await DisplayAlert("Error", $"Ha ocurrido un error: {ex.Message}", "OK");
            }


        }

        public async Task GrabarAudio()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.Microphone>();
                var status2 = await Permissions.RequestAsync<Permissions.StorageRead>();
                var status3 = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted && status2 != PermissionStatus.Granted && status3 != PermissionStatus.Granted)
                {
                    return; // si no tiene los permisos no avanza
                }

                if (!recorder.IsRecording)
                {
                    progressBar2.Progress = 0;

                    await recorder.StartRecording();
                    return;
                }
                else
                {
                    await recorder.StopRecording();

                    btnGrabarAudio.Text = "Grabar";
                    lbAudio.Text = "Parado";
                    lbAudio.TextColor = Color.Red;

                }

                var file = recorder.GetAudioFileStream();

                // Uri uri = new Uri();
                lbRutaAudio.Text = await EnviarAudio(file, txtTitu.Text + "audio.wav");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Aviso", $"{ex}", "Ok");
            }

        }


        private async void AgregarAudio_Clicked(object sender, EventArgs e)
        {
            try
            {
                lbAudio.Text = "Grabando";
                lbAudio.TextColor = Color.Green;
                btnGrabarAudio.Text = "Pausar";
                await GrabarAudio();


            }
            catch (Exception ex)
            {

                await DisplayAlert("Aviso", $"{ex}", "Ok");

            }
        }

    }
}