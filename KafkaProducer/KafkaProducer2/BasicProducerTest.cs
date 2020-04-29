using Confluent.Kafka;
using LogsProcesor;
using System;
using System.Diagnostics;

namespace KafkaProducer
{

	internal class BasicProducerTest
	{
		internal BasicProducerTest(Logger producerLog)
		{
			string server = "localhost:9092";
			string topic1 = "test1";
			string topic2 = "test2";
			string message = "test message Sync";

			IProducer<Null, string> p  = null;

			var config = new ProducerConfig { BootstrapServers = server };
			Action<DeliveryReport<Null, string>> handler = r =>
			{
				if (r.Error.IsError)
				{
					producerLog.Debug($"Delivery Error: {r.Error.Reason}"); 
				}
				else
				{
					producerLog.Debug($"Delivered message to {r.TopicPartitionOffset}");
				}
				
			};


			p = new ProducerBuilder<Null, string>(config).Build();

			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();

			for (int i =0; i <10000; i++)
			{
				p.Produce(topic1, new Message<Null, string> { Value = $"{message} - {i}" }, handler);
			}
			for (int i = 0; i < 10000; i++)
			{
				p.Produce(topic2, new Message<Null, string> { Value = $"{message} - {i}" }, handler);
			}

			stopWatch.Stop();
			TimeSpan ts = stopWatch.Elapsed;
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
			Console.WriteLine($"RunTime for {this.GetType().Name}: " + elapsedTime);

			p.Flush(TimeSpan.FromSeconds(10));
			p.Dispose();

		}
	}
}

