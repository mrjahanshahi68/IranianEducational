using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IrEdu.Web.Log
{
    public interface ILogger
    {
        void WriteLog(object contents,string filename="");
    }
}