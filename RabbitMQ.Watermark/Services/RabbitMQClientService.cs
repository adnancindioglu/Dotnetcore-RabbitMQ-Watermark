using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Watermark.Services
{
    public class RabbitMQClientService:IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;

        private IConnection _connection;

        private IModel _channal;

        public static string ExcahngeName = "ImageDirectExchange";

        public static string RoutingWatermark = "watermark-route-image";

        public static string QueueName = "watermark-queue-image";

        private readonly ILogger<RabbitMQClientService> _logger;

        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channal is { IsOpen:true })
            {
                return _channal;
            }

            _channal = _connection.CreateModel();

            _channal.ExchangeDeclare(ExcahngeName, ExchangeType.Direct, true, false);

            _channal.QueueDeclare(QueueName, true, false, false, null);

            _channal.QueueBind(QueueName, ExcahngeName, RoutingWatermark);

            _logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");

            return _channal;
        }

        public void Dispose()
        {
            _channal?.Close();
            _channal?.Dispose();
            _channal = default;

            _connection?.Close();
            _connection?.Dispose();
            _connection = default;

            _logger.LogInformation("RabbitMQ ile bağlantı koptu...");
        }
    }
}
