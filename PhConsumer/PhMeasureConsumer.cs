using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Core.Interfaces;
using Core;
using static Core.Enums;
using System.Text.Json;
using MongoDB.Bson;
using Core.Models;
using System.Diagnostics;
using System.Threading.Channels;
using MongoDB.Driver.Core.Bindings;

namespace PhConsumer
{
    public class PhMeasureConsumer : IDisposable
    {
        private readonly IPhRepository _repo;
        private readonly IConnection _brokerConnection;
        private IModel _channel;
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        private readonly string _queueName;
        private readonly string _routingKey;

        public PhMeasureConsumer(IPhRepository repo,
                                 string exchangeName = nameof(ExchangesNames.PhExchange),
                                 string exchangeType = ExchangeType.Direct,
                                 string hostName = "localhost",
                                 string queueName = "DbConsumers",
                                 string routingKey = null!)
        {
            _repo = repo;
            _exchangeType = exchangeType;
            _exchangeName = exchangeName;
            _queueName = queueName;
            _routingKey = routingKey;
            var factory = new ConnectionFactory { HostName = hostName };
            _brokerConnection = factory.CreateConnection();
        }

        public void Dispose()
        {
            _brokerConnection.Dispose();
        }

        public void StartConsuming()
        {
            try
            {
                _channel = _brokerConnection.CreateModel();

                declareConfigExchangeAndQueue(_channel);

                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += async (sender, args) =>
                {
                    var phModel = JsonSerializer.Deserialize<PhModel>(args.Body.Span);

                    Debug.WriteLine($"Recieved a value: {phModel?.Value}, cosumer:{args.ConsumerTag}");

                    if (phModel != null)
                    {
                        await _repo.WritePh(phModel);
                    }
                    _channel.BasicAck(args.DeliveryTag, false);
                };

                _channel.BasicConsume(queue: _queueName, consumer: consumer, autoAck: false);
            }
            catch(Exception ex)
            {
                ;
            }
        }

        private void declareConfigExchangeAndQueue(IModel channel)
        {
            channel.ExchangeDeclare(_exchangeName, _exchangeType);
            channel.QueueDeclare(_queueName, exclusive: false);
            channel.QueueBind(_queueName, _exchangeName, routingKey: _routingKey == null ? string.Empty: _routingKey);
            channel.BasicQos(0, 1, false);
        }
    }
}
