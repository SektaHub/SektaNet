﻿using AutoMapper;
using backend.Models.Discord;
using backend;

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
            throw new Exception($"DiscordServer with ID '{id}' not found.");
        }

        var dto = _mapper.Map<DiscordServerDto>(entity);
        return dto;
    }

    private void ProcessMessages(DiscordServer serverEntity)
    {
        foreach (var message in serverEntity.Messages)
        {
            message.TimeStamp = ConvertToUtc(message.TimeStamp);
            if (message.TimeStampEdited.HasValue)
                message.TimeStampEdited = ConvertToUtc(message.TimeStampEdited.Value);
            if (message.CallEndedTimeStamp.HasValue)
                message.CallEndedTimeStamp = ConvertToUtc(message.CallEndedTimeStamp.Value);
        }
    }

    private DateTime ConvertToUtc(DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc)
            return dateTime;

        // If it's local, convert to UTC
        if (dateTime.Kind == DateTimeKind.Local)
            return dateTime.ToUniversalTime();

        // If it's unspecified, assume it's UTC
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public DiscordServerDto Create(DiscordServerDto serverDto)
    {
        using var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            var serverEntity = _mapper.Map<DiscordServer>(serverDto);
            serverEntity.Id = Guid.NewGuid();

            ProcessGuild(serverEntity);
            ProcessMessages(serverEntity);
            ProcessUsers(serverEntity);
            ProcessAttachments(serverEntity);
            ProcessEmbeds(serverEntity);
            ProcessReactions(serverEntity);

            _dbContext.Set<DiscordServer>().Add(serverEntity);
            _dbContext.SaveChanges();

            transaction.Commit();

            return _mapper.Map<DiscordServerDto>(serverEntity);
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    private void ProcessGuild(DiscordServer serverEntity)
    {
        var existingGuild = _dbContext.Set<Guild>().FirstOrDefault(g => g.Id == serverEntity.Guild.Id);
        if (existingGuild != null)
        {
            // Update existing guild if necessary
            existingGuild.Name = serverEntity.Guild.Name;
            existingGuild.IconUrl = serverEntity.Guild.IconUrl;
            serverEntity.Guild = existingGuild;
        }
        else
        {
            // Add new guild
            _dbContext.Set<Guild>().Add(serverEntity.Guild);
        }
    }

    private void ProcessUsers(DiscordServer serverEntity)
    {
        var processedUsers = new Dictionary<string, DiscordUser>();

        foreach (var message in serverEntity.Messages)
        {
            if (message.Author != null)
            {
                message.Author = GetOrCreateUser(message.Author, processedUsers);
                message.AuthorId = message.Author.Id;
            }

            if (message.Mentions != null)
            {
                message.Mentions = message.Mentions.Select(u => GetOrCreateUser(u, processedUsers)).ToList();
            }
        }
    }

    private DiscordUser GetOrCreateUser(DiscordUser user, Dictionary<string, DiscordUser> processedUsers)
    {
        if (user == null) return null;

        if (string.IsNullOrEmpty(user.Id))
        {
            user.Id = Guid.NewGuid().ToString();
        }

        if (processedUsers.TryGetValue(user.Id, out var existingUser))
        {
            return existingUser;
        }

        var dbUser = _dbContext.Set<DiscordUser>().FirstOrDefault(u => u.Id == user.Id);
        if (dbUser == null)
        {
            _dbContext.Set<DiscordUser>().Add(user);
            processedUsers[user.Id] = user;
            return user;
        }

        processedUsers[user.Id] = dbUser;
        return dbUser;
    }


    private void ProcessAttachments(DiscordServer serverEntity)
    {
        foreach (var message in serverEntity.Messages)
        {
            if (message.Attachments != null)
            {
                for (int i = 0; i < message.Attachments.Count; i++)
                {
                    var attachment = message.Attachments[i];
                    if (string.IsNullOrEmpty(attachment.Id))
                    {
                        attachment.Id = Guid.NewGuid().ToString();
                    }
                }
            }
        }
    }

    private void ProcessEmbeds(DiscordServer serverEntity)
    {
        foreach (var message in serverEntity.Messages)
        {
            if (message.Embeds != null)
            {
                for (int i = 0; i < message.Embeds.Count; i++)
                {
                    var embed = message.Embeds[i];
                    embed.Id = Guid.NewGuid();
                    // No need to process EmbedAuthor as it's an owned type
                }
            }
        }
    }

    private void ProcessReactions(DiscordServer serverEntity)
    {
        foreach (var message in serverEntity.Messages)
        {
            if (message.Reactions != null)
            {
                for (int i = 0; i < message.Reactions.Count; i++)
                {
                    var reaction = message.Reactions[i];
                    if (reaction.Emoji != null && string.IsNullOrEmpty(reaction.Emoji.Id))
                    {
                        reaction.Emoji.Id = Guid.NewGuid().ToString();
                    }
                }
            }
        }
    }
}