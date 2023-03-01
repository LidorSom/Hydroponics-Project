using RabbitMQ.Client;
using RabbitMQ;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace PhReaderService.Services
{
    public class PhMeasureProducer
    { // publishing top topic exchnage, with routing key: <HydroSystemId>.<SensorType>
        private const string HOST_NAME = "localhost";
        private readonly string _exchangeName;
        private readonly PublicationAddress _publicationAdd;
        private int _ID = 0;

        public PhMeasureProducer(string exchangeName, string exchangeType, string routingKey)
        {
            _exchangeName = exchangeName;
            _publicationAdd = new PublicationAddress(exchangeType: exchangeType,
                                        exchangeName: exchangeName,
                                        routingKey);

            createExchange(exchangeName, exchangeType);
        }

        private IModel createChannel()
        {
            var factory = new ConnectionFactory { HostName = HOST_NAME };
            using var connection = factory.CreateConnection();
            return connection.CreateModel();
        }

        private void createExchange(string exchangeName, string exchangeType)
        {
            using var channel = createChannel();

            channel.ExchangeDeclare(exchangeName, exchangeType);
        }


        public void PublishPhMeasure()
        {
            using var channel = createChannel();
            string message;
            byte []encodedMessage;

            for (int i = 0; i < 10; i++)
            {
                message = $"my Message, message ID: {i}";
                encodedMessage = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(_publicationAdd, basicProperties: null, encodedMessage);
            }
        }
    }
}
