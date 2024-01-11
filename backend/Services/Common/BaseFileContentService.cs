using AutoMapper;
using backend.Models.Common;
using Xabe.FFmpeg;

namespace backend.Services.Common
{
    public class BaseFileContentService<TEntity, TDto>
        where TEntity : BaseFileContentEntity
        where TDto : BaseFileContentDto
    {

        protected readonly IWebHostEnvironment _env;
        protected readonly IMapper _mapper;
        protected readonly ApplicationDbContext _dbContext;

        //Folder name where the files are stored
        protected string FolderName;

        public BaseFileContentService(IWebHostEnvironment env, ApplicationDbContext dbContext, IMapper mapper)
        {
            _env = env;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public string GetFilePath(Guid fileId)
        {
            var imageEntity = _dbContext.Set<TEntity>().FirstOrDefault(img => img.Id == fileId);

            if (imageEntity != null)
            {
                var folderPath = Path.Combine(_env.WebRootPath, FolderName);
                var fileName = $"{fileId}.{imageEntity.FileExtension}";
                return Path.Combine(folderPath, fileName);
            }
            else
            {
                throw new InvalidOperationException($"File with Id {fileId} not found in the database.");
            }
        }


        public async Task<string> SaveFile(IFormFile file, Guid id, string fileExtension)
        {
            var folderPath = Path.Combine(_env.WebRootPath, FolderName);
            var fileName = $"{id}.{fileExtension}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                await stream.FlushAsync();
            }

            // After this, the file should be fully written to disk
            return filePath;
        }

        public virtual void InitDirectories()
        {
            //Create directory if it does not exist
            var folderPath = Path.Combine(_env.WebRootPath, FolderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

        }

    }
}
