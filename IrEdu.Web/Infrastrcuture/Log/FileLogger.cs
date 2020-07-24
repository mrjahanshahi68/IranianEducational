using IrEdu.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IrEdu.Web.Log
{
	public class FileLogger : ILogger
	{
        public static string LogPath=> Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfigurations.LogFolder);
		public void WriteLog(object contents,string filename)
        {
            File.WriteAllText(Path.Combine(LogPath,filename), contents.ToString());
        }
    }
}