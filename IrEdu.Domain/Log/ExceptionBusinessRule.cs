using IrEdu.DataAccess;
using IrEdu.DataAccess.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.Domain.Log
{
    public class ExceptionBusinessRule:BaseBusinessRule<IrEdu.Common.Entities.Log.Exception>
    {
        public ExceptionBusinessRule() : base()
        {
            UnitOfWork = new LogUnitOfWork();
        }
        public ExceptionBusinessRule(IUnitOfWork unitOfWork):base(unitOfWork)
        {

        }
    }
}
