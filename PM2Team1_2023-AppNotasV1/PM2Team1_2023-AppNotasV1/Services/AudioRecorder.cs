using Android.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using Android.App;
using Android.Views;
using System.Diagnostics;

namespace PM2Team1_2023_AppNotasV1.Services
{
    public class AudioRecorder
{
        AudioRecorderService recorder;

        public AudioRecorder()
        {
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds
            };

        }

        public async Task GrabarAudio()
        {
            try
            {

                if (!recorder.IsRecording)
                {
                    await recorder.StartRecording();
                    return;
                }
                else
                {
                    await recorder.StopRecording();
                   
                }
                
                var file = recorder.GetAudioFileStream();


            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Aviso",$"{ex}","Ok");  
	        }

        }

    }


}
