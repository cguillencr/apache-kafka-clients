using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KafkaProducer
{

	class Program
	{
		public static void Main(string[] args)
		{
			string server = "";
			string topic = "";
			string message = "";

			var config = new Dictionary<string, object> { { "bootstrap.servers", server } };
			var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));

			var commit = producer.ProduceAsync(topic, null, message);

			Console.WriteLine($"offset {commit.Result.Offset}");
		}
	}
}

