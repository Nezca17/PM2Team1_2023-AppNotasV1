using PM2Team1_2023_AppNotasV1.Models;
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
	public partial class NotaisNotRecordatorioPage : ContentPage
	{
		public NotaisNotRecordatorioPage ()
		{
			InitializeComponent ();
			BindingContext = new NotaIsNotRecordatorioViewModel();
		}

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Nota selectedItem)
            {
                await Navigation.PushAsync(new EditNotaPage(selectedItem));
            }

     // Limpiar la selección para permitir futuros eventos de selección
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}