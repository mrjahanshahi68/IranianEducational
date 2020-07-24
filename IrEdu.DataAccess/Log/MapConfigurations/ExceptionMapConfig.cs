using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.DataAccess.Log.MapConfigurations
{
    public class ExceptionMapConfig : EntityMapConfig<Common.Entities.Log.Exception>
    {
        public ExceptionMapConfig()
        {
            Property(e => e.ActionName);
            Property(e => e.IPAddress);
            Property(e => e.ExceptionMsg);
            Property(e => e.ExceptionType);
            Property(e => e.ExceptionSource);
            Property(e => e.ExceptionURL);
            Property(e => e.Logdate);

            ToTable("Exceptions");
        }
    }
}
