using EF.Core.Data;
using System.Collections.Generic;

namespace EF.Services.Service
{
	public interface ICustomPageService
	{
		void Insert(CustomPage customPage);

		void Update(CustomPage customPage);

		void Delete(int id);
	    IList<CustomPage> GetAllCustomPages(bool? onlyActive = null, bool? showSystemDefined = null);
		CustomPage GetCustomPageById(int pageId);

		CustomPage GetCustomPageByName(string pageName);

		CustomPage GetCustomPageBySystemName(string pageName);

		IList<CustomPage> GetAllCustomPagesByTemplate(int templateId);

	}
}
