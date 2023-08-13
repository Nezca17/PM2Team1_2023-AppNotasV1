using Plugin.LocalNotification;
using PM2Team1_2023_AppNotasV1.Models;
using PM2Team1_2023_AppNotasV1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static Android.Icu.Text.CaseMap;
using static Android.Telecom.Call;
using System.Threading.Tasks;

namespace PM2Team1_2023_AppNotasV1.ViewModels
{
   public class DashBoardViewModel : BaseViewModel
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public DashBoardViewModel() {

            LoadDataDash();
        }
        public int IdNotiR;
        public ObservableCollection<Nota> listViewSource1;


        public int IdNotif
        {
            get { return IdNotiR; }
            set { SetValue(ref IdNotiR, value); }
        }

        public ObservableCollection<Nota> ListViewSource
        {
            get { return this.listViewSource1; }
            set
            {
                SetValue(ref this.listViewSource1, value);
            }

        }

        public async void LoadDataDash()
        {
            try {
                var notas = await firebaseHelper.GetNotas();
                ListViewSource = new ObservableCollection<Nota>(notas);


                foreach (var item in ListViewSource)
                {

                    TimeSpan horaMenos20 = item.Hora.Subtract(TimeSpan.FromMinutes(20));
                    DateTime HorayFecha = item.Fecha.Date + horaMenos20;
                    DateTime HorayFecha2 = item.Fecha.Date + item.Hora;
                    var notification = new NotificationRequest
                    {


                        Title = item.Titulo,
                        NotificationId = item.IdNoti,
                        Description = item.Detalles,
                        Schedule =
                    {
                        NotifyTime = HorayFecha
                    }

                    };

                    await LocalNotificationCenter.Current.Show(notification);

                    var notification2 = new NotificationRequest
                    {
                        Title = item.Titulo,
                        NotificationId = item.IdNoti + 1,
                        Description = item.Detalles,
                        Schedule =
                    {
                        NotifyTime = HorayFecha2
                    }

                    };

                    await LocalNotificationCenter.Current.Show(notification2);
                }
                Console.WriteLine("***********************************Notificaciones Cargadas Dash");

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
