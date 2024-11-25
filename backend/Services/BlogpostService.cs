using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Services
{
    public class BlogpostService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public BlogpostService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Get all blogposts
        public async Task<IEnumerable<BlogpostResponse>> GetBlogpostsAsync()
        {
            var blogposts = await _dbContext.Blogposts.Include(bp => bp.Publisher).ToListAsync();
            return _mapper.Map<IEnumerable<BlogpostResponse>>(blogposts);
        }

        // Get a single blogpost by ID
        public async Task<BlogpostResponse> GetBlogpostAsync(Guid id)
        {
            var blogpost = await _dbContext.Blogposts.Include(bp => bp.Publisher).FirstOrDefaultAsync(bp => bp.Id == id);
            return _mapper.Map<BlogpostResponse>(blogpost);
        }

        // Create a new blogpost
        public async Task<BlogpostResponse> CreateBlogpostAsync(HttpContext httpContext, BlogpostRequest blogpostRequest)
        {
            string? currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var blogpost = _mapper.Map<Blogpost>(blogpostRequest);
            blogpost.Id = Guid.NewGuid();
            blogpost.DateCreated = DateTime.UtcNow;
            if (currentUserId != null) { 
                blogpost.PublisherId = currentUserId;
            }

            _dbContext.Blogposts.Add(blogpost);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BlogpostResponse>(blogpost);
        }


    }
}
