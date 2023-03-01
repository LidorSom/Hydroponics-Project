using PhProducer;
using PhConsumer;
using PhConsumer.Services;
using PhDataProviding;

namespace BrokerTestings
{
    public class ProducerConsumerTests
    {
        [Fact]
        public async Task PhConsumer_TestExactNumberOfPublishedPhValues()
        {
            // TO CHECK!
            PhMeasurePublisher producer = new PhMeasurePublisher(new PhDataProvider(), 1);
            PhMeasureConsumer consumer1 = new PhMeasureConsumer(new PhDBService(connectionString: "mongodb://localhost:27017", "HydroDB", "PhMeasures"));
            //PhMeasureConsumer consumer2 = new PhMeasureConsumer(new PhDBService(connectionString: "mongodb://localhost:27017", "HydroDB", "PhMeasures"));

            consumer1.StartConsuming();
            await producer.StartPublishing();

            Thread.Sleep(10000);
        }


    }
}