using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace backend.Services
{
    public class ImageService : BaseFileContentService<Image, ImageDto>
    {

        public ImageService(IWebHostEnvironment env, ApplicationDbContext dbContext, IMapper mapper) : base(env, dbContext, mapper)
        {
            FolderName = "Images";
        }

    }
}
