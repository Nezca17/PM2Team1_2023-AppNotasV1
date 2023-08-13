using Android.App;
using Android.Content;
using Android.Provider;
using PM2Team1_2023_AppNotasV1.Droid.Services;
using PM2Team1_2023_AppNotasV1.Interfaces;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(CalendarService))]
namespace PM2Team1_2023_AppNotasV1.Droid.Services
{
    public class CalendarService : ICalendarService
    {
        public void AddEventToCalendar(string title, string description, DateTime startDate, DateTime endDate)
        {
            Intent intent = new Intent(Intent.ActionInsert)
                .SetData(CalendarContract.Events.ContentUri)
                .PutExtra(CalendarContract.Events.InterfaceConsts.Title, title)
                .PutExtra(CalendarContract.Events.InterfaceConsts.Description, description)
                .PutExtra(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMillis(startDate))
                .PutExtra(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMillis(endDate));


            Android.App.Application.Context.StartActivity(intent);
        }

        private long GetDateTimeMillis(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}