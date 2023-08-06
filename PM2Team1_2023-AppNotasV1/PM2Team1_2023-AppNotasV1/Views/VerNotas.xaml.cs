using PM2Team1_2023_AppNotasV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2Team1_2023_AppNotasV1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerNotas : ContentPage
    {

        private NotasViewModel _viewModel;

        public VerNotas()
        {

            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel = new NotasViewModel();
            _viewModel.LoadData();
            BindingContext = _viewModel;
           
        }

        public async void ListViewNotas_ItemSelected(object sender, EventArgs e)
        {
         //   await Navigation.PushAsync(new EditNotaPage(e.SelectedItem as Nota));
        }
    }

    
}