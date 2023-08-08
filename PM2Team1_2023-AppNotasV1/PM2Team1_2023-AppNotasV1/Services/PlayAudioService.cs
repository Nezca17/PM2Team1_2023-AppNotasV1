using Android.Content.Res;
using Android.Media;
using Android.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PM2Team1_2023_AppNotasV1.Services
{
    public class PlayAudioService
{
        public void PlayLoadService()
        {
            //I do not have DB to store the base64 string, So I put the base64 string to assets folder called `AboutAssets.txt`, read base64 string to `content`  
            string content;
            AssetManager assets = Android.App.Application.Context.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("AboutAssets.txt")))
            {
                content = sr.ReadToEnd();
            }

            byte[] decodedString = Base64.Decode(content, Base64Flags.Default);

            //play it  
            Play(decodedString);

        }
        MediaPlayer currentPlayer;
        public void Play(byte[] AudioFile)
         {
        Stop();
        currentPlayer = new MediaPlayer();
        currentPlayer.Prepared += (sender, e) =>
        {
            currentPlayer.Start();
        };
        currentPlayer.SetDataSource(new StreamMediaDataSource(new System.IO.MemoryStream(AudioFile)));
        currentPlayer.Prepare();
           }

    void Stop()
    {
        if (currentPlayer == null)
            return;

        currentPlayer.Stop();
        currentPlayer.Dispose();
        currentPlayer = null;
    }

}
    public class StreamMediaDataSource : MediaDataSource
    {
        System.IO.Stream data;

        public StreamMediaDataSource(System.IO.Stream Data)
        {
            data = Data;
        }

        public override long Size
        {
            get
            {
                return data.Length;
            }
        }

        public override int ReadAt(long position, byte[] buffer, int offset, int size)
        {
            data.Seek(position, System.IO.SeekOrigin.Begin);
            return data.Read(buffer, offset, size);
        }

        public override void Close()
        {
            if (data != null)
            {
                data.Dispose();
                data = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (data != null)
            {
                data.Dispose();
                data = null;
            }
        }
    }
}
