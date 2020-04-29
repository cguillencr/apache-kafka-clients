using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogsProcesor
{
	public sealed class Logger
	{
		private ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		internal Logger()
		{
		}

		public void Error(string message, Exception e)
		{
			logger.Error(message, e);
		}

		public void Debug(string message)
		{
			logger.Debug(message);
		}

		public void Debug(int message)
		{
			logger.Debug(message);
		}


		public Logger SetLogger(ILog log)
		{
			this.logger = log;
			return this;
		}
	}
}
