using IrEdu.Common.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IrEdu.Common.AppConstants;

namespace IrEdu.DataAccess.Security.MapConfigurations
{
	public class RoleMapConfig : EntityMapConfig<Role>
	{
		public RoleMapConfig()
		{
			Property(e => e.Name);
			Property(e => e.IsDeleted);

			ToTable("Roles");
		}
	}
}
