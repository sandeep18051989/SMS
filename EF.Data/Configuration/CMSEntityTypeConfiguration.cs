using System.Data.Entity.ModelConfiguration;

namespace EF.Data.Configuration
{
    public abstract class CMSEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected CMSEntityTypeConfiguration()
        {
            PostInitialize();
        }

        protected virtual void PostInitialize()
        {
            
        }
    }
}