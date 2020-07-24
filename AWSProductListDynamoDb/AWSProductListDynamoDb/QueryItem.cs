using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using AWSProductListDynamoDb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public class QueryItem: IQueryItem
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static readonly string tableName = Environment.GetEnvironmentVariable("AWS_CONTENT");

        public QueryItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }        

        public async Task<DynamoDbTableItems> GetItems(string? productName)
        {
            var queryRequest = RequestBuilder(productName);

            var result = await ScanAsync(queryRequest);

            return new DynamoDbTableItems()
            {
                Items = result.Items.Select(Map).ToList()
            };
        }

        private Item Map(Dictionary<string, AttributeValue> result)
        {
            return new Item()
            {
                productName = result["ProductName"].S,
                productQuantity = int.Parse(result["ProductQuantity"].N)
            };
        }

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var response = await _dynamoDbClient.ScanAsync(request);
            return response;
        }

        private ScanRequest RequestBuilder(string? productName)
        {
            if (string.IsNullOrEmpty(productName) || productName.Length <= default(int))
                return new ScanRequest() { TableName = tableName };
            
            return new ScanRequest()
            {
                TableName = tableName,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    { ":v_ProductName", new AttributeValue{ S = productName} }
                },
                FilterExpression = "ProductName = :v_ProductName",
                ProjectionExpression = "ProductName, ProductQuantity"
            };
        }        
    }
}
