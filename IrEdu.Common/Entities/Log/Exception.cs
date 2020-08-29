using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.Common.Entities.Log
{
	public class Exception : BaseEntity
	{
		public string ActionName { get; set; }
		public string ExceptionMsg { get; set; }
		public string ExceptionType { get; set; }
		public string ExceptionSource { get; set; }
		public string ExceptionURL { get; set; }
		public string IPAddress { get; set; }
		public DateTime? Logdate { get; set; }

	}
}
