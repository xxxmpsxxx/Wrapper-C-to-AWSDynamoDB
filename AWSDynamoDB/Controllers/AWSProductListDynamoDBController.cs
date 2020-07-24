using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AWSProductListDynamoDb.AWSProductListDynamoDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AWSDynamoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AWSProductListDynamoDBController : ControllerBase
    {
        private readonly IAWSProductListDynamoDbExamples _dynamoDbExamples;
        private readonly IInsertItem _insertItem;
        private readonly IQueryItem _queryItem;
        private readonly IDeleteItem _deleteItem;
        private readonly IUpdateItem _updateItem;

        public AWSProductListDynamoDBController(IAWSProductListDynamoDbExamples dynamoDbExamples, IInsertItem insertItem, IQueryItem queryItem, IDeleteItem deleteItem, IUpdateItem updateItem)
        {
            _dynamoDbExamples = dynamoDbExamples;
            _insertItem = insertItem;
            _queryItem = queryItem;
            _deleteItem = deleteItem;
            _updateItem = updateItem;
        }

        [Route("createtable")]
        public IActionResult CreateDynamoDbTable()
        {
            _dynamoDbExamples.CreateDynamoDbTable();

            return Ok();
        }

        [HttpPost("insertitem")]        
        public IActionResult InsertItem([FromQuery]string productName, int productQuantity)
        {
            _insertItem.AddNewEntry(productName, productQuantity);

            return Ok();
        }

        [HttpPost]
        [Route("insertitemnew")]
        public IActionResult InsertItemNew([FromQuery]string productName, int productQuantity, decimal price)
        {
            _insertItem.AddNewEntry(productName, productQuantity, price);

            return Ok();
        }

        [HttpGet("queryitems")]        
        public async Task<IActionResult> GetItems([FromQuery]string productName)
        {
            var response = await _queryItem.GetItems(productName);

            return Ok(response);
        }

        [HttpDelete("deleteitems")]
        public IActionResult DeleteItems([FromQuery]string productName)
        {
            _deleteItem.DeleteEntry(productName);

            return Ok();
        }

        [HttpPost("updateitems")]
        public IActionResult UpdateItem([FromQuery]string productName, int productQuantity)
        {
            _updateItem.Update(productName, productQuantity);

            return Ok();
        }
    }
}
