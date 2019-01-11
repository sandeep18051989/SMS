using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using EF.Core;
using EF.Core.Data;
using System.Data.Entity.Validation;
using TrackerEnabledDbContext.Common.Interfaces;

namespace EF.Data
{
	public partial class EFRepository<T> : IRepository<T> where T : BaseEntity
	{
		private readonly ITrackerContext _context;
		private IDbSet<T> _entity;

		public EFRepository(ITrackerContext context)
		{
			this._context = context;
		}
		public IDbSet<T> Entity
		{
			get
			{
				if (this._entity == null)
				{
					this._entity = _context.Set<T>();
				}
				return this._entity;
			}
		}

		#region Members

		public void Delete(int Id)
		{
			var entity = this.Entity.Find(Id);
			this.Entity.Remove(entity);

			this._context.SaveChanges(entity.UserId);
		}

		public virtual void Delete(T entity)
		{
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");

				this.Entities.Remove(entity);

				this._context.SaveChanges(entity.UserId);
			}
			catch (DbEntityValidationException dbEx)
			{
				throw new Exception(GetFullErrorText(dbEx), dbEx);
			}
		}

		public virtual void Delete(IEnumerable<T> entities)
		{
			try
			{
				if (entities == null)
					throw new ArgumentNullException("entities");

				foreach (var entity in entities)
					this.Entities.Remove(entity);

				this._context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				throw new Exception(GetFullErrorText(dbEx), dbEx);
			}
		}

		public virtual IEnumerable<T> GetAll()
		{
			return this.Entity.ToList();
		}

		public virtual T GetByID(int Id)
		{
			return Entity.Find(Id);
		}

		public virtual void Insert(T entity)
		{
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");

				this.Entities.Add(entity);
				this._context.SaveChanges(entity.UserId);
			}
			catch (DbEntityValidationException dbEx)
			{
				var msg = string.Empty;

				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

				var fail = new Exception(msg, dbEx);
				//Debug.WriteLine(fail.Message, fail);
				throw fail;
			}
		}

		public virtual void Insert(IEnumerable<T> entities)
		{
			try
			{
				if (entities == null)
					throw new ArgumentNullException("entities");

				foreach (var entity in entities)
					this.Entities.Add(entity);

				this._context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				throw new Exception(GetFullErrorText(dbEx), dbEx);
			}
		}

		public virtual void Update(T entity)
		{
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");

                this._context.SaveChanges(entity.UserId);
			}
			catch (DbEntityValidationException dbEx)
			{
				var msg = string.Empty;

				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

				var fail = new Exception(msg, dbEx);
				throw fail;
			}
		}

	    /// <summary>
	    /// Update entities
	    /// </summary>
	    /// <param name="entities">Entities</param>
	    public virtual void Update(IEnumerable<T> entities)
	    {
	        try
	        {
	            if (entities == null)
	                throw new ArgumentNullException("entities");

                _context.Entry(this.Entity).State = EntityState.Modified;
                this._context.SaveChanges();
	        }
	        catch (DbEntityValidationException dbEx)
	        {
	            throw new Exception(GetFullErrorText(dbEx), dbEx);
	        }
	    }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
		{
			get
			{
				return this.Entities;
			}
		}

		/// <summary>
		/// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
		/// </summary>
		public virtual IQueryable<T> TableNoTracking
		{
			get
			{
				return this.Entities.AsNoTracking();
			}
		}

		/// <summary>
		/// Entities
		/// </summary>
		protected virtual IDbSet<T> Entities
		{
			get
			{
				if (_entity == null)
					_entity = _context.Set<T>();
				return _entity;
			}
		}

		protected string GetFullErrorText(DbEntityValidationException exc)
		{
			var msg = string.Empty;
			foreach (var validationErrors in exc.EntityValidationErrors)
				foreach (var error in validationErrors.ValidationErrors)
					msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
			return msg;
		}

		#endregion

	}

}
