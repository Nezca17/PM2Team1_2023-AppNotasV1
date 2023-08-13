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
using System.IO;
using static Android.Provider.MediaStore;

namespace PM2Team1_2023_AppNotasV1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EditNotaPage : ContentPage
{
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
            playbites=  new PlayAudioService();
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds
            };
        }
    
        public EditNotaPage(Nota _nota)
        {
            InitializeComponent();
            BindingContext = new EditNotaViewModel(_nota);
            playbites =  new PlayAudioService();
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds
            };

            if (!swEsRecordatorio.IsToggled)
            {
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
            catch (Exception ex) {

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

        private void swEsRecordatorio_Toggled(object sender, ToggledEventArgs e)
        {
            if (!swEsRecordatorio.IsToggled)
            {
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
    }
}