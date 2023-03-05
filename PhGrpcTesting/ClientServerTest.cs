using PhProducerGRPC;
using PhConsumerGRPC;
using Grpc.Net.Client;
using System.Diagnostics;

namespace PhGrpcTesting
{
    public class ClientServerTest
    {
        [Fact]
        public async void PhMeasurementSerivce_TestCommunication()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5180");
            var client = new PhConsumerGRPC.PhMeasurementService.PhMeasurementServiceClient(channel);
            var reply = await client.PhMeasurementAsync(new PhConsumerGRPC.PhMessage() { PhValue = 7.777f, SystemID = 1 });

            Debug.WriteLine(reply?.PhValueGot);
        }
    }
}