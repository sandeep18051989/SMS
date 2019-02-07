using System.Linq;
using TrackerEnabledDbContext;
using TrackerEnabledDbContext.Common.Models;

namespace EF.Data.Track
{
    public class EFContext : TrackerContext
    {
        public System.Data.Entity.DbSet<EF.Core.Data.User> Users { get; set; }
    }
}
