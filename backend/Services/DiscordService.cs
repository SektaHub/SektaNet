using AutoMapper;
using backend.Models.Discord;
using backend;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using backend.Services;

public class DiscordService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ImageService _imageService;

    public DiscordService(ApplicationDbContext dbContext, IMapper mapper, ImageService imageService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _imageService = imageService;
    }

    public string GenerateChatJson(string channelId)
    {
        var server = _dbContext.DiscordChannelExports
            .Include(s => s.Guild)
            .Include(s => s.Channel)
            .Include(s => s.Messages)
                .ThenInclude(m => m.Author)
            .Include(s => s.Messages)
                .ThenInclude(m => m.Embeds)
            .Include(s => s.Messages)
                .ThenInclude(m => m.Attachments)
            .FirstOrDefault(s => s.Channel.Id == channelId);

        if (server == null)
        {
            return JsonConvert.SerializeObject(new { error = "Server not found" });
        }

        var chatData = new Dictionary<string, List<string>>();

        foreach (var message in server.Messages.OrderBy(m => m.TimeStamp))
        {
            // Skip messages with embeds or attachments
            if (message.Embeds.Any() || message.Attachments.Any())
            {
                continue;
            }

            // Adjust timestamp to local time (assuming server time is UTC)
            var localTimestamp = message.TimeStamp.ToLocalTime();

            // Adjust day boundary to 5 AM
            var dayBoundary = localTimestamp.Date.AddHours(5);
            if (localTimestamp.Hour < 5)
            {
                dayBoundary = dayBoundary.AddDays(-1);
            }

            var key = $"SERVER: {server.Guild.Name}\nCHANNEL: {server.Channel.Name}\nDATE: {dayBoundary:yyyy-MM}\n\n";

            if (!chatData.ContainsKey(key))
            {
                chatData[key] = new List<string>();
            }

            chatData[key].Add($"{message.Author.Name}:\n{message.Content}\n");
        }

        var result = new StringBuilder();
        foreach (var entry in chatData)
        {
            result.AppendLine("{");
            result.AppendLine(entry.Key);
            result.AppendLine(string.Join("\n", entry.Value));
            result.AppendLine("}");
            result.AppendLine();
        }

        return result.ToString();
    }

    public async Task<List<string>> GenerateChatJsonFiles(string channelId, string directory, int daysPerFile = 10000)
    {
        var fileNames = new List<string>();

        var server = await _dbContext.DiscordChannelExports
            .Include(s => s.Guild)
            .Include(s => s.Channel)
            .FirstOrDefaultAsync(s => s.Channel.Id == channelId);

        if (server == null)
        {
            throw new Exception("Server not found for the given channel ID");
        }

        var messagesQuery = _dbContext.DiscordMessages
            .Where(m => m.DiscordServerId == server.Id)
            .Where(m => (m.Embeds == null || !m.Embeds.Any()) && (m.Attachments == null || !m.Attachments.Any()))
            .OrderBy(m => m.TimeStamp)
            .Select(m => new
            {
                m.TimeStamp,
                AuthorName = m.Author.Name,
                m.Content
            });

        var chatData = new Dictionary<DateTime, List<string>>();

        await foreach (var batch in BatchAsync(messagesQuery, 20000))
        {
            foreach (var message in batch)
            {
                var localTimestamp = message.TimeStamp.ToLocalTime();
                var dayBoundary = localTimestamp.Date.AddHours(5);
                if (localTimestamp.Hour < 5)
                {
                    dayBoundary = dayBoundary.AddDays(-1);
                }

                var messageEntry = $"{message.AuthorName ?? "Unknown"}:\n{message.Content}\n";
                if (!chatData.ContainsKey(dayBoundary))
                {
                    chatData[dayBoundary] = new List<string>();
                }
                chatData[dayBoundary].Add(messageEntry);
            }
        }

        var sortedDays = chatData.OrderBy(kvp => kvp.Key).ToList();
        var dayChunks = sortedDays.Chunk(daysPerFile);

        foreach (var chunk in dayChunks)
        {
            var startDate = chunk.First().Key;
            var endDate = chunk.Last().Key;
            var fileName = $"ChatExport_{server.Guild.Name}_{server.Channel.Name}_{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.json";
            var filePath = Path.Combine(directory, fileName);

            var result = new StringBuilder();
            result.AppendLine("{");

            foreach (var day in chunk)
            {
                var key = $"SERVER: {server.Guild.Name}\nCHANNEL: {server.Channel.Name}\nDATE: {day.Key:yyyy-MM}\n\n";
                result.AppendLine($"\"{key}");
                result.AppendLine(string.Join("\n", day.Value));
                result.AppendLine("\",");
            }

            // Remove the last comma and close the JSON object
            if (result.Length > 2)
            {
                result.Length -= 3;
            }
            result.AppendLine("\n}");

            File.WriteAllText(filePath, result.ToString());
            fileNames.Add(fileName);
        }

        return fileNames;
    }

    public async Task<List<string>> GenerateChatJsonFilesWithAttachments(string channelId, string directory, bool includeAttachments=false, bool includeEmbeds=false, int daysPerFile = 10000)
    {
        var fileNames = new List<string>();
        var server = _dbContext.DiscordChannelExports
            .Include(s => s.Guild)
            .Include(s => s.Channel)
            .FirstOrDefault(s => s.Channel.Id == channelId);
        if (server == null)
        {
            throw new Exception("Server not found for the given channel ID");
        }

        var messagesQuery = _dbContext.DiscordMessages
         .Include(m => m.Author)
         .Where(m => m.DiscordServerId == server.Id);

        if (!includeAttachments)
        {
            messagesQuery = messagesQuery.Where(m => m.Attachments == null || !m.Attachments.Any());
        }
        if (!includeEmbeds)
        {
            messagesQuery = messagesQuery.Where(m => m.Embeds == null || !m.Embeds.Any());
        }
        var finalQuery = messagesQuery
            .OrderBy(m => m.TimeStamp)
            .Select(m => new
            {
                m.TimeStamp,
                m.Author,
                m.Content,
                m.Attachments,
                m.Embeds
            });

        var chatData = new Dictionary<DateTime, List<string>>();
        await foreach (var batch in BatchAsync(messagesQuery, 20000))
        {
            foreach (var message in batch)
            {
                var localTimestamp = message.TimeStamp.ToLocalTime();
                var dayBoundary = localTimestamp.Date.AddHours(5);
                if (localTimestamp.Hour < 5)
                {
                    dayBoundary = dayBoundary.AddDays(-1);
                }

                var messageContent = new StringBuilder();
                messageContent.AppendLine($"{message.Author!.Name ?? "Unknown"}:");

                if (message.Embeds == null || !message.Embeds.Any())
                {
                    if (!string.IsNullOrEmpty(message.Content))
                        messageContent.AppendLine(message.Content);
                }
                messageContent.Append("\n");

                // Process attachments
                if (includeAttachments)
                {
                    foreach (var attachment in message.Attachments)
                    {
                        var image = await _imageService.FindImageByOriginalSource(attachment.Url);
                        if (image != null && !string.IsNullOrEmpty(image.GeneratedCaption))
                        {
                            messageContent.AppendLine($"<Image>{image.GeneratedCaption}</Image>");
                        }
                        else
                        {
                            messageContent.AppendLine($"<Image>IMAGE_NOT_FOUND</Image>");
                        }
                    }
                }

                // Process embeds
                if (includeEmbeds)
                {
                    foreach (var embed in message.Embeds)
                    {
                        if (string.IsNullOrEmpty(embed.Title) || string.IsNullOrEmpty(embed.Description))
                            messageContent.AppendLine($"<Embed>EMBED_ERROR</Embed>");
                        else
                            messageContent.AppendLine($"<Embed>\n{embed.Title}\n{embed.Description}\n</Embed>");

                        if (embed.Thumbnail != null && !string.IsNullOrEmpty(embed.Thumbnail.Url))
                        {
                            var image = await _imageService.FindImageByOriginalSource(embed.Thumbnail.Url);
                            if (image != null && !string.IsNullOrEmpty(image.GeneratedCaption))
                            {
                                messageContent.AppendLine($"<Image>{image.GeneratedCaption}</Image>");
                            }
                            else if(includeAttachments)
                            {
                                messageContent.AppendLine($"<Image>IMAGE_NOT_FOUND</Image>");
                            }
                        }
                    }
                }
                
                if (!chatData.ContainsKey(dayBoundary))
                {
                    chatData[dayBoundary] = new List<string>();
                }
                chatData[dayBoundary].Add(messageContent.ToString());
            }
        }

        var sortedDays = chatData.OrderBy(kvp => kvp.Key).ToList();
        var dayChunks = sortedDays.Chunk(daysPerFile);
        foreach (var chunk in dayChunks)
        {
            var startDate = chunk.First().Key;
            var endDate = chunk.Last().Key;
            var fileName = $"ChatExport_{server.Guild.Name}_{server.Channel.Name}_{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.json";
            var filePath = Path.Combine(directory, fileName);
            var result = new StringBuilder();
            result.AppendLine("[");
            foreach (var day in chunk)
            {
                var key = $"Server: {server.Guild.Name}\nChannel: {server.Channel.Name}\nDate: {day.Key:yyyy-MM}\n\n";
                var content = new StringBuilder(key);
                foreach (var message in day.Value)
                {
                    content.Append(message);
                }

                // Standardize all newlines to '\n' and remove the final trailing newline
                string standardizedContent = content.ToString().Replace("\r\n", "\n").Replace("\r", "\n").TrimEnd('\n');

                result.AppendLine($"  {{\"text\": {JsonConvert.SerializeObject(standardizedContent)}}},");
            }
            // Remove the last comma and close the JSON array
            if (result.Length > 2)
            {
                result.Length -= 2;
            }
            result.AppendLine("\n]");
            File.WriteAllText(filePath, result.ToString());
            fileNames.Add(fileName);
        }
        return fileNames;
    }

    // Helper method to properly escape JSON string content
    private string JsonEncode(string content)
    {
        return JsonConvert.ToString(content).Trim('"');
    }

    public void CombineAndRandomizeJsonFiles(List<string> inputFiles, string outputFilePath)
    {
        var random = new Random();
        var allTexts = new List<string>();

        // Read all texts from input files
        foreach (var file in inputFiles)
        {
            string jsonContent = File.ReadAllText(file);
            var texts = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonContent);
            allTexts.AddRange(texts.Select(t => t["text"]));
        }

        // Randomize the order
        var randomizedTexts = allTexts.OrderBy(x => random.Next()).ToList();

        // Create the new JSON structure
        var randomizedJson = randomizedTexts.Select(text => new Dictionary<string, string> { { "text", text } }).ToList();

        // Write to the output file
        File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(randomizedJson, Formatting.Indented));
    }

    private static async IAsyncEnumerable<List<T>> BatchAsync<T>(IQueryable<T> query, int batchSize, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        int skip = 0;
        List<T> batch;
        do
        {
            batch = await query.Skip(skip).Take(batchSize).ToListAsync(cancellationToken);
            if (batch.Any())
            {
                yield return batch;
                skip += batchSize;
            }
        } while (batch.Count == batchSize);
    }

    public string GenerateAttachmentUrlsJson(string channelId)
    {
        var server = _dbContext.DiscordChannelExports
            .Include(s => s.Messages)
                .ThenInclude(m => m.Attachments)
            .FirstOrDefault(s => s.Channel.Id == channelId);

        if (server == null)
        {
            return JsonConvert.SerializeObject(new { error = "Server not found" });
        }

        var attachmentUrls = server.Messages
            .SelectMany(m => m.Attachments)
            .Select(a => a.Url)
            .ToList();

        return JsonConvert.SerializeObject(attachmentUrls, Formatting.Indented);
    }

    //UPSERT CODE BEGINS HERE

    public IQueryable<DiscordChannelExportDto> GetAll()
    {
        var entities = _dbContext.Set<DiscordChannelExport>();
        var dtos = _mapper.ProjectTo<DiscordChannelExportDto>(entities);
        return dtos;
    }

    public DiscordChannelExportDto GetDtoById(Guid id)
    {
        var entity = _dbContext.Set<DiscordChannelExport>().Find(id);

        if (entity == null)
        {
            throw new Exception($"DiscordServer with ID '{id}' not found.");
        }

        var dto = _mapper.Map<DiscordChannelExportDto>(entity);
        return dto;
    }

    private void ProcessMessages(DiscordChannelExport serverEntity)
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

    public DiscordChannelExportDto Create(DiscordChannelExportDto serverDto)
    {
        using var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            var serverEntity = _mapper.Map<DiscordChannelExport>(serverDto);
            serverEntity.Id = Guid.NewGuid();

            ProcessGuild(serverEntity);
            ProcessMessages(serverEntity);
            ProcessUsers(serverEntity);
            ProcessAttachments(serverEntity);
            ProcessEmbeds(serverEntity);
            ProcessReactions(serverEntity);

            _dbContext.Set<DiscordChannelExport>().Add(serverEntity);
            _dbContext.SaveChanges();

            transaction.Commit();

            return _mapper.Map<DiscordChannelExportDto>(serverEntity);
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    private void ProcessGuild(DiscordChannelExport serverEntity)
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

    private void ProcessUsers(DiscordChannelExport serverEntity)
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


    private void ProcessAttachments(DiscordChannelExport serverEntity)
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

    private void ProcessEmbeds(DiscordChannelExport serverEntity)
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

    private void ProcessReactions(DiscordChannelExport serverEntity)
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