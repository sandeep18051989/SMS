using EF.Core;
using EF.Core.Mapper;

namespace SMS.Areas.Admin.Mappers
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public void Execute()
        {
            AdminMapperConfiguration.Init();
        }
        
        public int Order
        {
            get { return 0; }
        }
    }
}