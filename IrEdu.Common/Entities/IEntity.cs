using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IrEdu.Common.AppEnums;

namespace IrEdu.Common.Entities
{
	public interface IEntity
	{
		int ID { get; set; }
		ObjectState ObjectState { get; set; }
	}
}
