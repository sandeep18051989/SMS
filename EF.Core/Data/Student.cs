using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class Student : BaseEntity, ISlugSupported
	{
		public Guid StudentUniqueId { get; set; }
		public string IdentityNumber { get; set; }
		public string RollNumber { get; set; }
		public string FName { get; set; }
		public string MName { get; set; }
		public string LName { get; set; }
		public string FatherFName { get; set; }
		public string FatherMName { get; set; }
		public string FatherLName { get; set; }
		public string MotherFName { get; set; }
		public string MotherMName { get; set; }
		public string MotherLName { get; set; }
		public string BGroup { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public double? Weight { get; set; }
		public double? Height { get; set; }
		public int? ReligionId { get; set; }
		public int? CasteId { get; set; }
		public bool BusFacility { get; set; }
		public string MotherTounge { get; set; }
		public string BirthMark { get; set; }
		public string Disease { get; set; }
		public string BusNumber { get; set; }
        public string RouteNumber { get; set; }
        public string AadharCardNo { get; set; }
		public string Pre_Institute_Name { get; set; }
		public string Pre_Institute_Address { get; set; }
		public string E_Phisician_Name { get; set; }
		public string E_Phisician_Address { get; set; }
		public string E_Phisician_Phone { get; set; }
		public int ClassId { get; set; }
		public int DivisionId { get; set; }
		public string Father_Occupation { get; set; }
		public string Father_Education { get; set; }
		public string Father_BGroup { get; set; }
		public string EmailAddress { get; set; }
		public string Father_Office_Address { get; set; }
		public string Father_Contact { get; set; }
		public string Mother_Occupation { get; set; }
		public string Mother_Office_Address { get; set; }
		public string Mother_Contact { get; set; }
		public string Temporary_Address { get; set; }
		public string Permanent_Address { get; set; }
		public string TalukaPer { get; set; }
		public string DistrictPer { get; set; }
		public int PinPer { get; set; }
		public string TalukaTemp { get; set; }
		public string DistrictTemp { get; set; }
		public int PinTemp { get; set; }
		public string Sex { get; set; }
		public string SeoUrl { get; set; }
		public DateTime? AdmissionDate { get; set; }
		public int StudentPictureId { get; set; }
		public int FatherPictureId { get; set; }
		public int MotherPictureId { get; set; }
		public string Nationality { get; set; }
		public string FatherNationality { get; set; }
		public string MotherNationality { get; set; }
		public int AdmissionStatusId { get; set; }
		public int PersonalityStatusId { get; set; }
		public string ReferredBy { get; set; }
		public string Remarks { get; set; }
		public string UserName { get; set; }
		public int CoverPictureId { get; set; }
		public string FacebookLink { get; set; }
		public string TweeterLink { get; set; }
		public string InstagramLink { get; set; }
		public string GooglePlusLink { get; set; }
		public string PInterestLink { get; set; }
		public string LinkedInLink { get; set; }
		public string Hi5Link { get; set; }
		public bool IsPhoneVerified { get; set; }
		public bool IsEmailVerified { get; set; }
		public int HouseId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

		[NotMapped]
		public AdmissionStatus AdmissionStatus
		{
			get
			{
				return (AdmissionStatus)this.AdmissionStatusId;
			}
			set
			{
				this.AdmissionStatusId = (int)value;
			}
		}
		[NotMapped]
		public PersonalityStatus PersonalityStatus
		{
			get
			{
				return (PersonalityStatus)this.PersonalityStatusId;
			}
			set
			{
				this.PersonalityStatusId = (int)value;
			}
		}
		[NotMapped]
		public StudentHouse StudentHouse
		{
			get
			{
				return (StudentHouse)this.HouseId;
			}
			set
			{
				this.HouseId = (int)value;
			}
		}

		[NotMapped]
		public virtual ICollection<Assessment> _Assessments { get; set; }
		[NotMapped]
		public virtual ICollection<File> _Files { get; set; }

		[NotMapped]
		public virtual ICollection<MessageGroup> _MessageGroups { get; set; }
		public virtual Class Class { get; set; }
		public virtual House House { get; set; }

		#region Navigation Properties

		public virtual ICollection<File> Files
		{
			get { return _Files ?? (_Files = new List<File>()); }
			protected set { _Files = value; }
		}

		public virtual ICollection<Assessment> Assessments
		{
			get { return _Assessments ?? (_Assessments = new List<Assessment>()); }
			protected set { _Assessments = value; }
		}

		public virtual ICollection<MessageGroup> MessageGroups
		{
			get { return _MessageGroups ?? (_MessageGroups = new List<MessageGroup>()); }
			protected set { _MessageGroups = value; }
		}

		#endregion
	}
}
