﻿using GalaSoft.MvvmLight.Command;
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
using System.Collections.ObjectModel;
using PM2Team1_2023_AppNotasV1.Converters;
using Acr.UserDialogs;
using Firebase.Storage;
using System.ComponentModel;

namespace PM2Team1_2023_AppNotasV1.ViewModels
{
    public class NotasViewModel : BaseViewModel
{
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        private ImageSource imageData;

        ConvertStreamToByteArray convert = new ConvertStreamToByteArray();
/*
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
        }*/


        public  NotasViewModel() {

            GetLocation();
            LoadData();
        
        }

        public NotasViewModel(INavigation navigation)
        {

            Navigation = navigation;
            LoadData();
        }
       
        #region Attributes
        public Guid ID;
        public string txtTitulo;
        public string txtDetalles;
        public DateTime txtFechaIngreso;
       // public ObservableCollection<DateTime> fechaCo;
        public bool txtIsRecordatorio;
        public DateTime txtfecha;
        public TimeSpan txtHora;
        public byte[] txtaudioFile;
        public byte[] txtImagenFile;
        public string txtlongitud;
        public string txtLatitude;
        public bool isRefreshing = false;
        public ObservableCollection<Nota> listViewSource1;
        public string fechaConvertido ;
        public Stream streamFoto;
        public string txtRutaImagenFile;
        public string txtRutaAudioFile;
        public Uri txtRutaAudioFileUri;
        public Uri txtRutaImagenFileUri;
        #endregion




        #region Properties

        public Stream StreamFoto
        {
            get { return streamFoto; }
            set { SetValue(ref streamFoto, value); } 
        }
        public string RutaImagenFile
        {
            get { return txtRutaImagenFile; }
            set { SetValue(ref txtRutaImagenFile, value); }
        }
        public string RutaAudioFile
        {
            get { return txtRutaAudioFile; }
            set { SetValue(ref txtRutaAudioFile, value); }
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

        public INavigation Navigation { get; set; }
        public Guid Id
        {
            get { return ID; }
        }

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
            get { return ConvertirFechaTexto(this.txtfecha.Date); }
            
        }


        public DateTime FechaIngreso
        {
            get { return txtFechaIngreso.Date; }
            set { SetValue(ref txtFechaIngreso, value); }
        }

        public bool IsRecordatorio
        {
            get { return txtIsRecordatorio; }
            set { SetValue(ref txtIsRecordatorio, value); }
        }

        public DateTime Fecha
        {
            get { return txtfecha.Date; }
            set { SetValue(ref txtfecha, value); }
        }

        public TimeSpan Hora
        {
            get { return txtHora; }
            set { SetValue(ref txtHora, value); }
        }

        public byte[] AudioFile
        {
            get { return txtaudioFile; }
            set { SetValue(ref txtaudioFile, value); }
        }

        public byte[] ImagenFile
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

        public ObservableCollection<Nota> ListViewSource
        {
            get { return this.listViewSource1; }
            set
            {
                SetValue(ref this.listViewSource1, value );
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
                return new RelayCommand(LoadData2);

            }
        }

        public ICommand CamaraCommand
        {
            get
            {
                return new RelayCommand(LoadData2);

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
                    FechaIngreso = txtfecha.Date,
                    IsRecordatorio = txtIsRecordatorio,
                    Fecha = txtfecha.Date,
                    Hora = txtHora,
                    RutaImagenFile = txtRutaImagenFile,
                    RutaAudioFile = txtRutaAudioFile,
                    longitud = double.Parse(txtlongitud),
                    latitude = double.Parse(txtLatitude)

                };

                await firebaseHelper.AddNota(nota);
               

                await App.Current.MainPage.DisplayAlert("Aviso", "Guardado", "Ok");
                await App.Current.MainPage.Navigation.PushAsync(new Dashboard());

               
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

        public async Task<ObservableCollection<Nota>> LoadData()
        {


            this.IsRefreshing = true;
            var notas = await firebaseHelper.GetNotas();
            await Task.Delay(1000);
            ListViewSource = new ObservableCollection<Nota>(notas);
            this.IsRefreshing = false;
            return ListViewSource;
        }

        public async void LoadData2()
        {


            this.IsRefreshing = true;
            
            var notas = await firebaseHelper.GetNotas();
            await Task.Delay(1000);
            ListViewSource = new ObservableCollection<Nota>(notas);
            this.IsRefreshing = false;
           
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

