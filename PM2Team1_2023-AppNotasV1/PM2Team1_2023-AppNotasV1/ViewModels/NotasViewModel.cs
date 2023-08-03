using GalaSoft.MvvmLight.Command;
using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.Services;
using PM2Team1_2023_AppNotasV1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PM2Team1_2023_AppNotasV1.ViewModels
{
    public class NotasViewModel :BaseViewModel
{

        FirebaseHelper firebaseHelper = new FirebaseHelper();


        public NotasViewModel() {

            LoadData();
        } 


        #region Attributes
        public string _Titulo;
        public string _detalles;
        public DateTime _fechaIngreso;
        public bool _IsRecordatorio;
        public DateTime _fecha;
        public TimeSpan _hora;
        public string _audioFile;
        public string _imagenFile;
        public string _longitud;
        public string _latitude;
        public bool isRefreshing = false;
        public object listViewSource;
        #endregion



        #region Properties
        public string Titulo
        {
            get { return _Titulo; }
            set { SetValue(ref _Titulo, value); }
        }

        public string Detalles
        {
            get { return _detalles; }
            set { SetValue(ref _detalles, value); }
        }

        public DateTime FechaIngreso
        {
            get { return _fechaIngreso; }
            set { SetValue(ref _fechaIngreso, value); }
        }

        public bool IsRecordatorio
        {
            get { return _IsRecordatorio; }
            set { SetValue(ref _IsRecordatorio, value); }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { SetValue(ref _fecha, value); }
        }

        public TimeSpan Hora
        {
            get { return _hora; }
            set { SetValue(ref _hora, value); }
        }

        public string AudioFile
        {
            get { return _audioFile; }
            set { SetValue(ref _audioFile, value); }
        }

        public string ImagenFile
        {
            get { return _imagenFile; }
            set { SetValue(ref _imagenFile, value); }
        }

        public string Longitud
        {
            get { return _longitud; }
            set { SetValue(ref _longitud, value); }
        }

        public string Latitude
        {
            get { return _latitude; }
            set { SetValue(ref _latitude, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetValue(ref isRefreshing, value); }
        }

        // Resto del ViewModel...
        #endregion

        #region Commands
        public ICommand insertCommand
        {
            get
            {
                return new RelayCommand(InsertMethod);

            }

        }


        #endregion

        #region Methods

        private async void InsertMethod()
        {
            var nota = new Nota
            {
                Titulo = _Titulo,
                Detalles = _detalles,
                fechaIngreso = _fecha.Date,
                isRecordatorio = _IsRecordatorio,
                fecha = _fecha.Date,
                hora = _hora,
                audioFile = "aas",
                imagenFile = "aaa",
                longitud = double.Parse(_longitud),
                latitude = double.Parse(_latitude)

            };

            await firebaseHelper.AddNota(nota);
            this.IsRefreshing = true;
            await Task.Delay(1000);

           await LoadData();

            this.IsRefreshing = false;
        }

        public async Task LoadData()
        {
            this.listViewSource = await firebaseHelper.GetNotas();

        }

        #endregion




    }
}

