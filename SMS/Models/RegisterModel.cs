using SMS.Validations;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Models
{
    [Validator(typeof(RegisterModelValidator))]
    public partial class RegisterModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [Remote("CheckUsernameExists", "Account", ErrorMessage = "Username already in use")]
        public string Username { get; set; }
        public string Phone { get; set; }
        [Remote("CheckEmailExists", "Account", ErrorMessage = "Email address already in use")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [UIHint("Picture")]
        public int ProfilePictureId { get; set; }
        [UIHint("Picture")]
        public int CoverPictureId { get; set; }
        public bool AcceptTermAndConditions { get; set; }

    }
}