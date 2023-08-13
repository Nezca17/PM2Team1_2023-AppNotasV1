using System;
using System.Collections.Generic;
using System.Text;

namespace PM2Team1_2023_AppNotasV1.Interfaces
{
        public interface ICalendarService
    {
        void AddEventToCalendar(string title, string description, DateTime startDate, DateTime endDate);
    }

}
