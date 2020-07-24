using IrEdu.Domain;
using IrEdu.Domain.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IrEdu.Web.Log
{
    public class DatabaseLogger : ILogger
    {
        public BaseBusinessRule<Common.Entities.Log.Exception> rule;
        public DatabaseLogger()
        {
            rule = new ExceptionBusinessRule();
        }
        public void WriteLog(object contents,string connectionstring="")
        {
            var excption = contents as Common.Entities.Log.Exception;
            if (excption != null)
            {
                rule.InsertEntity(excption);
            }
        }
    }
}