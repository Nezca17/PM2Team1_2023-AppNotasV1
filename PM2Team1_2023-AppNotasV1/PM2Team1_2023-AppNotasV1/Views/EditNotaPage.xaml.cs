
using Android.Media;
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

namespace PM2Team1_2023_AppNotasV1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EditNotaPage : ContentPage
{
        PlayAudioService playbites;

        AudioPlayer audioPlayer = new AudioPlayer();
        
        
        
    public EditNotaPage()
    {
        InitializeComponent();
        BindingContext = new EditNotaViewModel(); 
            playbites=  new PlayAudioService();
        }
    
    public EditNotaPage(Nota _nota)
        {
            InitializeComponent();
            BindingContext = new EditNotaViewModel(_nota);
            playbites =  new PlayAudioService();
            /*
            CrossMediaManager.Current.ShuffleMode = ShuffleMode.All;
            CrossMediaManager.Current.PlayNextOnFailed = true;
            CrossMediaManager.Current.AutoPlay = true;*/
        }



        private void btnActualizarAudio_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnEscucharAudio_Clicked(object sender, EventArgs e)
        {
            try
            {

                var audioUrl = lbUriAudio.Text;
               // var result = false;
                await CrossMediaManager.Current.Play(audioUrl);


                /*
                byte[] audioFile = await DownloadByteArrayAsync(new Uri(lbUriAudio.Text));


                if(audioFile != null)
                {
                    Console.WriteLine("Bytes descargados: " + audioFile.Length);
                    playbites.Play(audioFile);
                   // audioPlayer.Play();
                }
                else
                {
                    await DisplayAlert("Aviso","No logro descargar el Audio","Ok");
                }*/

              
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", $"{ex}", "Ok");
                Console.WriteLine($"{ex}");
            }
        }



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
    }
}