using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.Services;
using PM2Team1_2023_AppNotasV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.AudioRecorder;
using MediaManager.Player;
using MediaManager;
using Android.Views.Accessibility;
using MediaManager.Queue;
using Android.Views.Animations;
using Plugin.Media.Abstractions;
using Firebase.Storage;
using Xamarin.Forms.Maps;
using System.IO;
using static Android.Provider.MediaStore;
using Xamarin.Forms.OpenWhatsApp;



namespace PM2Team1_2023_AppNotasV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditNotaPage : ContentPage
    {
        private Pin selectedPin;
        PlayAudioService playbites;

        AudioPlayer audioPlayer = new AudioPlayer();
        private MediaFile photo;
        private string filePath;
        AudioRecorderService recorder;
        private Color colorOriginalbotonfoto3;


        public EditNotaPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, true);
            BindingContext = new EditNotaViewModel();
            playbites = new PlayAudioService();
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds
            };

            // lbUriAudio.Text = "https://firebasestorage.googleapis.com/v0/b/pm2team1-2023.appspot.com/o/Notas%2Faudio.wav?alt=media&token=cba091af-0f8f-49fb-805f-f7bd379fd0ef";
            //  lbRutaImagen.Text = "https://firebasestorage.googleapis.com/v0/b/pm2team1-2023.appspot.com/o/Notas%2FcapturedImage_17.jpg?alt=media&token=70a3d78f-a0a1-4e41-90d3-021005f1c03a";
           
        }


        public EditNotaPage(Nota _nota)
        {
            InitializeComponent();
            BindingContext = new EditNotaViewModel(_nota);
            MostrarUbicacionActual2();
            playbites = new PlayAudioService();
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds
            };

            if (!swEsRecordatorio.IsToggled)
            {
                mapView1.IsVisible = false;
                dtActHora.IsVisible = false;
                dtpActFecha.IsVisible = false;
                txtactLongitud.IsVisible = false;
                txtactLatitude.IsVisible = false;
                lbfecha.IsVisible = false;
                lbHora.IsVisible = false;
                lbLatitud.IsVisible = false;
                lbLongitud.IsVisible=false;

            }
            else
            {
                mapView1.IsVisible = true;
                dtActHora.IsVisible = true;
                dtpActFecha.IsVisible = true;
                txtactLongitud.IsVisible = true;
                txtactLatitude.IsVisible = true;
                lbfecha.IsVisible = true;
                lbHora.IsVisible = true;
                lbLatitud.IsVisible = true;
                lbLongitud.IsVisible = true;
            }

        }



        private async void btnActualizarAudio_Clicked(object sender, EventArgs e)
        {
            try
            {
                btnActualizarAudio.BackgroundColor = Color.FromHex("#AED6F1"); // Cambiar el color a azul celeste
                Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
                {
                    btnActualizarAudio.BackgroundColor = colorOriginalbotonfoto3; // Volver al color original
                    return false;
                });
                try
                {
                    lbEstadoAudio.Text = "Grabando";
                    lbEstadoAudio.TextColor = Color.Green;
                    btnActualizarAudio.Text = "Pausar";
                    btnActualizar.IsEnabled = false;
                    await GrabarAudio();
                    btnEscucharAudio.IsEnabled = true;
                    await Task.Delay(1000);
                    btnActualizar.IsEnabled = true;
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Aviso", $"{ex}", "Ok");

                }
                // await GrabarAudio();

            }
            catch (Exception ex)
            {

                Console.WriteLine($"*******************************{ex}");

            }

        }

        private async void btnEscucharAudio_Clicked(object sender, EventArgs e)
        {
            try
            {

                var audioUrl = lbUriAudio.Text;
                // var result = false;
                await CrossMediaManager.Current.Play(audioUrl);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", $"{ex}", "Ok");
                Console.WriteLine($"{ex}");
            }
        }

        #region Methods
        static async Task<byte[]> DownloadByteArrayAsync(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    throw new Exception("No se pudo descargar el recurso.");
                }
            }
        }

        public async void TomarNuevaFoto()
        {
            try
            {
                btnActualizar.IsEnabled = false;
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

                    imageActField.Source = ImageSource.FromFile(filePath);

                }
                var stream = photo.GetStream();

                //  NotasViewModel.StreamFoto = stream;
                lbRutaImagen.Text = await TomarFoto(stream, photo.OriginalFilename);
                btnActualizar.IsEnabled = true;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                await DisplayAlert("Error", $"Ha ocurrido un error: {ex.Message}", "OK");
            }

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

                    }).Child("Notas").Child(txtactTitulo.Text).Child(nombre).PutAsync(photo);

                    task.Progress.ProgressChanged += (s, args) =>
                    {
                        PbImagen.Progress = args.Percentage;
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
                        PbAudio.Progress = args.Percentage;
                    };

                    var dowloadlink2 = await task;
                    await Task.Delay(1000);
                    PbAudio.Progress = 1;
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
                    PbAudio.Progress = 0;

                    await recorder.StartRecording();
                    return;
                }
                else
                {
                    await recorder.StopRecording();

                    btnActualizarAudio.Text = "Grabar";
                    lbEstadoAudio.Text = "Parado";
                    lbEstadoAudio.TextColor = Color.Red;

                }

                var file = recorder.GetAudioFileStream();

                // Uri uri = new Uri();
                lbUriAudio.Text = await EnviarAudio(file, txtactTitulo.Text + "audio.wav");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Aviso", $"{ex}", "Ok");
            }

        }
        #endregion

        private async void btnActualizarImagen_Clicked(object sender, EventArgs e)
        {
            try
            {
                btnActualizar.IsEnabled = false;
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

                    imageActField.Source = ImageSource.FromFile(filePath);

                }
                var stream = photo.GetStream();

                //  NotasViewModel.StreamFoto = stream;
                lbRutaImagen.Text = await TomarFoto(stream, photo.OriginalFilename);
                btnActualizar.IsEnabled = true;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                await DisplayAlert("Error", $"Ha ocurrido un error: {ex.Message}", "OK");
            }
        }

        private async void EnviarDatosWhatsApp()
        {
            string mensaje = $"Título: {txtactTitulo.Text}\nDetalles: {txtactDetal.Text}\nFecha: {dtpActFecha.Date} {dtActHora.Time}\nLongitud: {txtactLongitud.Text}\nLatitud: {txtactLatitude.Text}";

            try
            {
                if (!string.IsNullOrEmpty(lbRutaImagen.Text))
                {
                    mensaje += $"\nImagen: {lbRutaImagen.Text}";
                }

                if (!string.IsNullOrEmpty(lbUriAudio.Text))
                {
                    mensaje += $"\nAudio: {lbUriAudio.Text}";
                }

                var request = new ShareTextRequest
                {
                    Text = mensaje,
                    Title = "Compartir a través de WhatsApp"
                };

                await Share.RequestAsync(request);
            }
            catch (Exception ex)
            {
                // Manejar errores aquí
            }
        }

        private void btnwhatsap_Clicked(object sender, EventArgs e)
        {
            EnviarDatosWhatsApp();
        }

        private void swEsRecordatorio_Toggled(object sender, ToggledEventArgs e)
        {
            if (!swEsRecordatorio.IsToggled)
            {

                mapView1.IsVisible = false;
                dtActHora.IsVisible = false;
                dtpActFecha.IsVisible = false;
                txtactLongitud.IsVisible = false;
                txtactLatitude.IsVisible = false;
                lbfecha.IsVisible = false;
                lbHora.IsVisible = false;
                lbLatitud.IsVisible = false;
                lbLongitud.IsVisible = false;

            }
            else
            {

                mapView1.IsVisible = true;
                dtActHora.IsVisible = true;
                dtpActFecha.IsVisible = true;
                txtactLongitud.IsVisible = true;
                txtactLatitude.IsVisible = true;
                lbfecha.IsVisible = true;
                lbHora.IsVisible = true;
                lbLatitud.IsVisible = true;
                lbLongitud.IsVisible = true;
            }
        }

        private void mapView1_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {

            Position selectedPosition = e.Position;
            mapView1.Pins.Remove(selectedPin);

            selectedPin = new Pin
            {
                Position = selectedPosition,
                Label = "Ubicación seleccionada",
                Type = PinType.Place
            };
            mapView1.Pins.Add(selectedPin);

            txtactLongitud.Text = selectedPosition.Longitude.ToString();
            txtactLatitude.Text = selectedPosition.Latitude.ToString();
        }

        private async void MostrarUbicacionActual2()
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
                    mapView1.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Position(location.Latitude, location.Longitude),
                        Distance.FromMiles(0.1))); // Ajusta el radio según tus necesidades
                }
            }
            else
            {
                // No se otorgó el permiso de ubicación, manejar según tus necesidades
            }
        }
    }
}