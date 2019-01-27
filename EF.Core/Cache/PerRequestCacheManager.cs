using System.Collections;
using System.Web;
using System.Linq;

namespace EF.Core.Cache
{
    public partial class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContextBase _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public PerRequestCacheManager(HttpContextBase context)
        {
            this._context = context;
        }
        
        protected virtual IDictionary GetItems()
        {
            if (_context != null)
                return _context.Items;

            return null;
        }

        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }

        public virtual bool IsSet(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;
            
            return (items[key] != null);
        }

        public virtual void Remove(string key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key);
        }
        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

        public virtual void Clear()
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Clear();
        }

        public virtual void Dispose()
        {
        }
    }
}
