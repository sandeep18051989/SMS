using System;
using System.Web.Routing;
namespace EF.Core
{
    public partial class SocialAuthenticationMethodModel
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}