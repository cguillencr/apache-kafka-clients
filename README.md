# apache-kafka-clients


This project presents a basic kafka consumer and producer created in C#, that handles 10k string through the server to measure the statistics with different configurations that will be detailed below. 

A server must be created in order to send and consumer messages. The following line will create a kafka server using a dokcer container.
```
docker run -d --name kafka -p 2181:2181 -p 9092:9092 --env ADVERTISED_HOST=127.0.0.1 --env ADVERTISED_PORT=9092 spotify/kafka
```

## Consumer

Setup the variables: topic, groupIp and server before run the consumer main. Consumed messages will be stored through Log4net in /tmp/consumer/consumer*.log file

These are the stats for each configuration.
```
Reached end of topic test1 for BasicConsumerNoAutoCommit: 00:00:13.96
Reached end of topic test1 for BasicConsumerAutoCommit: 00:00:03.17
Reached end of topic test1 for BasicConsumerNoAutoCommitOnDifferentThread: 00:00:13.37
Reached end of topic test1 for BasicConsumerAutoCommitOnDifferentThread: 00:00:04.36
```

## Producer
Setup the variables: topic, message and server before run the producer in each test class. The DeliveryReports will send data to /tmp/producer/producer.log file

These are the stats for each configuration.
```
RunTime for BasicProducerTest: 00:00:00.06
RunTime for BasicProducerAsyncTest: 00:00:01.45
RunTime for BasicProducerAsyncMultiProducerTest: 00:00:02.31
RunTime for BasicProducerAsyncMultiProducerTest: 00:00:02.52
```

The first configuration has the best performance sending 20k messages to kafka server.


## Results
As the stats shows above the best performance it's  achieved by using BasicConsumerAutoCommit to get the message from the server and BasicProducerTest to generate the message.