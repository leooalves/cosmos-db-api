using Microsoft.AspNetCore.Mvc;
using cosmos_db_api.Services;
using cosmos_db_api.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cosmos_db_api.Controllers
{

    [Route("v1/item")]
    public class ItemController : Controller
    {

        private readonly ICosmosDbService _cosmosDbService;
        public ItemController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get()
        {      
            IEnumerable<Item> items = null;

            try
            {
                 items = await _cosmosDbService.GetItemsAsync("select * from c");
            }
            catch (System.Exception)
            {

                return BadRequest(new { message = "Não conseguiu atualizar o item" });
            }
            
                
            return Ok(items);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetById(string id)
        {      
            Item item = null;

            try
            {
                item = await _cosmosDbService.GetItemAsync(id);
            }
            catch (System.Exception)
            {

                return BadRequest(new { message = "Não conseguiu atualizar o item" });
            }

            if(item == null)
                return NotFound("Item não encontrado");

            return Ok(item);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Post([FromBody] Item model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                model.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddItemAsync(model);
            }
            catch (System.Exception)
            {

                return BadRequest(new { message = "Não conseguiu criar o item" });
            }

            return Ok(new { message = "item criado com sucesso", model });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _cosmosDbService.DeleteItemAsync(id);
            }
            catch (System.Exception)
            {

                return BadRequest(new { message = "Não conseguiu deletar o item" });
            }

            return Ok(new { message = "item deletado com sucesso" });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put([FromBody] Item model, string id)
        {

            if(model.Id != id)
                return NotFound(new { message = "itemn não encontrado" });
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            try
            {
                await _cosmosDbService.UpdateItemAsync(id, model);
            }
            catch (System.Exception)
            {

                return BadRequest(new { message = "Não conseguiu atualizar o item" });
            }

            return Ok(new { message = "item atualizado com sucesso", model });
        }



    }
}