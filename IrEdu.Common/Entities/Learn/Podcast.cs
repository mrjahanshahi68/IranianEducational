using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IrEdu.Common.AppEnums;

namespace IrEdu.Common.Entities.Learn
{
	public class Podcast : LoggableEntity, ILogicalDeletable
	{
		public string HashKey { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string FileName { get; set; }
		public bool IsDeleted { get; set; }
		public EducationalLevel? EducationalLevel { get; set; }
	}
}
