using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using Core.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Core.Interfaces;
using System.Runtime.CompilerServices;
using MongoDB.Utils;
using MongoDB.Utils.BaseClasses;
using MongoDB.Bson;

namespace PhConsumer.Services
{
    public class PhDBService : MongoDBServiceBase, IPhRepository
    {
        private readonly string _collectionName;

        public PhDBService(string connectionString, string dbName, string collectionName)
                          : base(connectionString, dbName)
        {
            _collectionName = collectionName;
            base.connectToDB();
        }

        public async Task WritePh(PhModel data)
        {
            try
            {
                var phCollection = _db?.GetCollection<PhModel>(_collectionName, new MongoCollectionSettings() { AssignIdOnInsert = true });
                
                if(phCollection != null )
                {
                    await phCollection.InsertOneAsync(data);
                }
            }
            catch(Exception ex)
            {
                // to log
                throw;
            }
        }

        public async Task<PhModel> ReadPh(Guid guid)
        {
            try
            {
                PhModel result = null!;
                var collection = _db.GetCollection<PhModel>(_collectionName);
                var filter = guid == Guid.Empty ? new BsonDocument() : Builders<PhModel>.Filter.Eq((value) => value.Id, guid);

                if(collection != null)
                {
                    var res = await collection.FindAsync(filter);

                    var phList = await (res?.ToListAsync() ?? Task.FromResult<List<PhModel>>(null));
                    
                    result = phList?.FirstOrDefault();
                }
                
                return result;
            }
            catch(Exception ex)
            {
                return await Task.FromException<PhModel>(ex);
            }
        }
    }
}
