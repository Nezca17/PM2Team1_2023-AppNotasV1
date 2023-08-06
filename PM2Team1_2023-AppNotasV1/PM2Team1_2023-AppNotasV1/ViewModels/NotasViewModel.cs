using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.Services;
using PM2Team1_2023_AppNotasV1.Views;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Path = System.IO.Path;

namespace PM2Team1_2023_AppNotasV1.ViewModels
{
    public class NotasViewModel :BaseViewModel
{
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        private ImageSource imageData;
        public ImageSource ImageData
        {
            get => imageData;
            set { SetValue(ref imageData, value); }
        }
        private string imageName;
        public string ImageName
        {
            get => imageName;
            set { SetValue(ref imageName, value); }
        }


        public  NotasViewModel() {

            GetLocation();
            LoadData();
        
        } 


        #region Attributes
        public string txtTitulo;
        public string txtDetalles;
        public DateTime txtFechaIngreso;
        public bool txtIsRecordatorio;
        public DateTime txtfecha;
        public TimeSpan txtHora;
        public string txtaudioFile;
        public string txtImagenFile;
        public string txtlongitud;
        public string txtLatitude;
        public bool isRefreshing = false;
        public object listViewSource1;
        public string fechaConvertido ;
        #endregion


        

        #region Properties
        public string Titulo
        {
            get { return txtTitulo; }
            set { SetValue(ref txtTitulo, value); }
        }

        public string Detalles
        {
            get { return txtDetalles; }
            set { SetValue(ref txtDetalles, value); }
        }
        public string FechaConvertida
        {
            get { return ConvertirFechaTexto(this.txtfecha); }
            
        }

        public DateTime FechaIngreso
        {
            get { return txtFechaIngreso; }
            set { SetValue(ref txtFechaIngreso, value); }
        }

        public bool IsRecordatorio
        {
            get { return txtIsRecordatorio; }
            set { SetValue(ref txtIsRecordatorio, value); }
        }

        public DateTime Fecha
        {
            get { return txtfecha; }
            set { SetValue(ref txtfecha, value); }
        }

        public TimeSpan Hora
        {
            get { return txtHora; }
            set { SetValue(ref txtHora, value); }
        }

        public string AudioFile
        {
            get { return txtaudioFile; }
            set { SetValue(ref txtaudioFile, value); }
        }

        public string ImagenFile
        {
            get { return txtImagenFile; }
            set { SetValue(ref txtImagenFile, value); }
        }

        public string Longitud
        {
            get { return txtlongitud; }
            set { SetValue(ref txtlongitud, value); }
        }

        public string Latitude
        {
            get { return txtLatitude; }
            set { SetValue(ref txtLatitude, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetValue(ref isRefreshing, value); }
        }

        public object ListViewSource
        {
            get { return this.listViewSource1; }
            set
            {
                SetValue(ref this.listViewSource1, value);
            }

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

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadData);

            }
        }

        #endregion

        #region Methods

        private async void InsertMethod()
        {
            try
            {
                var nota = new Nota
                {
                    Titulo = txtTitulo,
                    Detalles = txtDetalles,
                    fechaIngreso = txtfecha.Date,
                    isRecordatorio = txtIsRecordatorio,
                    fecha = txtfecha.Date,
                    hora = txtHora,
                    audioFile = "aaa",
                    imagenFile = "aaa",
                    longitud = double.Parse(txtlongitud),
                    latitude = double.Parse(txtLatitude)

                };

                await firebaseHelper.AddNota(nota);
                this.IsRefreshing = true;
                await Task.Delay(1000);

               // await LoadData();

                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }

        }
        public string  ConvertirFechaTexto(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

        public async void LoadData()
        {

            //await Task.Delay(1000);
            this.ListViewSource = await firebaseHelper.GetNotas();
            //listViewSource. = ConvertirFechaTexto(txtfecha);
            
        }




        public async void GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
             
                    this.Latitude = location.Latitude.ToString();
                    this.Longitud = location.Longitude.ToString();
                }
                else
                {
                
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }






        #endregion




    }
}

