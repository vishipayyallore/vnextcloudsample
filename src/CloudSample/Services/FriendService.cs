using CloudSample.Models;
using CloudSample.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudSample.Services
{
    public class FriendService
    {
        #region Fields

        string ConnectionString1 = "DefaultEndpointsProtocol=https;AccountName=cloudprogramming;AccountKey=wmExjMIM20886BUOSwsLdQsFXcgVdSE6Bo57NT9od118H8nt/t9JFrIeGvHBCc39tzDagaZScqIA+zsUdg1hPw==";
        string ConnectionString2 = "DefaultEndpointsProtocol=https;AccountName=cloudprogrammingfake;AccountKey=wmExjMIM20886BUOSwsLdQsFXcgVdSE6Bo57NT9od118H8nt/t9JFrIeGvHBCc39tzDagaZScqIA+zsUdg1hPw==";
        string TableName = "friend";

        #endregion

        #region Private methods

        private CloudTable GetTableReference(string conString, string tableName)
        {
            CloudTable table = default(CloudTable);

            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                table = tableClient.GetTableReference("friend");
                table.GetPermissions();
            }
            catch (StorageException ex)
            {
                table = default(CloudTable);
            }

            return table;
        }

        private CloudTable GetTableReference()
        {
            CloudTable friendTable = GetTableReference(this.ConnectionString1, this.TableName);

            if (friendTable == default(CloudTable))
            {
                friendTable = GetTableReference(this.ConnectionString2, this.TableName);
                if (friendTable == default(CloudTable)) return null;
            }

            return friendTable;
        }

        #endregion

        #region Public methods

        public void InsertFriend()
        {
            CloudTable friendTable = GetTableReference();
            if (friendTable == null) return;

            TableOperation insertOperation = TableOperation.Insert(new Friend()
            {
                PartitionKey = "Dutt",
                RowKey = "Barkha",
                Phone = "8780000818",
                Email = "barkha@ndtv.com",
                Twitter = "https://twitter.com/ndtv"

            });

            friendTable.Execute(insertOperation);
        }

        public Friend GetFriend(string lastName, string firstName)
        {
            CloudTable friendTable = GetTableReference();
            if (friendTable == null) return null;

            TableOperation readOperation = TableOperation.Retrieve<Friend>("Johnson", "Ben");
            TableResult result = friendTable.Execute(readOperation);
            return (Friend)result.Result;
        }

        public IEnumerable<Friend> GetFriends(string firstName, string lastName)
        {
            CloudTable friendTable = GetTableReference();
            if (friendTable == null) return new List<Friend>();

            TableQuery<Friend> query = default(TableQuery<Friend>);

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                string partitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, lastName);
                string rowFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, firstName);
                string finalFilter = TableQuery.CombineFilters(partitionFilter, TableOperators.And, rowFilter);

                query = new TableQuery<Friend>().Where(finalFilter);
            }
            else if (!string.IsNullOrEmpty(firstName))
            {
                query = new TableQuery<Friend>()
                    .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, firstName));
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                query = new TableQuery<Friend>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, lastName));
            }
            else 
            {
                query = new TableQuery<Friend>();
            }


            return friendTable.ExecuteQuery<Friend>(query);
        }

        #endregion
    }
}
