using PM2Team1_2023_AppNotasV1.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PM2Team1_2023_AppNotasV1.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}