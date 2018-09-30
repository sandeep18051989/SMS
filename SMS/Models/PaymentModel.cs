using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(PaymentModelValidator))]
	public partial class PaymentModel : BaseEntityModel
	{
		public PaymentModel()
		{
			Employee = new EmployeeModel();
			Allowance = new AllowanceModel();
		}
		public int EmployeeId { get; set; }
		public int AllowanceId { get; set; }
		public string PDate { get; set; }
		public double BasicPay { get; set; }
		public double DA { get; set; }
		public double TA { get; set; }
		public double HRA { get; set; }
		public double PF { get; set; }
		public double Gross_Pay { get; set; }
		public double Net_Pay { get; set; }
		public double TDS { get; set; }
		public double TotalPayment { get; set; }
		public EmployeeModel Employee { get; set; }
		public AllowanceModel Allowance { get; set; }
	}
}
