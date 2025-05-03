using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions.Mappers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentsDto = comments.Select(c => c.ToCommentDto());

            return Ok(commentsDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment is null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{id:guid}")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentRequestDto commentDto, [FromRoute] Guid id)
        {
            if(!await _stockRepo.StockExists(id))
            {
                return BadRequest("Stock does not exist.");
            }

            var commentModel = commentDto.ToCommentFromCreateDto(id);
            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new {Id = commentModel.Id}, commentModel.ToCommentDto());
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequestDto commentDto, [FromRoute] Guid id)
        {
            var commentModel = await _commentRepo.UpdateAsync(id, commentDto);

            if(commentModel is null)
            {
                return NotFound();
            }

            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            var existingComment = await _commentRepo.DeleteAsync(id);

            if(existingComment is null)
            {
                return NotFound("Comment does not exist");
            }
            return NoContent();
        }
    }
}