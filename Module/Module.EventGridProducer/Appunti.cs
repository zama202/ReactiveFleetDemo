using System;
using System.Collections.Generic;
using System.Text;

namespace Module.EventGridProducer
{
    class Appunti
    {
        //public static void Main(string[] args)
        //{
        //    string brokerList = "gab2019vrehn001.servicebus.windows.net:9093";
        //    string connStr = "Endpoint=sb://gab2019vrehn001.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=7s7LYUr0kGRflc0xQi74c0XgT4lMV9D82OYLpeEvKco=";
        //    string topic = "gab2019vreht001";
        //    string consumergroup = "kafka002";
        //    string cacertlocation = ".\\cacert.pem";

        //    //Consumer<long, string> consumer = KafkaProvider.GetConsumer(brokerList, connStr, consumergroup, topic, cacertlocation);

        //    var config = new Dictionary<string, object> {
        //        { "bootstrap.servers", brokerList },
        //        { "security.protocol","SASL_SSL" },
        //        { "sasl.mechanism","PLAIN" },
        //        { "sasl.username", "$ConnectionString" },
        //        { "sasl.password", connStr },
        //        { "ssl.ca.location", cacertlocation },
        //        { "group.id", consumergroup },
        //        { "request.timeout.ms", 60000 },
        //        { "broker.version.fallback", "1.0.0" },         //Event Hubs for Kafka Ecosystems supports Kafka v1.0+, 
        //        //a fallback to an older API will fail
        //        //{ "debug", "security,broker,protocol" }    ,   //Uncomment for librdkafka debugging information
        //    };





        //    using (var consumer = new Consumer<long, string>(config, new LongDeserializer(), new StringDeserializer(Encoding.UTF8)))
        //    {
        //        consumer.OnMessage += (_, msg) =>
        //        {

        //            //Console.WriteLine(msg.Value+"!!");

        //            //MessageEvent message = JsonConvert.DeserializeObject<MessageEvent>(msg.Value);
        //            //if (message.PayloadFamily == "p")
        //            //    sendEventGridMessageWithEventGridClientAsync(topicHostName, "position", msg.Value).GetAwaiter().GetResult();
        //            //if (message.PayloadFamily == "av")
        //            //    sendEventGridMessageWithEventGridClientAsync(topicHostName, "activation", msg.Value).GetAwaiter().GetResult();
        //            Console.WriteLine($"Received: '{msg}'");
        //        };

        //        consumer.OnError += (_, error)
        //            => Console.WriteLine($"Error: {error}");//Console.WriteLine("");

        //        consumer.OnConsumeError += (_, msg)
        //            => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}"); //Console.WriteLine(""); 

        //        Console.WriteLine("Consuming messages from topic: " + topic + ", broker(s): " + brokerList);
        //        consumer.Subscribe(topic);

        //        while (true)
        //        {
        //            Console.Write(".");
        //            consumer.Poll(TimeSpan.FromMilliseconds(1000));
        //        }
        //    }



        //}
    }
}
