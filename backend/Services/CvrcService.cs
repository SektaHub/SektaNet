using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;

namespace backend.Services;

public class CvrcService
{
    
    protected readonly ApplicationDbContext _dbContext;
    protected readonly IMapper _mapper;

    public CvrcService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public IQueryable<Cvrc> GetAll()
    {
        return _dbContext.Cvrces;
    }

    public Cvrc GetById(Guid id)
    {
        return _dbContext.Cvrces.FirstOrDefault(cvrc => cvrc.Id == id);
    }

    public void Delete(Guid id)
    {
        Cvrc cvrc = _dbContext.Cvrces.FirstOrDefault(cvrc => cvrc.Id == id);
        _dbContext.Cvrces.Remove(cvrc);
        _dbContext.SaveChanges();
    }

    public void Save(CvrcDto cvrcDto)
    {
        Cvrc cvrc = _mapper.Map<Cvrc>(cvrcDto);
        _dbContext.Cvrces.Add(cvrc);
        _dbContext.SaveChanges();
    }


    public void Update(CvrcDto cvrcDto)
    {
        Cvrc cvrc = _mapper.Map<Cvrc>(cvrcDto);
        _dbContext.Cvrces.Update(cvrc);
        _dbContext.SaveChanges();
    }

}