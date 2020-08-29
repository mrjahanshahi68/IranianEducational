using IrEdu.Common.Entities.Learn;
using IrEdu.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.Domain.Learn
{
	public class VideoBusinessRule : BaseBusinessRule<Video>
	{
		public VideoBusinessRule() : base()
        {
			UnitOfWork = new AppUnitOfWork();
		}
		public VideoBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
	}
}
