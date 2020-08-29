using IrEdu.Common.Entities.Learn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.DataAccess.Learn.MapConfigurations
{
	public class PodcastMapConfig : LoggableEntityMapConfig<Podcast>
	{
		public PodcastMapConfig()
		{
			Property(e => e.HashKey);
			Property(e => e.Title);
			Property(e => e.Description);
			Property(e => e.FileName);
			Property(e => e.EducationalLevel);
			Property(e => e.IsDeleted);

			ToTable("Podcasts");
		}
	}
}
