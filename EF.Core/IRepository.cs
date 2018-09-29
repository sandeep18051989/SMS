using System.Collections.Generic;
using System.Linq;

namespace EF.Core
{
	public partial interface IRepository<T> where T : BaseEntity
	{
		IEnumerable<T> GetAll();
		T GetByID(int Id);
		void Insert(T entity);
		void Insert(IEnumerable<T> entities);

		void Delete(int Id);
		void Delete(T entity);
		void Update(T entity);
		IQueryable<T> Table { get; }
		void Delete(IEnumerable<T> entities);

		/// <summary>
		/// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
		/// </summary>
		IQueryable<T> TableNoTracking { get; }
	}
}
