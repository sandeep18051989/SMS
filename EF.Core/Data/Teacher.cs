using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class Teacher : BaseEntity, ISlugSupported
	{

        [NotMapped]
        public virtual ICollection<TeacherExam> _TeacherExams { get; set; }

        public int EmployeeId { get; set; }
		public string Name { get; set; }
		public int QualificationId { get; set; }
		public string Username { get; set; }
		public int ProfilePictureId { get; set; }
		public int CoverPictureId { get; set; }
		public string FacebookLink { get; set; }
		public string TweeterLink { get; set; }
		public string InstagramLink { get; set; }
		public string GooglePlusLink { get; set; }
		public string PInterestLink { get; set; }
		public string LinkedInLink { get; set; }
		public string Hi5Link { get; set; }
		public bool? IsPhoneVerified { get; set; }
		public bool? IsEmailVerified { get; set; }
		public string Description { get; set; }
		public int? PersonalityStatusId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual Qualification Qualification { get; set; }

		[NotMapped]
		public virtual ICollection<Subject> _Subjects { get; set; }
		[NotMapped]
		public virtual ICollection<ClassRoomDivision> _ClassRoomDivisions { get; set; }

		[NotMapped]
		public virtual ICollection<MessageGroup> _MessageGroups { get; set; }

		[NotMapped]
		public virtual ICollection<File> _Files { get; set; }
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

		#region navigation Properties

		public virtual ICollection<Subject> Subjects
		{
			get { return _Subjects ?? (_Subjects = new List<Subject>()); }
			protected set { _Subjects = value; }
		}

		public virtual ICollection<ClassRoomDivision> ClassRoomDivisions
		{
			get { return _ClassRoomDivisions ?? (_ClassRoomDivisions = new List<ClassRoomDivision>()); }
			protected set { _ClassRoomDivisions = value; }
		}

		public virtual ICollection<MessageGroup> MessageGroups
		{
			get { return _MessageGroups ?? (_MessageGroups = new List<MessageGroup>()); }
			protected set { _MessageGroups = value; }
		}

		public virtual ICollection<File> Files
		{
			get { return _Files ?? (_Files = new List<File>()); }
			protected set { _Files = value; }
		}

        public virtual ICollection<TeacherExam> TeacherExams
        {
            get { return _TeacherExams ?? (_TeacherExams = new List<TeacherExam>()); }
            protected set { _TeacherExams = value; }
        }

        #endregion

    }
}
