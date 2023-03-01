using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Utils.BaseClasses
{
    public abstract class MongoDBServiceBase
    {
        private static Dictionary<string, bool> collectionsExistence= new Dictionary<string, bool>();
        private static object lockContext = new object();
        protected IMongoDatabase _db = null!;
        protected readonly string _connectionString;
        protected readonly string _dbName;

        public MongoDBServiceBase(string connectionString, string dbName)
        {
            _connectionString = connectionString;
            _dbName = dbName;
        }

        protected void connectToDB()
        {
            var mongoClient = new MongoClient(_connectionString);
            _db = mongoClient.GetDatabase(_dbName, new MongoDatabaseSettings());
        }


        // NO NEED TO? GetCollection creates if doesn't exist....
        //protected void insureCollectionExists(string collectionName)
        //{
        //    try
        //    {
        //        var names = _db?.ListCollectionNames();

        //        if (names?.ToList().Contains(collectionName) == false)
        //        {
        //            _db?.CreateCollection(collectionName);
        //        }
        //    }
        //    catch (MongoCommandException) // if collection was created by other thread
        //    {
        //        ;
        //    }
        //}


    }
}
