using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
    public class SliderController : AdminAreaController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IUserContext _userContext;
        private readonly ISliderService _sliderService;
        public readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;

        #endregion Fileds

        #region Constructor

        public SliderController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IPermissionService permissionService)
        {
            this._userService = userService;
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._sliderService = sliderService;
            this._settingService = settingService;
            this._permissionService = permissionService;
        }

        #endregion

        #region Utilities

        public ActionResult LoadGrid()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() + "][name]")?.FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]")?.FirstOrDefault();


                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all data    
                var sliderData = (from tempsliders in _sliderService.GetAllSliders() select tempsliders);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    sliderData = sliderData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var enumerable = sliderData as Slider[] ?? sliderData.ToArray();
                recordsTotal = enumerable.Count();
                //Paging     
                var data = enumerable.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new SliderModel()
                        {
                            IsActive = x.IsActive,
                            Id = x.Id,
                            Name = x.Name.Trim(),
                            UserId = x.UserId,
                            CreatedOn = x.CreatedOn,
                            DisplayArea = x.DisplayArea,
                            DisplayAreaString = x.DisplayArea > 0 ? EnumExtensions.GetDescriptionByValue<DisplayAreas>(x.DisplayArea) : "",
                            DisplayOrder = x.DisplayOrder,
                            MaxPictures = x.MaxPictures,
                            ShowCaption = x.ShowCaption,
                            ShowNextPrevIndicators = x.ShowNextPrevIndicators,
                            ShowThumbnails = x.ShowThumbnails
                        }).OrderBy(x => x.Name).ToList()
                    },
                    ContentEncoding = Encoding.Default,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        public ActionResult LoadPictureGrid(int id)
        {
            try
            {
                var eventData = (from associatedpicture in _sliderService.GetAllSliderPicturesBySliderId(id) select associatedpicture).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = eventData.Select(x => new PictureModel()
                        {
                            Id = x.Id,
                            Src = x.PictureSrc,
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            CreatedDateString = x.CreatedOn.HasValue ? x.CreatedOn.Value.ToString("U") : "",
                            AlternateText = x.AlternateText
                        })
                    },
                    ContentEncoding = Encoding.Default,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        public ActionResult DeleteSliderPicture(int id, int sliderid)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (id == 0 || sliderid == 0)
                throw new Exception("Picture id not found");

            var slider = _sliderService.GetSliderById(sliderid);
            if (slider != null)
            {
                var picture = slider.Pictures.FirstOrDefault(x => x.Id == id);
                if (picture != null)
                    slider.Pictures.Remove(picture);

                _sliderService.Update(slider);
            }

            SuccessNotification("Slider picture deleted successfully");
            return new JsonResult()
            {
                Data = true,
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SliderPictureAdd(int pictureId, int sliderId, string captionText = null)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (pictureId == 0)
                throw new ArgumentException();

            var thisslider = _sliderService.GetSliderById(sliderId);
            if (thisslider == null)
                throw new ArgumentException("No slider found with the specified id");

            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            picture.AlternateText = !string.IsNullOrEmpty(captionText) ? captionText.Trim() : "";
            _pictureService.Update(picture);

            thisslider.Pictures.Add(picture);
            _sliderService.Update(thisslider);

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult UpdateSliderPicture(int pictureId, string captionText=null)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (pictureId == 0)
                throw new ArgumentException();

            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            picture.AlternateText = !string.IsNullOrEmpty(captionText) ? captionText.Trim() : "";
            _pictureService.Update(picture);

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Action Methods

        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            var model = new SliderModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            var model = new SliderModel();
            if (id == 0)
                throw new Exception("Slider Id Missing");

            var eve = _sliderService.GetSliderById(id);
            if (eve != null)
            {
                model = eve.ToModel();
            }

            // Bind Display Areas
            var displayAreasList = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(typeof(DisplayAreas)))
            {
                displayAreasList.Add((int)item, EnumExtensions.GetDescriptionByValue<DisplayAreas>(Convert.ToInt32(item)));
            }
            model.AvailableAreas.Add(new SelectListItem() { Text = "Select Display Area", Value = "0", Selected = true });
            foreach (var item in displayAreasList)
            {
                model.AvailableAreas.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }

            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(SliderModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate slider, if any
            var slider = _sliderService.GetSliderByName(model.Name);
            if (slider != null && slider.Id != model.Id)
                ModelState.AddModelError("Name", "An Slider with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                model.CreatedOn = slider.CreatedOn;
                slider = model.ToEntity(slider);
                slider.ModifiedOn = DateTime.Now;
                _sliderService.Update(slider);
            }
            else
            {
                ErrorNotification("An error occured while updating slider. Please try again.");
                // Bind Display Areas
                var displayAreasList = new Dictionary<int, string>();
                foreach (var item in Enum.GetValues(typeof(DisplayAreas)))
                {
                    displayAreasList.Add((int)item, EnumExtensions.GetDescriptionByValue<DisplayAreas>(Convert.ToInt32(item)));
                }
                model.AvailableAreas.Clear();
                model.AvailableAreas.Add(new SelectListItem() { Text = "Select Display Area", Value = "0", Selected = true });
                foreach (var item in displayAreasList)
                {
                    model.AvailableAreas.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
                }
                return View(model);
            }

            SuccessNotification("Slider updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            var model = new SliderModel();
            // Bind Display Areas
            var displayAreasList = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(typeof(DisplayAreas)))
            {
                displayAreasList.Add((int)item, EnumExtensions.GetDescriptionByValue<DisplayAreas>(Convert.ToInt32(item)));
            }
            model.AvailableAreas.Add(new SelectListItem() { Text = "Select Display Area", Value = "0", Selected = true });
            foreach (var item in displayAreasList)
            {
                model.AvailableAreas.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(SliderModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            var currentUser = _userContext.CurrentUser;
            // Check for duplicate slider, if any
            var slider = _sliderService.GetSliderByName(model.Name);
            if (slider != null)
                ModelState.AddModelError("Name", "An Slider with the same name already exists. Please choose a different name.");

            model.UserId = currentUser.Id;
            if (ModelState.IsValid)
            {
                slider = model.ToEntity();
                slider.ModifiedOn = DateTime.Now;
                slider.CreatedOn = DateTime.Now;
                slider.UserId = _userContext.CurrentUser.Id;
                _sliderService.Insert(slider);
            }
            else
            {
                ErrorNotification("An error occured while creating slider. Please try again.");
                // Bind Display Areas
                var displayAreasList = new Dictionary<int, string>();
                foreach (var item in Enum.GetValues(typeof(DisplayAreas)))
                {
                    displayAreasList.Add((int)item, EnumExtensions.GetDescriptionByValue<DisplayAreas>(Convert.ToInt32(item)));
                }
                model.AvailableAreas.Clear();
                model.AvailableAreas.Add(new SelectListItem() { Text = "Select Display Area", Value = "0", Selected = true });
                foreach (var item in displayAreasList)
                {
                    model.AvailableAreas.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
                }
                return View(model);
            }

            SuccessNotification("Slider created successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = slider.Id });
            }
            return RedirectToAction("List");
        }

        #endregion

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            if (!_sliderService.GetSliderById(id).IsSystemDefined)
            {
                _sliderService.Delete(id);
            }

            SuccessNotification("Slider deleted successfully.");
            return RedirectToAction("List");
        }

        public PartialViewResult GetPicturesList()
        {
            var pictureList = _pictureService.GetAllPictures().ToList();
            var model = pictureList.Select(item => new PictureModel
            {
                AlternateText = item.AlternateText,
                Height = item.Height,
                IsActive = item.IsActive,
                IsLogo = item.IsLogo,
                Src = item.PictureSrc,
                Url = item.Url,
                UserId = item.UserId,
                Id = item.Id
            }).ToList();
            return PartialView("_PicturesList", model);
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _sliderService.DeleteSliders(_sliderService.GetSliderByIds(selectedIds.ToArray()).ToList());
            }

            SuccessNotification("Sliders deleted successfully.");
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ToggleActiveStatus(int id)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var slider = _sliderService.GetSliderById(id);

            if (slider != null)
                _sliderService.ToggleActiveStatus(id);

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult ToggleCaptionStatus(int id)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var slider = _sliderService.GetSliderById(id);

            if (slider != null)
                _sliderService.ToggleCaptionStatus(id);

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult ToggleIndicatorStatus(int id)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var slider = _sliderService.GetSliderById(id);

            if (slider != null)
                _sliderService.ToggleIndicatorStatus(id);

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult ToggleThumbnailStatus(int id)
        {
            if (!_permissionService.Authorize("ManageSlider"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var slider = _sliderService.GetSliderById(id);

            if (slider != null)
                _sliderService.ToggleThumbnailStatus(id);

            return Json(new { Result = true });
        }
    }
}
