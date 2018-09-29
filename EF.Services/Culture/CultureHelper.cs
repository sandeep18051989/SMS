using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Services.Culture
{
    public class CultureHelper : ICultureHelper
    {

        public IList<Tuple<int, DateTime, DateTime>> GetTotalWeeksInAMonth(int year, int month)
        {
            Calendar cal = CultureInfo.CurrentCulture.Calendar;
            IEnumerable<int> daysInMonth = Enumerable.Range(1, cal.GetDaysInMonth(year, month));

            List<Tuple<int, DateTime, DateTime>> listOfWeeks = daysInMonth
                .Select(day => new DateTime(year, month, day))
                .GroupBy(d => cal.GetWeekOfYear(d, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                //.Select(g => Tuple.Create(g.Key, g.First(), g.Last(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)))
                .Select(g => Tuple.Create(g.Key, g.First(), g.Last()))
                .ToList();

            return listOfWeeks;
        }
    }
}
