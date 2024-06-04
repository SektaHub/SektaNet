using AutoMapper;
using backend.Models.Discord;
using System.Security.AccessControl;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace backend.Services
{
    public class DiscordService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DiscordService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IQueryable<DiscordServerDto> GetAll()
        {
            var entities = _dbContext.Set<DiscordServer>();
            var dtos = _mapper.ProjectTo<DiscordServerDto>(entities);
            return dtos;
        }

        public DiscordServerDto GetDtoById(Guid id)
        {
            var entity = _dbContext.Set<DiscordServer>().Find(id);

            if (entity == null)
            {
                // Entity with the specified ID was not found
                //throw new EntityNotFoundException($"Entity with ID '{id}' not found.");
            }

            var dto = _mapper.Map<DiscordServerDto>(entity);
            return dto;
        }

        //public DiscordServerDto Create(DiscordServerDto server)
        //{
        //    var entity = _mapper.Map<DiscordServer>(server);
        //    entity.Id = Guid.NewGuid();
        //    _dbContext.Set<DiscordServer>().Add(entity);
        //    _dbContext.SaveChanges();
        //    return _mapper.Map<DiscordServerDto>(entity);
        //}

        public DiscordServerDto Create(DiscordServerDto serverDto)
        {
            var serverEntity = _mapper.Map<DiscordServer>(serverDto);

            // Create a new Guid for the server itself
            serverEntity.Id = Guid.NewGuid();

            // Handle nested entities
            ResolveNestedEntities(serverEntity);

            // Add the server entity
            _dbContext.Set<DiscordServer>().Add(serverEntity);

            // Save changes to the database
            _dbContext.SaveChanges();

            // Map back to DTO and return
            return _mapper.Map<DiscordServerDto>(serverEntity);
        }

        private void ResolveNestedEntities<TEntity>(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;

                if (typeof(IEnumerable<object>).IsAssignableFrom(propertyType) && propertyType != typeof(string))
                {
                    var collection = (IEnumerable<object>)property.GetValue(entity);
                    if (collection != null)
                    {
                        // Use a list instead to access items by index
                        var itemList = collection.Cast<object>().ToList();
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            var item = itemList[i];
                            AttachOrReplaceEntity(ref item);
                            // Put the item back into the list after modification
                            itemList[i] = item;
                        }
                        // Set the modified list back to the property
                        property.SetValue(entity, itemList);
                    }
                }
                else if (propertyType.IsClass && propertyType != typeof(string))
                {
                    var nestedEntity = property.GetValue(entity);
                    if (nestedEntity != null)
                    {
                        AttachOrReplaceEntity(ref nestedEntity);
                        property.SetValue(entity, nestedEntity);
                    }
                }
            }
        }

        private void AttachOrReplaceEntity(ref object entity)
        {
            var entityType = entity.GetType();
            var primaryKeyProperty = GetPrimaryKeyProperty(entityType);

            if (primaryKeyProperty != null)
            {
                var primaryKeyValue = primaryKeyProperty.GetValue(entity);

                if (primaryKeyValue != null && !primaryKeyValue.Equals(GetDefaultValue(primaryKeyProperty.PropertyType)))
                {
                    // Check if the entity is already being tracked
                    var trackedEntity = _dbContext.ChangeTracker.Entries()
                        .FirstOrDefault(e => e.Entity.GetType() == entityType && GetPrimaryKeyProperty(e.Entity.GetType()).GetValue(e.Entity).Equals(primaryKeyValue))?.Entity;

                    if (trackedEntity != null)
                    {
                        entity = trackedEntity;
                    }
                    else
                    {
                        // Look for an existing entity in the database
                        var dbEntity = _dbContext.Find(entityType, primaryKeyValue);
                        if (dbEntity != null)
                        {
                            _dbContext.Entry(dbEntity).CurrentValues.SetValues(entity);
                            entity = dbEntity;
                        }
                        else
                        {
                            _dbContext.Attach(entity);
                        }
                    }

                    ResolveNestedEntities(entity);
                }
            }
        }

        private PropertyInfo GetPrimaryKeyProperty(Type type)
        {
            var key = _dbContext.Model.FindEntityType(type)?.FindPrimaryKey();
            if (key != null)
            {
                return key.Properties.Select(p => type.GetProperty(p.Name)).FirstOrDefault();
            }
            return null;
        }

        private object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    
    
    }
}
