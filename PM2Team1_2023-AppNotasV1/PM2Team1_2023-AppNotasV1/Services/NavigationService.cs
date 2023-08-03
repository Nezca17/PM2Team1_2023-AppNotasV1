using PM2Team1_2023_AppNotasV1.Interfaces;
using PM2Team1_2023_AppNotasV1.ViewModels;
using PM2Team1_2023_AppNotasV1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PM2Team1_2023_AppNotasV1.Services
{
    public class NavigationService
{
        public Task InitializeAsync()
        {
            var _firebaseService = DependencyService.Get<IFirebaseAuthService>();
            if (_firebaseService.IsUserSigned())
            {
                return NavigateToAsync<Dashboard>();
            }
            else
            {
                return NavigateToAsync<LoginPage>();
            }
        }

        private Task NavigateToAsync<T>()
        {
            throw new NotImplementedException();
        }
    }

}
