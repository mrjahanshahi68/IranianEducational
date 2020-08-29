using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static IrEdu.Common.AppEnums;

namespace IrEdu.Web.Models.Learn
{
	public class PodCastResponse
	{
		public int ID { get; set; }
		public string HashKey { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public EducationalLevel? EducationalLevel { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public bool IsDeleted { get; set; }
	}
}