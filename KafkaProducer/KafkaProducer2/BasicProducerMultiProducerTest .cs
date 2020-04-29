using Confluent.Kafka;
using LogsProcesor;
using System;
using System.Diagnostics;

namespace KafkaProducer
{

	internal class BasicProducerMultiProducerTest
	{
		internal BasicProducerMultiProducerTest(Logger producerLog)
		{
			string server = "localhost:9092";
			string topic1 = "test3";
			string topic2 = "test4";
			string message = "test message Sync";

			IProducer<Null, string> p1  = null;
			IProducer<Null, string> p2 = null;

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


			p1 = new ProducerBuilder<Null, string>(config).Build();
			p2 = new ProducerBuilder<Null, string>(config).Build();

			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();

			for (int i =0; i <10000; i++)
			{
				p1.Produce(topic1, new Message<Null, string> { Value = $"{message} - {i}" }, handler);
			}
			for (int i = 0; i < 10000; i++)
			{
				p2.Produce(topic2, new Message<Null, string> { Value = $"{message} - {i}" }, handler);
			}

			stopWatch.Stop();
			TimeSpan ts = stopWatch.Elapsed;
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
			Console.WriteLine($"RunTime for {this.GetType().Name}: " + elapsedTime);

			p1.Flush(TimeSpan.FromSeconds(10));
			p1.Dispose();
			p2.Dispose();
		}
	}
}

