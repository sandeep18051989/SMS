using System;
using System.Collections.Generic;

namespace EF.Core
{
    public class SinglePattern<T> : SinglePattern
    {
        static T instance;

        public static T Instance
        {
            get { return instance; }
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }

    public class SingletonList<T> : SinglePattern<IList<T>>
    {
        static SingletonList()
        {
            SinglePattern<IList<T>>.Instance = new List<T>();
        }

        /// <summary>The singleton instance for the specified type T. Only one instance (at the time) of this list for each type of T.</summary>
        public new static IList<T> Instance
        {
            get { return SinglePattern<IList<T>>.Instance; }
        }
    }

    public class SingletonDictionary<TKey, TValue> : SinglePattern<IDictionary<TKey, TValue>>
    {
        static SingletonDictionary()
        {
            SinglePattern<Dictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
        }

        /// <summary>The singleton instance for the specified type T. Only one instance (at the time) of this dictionary for each type of T.</summary>
        public new static IDictionary<TKey, TValue> Instance
        {
            get { return SinglePattern<Dictionary<TKey, TValue>>.Instance; }
        }
    }

    public class SinglePattern
    {
        static SinglePattern()
        {
            allSingletons = new Dictionary<Type, object>();
        }

        static readonly IDictionary<Type, object> allSingletons;

        public static IDictionary<Type, object> AllSingletons
        {
            get { return allSingletons; }
        }
    }
}
