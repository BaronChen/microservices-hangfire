using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.Data
{
    public static class DatabaseCleaner
    {
        public static void ClearDatabase(string connectionString, string databaseName)
        {

            var client = new MongoClient(connectionString);

            client.DropDatabase(databaseName);

        }
    }
}
