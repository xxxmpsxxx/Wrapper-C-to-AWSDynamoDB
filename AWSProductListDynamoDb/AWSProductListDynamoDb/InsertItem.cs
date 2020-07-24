using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public class InsertItem : IInsertItem
    {
        private static readonly string tableName = Environment.GetEnvironmentVariable("AWS_CONTENT");

        private readonly IAmazonDynamoDB _dynamoDbClient;
        public InsertItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task AddNewEntry(string productName, int productQuantity, decimal price)
        {
            try
            {
                var queryRequest = RequestBuilder(productName, productQuantity, price);
                await PutItemAsync(queryRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.StackTrace);
            }
        }

        public async Task AddNewEntry(string productName, int productQuantity)
        {
            try
            {
                var queryRequest = RequestBuilder(productName, productQuantity);
                await PutItemAsync(queryRequest);
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

        private PutItemRequest RequestBuilder(string productName, int productQuantity, decimal price)
        {
            var attributes = new Dictionary<string, AttributeValue>();

            attributes["ProductName"] = new AttributeValue() { S = productName };
            attributes["ProductQuantity"] = new AttributeValue() { N = productQuantity.ToString() };
            attributes["ProductPrice"] = new AttributeValue() { N = price.ToString() };

            return new PutItemRequest()
            {
                TableName = tableName,
                Item = attributes
            };
        }
            

        private PutItemRequest RequestBuilder(string productName, int productQuantity)
        {
            var attributes = new Dictionary<string, AttributeValue>();

            attributes["ProductName"] = new AttributeValue() { S = productName };
            attributes["ProductQuantity"] = new AttributeValue() { N = productQuantity.ToString() };

            return new PutItemRequest()
            {
                TableName = tableName,
                Item = attributes
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            try
            {
                await _dynamoDbClient.PutItemAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.InnerException.StackTrace}");
            }
        }        
    }
}
