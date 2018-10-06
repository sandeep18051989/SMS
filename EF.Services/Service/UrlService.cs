using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public class UrlService : IUrlService
	{
		private readonly IRepository<CustomPageUrl> _customPageUrlRepository;
		public UrlService(IRepository<CustomPageUrl> customPageUrlRepository)
		{
			this._customPageUrlRepository = customPageUrlRepository;
		}

		#region Custom Page URL Methods

		public virtual void DeleteCustomPageUrl(CustomPageUrl urlRecord)
		{
			if (urlRecord == null)
				throw new ArgumentNullException("Custom Page URL");

			_customPageUrlRepository.Delete(urlRecord);
		}

		public virtual void DeleteCustomPageUrls(IList<CustomPageUrl> urlRecords)
		{
			if (urlRecords == null)
				throw new ArgumentNullException("Custom Page URL");

			_customPageUrlRepository.Delete(urlRecords);
		}

		public virtual IList<CustomPageUrl> GetCustomPageUrlsByIds(int[] urlRecordIds)
		{
			var query = _customPageUrlRepository.Table;

			return query.Where(p => urlRecordIds.Contains(p.Id)).ToList();
		}

		public virtual CustomPageUrl GetCustomPageUrlById(int urlRecordId)
		{
			if (urlRecordId == 0)
				return null;

			return _customPageUrlRepository.GetByID(urlRecordId);
		}

		public virtual void InsertCustomPageUrl(CustomPageUrl urlRecord)
		{
			if (urlRecord == null)
				throw new ArgumentNullException("Custom Page URL");

			_customPageUrlRepository.Insert(urlRecord);
		}

		public virtual void UpdateCustomPageUrl(CustomPageUrl urlRecord)
		{
			if (urlRecord == null)
				throw new ArgumentNullException("Custom Page URL");

			_customPageUrlRepository.Update(urlRecord);
		}

		public virtual CustomPageUrl GetBySlug(string slug)
		{
			if (String.IsNullOrEmpty(slug))
				return null;

			var query = from ur in _customPageUrlRepository.Table
							where ur.Slug == slug
							orderby ur.IsActive descending, ur.Id
							select ur;
			var urlRecord = query.FirstOrDefault();
			return urlRecord;
		}

		public virtual string GetSlug(int entityId, string entityName)
		{
			var source = _customPageUrlRepository.Table;
			var query = from ur in source
							where ur.EntityId == entityId &&
							ur.EntityName == entityName &&
							ur.IsActive
							orderby ur.Id descending
							select ur.Slug;
			var slug = query.FirstOrDefault();

			if (slug == null)
				slug = "";

			return slug;
		}

		public virtual IList<CustomPageUrl> GetAllCustomPageUrls(string slug = "", bool? active = null)
		{
			var query = _customPageUrlRepository.Table.ToList();
			if (!String.IsNullOrWhiteSpace(slug))
				query = query.Where(ur => ur.Slug.Contains(slug)).ToList();

			query = query.OrderBy(ur => ur.Slug).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.ToList();
		}

		public virtual void SaveSlug<T>(T entity, string slug) where T : BaseEntity, ISlugSupported
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			int entityId = entity.Id;
			string entityName = typeof(T).Name;

			var query = from ur in _customPageUrlRepository.Table
							where ur.EntityId == entityId &&
							ur.EntityName == entityName
							orderby ur.Id descending
							select ur;
			var allCustomPageUrls = query.ToList();
			var activeCustomPageUrl = allCustomPageUrls.FirstOrDefault(x => x.IsActive);

			if (activeCustomPageUrl == null && !string.IsNullOrWhiteSpace(slug))
			{
				//find in non-active records with the specified slug
				var nonActiveRecordWithSpecifiedSlug = allCustomPageUrls
					 .FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase) && !x.IsActive);
				if (nonActiveRecordWithSpecifiedSlug != null)
				{
					//mark non-active record as active
					nonActiveRecordWithSpecifiedSlug.IsActive = true;
					UpdateCustomPageUrl(nonActiveRecordWithSpecifiedSlug);
				}
				else
				{
					//new record
					var urlRecord = new CustomPageUrl
					{
						EntityId = entityId,
						EntityName = entityName,
						Slug = slug,
						IsActive = true,
						CreatedOn = DateTime.Now,
						ModifiedOn = DateTime.Now,
						UserId = entity.UserId
					};
					InsertCustomPageUrl(urlRecord);
				}
			}

			if (activeCustomPageUrl != null && string.IsNullOrWhiteSpace(slug))
			{
				//disable the previous active URL record
				activeCustomPageUrl.IsActive = false;
				UpdateCustomPageUrl(activeCustomPageUrl);
			}

			if (activeCustomPageUrl != null && !string.IsNullOrWhiteSpace(slug))
			{
				//it should not be the same slug as in active URL record
				if (!activeCustomPageUrl.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
				{
					//find in non-active records with the specified slug
					var nonActiveRecordWithSpecifiedSlug = allCustomPageUrls
						 .FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase) && !x.IsActive);
					if (nonActiveRecordWithSpecifiedSlug != null)
					{
						//mark non-active record as active
						nonActiveRecordWithSpecifiedSlug.IsActive = true;
						UpdateCustomPageUrl(nonActiveRecordWithSpecifiedSlug);

						//disable the previous active URL record
						activeCustomPageUrl.IsActive = false;
						UpdateCustomPageUrl(activeCustomPageUrl);
					}
					else
					{
						//insert new record
						//we do not update the existing record because we should track all previously entered slugs
						//to ensure that URLs will work fine
						var urlRecord = new CustomPageUrl
						{
							EntityId = entityId,
							EntityName = entityName,
							Slug = slug,
							IsActive = true,
							CreatedOn = DateTime.Now,
							ModifiedOn = DateTime.Now,
							UserId = entity.UserId
						};
						InsertCustomPageUrl(urlRecord);

						//disable the previous active URL record
						activeCustomPageUrl.IsActive = false;
						UpdateCustomPageUrl(activeCustomPageUrl);
					}

				}
			}
		}

		#endregion

	}
}
