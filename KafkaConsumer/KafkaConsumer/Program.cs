using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Threading;
namespace KafkaConsumer
{
	class Program
	{
		static void Main(string[] args)
		{
			string topic = "";
			string goupIp = "";
			string server = "";
			var conf = new Dictionary<string, object>
			{
				{ "group.id", goupIp },
				{ "bootstrap.servers", server},
				{ "enable.auto.commit", "false"}, // Commit message with method Commit(consumer, data);
				{ "auto.offset.reset", "earliest"} //From la begining.
			};

			CreateConsumer(1, topic, conf);

		}
		private static void CreateConsumer(int id, string topic, Dictionary<string, object> conf)
		{
			using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
			{
				 consumer.OnMessage += (_, data) =>
				{
					Console.WriteLine($"Offset { data.Offset} | {data.Timestamp.UtcDateTime}");
					try
					{
						// Inspect message here

						Commit(consumer, data);
					}
					catch (Exception e)
					{
						Console.WriteLine($"Enviar error {e} offset:{data.TopicPartitionOffset}");
					}
				};

				consumer.OnError += (_, error)
				=>
				{
					Console.WriteLine($" Kafka consumer.OnError Consumer ID: {id} Error: {error}");
				};

				consumer.OnOffsetsCommitted += (_, commit) =>
				{
					Console.WriteLine($"[{string.Join(", ", commit.Offsets)}]");

					if (commit.Error.HasError)
					{
						Console.WriteLine($" Kafka commit.Error Consumer ID: {id} Failed to commit offsets: {commit.Error}");
					}
					else
					{
						Console.WriteLine($"Successfully committed offsets: [{string.Join(", ", commit.Offsets)}]");
					}
				};

				consumer.OnPartitionsAssigned += (_, partitions) =>
				{
					Console.WriteLine($"Consumer ID: {id} Assigned partitions: [{string.Join(", ", partitions)}], member id: {consumer.MemberId}");
					consumer.Assign(partitions);
				};

				consumer.OnPartitionsRevoked += (_, partitions) =>
				{
					Console.WriteLine($"Consumer ID: {id} Revoked partitions: [{string.Join(", ", partitions)}]");
					consumer.Unassign();
				};

				consumer.OnStatistics += (_, json)
					=> Console.WriteLine($"Consumer ID: {id} Statistics: {json}");

				consumer.Subscribe(topic);

				while (true)
				{
					consumer.Poll(TimeSpan.FromMilliseconds(100));
				}
			}
		}

		private static void Commit(Consumer<Null, string> consumer, Message<Null, string> data)
		{
			_ = Task.Run(() => {
				consumer.CommitAsync(data);
			});
		}
	}
}
