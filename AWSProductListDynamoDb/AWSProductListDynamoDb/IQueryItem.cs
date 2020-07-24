using AWSProductListDynamoDb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public interface IQueryItem
    {
        Task<DynamoDbTableItems> GetItems(string? productName);
    }
}
