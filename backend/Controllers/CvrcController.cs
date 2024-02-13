using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CvrcController : ControllerBase
{
        protected readonly IMapper _mapper;
        protected readonly CvrcService _cvrcService;

        public CvrcController(IMapper mapper, CvrcService cvrcService)
        {
                _mapper = mapper;
                _cvrcService = cvrcService;
        }

        [HttpGet()]
        public IEnumerable<CvrcDto> Get()
        {
                var cvrcEntities = _cvrcService.GetAll().ToList();
                List<CvrcDto> cvrcDtos = _mapper.Map<List<CvrcDto>>(cvrcEntities);
                return cvrcDtos;
        }
        [HttpGet("{id}")]
        public CvrcDto GetById([FromRoute] Guid id)
        {
                Cvrc cvrc = _cvrcService.GetById(id);
                CvrcDto cvrcDto = _mapper.Map<CvrcDto>(cvrc);
                return cvrcDto;
        }

        [HttpPost()]
        public IActionResult Save(CvrcDto cvrcDto)
        {
                _cvrcService.Save(cvrcDto);
                return StatusCode(200, "BRAVO BRAT");
        }


}