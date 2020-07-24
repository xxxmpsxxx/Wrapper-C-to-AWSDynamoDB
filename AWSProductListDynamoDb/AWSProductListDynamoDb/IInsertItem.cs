using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public interface IInsertItem
    {
        Task AddNewEntry(string productName, int productQuantity);
        Task AddNewEntry(string productName, int productQuantity, decimal price);
    }
}
