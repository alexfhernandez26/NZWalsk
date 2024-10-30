using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalskApi.CustomActionFilter;
using NZWalskApi.Data;
using NZWalskApi.Models.Domain;
using NZWalskApi.Models.DTO;
using NZWalskApi.Repositories;
using System.Text.Json;

namespace NZWalskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionController> logger;

        public RegionController( IRegionRepository regionRepository,IMapper mapper, ILogger<RegionController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
      //  [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("logger before method");
            var regionsDomain =await regionRepository.GetAllAsync();

            var regionDto = mapper.Map<List<RegionDto>>(regionsDomain);

            logger.LogInformation($"Regions here  {JsonSerializer.Serialize(regionDto)}");
            return Ok(regionDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetId([FromRoute] Guid id)
        {
            var regionsDomain =await regionRepository.GetById(id);

            if (regionsDomain == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionsDomain);

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModelAtribute]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionDomainDto addRegionDomainDto)
        {
            
                var regionDomainModel = mapper.Map<Region>(addRegionDomainDto);


                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                var regionDto = mapper.Map<Region>(regionDomainModel);


                return CreatedAtAction(nameof(GetId), new { id = regionDto.Id }, regionDto);
           
            
        }

        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var region = mapper.Map<Region>(updateRegionRequestDto);
           
            var regionModel = await regionRepository.UpdateAsync(id, region);

            var regionDtos = mapper.Map<RegionDto>(regionModel);
           

            return Ok(regionDtos);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regio =await regionRepository.DeleteAsync(id);
            if(regio == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}

