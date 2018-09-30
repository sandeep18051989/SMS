using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class MenuController : PublicHttpController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IUserContext _userContext;
        private readonly ISliderService _sliderService;
        private readonly ISettingService _settingService;
        private readonly IMenuService _menuService;

        #endregion Fileds

        #region Constructor

        public MenuController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IMenuService menuService)
        {
            this._userService = userService;
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._sliderService = sliderService;
            this._settingService = settingService;
            this._menuService = menuService;
        }

        #endregion

        // GET: Menu
        public ActionResult Index()
        {
            var model = new List<MenuModel>();
            var lstMenu = _menuService.GetAllMenus().ToList();

            if (lstMenu.Count > 0)
            {
                var menuModel = new MenuModel();
                foreach(var menu in lstMenu)
                {
                    menuModel.IsActive = menu.IsActive;
                    menuModel.DisplayOrder = menu.DisplayOrder;
                    menuModel.MenuId = menu.Id;
                    menuModel.Name = menu.Name;
                    menuModel.ParentMenuId = menu.ParentMenuId;
                    menuModel.Title = menu.Title;
                    menuModel.Url = menu.Url;
                    menuModel.UserId = menu.UserId;
                    model.Add(menuModel);
                }
            }

            return View(model);
        }

        // GET: Menu/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Menu/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Menu/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Menu/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
