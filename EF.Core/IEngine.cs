using System;

namespace EF.Core
{
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }
        
        void Initialize(CMSConfig config);

        T Resolve<T>() where T : class;

        object Resolve(Type type);

        T[] ResolveAll<T>();
    }
}
