using Intwenty.DataClient;
using Intwenty.DataClient.Model;
using Intwenty.DataClient.Reflection;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Text.Json;
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
            //var client = new DbConnection(DBMS.MariaDB, @"Server=127.0.0.1;Database=IntwentyDb;uid=root;Password=your_password");
            //var client = new DbConnection(DBMS.MSSqlServer, @"Data Source=localhost;Initial Catalog=IntwentyDB;User ID=sa;Password=your_password;MultipleActiveResultSets=true");
            //var client = new DbConnection(DBMS.SQLite, @"Data Source=wwwroot/sqlite/testdb.db");
            var client = new DbConnection(DBMS.PostgreSQL, @"Server = 127.0.0.1; Port = 5432; Database = IntwentyDb; User Id = postgres; Password = your_password; ");
            client.Open();

            try
            {
              
                if (client.TableExists("DataClient_SyncTestTable"))
                    client.RunCommand("DROP TABLE DataClient_SyncTestTable");

                client.CreateTable<DataClientSyncTest>();

                var tmpstart = DateTime.Now;
                for (int i = 0; i < 1000; i++)
                    client.InsertEntity(new DataClientSyncTest() { Name = "Dog " + i, BirthDate = DateTime.Now, DtOffset = DateTime.Now.ToUniversalTime(), TestValue = 2.34F });

                Console.WriteLine("Insert 1000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

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
                Console.WriteLine("Update 1000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                var nulltest = client.GetEntity<DataClientSyncTest>(678234);

                t = client.GetEntities<DataClientSyncTest>();

                t = client.GetEntities<DataClientSyncTest>("select Name from DataClient_SyncTestTable", false);

                t = client.GetEntities<DataClientSyncTest>();

                var jsonarr = client.GetJsonArray("select Id, Name, TestValue from DataClient_SyncTestTable", false);

                var resultset = client.GetResultSet("select * from DataClient_SyncTestTable",false);

                var typedresult = client.GetEntity<DataClientSyncTest>("select Name, TestValue from DataClient_SyncTestTable", false);

                var typedresult2 = client.GetEntity<DataClientSyncTest>(t[5].Id);

                tmpstart = DateTime.Now;
                foreach (var q in t)
                {
                    client.DeleteEntity(q);
                }
               
                Console.WriteLine("Delete 5000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");


                var testtable = new DbTestTable() { DbTableName = "ToDoTable" };
                testtable.DataColumns = new List<IBasicDbColumn>();
                var pkcol = new DbTestColumn() { DataType = IntwentyDataType.String, DbColumnName = "EntityId", IsAutoIncremental = false, IsPrimaryKey = true };
                testtable.PrimaryKeyColumn = pkcol;
                testtable.DataColumns.Add(pkcol);
                testtable.DataColumns.Add(new DbTestColumn() { DataType = IntwentyDataType.String, DbColumnName = "SomeStringData", IsAutoIncremental = false, IsPrimaryKey = false });
                testtable.DataColumns.Add(new DbTestColumn() { DataType = IntwentyDataType.DateTime, DbColumnName = "SomeDateData", IsAutoIncremental = false, IsPrimaryKey = false });
                testtable.DataColumns.Add(new DbTestColumn() { DataType = IntwentyDataType.Bool, DbColumnName = "SomeBoolData", IsAutoIncremental = false, IsPrimaryKey = false });
                testtable.DataColumns.Add(new DbTestColumn() { DataType = IntwentyDataType.Int, DbColumnName = "SomeIntData", IsAutoIncremental = false, IsPrimaryKey = false });
                testtable.DataColumns.Add(new DbTestColumn() { DataType = IntwentyDataType.TwoDecimal, DbColumnName = "SomeDecData", IsAutoIncremental = false, IsPrimaryKey = false });

                tmpstart = DateTime.Now;
                client.CreateTable(testtable);

                client.RunCommand("delete from ToDoTable");

                for (int i = 0; i < 500; i++) 
                {
                    var ToDoTable = new
                    {
                        EntityId = "key_"+Convert.ToString(i),
                        SomeStringData = "Here's a nice string",
                        SomeDateData = DateTime.Now,
                        SomeBoolData = true,
                        SomeIntData =777,
                        SomeDecData=5.6678
                    };

                    string json = JsonSerializer.Serialize(ToDoTable, new JsonSerializerOptions { WriteIndented = true });
                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;

                    client.InsertEntity(testtable, root);
                }

                Console.WriteLine("Insert 500 json entities took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                var resset = client.GetResultSet("select * from ToDoTable");
                var jsonset = client.GetJsonArray("ToDoTable");
                var jsonset2 = client.GetJsonArray("select * from ToDoTable where EntityId = 'key_25'",false);

                tmpstart = DateTime.Now;
                for (int i = 0; i < 500; i++)
                {
                    var ToDoTable = new
                    {
                        EntityId = "key_" + Convert.ToString(i),
                        SomeStringData = "Here's a nice string",
                        SomeDateData = DateTime.Now,
                        SomeBoolData = false,
                        SomeIntData = 888,
                        SomeDecData = 2.2222
                    };

                    string json = JsonSerializer.Serialize(ToDoTable, new JsonSerializerOptions { WriteIndented = true });
                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;

                    client.UpdateEntity(testtable, root);
                }


                Console.WriteLine("Update 500 json entities took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");
                tmpstart = DateTime.Now;
                for (int i = 0; i < 500; i++)
                {
                    client.DeleteEntity(testtable, "key_" + Convert.ToString(i));
                }

                Console.WriteLine("Delete 500 json entities took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

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
            //var client = new DbConnection(DBMS.MariaDB, @"Server=127.0.0.1;Database=IntwentyDb;uid=root;Password=your_password");
            //var client = new DbConnection(DBMS.MSSqlServer, @"Data Source=localhost;Initial Catalog=IntwentyDB;User ID=sa;Password=your_password;MultipleActiveResultSets=true");
            //var client = new DbConnection(DBMS.SQLite, @"Data Source=wwwroot/sqlite/testdb.db");
            var client = new DbConnection(DBMS.PostgreSQL, @"Server = 127.0.0.1; Port = 5432; Database = IntwentyDb; User Id = postgres; Password = your_password; ");
            await client.OpenAsync();

            try
            {
              
                if (await client.TableExistsAsync("DataClient_Async_TestTable"))
                    await client.RunCommandAsync("DROP TABLE DataClient_Async_TestTable");

                await client.CreateTableAsync<DataClientAsyncTest>();

                var tmpstart = DateTime.Now;
                for (int i = 0; i < 1000; i++)
                    await client.InsertEntityAsync(new DataClientAsyncTest() { Name = "Dog " + i, BirthDate = DateTime.Now, DtOffset = DateTime.Now.ToUniversalTime(), TestValue = 2.34F });

                Console.WriteLine("Insert 1000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

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
                Console.WriteLine("Update 1000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

                var nulltest = await client.GetEntityAsync<DataClientAsyncTest>(678234);

                t = await client.GetEntitiesAsync<DataClientAsyncTest>();

                t = await client.GetEntitiesAsync<DataClientAsyncTest>("select Name from DataClient_Async_TestTable", false);

                t = await client.GetEntitiesAsync<DataClientAsyncTest>();

                var jsonarr = await client.GetJsonArrayAsync("select id as \"Id\", name as \"Name\", testvalue as \"TestValue\" from DataClient_Async_TestTable", false);

                var resultset = await client.GetResultSetAsync("select * from DataClient_Async_TestTable", false);

                var typedresult = await client.GetEntityAsync<DataClientAsyncTest>("select Name, TestValue from DataClient_Async_TestTable", false);

                var typedresult2 = await client.GetEntityAsync<DataClientAsyncTest>(t[5].Id);

                tmpstart = DateTime.Now;
                foreach (var q in t)
                {
                    await client.DeleteEntityAsync(q);
                }
           
                Console.WriteLine("Delete 1000 took: " + DateTime.Now.Subtract(tmpstart).TotalMilliseconds + " ms");

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

    public class DbTestTable : IBasicDbTable
    {
        public string DbTableName { get; set; }

        public List<IBasicDbColumn> DataColumns { get; set; }

        public IBasicDbColumn PrimaryKeyColumn { get; set; }
    }

    public class DbTestColumn : IBasicDbColumn
    {
        public string DbColumnName { get; set; }

        public IntwentyDataType DataType { get; set; }

        public bool IsAutoIncremental { get; set; }

        public bool IsPrimaryKey { get; set; }
    }



}
