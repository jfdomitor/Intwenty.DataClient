using Intwenty.DataClient;
using Intwenty.DataClient.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace DataClientTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("###########################");
            Console.WriteLine("Running synchronous test...");
            RunSynchTest();
            Console.WriteLine("");
            Console.WriteLine("###########################");
            Console.WriteLine("Running asynchronous test...");
            await RunAsynchTest();
            Console.WriteLine("Tests finnished");
            Console.ReadLine();

        }


        private static void RunSynchTest()
        {
            var start = DateTime.Now;
            var client = new Connection(DBMS.MariaDB, @"Server=127.0.0.1;Database=IntwentyDb;uid=root;Password=xxxxxxxx");
            client.Open();

            try
            {
              
                if (client.TableExists("DataClient_SyncTestTable"))
                    client.RunCommand("DROP TABLE DataClient_SyncTestTable");

                for (int i = 0; i < 10; i++)
                {
                    var ent = new DataClientSyncTest() { Name = "Dog " + i, BirthDate = DateTime.Now, DtOffset = DateTime.Now, TestValue = 2.34F };
                    var s = client.GetCreateTableSqlStatement<DataClientSyncTest>();
                    Console.WriteLine(s);
                    s = client.GetInsertSqlStatement(ent);
                    Console.WriteLine(s);
                    s = client.GetUpdateSqlStatement(ent);
                    Console.WriteLine(s);
                    System.Threading.Thread.Sleep(5000);
                }
                client.CreateTable<DataClientSyncTest>();

                var tmpstart = DateTime.Now;
                for (int i = 0; i < 5000; i++)
                    client.InsertEntity(new DataClientSyncTest() { Name = "Dog " + i, BirthDate = DateTime.Now, DtOffset = DateTime.Now, TestValue = 2.34F });

                Console.WriteLine("Insert 5000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                var t = client.GetEntities<DataClientSyncTest>();

                tmpstart = DateTime.Now;
                foreach (var q in t)
                {
                    var s = client.GetEntity<DataClientSyncTest>(q.Id);
                    s.Name = "Test " + q.Id;
                    s.TestValue = 777.77F;
                    s.DtOffset = null;
                    s.BirthDate = null;
                    client.UpdateEntity(s);
                }
                Console.WriteLine("Update 5000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                var nulltest = client.GetEntity<DataClientSyncTest>(678234);

                t = client.GetEntities<DataClientSyncTest>();

                t = client.GetEntities<DataClientSyncTest>("select Name from DataClient_SyncTestTable", false);

                t = client.GetEntities<DataClientSyncTest>();

                var jsonarr = client.GetJsonArray("select id as \"Id\", name as \"Name\", testvalue as \"TestValue\" from DataClient_SyncTestTable");

                var resultset = client.GetResultSet("select * from DataClient_SyncTestTable");

                var typedresult = client.GetEntity<DataClientSyncTest>("select Name, TestValue from DataClient_SyncTestTable", false);

                var typedresult2 = client.GetEntity<DataClientSyncTest>(t[5].Id);

                tmpstart = DateTime.Now;
                foreach (var q in t)
                {
                    client.DeleteEntity(q);
                }
               
                Console.WriteLine("Delete 5000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                Console.WriteLine("Sync test lasted: " + DateTime.Now.Subtract(start).TotalMilliseconds + " ms");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                client.Close();
            }
        }

        private static async Task RunAsynchTest()
        {
            var start = DateTime.Now;
            var client = new Connection(DBMS.MariaDB, @"Server=127.0.0.1;Database=IntwentyDb;uid=root;Password=xxxxx");
            await client.OpenAsync();

            try
            {
              
                if (await client.TableExistsAsync("DataClient_AsyncTestTable"))
                    await client.RunCommandAsync("DROP TABLE DataClient_AsyncTestTable");

                await client.CreateTableAsync<DataClientAsyncTest>();

                var tmpstart = DateTime.Now;
                for (int i = 0; i < 5000; i++)
                    await client.InsertEntityAsync(new DataClientAsyncTest() { Name = "Dog " + i, BirthDate = DateTime.Now, DtOffset = DateTime.Now, TestValue = 2.34F });

                Console.WriteLine("Insert 5000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                var t = await client.GetEntitiesAsync<DataClientAsyncTest>();

                tmpstart = DateTime.Now;
                foreach (var q in t)
                {
                    var s = await client.GetEntityAsync<DataClientAsyncTest>(q.Id);
                    s.Name = "Test " + q.Id;
                    s.TestValue = 777.77F;
                    s.DtOffset = null;
                    s.BirthDate = null;
                    client.UpdateEntity(s);

                }
                Console.WriteLine("Update 5000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                var nulltest = await client.GetEntityAsync<DataClientAsyncTest>(678234);

                t = await client.GetEntitiesAsync<DataClientAsyncTest>();

                t = await client.GetEntitiesAsync<DataClientAsyncTest>("select Name from DataClient_AsyncTestTable", false);

                t = await client.GetEntitiesAsync<DataClientAsyncTest>();

                var jsonarr = await client.GetJsonArrayAsync("select id as \"Id\", name as \"Name\", testvalue as \"TestValue\" from DataClient_AsyncTestTable");

                var resultset = await client.GetResultSetAsync("select * from DataClient_AsyncTestTable");

                var typedresult = await client.GetEntityAsync<DataClientAsyncTest>("select Name, TestValue from DataClient_AsyncTestTable", false);

                var typedresult2 = await client.GetEntityAsync<DataClientAsyncTest>(t[5].Id);

                tmpstart = DateTime.Now;
                foreach (var q in t)
                {
                    await client.DeleteEntityAsync(q);
                }
           
                Console.WriteLine("Delete 5000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                Console.WriteLine("Async test lasted: " + DateTime.Now.Subtract(start).TotalMilliseconds + " ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                client.Close();
            }
        }


    }


    [DbTablePrimaryKey("Id")]
    [DbTableName("DataClient_SyncTestTable")]
    public class DataClientSyncTest
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        [Ignore]
        public string StringField { get; set; }

        [Ignore]
        public List<DataClientSyncTest> ListField { get; set; }

        public DateTimeOffset? DtOffset { get; set; }

        public float? TestValue { get; set; }

        public bool BoolTestValue { get; set; }


    }

    [DbTablePrimaryKey("Id")]
    [DbTableName("DataClient_Async_TestTable")]
    public class DataClientAsyncTest
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        [Ignore]
        public string StringField { get; set; }

        [Ignore]
        public List<DataClientAsyncTest> ListField { get; set; }

        public DateTimeOffset? DtOffset { get; set; }

        public float? TestValue { get; set; }

        public int NewTestIntField { get; set; }

        public string NewTestStringField { get; set; }

        public bool NewBooleanField { get; set; }

        public int NewTestIntField2 { get; set; }

        public string NewTestStringField2 { get; set; }



    }



}
