using AutoMapper;
using backend.Models.Common;

namespace backend.Services.Common;

public class BaseCrudService<TEntity, TDto>
    where TEntity : BaseCrudEntity
    where TDto : BaseCrudDto
{
    protected readonly IMapper _mapper;
    protected readonly ApplicationDbContext _dbContext;

    public BaseCrudService(IMapper mapper, ApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public List<TDto> GetAll()
    {
        var entities = _dbContext.Set<TEntity>();
        List<TDto> dtos = _mapper.Map<List<TDto>>(entities);
        return dtos;

    }

  public TDto GetById(Guid id)
     {
         TEntity entity = _dbContext.Set<TEntity>().FirstOrDefault(entity=> entity.Id == id);
         TDto dto = _mapper.Map<TDto>(entity);
         return dto;
     }

   public void Delete(Guid id)
      {
          TEntity entity = _dbContext.Set<TEntity>().FirstOrDefault(entity=> entity.Id == id);
          _dbContext.Set<TEntity>().Remove(entity);
          _dbContext.SaveChanges();
      }
    
    public void Save(TDto dto)
    {
        TEntity entity= _mapper.Map<TEntity>(dto);
        _dbContext.Set<TEntity>().Add(entity);
        _dbContext.SaveChanges();
    }


    public void Update(TDto dto)
    {
        TEntity entity = _mapper.Map<TEntity>(dto);
        _dbContext.Set<TEntity>().Update(entity);
        _dbContext.SaveChanges();
    }

}