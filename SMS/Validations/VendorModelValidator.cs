using System;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class VendorModelValidator : EntityValidatorBase<VendorModel>
	{
		public VendorModelValidator()
		{
			RuleFor(x => x.RegNumber).NotEmpty().WithMessage("Please enter registration number");
			RuleFor(x => x.MobileContact).NotEmpty().Length(10).WithMessage("Please enter valid mobile contact without country code");
			RuleFor(x => x.OfficeContact).NotEmpty().Length(10).WithMessage("Please enter valid office contact without country code");
			RuleFor(x => x.VendorName).NotEmpty().WithMessage("Please enter vendor name");
			RuleFor(x => x.AcadmicYear).NotEmpty().WithMessage("Please choose academic year");
		}
	}
}
