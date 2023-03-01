using Core.Models;
using PhConsumer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokerTestings
{
    public class MongoDBServiceTests
    {
        [Fact]
        public async Task PhConsumer_TestMongoBaseClass()
        {
            var db = new PhDBService(connectionString: "mongodb://localhost:27017", "HydroDB", "PhMeasures");

            var readVal = await db.ReadPh(new Guid("42752a64-7d4b-48d1-89b9-bff462b8ec74")); //await db.ReadPh(Guid.Empty);

            Debug.WriteLine(readVal?.Value);
        }
    }
}
