using Microsoft.AspNetCore.Mvc;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQProducer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMQController : ControllerBase
    {


        private readonly ILogger<RabbitMQController> _logger;

        public RabbitMQController(ILogger<RabbitMQController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostMessageToQueue")]
        public void PostMsg(String msg)
        {
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

            var body = Encoding.UTF8.GetBytes(msg);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "DefaultQueue",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {msg}");
        }
    }
}