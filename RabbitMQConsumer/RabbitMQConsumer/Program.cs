// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
Thread.Sleep(30000);




var factory = new ConnectionFactory();
factory.UserName = "guest";
factory.Password = "guest";
factory.HostName = "rabbitmq";
factory.Port = 5672;
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "DefaultQueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);


Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message);

};
channel.BasicConsume(queue: "DefaultQueue",
                     autoAck: true,
                     consumer: consumer);

Console.ReadLine();
