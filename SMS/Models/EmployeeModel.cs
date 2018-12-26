using System;
using System.Collections.Generic;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
	[Validator(typeof(EmployeeModelValidator))]
	public partial class EmployeeModel : BaseEntityModel
	{
		public EmployeeModel()
		{
            AvailableDesignations = new List<SelectListItem>();
            AvailableReligions = new List<SelectListItem>();
            AvailableQualifications = new List<SelectListItem>();
            AvailableCastes = new List<SelectListItem>();
            AvailableContracts = new List<SelectListItem>();
            AvailableContractTypes = new List<SelectListItem>();
            AvailableAcadmicYears = new List<SelectListItem>();
        }

        #region Properties
        public string EmpFName { get; set; }
		public string EmpMName { get; set; }
		public string EmpLName { get; set; }
		public string FatherFName { get; set; }
		public string FatherMName { get; set; }
		public string FatherLName { get; set; }
		public string MotherFName { get; set; }
		public string MotherMName { get; set; }
		public string MotherLName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public double Weight { get; set; }
		public double Height { get; set; }
		public string MarriedStatus { get; set; }
        [UIHint("Date")]
		public DateTime? JoiningDate { get; set; }
		public int DesignationId { get; set; }
        public string Designation { get; set; }
        public int QualificationId { get; set; }
		public string Sex { get; set; }
		public int ReligionId { get; set; }
		public int CasteId { get; set; }

        public int ContractTypeId { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractStartDateString { get; set; }
        public string ContractEndDateString { get; set; }
        [UIHint("DateRange")]
        public DateTime? ContractStartDate { get; set; }
        [UIHint("DateRange")]
        public DateTime? ContractEndDate { get; set; }
        public bool IsActive { get; set; }
        public string ContractType { get; set; }
        public string ContractStatus { get; set; }
        public string BGroup { get; set; }
		public string BirthMark { get; set; }
        public string BusNumber { get; set; }
        public string RouteNumber { get; set; }
        public bool? BusFacility { get; set; }
        public string AadharCardNo { get; set; }
		public string Pre_Institute_Name { get; set; }
		public string Pre_Institute_Address { get; set; }
		public bool? Bus_Facility { get; set; }
		public string E_Phisician_Name { get; set; }
		public string E_Phisician_Address { get; set; }
		public int E_Phisician_Phone { get; set; }
		public string Father_Occupation { get; set; }
		public string Email { get; set; }
		public string Father_Office_Address { get; set; }
		public string Contact1 { get; set; }
		public string Contact2 { get; set; }
		public string Occupation_Spouse { get; set; }
		public string Edu_Spouse { get; set; }
		public string Temporary_Address { get; set; }
		public string Permanent_Address { get; set; }
		public string TalukaPer { get; set; }
		public string DistrictPer { get; set; }
		public int PinPer { get; set; }
		public string TalukaTemp { get; set; }
		public string DistrictTemp { get; set; }
		public int PinTemp { get; set; }
		public string Username { get; set; }
		public string Emergency_Email { get; set; }
		public string Emergency_Contact { get; set; }
		public string DD { get; set; }
		public string MM { get; set; }
		public string YY { get; set; }
        [UIHint("Picture")]
        public int EmployeePictureId { get; set; }
        [UIHint("Picture")]
        public int FatherPictureId { get; set; }
        [UIHint("Picture")]
        public int MotherPictureId { get; set; }
		public int AcadmicYearId { get; set; }
		public string AcadmicYear { get; set; }

        #endregion

        public IList<SelectListItem> AvailableDesignations { get; set; }

        public IList<SelectListItem> AvailableReligions { get; set; }

        public IList<SelectListItem> AvailableQualifications { get; set; }

        public IList<SelectListItem> AvailableCastes { get; set; }

        public IList<SelectListItem> AvailableContracts { get; set; }

        public IList<SelectListItem> AvailableContractTypes { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }

	}
}
