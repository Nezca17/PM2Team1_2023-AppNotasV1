using GalaSoft.MvvmLight.Command;
using Plugin.LocalNotification;
using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.Services;
using PM2Team1_2023_AppNotasV1.Views;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PM2Team1_2023_AppNotasV1.ViewModels
{
    public class EditNotaViewModel : BaseViewModel
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        #region Attributes
        private Guid Id;
        private string Titulo;
        private string Detalles;
        private DateTime FechaIngreso;
        private bool isRecordatorio;
        private DateTime Fecha;
        private TimeSpan Hora;
        private byte[] audioFile;
        private byte[] ImagenFile;
        private double longitud;
        private double latitude;
        private string RutaAudioFile;
        private string RutaImagenFile;
        public Uri txtRutaAudioFileUri;
        public Uri txtRutaImagenFileUri;
        public int IdNotiR;
        #endregion

        #region Properties

        public Guid txtID
        {
            get { return Id;}
            set { SetValue(ref this.Id, value); }

        }
        public int IdNotif
        {
            get { return IdNotiR; }
            set { SetValue(ref IdNotiR, value); }
        }
        public string txtTitulo
        {
            get { return Titulo; }
            set { SetValue(ref this.Titulo, value); }
        }
        public Uri RutaImagenFileUri
        {
            get { return txtRutaImagenFileUri; }
            set { SetValue(ref txtRutaImagenFileUri, value); }
        }
        public Uri RutaAudioFileUri
        {
            get { return txtRutaAudioFileUri; }
            set { SetValue(ref txtRutaAudioFileUri, value); }
        }

        public string txtDetalles
        {
            get { return Detalles; }
            set { SetValue(ref this.Detalles, value); }
        }

        public DateTime dtFechaIngreso
        {
            get { return FechaIngreso; }
            set { SetValue(ref this.FechaIngreso, value); }
        }

        public bool IsRecordatorio
        {
            get { return isRecordatorio; }
            set { SetValue(ref this.isRecordatorio, value); }
        }

        public DateTime dtFecha
        {
            get { return Fecha; }
            set { SetValue(ref this.Fecha, value); }
        }

        public TimeSpan timHora
        {
            get { return Hora; }
            set { SetValue(ref this.Hora, value); }
        }

        public byte[] bitsAudioFile
        {
            get { return audioFile; }
            set { SetValue(ref this.audioFile, value); }
        }

        public byte[] bitsImagenFile
        {
            get { return ImagenFile; }
            set { SetValue(ref this.ImagenFile, value); }
        }

        public double txtLongitud
        {
            get { return longitud; }
            set { SetValue(ref this.longitud, value); }
        }

        public double txtLatitude
        {
            get { return latitude; }
            set { SetValue(ref this.latitude, value); }
        }

        public string txtRutaAudioFile
        {
            get { return RutaAudioFile; }
            set { SetValue(ref this.RutaAudioFile, value); }
        }

        public string TxtRutaImagenFile
        {
            get { return RutaImagenFile; }
            set { SetValue(ref this.RutaImagenFile, value); }
        }

        #endregion

        #region Commands

        public ICommand UpdateCommand {

            get {

                return new RelayCommand(UpdateMethod);

            }
        }
        public ICommand DeleteCommand
        {

            get
            {

                return new RelayCommand(DeleteMethod);

            }
        }

        #endregion

        #region Methods

        private async void UpdateMethod()
        {
            try
            {
                //Nota Simple
                Random random = new Random();
                IdNotif = random.Next(0, 999 + 1);
                var nota = new Nota
                {
                    Id = Id,
                    Titulo = Titulo,
                    Detalles = Detalles,
                    Fecha = Fecha,
                    FechaIngreso = FechaIngreso,
                    IsRecordatorio = IsRecordatorio,
                    Hora = Hora,
                    RutaAudioFile = txtRutaAudioFile,
                    RutaImagenFile = TxtRutaImagenFile,
                    latitude = latitude,
                    longitud = longitud
                };
                await firebaseHelper.UpdateNota(nota);


                TimeSpan horaMenos20 = Hora.Subtract(TimeSpan.FromMinutes(20));
                DateTime HorayFecha = Fecha.Date + horaMenos20;

                var notification = new NotificationRequest
                {
                    Title = txtTitulo,
                    NotificationId = IdNotif,
                    Description = txtDetalles,
                    Schedule =
                    {
                        NotifyTime = HorayFecha
                    }

                };


                if (await LocalNotificationCenter.Current.Show(notification))
                {
                    Console.WriteLine("****************************Notificacion creada");
                }
                else
                {
                    Console.WriteLine("**************************Fallo al crear la notificacion");
                }


                await App.Current.MainPage.DisplayAlert("Aviso","Guardado Correctamente!", "Ok");
                
                await App.Current.MainPage.Navigation.PushAsync(new VerNotas());



            }
            catch (Exception ex)
            {
                Console.WriteLine($"************************************{ex}");

            }

        }

        private async void DeleteMethod() {

            await firebaseHelper.DeleteNota(Id);
            await App.Current.MainPage.Navigation.PushAsync(new VerNotas());
        }


        #endregion



        #region Constructor

        public EditNotaViewModel(Nota _nota)
        {
            Id = _nota.Id;
            Titulo = _nota.Titulo;
            Detalles = _nota.Detalles;
            FechaIngreso = _nota.FechaIngreso;
            IsRecordatorio = _nota.IsRecordatorio;
            Fecha = _nota.Fecha;
            Hora = _nota.Hora;
            RutaAudioFile = _nota.RutaAudioFile;
            RutaImagenFile = _nota.RutaImagenFile;
            RutaAudioFileUri = _nota.RutaAudioFileUri;
            RutaImagenFileUri =_nota.RutaImagenFileUri;
            longitud = _nota.longitud;
            latitude = _nota.latitude;
            IdNotiR = _nota.IdNoti;

        }

        public EditNotaViewModel()
        {

        }

        #endregion
    }
}
