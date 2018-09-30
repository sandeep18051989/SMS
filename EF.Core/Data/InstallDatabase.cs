using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Data
{
	public partial class InstallDatabase : BaseEntity
	{
		public string Datasource { get; set; }
		public string Database { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
