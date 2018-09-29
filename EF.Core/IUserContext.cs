using EF.Core.Data;

namespace EF.Core
{
	/// <summary>
	/// UserContext
	/// </summary>
	public partial interface IUserContext
	{
		User CurrentUser { get; set; }
		AcadmicYear CurrentAcadmicYear { get; set; }
		School CurrentSchool { get; set; }

		bool IsAdmin { get; set; }
	}
}
