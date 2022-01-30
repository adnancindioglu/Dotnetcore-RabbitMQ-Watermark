using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQ.Watermark.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
        {
            var channel = _rabbitMQClientService.Connect();

            string bodyString = JsonSerializer.Serialize(productImageCreatedEvent);

            byte[] bodyByte = Encoding.UTF8.GetBytes(bodyString);

            //mesajı fiziksel olarak kaydetmek için
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange:RabbitMQClientService.ExcahngeName, routingKey:RabbitMQClientService.RoutingWatermark, basicProperties: properties, body: bodyByte);
        }
    }
}
