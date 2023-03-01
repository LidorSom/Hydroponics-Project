using RabbitMQ.Client;
using System.Text;
using Core;
using Core.Models;
using static Core.Enums;
using System.Text.Json.Nodes;
using System.Text.Json;
using Core.Interfaces;

namespace PhProducer
{
    public class PhMeasurePublisher : IDisposable
    {
        private readonly IPhDataProvider _dataProvider;
        private readonly IConnection _brokerConnection;
        private Dictionary<string, string> _routingKeys;
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        private readonly int _systemID;

        public PhMeasurePublisher(IPhDataProvider phDataProvider,
                                 int systemID,
                                 string exchangeName = nameof(ExchangesNames.PhExchange),
                                 string exchangeType = ExchangeType.Direct,
                                 Dictionary<string,string> routingKeys = null!,
                                 string hostName = "localhost")
                                 
        {
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
            _routingKeys = routingKeys;
            _dataProvider = phDataProvider;
            _systemID = systemID;

            var factory = new ConnectionFactory { HostName = hostName };
            _brokerConnection = factory.CreateConnection();
        }

        public async Task StartPublishing()
        {
            var channel = _brokerConnection.CreateModel();
            channel.ExchangeDeclare(_exchangeName, _exchangeType);

            string jsonString;
            byte []encodedMessage;
            PhModel dataToPub = new PhModel();

            while(true)
            {
                await foreach (var value in _dataProvider.GetData())
                {
                    dataToPub.SystemId = _systemID;
                    dataToPub.Value = value;

                    jsonString = JsonSerializer.Serialize(dataToPub);
                    encodedMessage = Encoding.UTF8.GetBytes(jsonString);

                    channel.BasicPublish(_exchangeName, routingKey: string.Empty, mandatory: false, basicProperties: null, body: encodedMessage);
                };
            }
        }

        private PublicationAddress createPubAddress(string exchangeType, string exchangeName, string routingKey = null!)
        {

            return new PublicationAddress(exchangeType: exchangeType,
                                        exchangeName: exchangeName,
                                        routingKey: routingKey);
        }

        public void Dispose()
        {
            _brokerConnection.Dispose();
        }
    }
}
