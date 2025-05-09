using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(Guid id);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> DeleteAsync(Guid id);
        Task<Stock?> UpdateAsync(Guid id, UpdateStockRequestDto stockDto);
        Task<bool> StockExists(Guid id);
    }
}