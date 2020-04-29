using Confluent.Kafka;
using LogsProcesor;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaConsumer
{
	public class BasicConsumerNoAutoCommit
	{
		public BasicConsumerNoAutoCommit(Logger consumerLog)
		{
			string server = "localhost:9092";
			string topic = "test1";
			string goupIp = "group1";

			var conf = new ConsumerConfig
			{
				GroupId = goupIp,
				BootstrapServers = server,
				// Note: The AutoOffsetReset property determines the start offset in the event
				// there are not yet any committed offsets for the consumer group for the
				// topic/partitions of interest. By default, offsets are committed
				// automatically, so in this example, consumption will only start from the
				// earliest message in the topic 'my-topic' the first time you run the program.
				EnableAutoCommit = false,
				AutoOffsetReset = AutoOffsetReset.Earliest
			};

			var t = Task.Run(()=> {
				using (var consumer = new ConsumerBuilder<Ignore, string>(conf)
				// Note: All handlers are called on the main .Consume thread.
				.SetErrorHandler((_, e) => consumerLog.Debug($"Error: {e.Reason}"))
				.SetStatisticsHandler((_, json) => consumerLog.Debug($"Statistics: {json}"))
				.SetPartitionsAssignedHandler((c, partitions) =>
				{
					consumerLog.Debug($"Assigned partitions: [{string.Join(", ", partitions)}]");
					// possibly manually specify start offsets or override the partition assignment provided by
					// the consumer group by returning a list of topic/partition/offsets to assign to, e.g.:
					// 
					// return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
				})
				.SetPartitionsRevokedHandler((c, partitions) =>
				{
					consumerLog.Debug($"Revoking assignment: [{string.Join(", ", partitions)}]");
				})
				.Build())
				{
					consumer.Subscribe(topic);

					CancellationTokenSource cts = new CancellationTokenSource();
					Console.CancelKeyPress += (_, e) => {
						e.Cancel = true; // prevent the process from terminating.
						cts.Cancel();
					};

					try
					{
						Stopwatch stopWatch = new Stopwatch();
						stopWatch.Start();

						bool listening = true;
						while (listening)
						{
							try
							{
								var consumeResult = consumer.Consume(cts.Token);

								consumerLog.Debug($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

								consumer.Commit(consumeResult);

								if (consumeResult.TopicPartitionOffset.Offset.Value == 9999)
								{
									stopWatch.Stop();
									TimeSpan ts = stopWatch.Elapsed;
									string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
									Console.WriteLine($"Reached end of topic {consumeResult.Topic} for {this.GetType().Name}: " + elapsedTime);
									listening = false;
								}
							}
							catch (ConsumeException e)
							{
								consumerLog.Debug($"Error occured: {e.Error.Reason}");
							}
						}
					}
					catch (OperationCanceledException)
					{
						// Ensure the consumer leaves the group cleanly and final offsets are committed.
						consumer.Close();
					}
				}

			});
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
			t.Wait(); //TODO this is a bad practice in a sync code. But I used just to see the stats
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
		}
	}
}
