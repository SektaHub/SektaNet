using AutoMapper;
using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CvrcController : BaseCrudController<CvrcService, Cvrc, CvrcDto>

{
    public CvrcController(CvrcService service) : base(service)
    {
    }
}