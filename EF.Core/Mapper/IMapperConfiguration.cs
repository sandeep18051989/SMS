using AutoMapper;
using System;

namespace EF.Core.Mapper
{
    public interface IMapperConfiguration
    {
        Action<IMapperConfigurationExpression> GetConfiguration();

        int Order { get; }
    }
}
