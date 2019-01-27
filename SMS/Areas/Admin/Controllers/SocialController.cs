using EF.Services.Social;
using System.Web.Mvc;
namespace SMS.Areas.Admin.Controllers
{
    public partial class SocialController : AdminAreaController
    {
		#region Fields

        private readonly ISocialModelFactory _socialAuthenticationModelFactory;

        #endregion

        #region Ctor

        public SocialController(ISocialModelFactory socialAuthenticationModelFactory)
        {
            this._socialAuthenticationModelFactory = socialAuthenticationModelFactory;
        }

        #endregion

        #region Methods

        public virtual RedirectResult RemoveParameterAssociation(string returnUrl)
        {
            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("Root");

            SocialAuthorizerHelper.RemoveParameters();
            return Redirect(returnUrl);
        }

        [ChildActionOnly]
        public virtual ActionResult Configurations()
        {
            var model = _socialAuthenticationModelFactory.PrepareSocialMethodsModel();
            return PartialView(model);
        }

        #endregion
    }
}
