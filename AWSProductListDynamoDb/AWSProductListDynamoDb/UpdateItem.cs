using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AWSProductListDynamoDb.Models;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public class UpdateItem : IUpdateItem
    {
        private readonly IQueryItem _queryItem;
        private static readonly string tableName = Environment.GetEnvironmentVariable("AWS_CONTENT");
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public UpdateItem(IQueryItem queryItem, IAmazonDynamoDB dynamoDbClient)
        {
            _queryItem = queryItem;
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<Item> Update(string productName, int productQuantity)
        {
            var response = await _queryItem.GetItems(productName);

            var currentQuantity = response.Items.Select(p => p.productQuantity).FirstOrDefault();

            var request = RequestBuilder(productName, productQuantity, currentQuantity);

            var result = await UpdateItemAsync(request);

            return new Item
            {
                productName = result.Attributes["ProductName"].S,
                productQuantity = Convert.ToInt32(result.Attributes["ProductQuantity"].N)
            };
        }

        private UpdateItemRequest RequestBuilder(string productName, int productQuantity, int currentQuantity)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>
                {
                    {
                        "ProductName", new AttributeValue
                        {
                            S = productName
                        }

                    }//,
                    //{
                    //    "ProductQuantity", new AttributeValue
                     //   {
                     //       N = currentQuantity.ToString()
                     //   }

                   // }
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#P", "ProductQuantity"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":newquantity", new AttributeValue
                        {
                            N = productQuantity.ToString()
                        }
                    },
                    {
                        ":currquantity", new AttributeValue
                        {
                            N = currentQuantity.ToString()
                        }
                    }
                },

                UpdateExpression = "SET #P = :newquantity",
                ConditionExpression = "#P = :currquantity",

                TableName = tableName,
                ReturnValues = "ALL_NEW"
            };

            return request;
        }
        private async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request)
        {
            var response = await _dynamoDbClient.UpdateItemAsync(request);

            return response;
        }
    }
}
