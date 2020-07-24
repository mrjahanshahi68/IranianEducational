using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IrEdu.Web.Log
{
    public class LogManager
    {
        private static Dictionary<Type,ILogger>  _logger=new Dictionary<Type, ILogger>();
        public static void SetProvider(ILogger loggerProvider)
        {
            if (!_logger.ContainsKey(loggerProvider.GetType()))
                _logger.Add(loggerProvider.GetType(), loggerProvider);
        }
        public static void SetProvider(List<ILogger> loggerProviders)
        {
            foreach (var provider in loggerProviders)
            {
                if (!_logger.ContainsKey(provider.GetType()))
                    _logger.Add(provider.GetType(), provider);
            }
        }
        public static void WriteLog<TLogger>(object contents, string filename=null) where TLogger: ILogger
        {
            if (_logger == null) throw new NullReferenceException("logger provider must be set");
            if (!_logger.ContainsKey(typeof(TLogger))) throw new NullReferenceException("logger provider must be set");

            var currentLogger = _logger[typeof(TLogger)];
            currentLogger.WriteLog(contents, filename);
        }
    }
}