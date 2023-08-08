
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

namespace PM2Team1_2023_AppNotasV1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EditNotaPage : ContentPage
{
        PlayAudioService playbites;
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
        }



        private void btnActualizarAudio_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnEscucharAudio_Clicked(object sender, EventArgs e)
        {
            try
            {

                byte[] audioFile = await DownloadByteArrayAsync(new Uri(lbUriAudio.Text));
                if(audioFile != null)
                {
                    Console.WriteLine("Bytes descargados: " + audioFile.Length);
                    playbites.Play(audioFile);

                }
                else
                {
                    await DisplayAlert("Aviso","No logro descargar el Audio","Ok");
                }

              
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", $"{ex}", "Ok");
                Console.WriteLine($"{ex}");
            }
        }

       /* static async Task<byte[]> AudioBytes( Uri AudioURi)
        {
            Uri uri = AudioURi;
            byte[] bytes = await DownloadByteArrayAsync(uri);
          
            Console.WriteLine("Bytes descargados: " + bytes.Length);
            return bytes;
        }*/

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