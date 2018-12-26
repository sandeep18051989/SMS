using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
    public class CustomPageService : ICustomPageService
    {
        public readonly IRepository<Template> _templateRepository;
        public readonly IRepository<DataToken> _tokenRepository;
        public readonly IRepository<CustomPage> _customPageRepository;
        public CustomPageService(IRepository<Template> templateRepository, IRepository<DataToken> tokenRepository, IRepository<CustomPage> customPageRepository)
        {
            this._templateRepository = templateRepository;
            this._tokenRepository = tokenRepository;
            this._customPageRepository = customPageRepository;
        }
        #region IBlogService Members

        public void Insert(CustomPage customPage)
        {
            _customPageRepository.Insert(customPage);
        }

        public void Update(CustomPage customPage)
        {
            _customPageRepository.Update(customPage);
        }

        public void Delete(int id)
        {
            _customPageRepository.Delete(id);
        }

        #endregion

        public IList<CustomPage> GetAllCustomPages(bool? onlyActive = null, bool? showSystemDefined = null)
        {
            return _customPageRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && (!showSystemDefined.HasValue || x.IsSystemDefined == showSystemDefined.Value) && x.IsDeleted == false).ToList();
        }

        public CustomPage GetCustomPageById(int pageId)
        {
            if (pageId > 0)
                return _customPageRepository.GetByID(pageId);

            return null;
        }

        public CustomPage GetCustomPageByName(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
                return _customPageRepository.Table.FirstOrDefault(x => x.Name.ToLower() == pageName.ToLower());

            return null;
        }

        public CustomPage GetCustomPageBySystemName(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
                return _customPageRepository.Table.FirstOrDefault(x => x.SystemName.Trim().ToLower() == pageName.Trim().ToLower());

            return null;
        }

        public IList<CustomPage> GetAllCustomPagesByTemplate(int templateId)
        {
            return _customPageRepository.Table.Where(a => a.TemplateId == templateId).OrderByDescending(a => a.ModifiedOn).ToList();
        }

    }
}
