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
            serverEntity.Id = Guid.NewGuid();

            ResolveNestedEntities(serverEntity);

            _dbContext.Set<DiscordServer>().Add(serverEntity);
            _dbContext.SaveChanges();

            return _mapper.Map<DiscordServerDto>(serverEntity);
        }

        private void ResolveNestedEntities(DiscordServer serverEntity)
        {
            serverEntity.Guild = GetOrAttachEntity(serverEntity.Guild);
            serverEntity.Channel = GetOrAttachEntity(serverEntity.Channel);

            if (serverEntity.Messages != null)
            {
                for (int i = 0; i < serverEntity.Messages.Count; i++)
                {
                    var message = GetOrAttachEntity(serverEntity.Messages[i]);
                    serverEntity.Messages[i] = message;

                    message.Author = GetOrAttachEntity(message.Author);

                    if (message.Attachments != null)
                    {
                        for (int j = 0; j < message.Attachments.Count; j++)
                        {
                            message.Attachments[j] = GetOrAttachEntity(message.Attachments[j]);
                        }
                    }

                    if (message.Embeds != null)
                    {
                        for (int j = 0; j < message.Embeds.Count; j++)
                        {
                            message.Embeds[j] = GetOrAttachEntity(message.Embeds[j]);
                        }
                    }

                    if (message.Reactions != null)
                    {
                        for (int j = 0; j < message.Reactions.Count; j++)
                        {
                            var reaction = GetOrAttachEntity(message.Reactions[j]);
                            message.Reactions[j] = reaction;

                            reaction.Emoji = GetOrAttachEntity(reaction.Emoji);
                        }
                    }

                    if (message.Mentions != null)
                    {
                        for (int j = 0; j < message.Mentions.Count; j++)
                        {
                            message.Mentions[j] = GetOrAttachEntity(message.Mentions[j]);
                        }
                    }
                }
            }
        }

        private T GetOrAttachEntity<T>(T entity) where T : class
        {
            if (entity == null) return null;

            var primaryKeyProperty = GetPrimaryKeyProperty(typeof(T));
            if (primaryKeyProperty != null)
            {
                var primaryKeyValue = primaryKeyProperty.GetValue(entity);
                if (primaryKeyValue != null && !primaryKeyValue.Equals(GetDefaultValue(primaryKeyProperty.PropertyType)))
                {
                    // Check if the entity is already being tracked
                    var trackedEntity = _dbContext.ChangeTracker.Entries()
                        .FirstOrDefault(e => e.Entity.GetType() == typeof(T) && GetPrimaryKeyProperty(e.Entity.GetType()).GetValue(e.Entity).Equals(primaryKeyValue))
                        ?.Entity as T;

                    if (trackedEntity != null)
                    {
                        return trackedEntity;
                    }

                    // Look for an existing entity in the database
                    var dbEntity = _dbContext.Find(typeof(T), primaryKeyValue) as T;
                    if (dbEntity != null)
                    {
                        return dbEntity;
                    }
                }
            }

            // If entity is not tracked and not found in the database, attach it
            _dbContext.Attach(entity);
            return entity;
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
