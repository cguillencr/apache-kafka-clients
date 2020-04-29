using Confluent.Kafka;
using LogsProcesor;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaConsumer
{
	class Program
	{
		static void Main(string[] args)
		{
			#region log4net
			var logRepository = log4net.LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
			log4net.Config.XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config.xml"));

			Logger consumerLog1 = new Logger().SetLogger(log4net.LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "consumer1"));
			Logger consumerLog2 = new Logger().SetLogger(log4net.LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "consumer2"));
			Logger consumerLog3 = new Logger().SetLogger(log4net.LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "consumer3"));
			Logger consumerLog4 = new Logger().SetLogger(log4net.LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "consumer4"));
			#endregion

			new BasicConsumerNoAutoCommit(consumerLog1);
			new BasicConsumerAutoCommit(consumerLog2);
			new BasicConsumerNoAutoCommitOnDifferentThread(consumerLog3);
			new BasicConsumerAutoCommitOnDifferentThread(consumerLog4);

		}
	}
}
