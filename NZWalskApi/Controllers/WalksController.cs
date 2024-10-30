using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalskApi.Models.Domain;
using NZWalskApi.Models.DTO;
using NZWalskApi.Repositories;

namespace NZWalskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalksRepository walksRepository;

        public WalksController(IMapper mapper,IWalksRepository walksRepository)
        {
            this.mapper = mapper;
            this.walksRepository = walksRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            //map dto to entity
            var walksRequest = mapper.Map<Walk>(addWalksRequestDto);

            var walks =await walksRepository.CreateAsync(walksRequest);

            if (walks == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalksDto>(walks));

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walksExist = await walksRepository.GetByIdAsync(id);

            if (walksExist == null) return NotFound();

            var walskDto = mapper.Map<WalksDto>(walksExist);

            return Ok(walskDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterData,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber =1, [FromQuery] int pageSize = 100)
        {
            var walksExist = await walksRepository.GetAllAsync(filterOn,filterData,sortBy,isAscending ?? true,pageNumber,pageSize);

            if (walksExist == null) return NotFound();

            var walskDto = mapper.Map<List<WalksDto>>(walksExist);

            return Ok(walskDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateWalksRequestDto updateWalksRequestDto)
        {
            var walksMapping = mapper.Map<Walk>(updateWalksRequestDto);
            var walk =await walksRepository.UpdateAsync(id, walksMapping);

            if (walk == null) return NotFound();

            var walksDto = mapper.Map<WalksDto>(walk);

            return Ok(walksDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walksExist = await walksRepository.DeleteAsync(id);

            if (walksExist == null) return NotFound();

            var walskDto = mapper.Map<WalksDto>(walksExist);

            return Ok(walskDto);
        }
    }
}
