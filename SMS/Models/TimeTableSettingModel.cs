
using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(TimeTableSettingModelValidator))]
	public partial class TimeTableSettingModel : BaseEntityModel
	{
		public DateTime? SchoolStartTime { get; set; }
		public DateTime? SchoolEndTime { get; set; }
		public double LectureTime { get; set; }
		public bool NoBreak { get; set; }
		public double RecessTimeMin { get; set; }
	}
}
