using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public interface IDeleteItem
    {
        Task DeleteEntry(string productName);
    }
}
