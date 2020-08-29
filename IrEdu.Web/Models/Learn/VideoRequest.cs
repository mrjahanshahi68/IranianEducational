using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static IrEdu.Common.AppEnums;

namespace IrEdu.Web.Models.Learn
{
	public class VideoRequest
	{
		public int ID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public EducationalLevel? EducationalLevel { get; set; }
		public decimal? Price { get; set; }
		public bool IsDeleted { get; set; }
	}
}