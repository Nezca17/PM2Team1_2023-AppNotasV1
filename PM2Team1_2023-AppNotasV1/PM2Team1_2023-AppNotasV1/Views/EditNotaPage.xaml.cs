
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
public partial class EditNotaPage : ContentPage
{
    public EditNotaPage()
    {
        InitializeComponent();
        BindingContext = new EditNotaViewModel(); 
    }
    
    public EditNotaPage(Nota _nota)
        {
            InitializeComponent();
            BindingContext = new EditNotaViewModel(_nota);
        }

}
}