using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Services.Culture
{
    public interface ICultureHelper
    {
        IList<Tuple<int, DateTime, DateTime>> GetTotalWeeksInAMonth(int year, int month);
    }
}
