using NZWalskApi.Models.Domain;

namespace NZWalskApi.Repositories
{
    public interface IIMageRepository
    {
        Task<Image> Upload(Image image);
    }
}
