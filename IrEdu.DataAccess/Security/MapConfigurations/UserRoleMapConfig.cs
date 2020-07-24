using IrEdu.Common.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IrEdu.Common.AppConstants;

namespace IrEdu.DataAccess.Security.MapConfigurations
{
	public class UserRoleMapConfig : EntityMapConfig<UserRole>
	{
		public UserRoleMapConfig()
		{
			Property(e => e.UserId);
			Property(e => e.RoleId);

			ToTable("UserRoles");
		}
	}
}
