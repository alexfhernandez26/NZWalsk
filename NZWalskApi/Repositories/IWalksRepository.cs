using Microsoft.AspNetCore.Mvc;
using NZWalskApi.Models.Domain;

namespace NZWalskApi.Repositories
{
    public interface IWalksRepository
    {
        Task<Walk> CreateAsync(Walk walk);

        Task<Walk?> UpdateAsync(Guid id, Walk walk);

        Task<Walk?> DeleteAsync(Guid id);

        Task<List<Walk>> GetAllAsync(string? filterOn,string? filterData,string? sortBy,bool isAscending =true, int pageNumber = 1,  int pageSize = 100);

        Task<Walk?> GetByIdAsync(Guid id);
    }
}
