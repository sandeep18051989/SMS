using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Areas.Admin.Models;
using SMS.Areas.Admin.Validations;
using SMS.Validations;
using System.Web.Mvc;

namespace SMS.Models
{
    [Validator(typeof(UserModelValidation))]
    public partial class UserModel : BaseEntityModel
	{
		public UserModel()
		{
			ChangePassword = new ChangePasswordModel();
			AvailableRoles = new List<RoleModel>();
            ProfilePicture = new PictureModel();
            CoverPicture = new PictureModel();
            Student = new StudentModel();
            Teacher = new TeacherModel();
            StudentProfilePicture = new PictureModel();
            StudentCoverPicture = new PictureModel();
            TeacherProfilePicture = new PictureModel();
            TeacherCoverPicture = new PictureModel();
        }

        [Remote("CheckUsernameExists", "Account", ErrorMessage = "User Name already in use")]
        public string Username { get; set; }

        [Remote("CheckEmailExists", "Account", ErrorMessage = "Email already in use")]
        public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		public int[] SelectedRoleIds { get; set; }
		public bool IsApproved { get; set; }
		public bool IsActive { get; set; }
		public bool IsBlocked { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public bool IsStudent { get; set; }
        public bool IsTeacher { get; set; }
        public Guid UserGuid { get; set; }
        public int ProfilePictureId { get; set; }
        public int CoverPictureId { get; set; }
        public PictureModel ProfilePicture { get; set; }
        public PictureModel CoverPicture { get; set; }

        public PictureModel StudentProfilePicture { get; set; }
        public PictureModel StudentCoverPicture { get; set; }

        public PictureModel TeacherProfilePicture { get; set; }
        public PictureModel TeacherCoverPicture { get; set; }

        public StudentModel Student { get; set; }
        public TeacherModel Teacher { get; set; }
        public ChangePasswordModel ChangePassword { get; set; }
		public IList<RoleModel> AvailableRoles { get; set; }

	}

    [Validator(typeof(AdminUserModelValidation))]
    public partial class AdminUserModel : BaseEntityModel
    {
        public AdminUserModel()
        {
            ChangePassword = new ChangePasswordModel();
            AvailableRoles = new List<RoleModel>();
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public int[] SelectedRoleIds { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public Guid UserGuid { get; set; }
        public int ProfilePictureId { get; set; }
        public int CoverPictureId { get; set; }
        public ChangePasswordModel ChangePassword { get; set; }
        public IList<RoleModel> AvailableRoles { get; set; }

    }
}