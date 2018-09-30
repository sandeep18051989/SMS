using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(AllowanceModelValidator))]
	public partial class AllowanceModel : BaseEntityModel
	{
		public AllowanceModel()
		{
			Designation = new DesignationModel();
		}
		public int DesignationId { get; set; }
		public string DesignationName { get; set; }
		public string EType { get; set; }
		public double DA { get; set; }
		public double TA { get; set; }
		public double HRA { get; set; }
		public double PF { get; set; }
		public string DA_Status { get; set; }
		public string TA_Status { get; set; }
		public string HRA_Status { get; set; }
		public string PF_Status { get; set; }
		public double TDS { get; set; }
		public string TDS_Status { get; set; }
		public DesignationModel Designation { get; set; }
	}
}
