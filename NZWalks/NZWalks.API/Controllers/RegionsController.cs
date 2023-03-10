using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public IMapper Mapper { get; }
        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions=await regionRepository.GetAllAsync();


            //return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id=region.Id,
            //        Code=region.Code,
            //        Name=region.Name,
            //        Area=region.Area,
            //        Lat=region.Lat,
            //        Long=region.Long,
            //        Population=region.Population,
            //    };

            //    regionsDTO.Add(regionDTO);
            //});


            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO); 
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region=await regionRepository.GetAsync(id);  

            if(region == null)
            {
                return NotFound();
            }

            var regionDTO=mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);   
        }


        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request(DTO) to domain model
            var region=new Models.Domain.Region()
            {
                Code=addRegionRequest.Code,
                Area=addRegionRequest.Area,
                Lat=addRegionRequest.Lat,
                Long=addRegionRequest.Long,
                Name=addRegionRequest.Name,
                Population=addRegionRequest.Population
            };
            
            //Pass details to the repository
            region=await regionRepository.AddAsync(region);

            //Convert back to the DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population

            };

             return CreatedAtAction(nameof(GetAllRegionsAsync),new {id=regionDTO.Id }, regionDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from the database
            var region=await regionRepository.DeleteAsync(id);    

            //If null NotFound
            if(region==null)
            {
                return NotFound();
            }

            //Convert response back to the DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            //return Ok response
            return Ok(regionDTO);
        }
    }
}
