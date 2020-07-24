using AWSProductListDynamoDb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public interface IUpdateItem
    {
        Task<Item> Update(string productName, int productQuantity);
    }
}
