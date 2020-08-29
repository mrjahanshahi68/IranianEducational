using IrEdu.DataAccess.Common.MapConfigurations;
using IrEdu.DataAccess.Learn.MapConfigurations;
using IrEdu.DataAccess.Security.MapConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.DataAccess
{
	public class AppDataContext : DataContext
	{
		public AppDataContext() : base("IrEduConnectionString")
		{

		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			#region Security
			modelBuilder.Configurations.Add(new UserMapConfig());
			modelBuilder.Configurations.Add(new RoleMapConfig());
			modelBuilder.Configurations.Add(new UserRoleMapConfig());
			#endregion

			#region Common
			modelBuilder.Configurations.Add(new AttachmentMapConfig());
			#endregion

			#region Learn
			modelBuilder.Configurations.Add(new PodcastMapConfig());
			modelBuilder.Configurations.Add(new VideoMapConfig());
			#endregion





			base.OnModelCreating(modelBuilder);
		}
	}
}
