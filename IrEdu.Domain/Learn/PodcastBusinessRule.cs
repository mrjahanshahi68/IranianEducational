using IrEdu.Common.Entities.Learn;
using IrEdu.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.Domain.Learn
{
	public class PodcastBusinessRule : BaseBusinessRule<Podcast>
	{
		public PodcastBusinessRule() : base()
        {
			UnitOfWork = new AppUnitOfWork();
		}
		public PodcastBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
	}
}
