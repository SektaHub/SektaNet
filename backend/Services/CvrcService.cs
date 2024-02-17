using AutoMapper;
using backend.Models.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services.Common;

namespace backend.Services;
    public class CvrcService : BaseCrudService<Cvrc, CvrcDto>
    {
        public CvrcService(IMapper mapper, ApplicationDbContext dbContext) : base(mapper, dbContext)
        {
        }
    }
