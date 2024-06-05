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

            AttachOrReplaceEntities(serverEntity);

            _dbContext.Set<DiscordServer>().Add(serverEntity);
            _dbContext.SaveChanges();

            return _mapper.Map<DiscordServerDto>(serverEntity);
        }

        private void AttachOrReplaceEntities(DiscordServer serverEntity)
        {
            if (serverEntity.Guild != null)
            {
                var guild = serverEntity.Guild;
                AttachOrReplaceEntity(ref guild);
                serverEntity.Guild = guild;
            }

            if (serverEntity.Channel != null)
            {
                var channel = serverEntity.Channel;
                AttachOrReplaceEntity(ref channel);
                serverEntity.Channel = channel;
            }

            if (serverEntity.Messages != null)
            {
                for (int i = 0; i < serverEntity.Messages.Count; i++)
                {
                    var message = serverEntity.Messages[i];
                    AttachOrReplaceEntity(ref message);
                    serverEntity.Messages[i] = message;

                    if (message.Author != null)
                    {
                        var author = message.Author;
                        AttachOrReplaceEntity(ref author);
                        message.Author = author;
                    }

                    if (message.Attachments != null)
                    {
                        for (int j = 0; j < message.Attachments.Count; j++)
                        {
                            var attachment = message.Attachments[j];
                            AttachOrReplaceEntity(ref attachment);
                            message.Attachments[j] = attachment;
                        }
                    }

                    if (message.Embeds != null)
                    {
                        for (int j = 0; j < message.Embeds.Count; j++)
                        {
                            var embed = message.Embeds[j];
                            AttachOrReplaceEntity(ref embed);
                            message.Embeds[j] = embed;
                        }
                    }

                    if (message.Reactions != null)
                    {
                        for (int j = 0; j < message.Reactions.Count; j++)
                        {
                            var reaction = message.Reactions[j];
                            AttachOrReplaceEntity(ref reaction);
                            message.Reactions[j] = reaction;

                            if (reaction.Emoji != null)
                            {
                                var emoji = reaction.Emoji;
                                AttachOrReplaceEntity(ref emoji);
                                reaction.Emoji = emoji;
                            }
                        }
                    }

                    if (message.Mentions != null)
                    {
                        for (int j = 0; j < message.Mentions.Count; j++)
                        {
                            var mention = message.Mentions[j];
                            AttachOrReplaceEntity(ref mention);
                            message.Mentions[j] = mention;
                        }
                    }
                }
            }
        }

        private void AttachOrReplaceEntity<T>(ref T entity) where T : class
        {
            var primaryKeyProperty = GetPrimaryKeyProperty(typeof(T));
            if (primaryKeyProperty != null)
            {
                var primaryKeyValue = primaryKeyProperty.GetValue(entity);
                if (primaryKeyValue != null && !primaryKeyValue.Equals(GetDefaultValue(primaryKeyProperty.PropertyType)))
                {
                    var trackedEntity = _dbContext.ChangeTracker.Entries()
                        .FirstOrDefault(e => e.Entity.GetType() == typeof(T) && GetPrimaryKeyProperty(e.Entity.GetType()).GetValue(e.Entity).Equals(primaryKeyValue))
                        ?.Entity as T;

                    if (trackedEntity != null)
                    {
                        entity = trackedEntity;
                    }
                    else
                    {
                        var dbEntity = _dbContext.Find(typeof(T), primaryKeyValue) as T;
                        if (dbEntity != null)
                        {
                            entity = dbEntity;
                        }
                        else
                        {
                            _dbContext.Attach(entity);
                        }
                    }
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
