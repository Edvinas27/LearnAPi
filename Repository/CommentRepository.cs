using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<bool> CommentExists(Guid id)
        {
            return await _context.Comment.AnyAsync(c => c.Id == id);
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comment.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(Guid id)
        {
            var CommentModel = await _context.Comment.FirstOrDefaultAsync(c => c.Id == id);

            if(CommentModel is null)
            {
                return null;
            }

            _context.Comment.Remove(CommentModel);
            await _context.SaveChangesAsync();

            return CommentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comment.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comment.FindAsync(id);
        }

        public async Task<Comment?> UpdateAsync(Guid id, UpdateCommentRequestDto commentDto)
        {
            var existingComment = await _context.Comment.FindAsync(id);

            if(existingComment is null)
            {
                return null;
            }

            _context.Entry(existingComment).CurrentValues.SetValues(commentDto);
            existingComment.CreatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingComment;
        }
    }
}