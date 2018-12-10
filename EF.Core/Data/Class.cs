using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
    public partial class Class : BaseEntity
    {
        [NotMapped]
        public virtual ICollection<MessageGroup> _MessageGroups { get; set; }

        [NotMapped]
        public virtual ICollection<ClassDivision> _Divisions { get; set; }

        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int AcadmicYearId { get; set; }

        public int DisplayOrder { get; set; }

        #region Navigation Properties

        //public virtual ICollection<SubjectAssessment> SubjectAssessments
        //{
        //	get { return _SubjectAssessments ?? (_SubjectAssessments = new List<SubjectAssessment>()); }
        //	protected set { _SubjectAssessments = value; }
        //}

        //public virtual ICollection<Division> Divisions
        //{
        //	get { return _Divisions ?? (_Divisions = new List<Division>()); }
        //	protected set { _Divisions = value; }
        //}

        //public virtual ICollection<Teacher> Teachers
        //{
        //	get { return _Teachers ?? (_Teachers = new List<Teacher>()); }
        //	protected set { _Teachers = value; }
        //}
        //public virtual ICollection<Student> Students
        //{
        //	get { return _Students ?? (_Students = new List<Student>()); }
        //	protected set { _Students = value; }
        //}

        public virtual ICollection<ClassDivision> Divisions
        {
            get { return _Divisions ?? (_Divisions = new List<ClassDivision>()); }
            protected set { _Divisions = value; }
        }

        public virtual ICollection<MessageGroup> MessageGroups
		{
			get { return _MessageGroups ?? (_MessageGroups = new List<MessageGroup>()); }
			protected set { _MessageGroups = value; }
		}
		#endregion


	}
}
