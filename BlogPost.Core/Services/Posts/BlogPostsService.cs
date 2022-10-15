using BlogPost.Data;
using BlogPost.Services.Interfaces;

namespace BlogPost.Services.Posts
{
    public class BlogPostsService : BasePostsService, IBlogPostsService
    {
        public BlogPostsService(ApplicationDbContext context) : base(context)
        {
        }

        public List<Post> GetAllPublished()
        {
            return _context.Posts
                .Where(p => p.StatusId == Enums.StatusesEnum.Published)
                .OrderByDescending(p => p.CreatedDate).Take(8).ToList();
        }
    }
}