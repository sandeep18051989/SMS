using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public class MenuService : IMenuService
    {
		#region Fields

		public readonly IRepository<Menus> _menuRepository;

		#endregion

		#region Const

		public MenuService(IRepository<Menus> menuRepository)
		{
			this._menuRepository = menuRepository;
		}

        #endregion


        #region IMenu Members

        public void Insert(Menus menu)
		{
            _menuRepository.Insert(menu);
		}

		public void Update(Menus menu)
		{
            _menuRepository.Update(menu);
		}

		#endregion

		#region Utilities

		public Menus GetMenuById(int menuId)
		{
			if (menuId > 0)
			{
				var menu = from c in _menuRepository.Table
							  orderby c.Id
							  where c.Id == menuId
                              select c;
				var query = menu.FirstOrDefault();
				return query;
			}
			else
			{
				return null;
			}
		}

        public IList<Menus> GetAllMenus()
        {
            return _menuRepository.Table.Distinct().ToList();
        }

        public IList<Menus> GetParentMenus()
        {
            return _menuRepository.Table.Where(x=>x.ParentMenuId == 0).Distinct().ToList();
        }

        public Menus GetParentMenuById(int menuId)
        {
            return _menuRepository.Table.FirstOrDefault(x=>x.ParentMenuId == menuId);
        }

        #endregion
    }
}
