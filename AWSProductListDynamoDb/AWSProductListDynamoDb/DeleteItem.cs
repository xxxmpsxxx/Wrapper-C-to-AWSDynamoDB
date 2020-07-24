using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public class DeleteItem : IDeleteItem
    {
        private static readonly string tableName = Environment.GetEnvironmentVariable("AWS_CONTENT");
        private readonly IAmazonDynamoDB _dynamoDbClient;
        public DeleteItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task DeleteEntry(string productName)
        {
            try
            {
                var queryRequest = RequestBuilder(productName);

                await DeleteItemAsync(queryRequest);
            }
            catch (InternalServerErrorException)
            {
                Console.WriteLine("Erro 1");
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine("Erro 2");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.StackTrace);
            }
        }

        private DeleteItemRequest RequestBuilder(string productName)
        {
            var item = new Dictionary<string, AttributeValue>()
            {
                { "ProductName", new AttributeValue() { S = productName } }
            };

            return new DeleteItemRequest()
            {
                TableName = tableName,
                Key = item
            };
        }

        private async Task DeleteItemAsync(DeleteItemRequest request)
        {
            await _dynamoDbClient.DeleteItemAsync(request);
        }
    }
}
