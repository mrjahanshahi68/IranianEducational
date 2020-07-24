using IrEdu.Common.Entities.Security;
using IrEdu.DataAccess;
using IrEdu.DataAccess.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.Domain.Security
{
	public class UserBusinessRule : BaseBusinessRule<User>
	{
		public UserBusinessRule() : base()
        {
            UnitOfWork = new AppUnitOfWork();
        }
		public UserBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
		public User FindUserByUserName(string userName)
		{
			return Queryable().Where(e => e.UserName == userName).SingleOrDefault();
		}
		 
	}
}
