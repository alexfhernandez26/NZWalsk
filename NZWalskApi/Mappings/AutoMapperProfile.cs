using AutoMapper;
using NZWalskApi.Models.Domain;
using NZWalskApi.Models.DTO;

namespace NZWalskApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
                CreateMap<Region,RegionDto>().ReverseMap();
                CreateMap<AddRegionDomainDto,Region>().ReverseMap();
                CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
                CreateMap<AddWalksRequestDto,Walk>().ReverseMap();
                CreateMap<Walk, WalksDto>().ReverseMap();
                CreateMap<UpdateWalksRequestDto, Walk>().ReverseMap();
            
        }
    }
}
