using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AWSProductListDynamoDb.AWSProductListDynamoDb
{
    public class AWSProductListDynamoDbExamples : IAWSProductListDynamoDbExamples
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static readonly string tableName = Environment.GetEnvironmentVariable("AWS_CONTENT");

        public AWSProductListDynamoDbExamples(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public void CreateDynamoDbTable()
        {
            try
            {
                CreateTempTable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async void CreateTempTable()
        {
            Console.WriteLine("Criando a tabela...");

            var request = new CreateTableRequest()
            {
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition()
                    {
                        AttributeName = "ProductName",
                        AttributeType = ScalarAttributeType.S
                    },
                    new AttributeDefinition()
                    {
                        AttributeName = "ProductQuantity",
                        AttributeType = ScalarAttributeType.N
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement()
                    {
                        AttributeName = "ProductName",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement()
                    {
                        AttributeName = "ProductQuantity",
                        KeyType = "RANGE"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput()
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = tableName
            };

            await _dynamoDbClient.CreateTableAsync(request);
            WaitUntilTableReady(tableName);
        }

        private void WaitUntilTableReady(string tableName)
        {
            string status = null;

            do
            {
                Thread.Sleep(5000);

                try
                {
                    var res = _dynamoDbClient.DescribeTableAsync(tableName);
                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {
                    Console.WriteLine(status);                    
                }
            } while (status != "ACTIVE");
        }
    }
}
