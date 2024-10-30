using NZWalskApi.Models.Domain;

namespace NZWalskApi.Models.DTO
{
    public class WalksDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        //Navigation Properties
        public Region Region { get; set; }

        public Difficulty Difficulty { get; set; }
    }
}
