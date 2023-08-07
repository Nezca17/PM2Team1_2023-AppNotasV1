
using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2Team1_2023_AppNotasV1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerNotas : ContentPage
    {

        private VerNotasViewModel _viewModel;
        
        bool editando = false;
        public Nota nota;

        public VerNotas()
        {

            InitializeComponent();
          
              BindingContext = new VerNotasViewModel();

            

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
         
        }

        public async void ListViewNotas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new EditNotaPage(e.SelectedItem as Nota));
            
           
        }

        private async void btnAtras_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Dashboard());

        }
    }

    
}