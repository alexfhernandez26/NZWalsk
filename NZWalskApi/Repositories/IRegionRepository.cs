using Microsoft.AspNetCore.Mvc;
using NZWalskApi.Models.Domain;
using NZWalskApi.Models.DTO;

namespace NZWalskApi.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?>  GetById(Guid id);

        Task<Region> CreateAsync(Region region);

        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
