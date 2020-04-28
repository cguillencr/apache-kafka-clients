# apache-kafka-clients


This project presents a basic kafka consumer and producer created in C#. A server must be created in order to send and consumer messages. The following line will create a kafka server using a dokcer container.

docker run -d --name kafka -p 2181:2181 -p 9092:9092 --env ADVERTISED_HOST=127.0.0.1 --env ADVERTISED_PORT=9092 spotify/kafka

## Consumer

Setup the variables: topic, groupIp and server before run the consumer main.

## Producer
Setup the variables: topic, message and server before run the producer main.