using EF.Core.Enums;
using System;
using System.Collections.Generic;

namespace EF.Core.Data
{
    public partial class SystemLog : BaseEntity
    {
        public int ErrorId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public bool IsException { get; set; }
        public int Level { get; set; }
        public bool IsFixed { get; set; }
        public int EntityId { get; set; }

        public string Url { get; set; }

        public string IpAddress { get; set; }

        public string EntityTypeName { get; set; }
        public EntityTypeEnum EntityType { get; set; }

        public LogLevel LogLevel { get; set; }

    }
}
