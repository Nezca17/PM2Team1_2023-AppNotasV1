using GalaSoft.MvvmLight.Command;
using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.Services;
using PM2Team1_2023_AppNotasV1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PM2Team1_2023_AppNotasV1.ViewModels
{
    public class VerNotasViewModel : BaseViewModel
{
        FirebaseHelper firebaseHelper = new FirebaseHelper();


        public VerNotasViewModel()
        {
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
        public string fechaConvertido;
        #endregion




        #region Properties
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
                SetValue(ref this.listViewSource1, value);
            }

        }

        // Resto del ViewModel...
        #endregion

        #region Commands

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

        public string ConvertirFechaTexto(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

        public async Task LoadData()
        {


            this.IsRefreshing = true;
            var notas = await firebaseHelper.GetNotas();
            await Task.Delay(1000);
            ListViewSource = new ObservableCollection<Nota>(notas);
            this.IsRefreshing = false;
           // return ListViewSource;
        }

        public async void LoadData2()
        {


            this.IsRefreshing = true;

            var notas = await firebaseHelper.GetNotas();
            await Task.Delay(1000);
            ListViewSource = new ObservableCollection<Nota>(notas);
            this.IsRefreshing = false;

        }




        public async Task TomarFoto(byte[] fotoFile)
        {

            ImagenFile = fotoFile;
        }




        #endregion


    }
}
