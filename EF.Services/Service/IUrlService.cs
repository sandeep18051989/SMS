using EF.Core.Data;
using System.Collections.Generic;
using EF.Core;

namespace EF.Services.Service
{
	public interface IUrlService
	{
		#region Custom Page URL Methods

		void DeleteCustomPageUrl(CustomPageUrl urlRecord);

		void DeleteCustomPageUrls(IList<CustomPageUrl> urlRecords);

		IList<CustomPageUrl> GetCustomPageUrlsByIds(int[] urlRecordIds);

		CustomPageUrl GetCustomPageUrlById(int urlRecordId);

		void InsertCustomPageUrl(CustomPageUrl urlRecord);

		void UpdateCustomPageUrl(CustomPageUrl urlRecord);

		CustomPageUrl GetBySlug(string slug);

		string GetSlug(int entityId, string entityName);

		IList<CustomPageUrl> GetAllCustomPageUrls(string slug = "", bool? active = null);

		void SaveSlug<T>(T entity, string slug) where T : BaseEntity, ISlugSupported;

		#endregion

	}
}
