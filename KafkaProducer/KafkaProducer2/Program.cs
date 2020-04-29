using Confluent.Kafka;
using LogsProcesor;
using System;
using System.Text;


namespace KafkaProducer
{

	class Program
	{
		public static void Main(string[] args)
		{
			#region log4net
			var logRepository = log4net.LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
			log4net.Config.XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config.xml"));

			Logger producerLog = new Logger().SetLogger(log4net.LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "producer"));
			#endregion

			new BasicProducerTest(producerLog);
			new BasicProducerAsyncTest(producerLog);
			new BasicProducerAsyncMultiProducerTest(producerLog);
			new BasicProducerAsyncMultiProducerTest(producerLog);
		}
	}
}

