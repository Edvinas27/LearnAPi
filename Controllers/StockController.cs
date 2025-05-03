using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Extensions.Mappers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;




namespace api.Controllers
{
    //automatically turns on modelstate.isvalid(check if models are all true)
    [ApiController]
    //If class was HelloController it would be api/Hello...
    [Route("api/[Controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController( IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var stocks = await _stockRepo.GetAllAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stockDto);
        }

        [HttpGet("{id:guid}")]
        //[FromRoute] not needed here since HttpGet("{id}") matches name of Guid, but more official
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var stock = await _stockRepo.GetByIdAsync(id);

            if(stock is null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        //tells to look for CreateStockRequestDto in JSON BODY
        public async Task<IActionResult> AddStock([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);

            // it is going to run GetById with the Id and return ToStockDto(), REST PRINCIPLE;
            return CreatedAtAction(nameof(GetById), new { Id = stockModel.Id}, stockModel.ToStockDto());
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStock([FromRoute] Guid id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _stockRepo.UpdateAsync(id,updateDto);

            if(stockModel is null)
            {
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStock([FromRoute] Guid id)
        {
            var stockModel = await _stockRepo.DeleteAsync(id);

            if(stockModel is null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}