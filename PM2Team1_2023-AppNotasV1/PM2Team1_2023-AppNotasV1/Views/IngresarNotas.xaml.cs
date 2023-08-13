using Plugin.Media.Abstractions;
using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.Services;
using PM2Team1_2023_AppNotasV1.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using System.IO;
using PM2Team1_2023_AppNotasV1.Converters;
using Firebase.Storage;
using Plugin.AudioRecorder;
using Xamarin.Essentials;
using MediaManager;
using static Android.Provider.MediaStore;
using Xamarin.Forms.Maps;
using Xamarin.Forms.OpenWhatsApp;

namespace PM2Team1_2023_AppNotasV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngresarNotas : ContentPage
    {
        private Pin selectedPin;
        private string filePath; // Variable para almacenar la ruta de la foto capturada
        private MediaFile photo; // Variable para almacenar la foto capturada
        ConvertStreamToByteArray converter;
        AudioRecorderService recorder;
        private Pin currentLocationPin;
        private Color colorOriginalbotonfoto;
        private Color colorOriginalbotonfoto2;
        private Color colorOriginalbotonfoto3;
        private Color colorOriginalbotonfoto4;

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
            dtFecha.Date = DateTime.Now;
            /*Datos por DEfecto*/
            lbRutaAudio.Text = "https://firebasestorage.googleapis.com/v0/b/pm2team1-2023.appspot.com/o/Notas%2Faudio.wav?alt=media&token=cba091af-0f8f-49fb-805f-f7bd379fd0ef";
            lbRutaFirebase.Text = "https://firebasestorage.googleapis.com/v0/b/pm2team1-2023.appspot.com/o/Notas%2FcapturedImage_17.jpg?alt=media&token=70a3d78f-a0a1-4e41-90d3-021005f1c03a";
            MostrarUbicacionActual();
            colorOriginalbotonfoto = MiBoton.BackgroundColor;
            colorOriginalbotonfoto2 = btnEscucharAudio.BackgroundColor;
            colorOriginalbotonfoto3 = btnGrabarAudio.BackgroundColor;
            colorOriginalbotonfoto4 = btnGrabarAudio.BackgroundColor;


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

                    }).Child("Notas")
                    .Child(txtTitu.Text)
                    .Child(nombre)
                    .PutAsync(photo);

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

                    }).Child("Notas")
                    .Child(txtTitu.Text)
                    .Child(nombre)
                    .PutAsync(audio);

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
            MiBoton.BackgroundColor = Color.FromHex("#349DA4"); // Cambiar el color a azul celeste
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                MiBoton.BackgroundColor = colorOriginalbotonfoto; // Volver al color original
                return false;
            });

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
            btnGrabarAudio.BackgroundColor = Color.FromHex("#349DA4"); // Cambiar el color a azul celeste
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                btnGrabarAudio.BackgroundColor = colorOriginalbotonfoto3; // Volver al color original
                return false;
            });
            try
            {
                lbAudio.Text = "Grabando";
                lbAudio.TextColor = Color.Green;
                btnGrabarAudio.Text = "Pausar";
                btnGuardar.IsEnabled = false;
                await GrabarAudio();
                btnGuardar.IsEnabled = true;
                btnEscucharAudio.IsEnabled = true;
            }
            catch (Exception ex)
            {

                await DisplayAlert("Aviso", $"{ex}", "Ok");

            }
        }

        private async void btnEscucharAudio_Clicked(object sender, EventArgs e)
        {
            btnEscucharAudio.BackgroundColor = Color.FromHex("#349DA4"); // Cambiar el color a azul celeste
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                btnEscucharAudio.BackgroundColor = colorOriginalbotonfoto4; // Volver al color original
                return false;
            });
            try
            {
                var audioFile = lbRutaAudio.Text;
                lbAudio.Text = "Escuchando";
                lbAudio.TextColor = Color.Yellow;
                await CrossMediaManager.Current.Play(audioFile);

            }
            catch (Exception ex)
            {

                await DisplayAlert("Aviso","Error al reproducir","ok");

            }
        }

        private void mapView_MapClicked(object sender, MapClickedEventArgs e)
        {
            Position selectedPosition = e.Position;

        
            mapView.Pins.Remove(selectedPin);

            selectedPin = new Pin
            {
                Position = selectedPosition,
                Label = "Ubicación seleccionada",
                Type = PinType.Place
            };

            mapView.Pins.Add(selectedPin);

            txtLongi.Text = selectedPosition.Longitude.ToString();
            txtLatit.Text = selectedPosition.Latitude.ToString();
          


        }

        private async void MostrarUbicacionActual()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                // Obtener la ubicación actual del dispositivo
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    // Centrar el mapa en la ubicación actual
                    mapView.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Position(location.Latitude, location.Longitude),
                        Distance.FromMiles(0.1))); // Ajusta el radio según tus necesidades
                }
            }
            else
            {
                // No se otorgó el permiso de ubicación, manejar según tus necesidades
            }
        }


        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            bool isToggled = e.Value;
            // Aquí puedes realizar acciones basadas en el valor del switch (encendido/apagado)
            if (isToggled)
            {
                // Código a ejecutar cuando el switch se enciende
                Elementos.IsVisible = true;
            }
            else
            {
                // Código a ejecutar cuando el switch se apaga
                Elementos.IsVisible = false;
            }
        }

        
    }
}