using EF.Core.Data;
using EF.Core.Enums;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface IMenuService
    {
        void Insert(Menus menu);
        void Update(Menus menu);
        Menus GetMenuById(int menuId);
        IList<Menus> GetAllMenus();
        IList<Menus> GetParentMenus();
        Menus GetParentMenuById(int menuId);
    }
}
