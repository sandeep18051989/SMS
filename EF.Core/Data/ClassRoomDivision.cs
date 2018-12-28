using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;
using System.Collections.Generic;

namespace EF.Core.Data
{
	public partial class ClassRoomDivision : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<DivisionSubject> _Subjects { get; set; }

        [NotMapped]
        public virtual ICollection<DivisionHomework> _DivisionHomeworks { get; set; }

        [NotMapped]
        public virtual ICollection<DivisionExam> _DivisionExams { get; set; }

        [NotMapped]
        public virtual ICollection<Teacher> _Teachers { get; set; }

        [NotMapped]
        public virtual ICollection<MessageGroup> _MessageGroups { get; set; }


        public int? ClassRoomId { get; set; }
		public virtual ClassRoom ClassRoom { get; set; }
        public int? ClassId { get; set; }
        public virtual Class Class { get; set; }
        public int? DivisionId { get; set; }
        public virtual Division Division { get; set; }

        #region Navigation Properties

        public virtual ICollection<Teacher> Teachers
        {
            get { return _Teachers ?? (_Teachers = new List<Teacher>()); }
            protected set { _Teachers = value; }
        }

        public virtual ICollection<DivisionSubject> Subjects
        {
            get { return _Subjects ?? (_Subjects = new List<DivisionSubject>()); }
            protected set { _Subjects = value; }
        }

        public virtual ICollection<DivisionHomework> DivisionHomeworks
        {
            get { return _DivisionHomeworks ?? (_DivisionHomeworks = new List<DivisionHomework>()); }
            protected set { _DivisionHomeworks = value; }
        }

        public virtual ICollection<DivisionExam> DivisionExams
        {
            get { return _DivisionExams ?? (_DivisionExams = new List<DivisionExam>()); }
            protected set { _DivisionExams = value; }
        }

        public virtual ICollection<MessageGroup> MessageGroups
        {
            get { return _MessageGroups ?? (_MessageGroups = new List<MessageGroup>()); }
            protected set { _MessageGroups = value; }
        }

        #endregion
    }
}
