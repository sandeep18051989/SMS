using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EF.Core.Data;
using FluentValidation.Attributes;
using SMS.Validations;
using EF.Services;

namespace SMS.Models
{
	[Validator(typeof(InstallDatabaseValidation))]
	public partial class InstallDatabaseModel : BaseEntityModel
	{
		public InstallDatabaseModel()
		{
			School = new SchoolModel();
		}
		public string Datasource { get; set; }
		public string Database { get; set; }
		public string Username { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

		public string AdminUsername { get; set; }

		[DataType(DataType.Password)]
		public string AdminPassword { get; set; }

		public SchoolModel School { get; set; }
	}
}