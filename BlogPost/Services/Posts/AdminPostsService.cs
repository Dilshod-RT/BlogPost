using BlogPost.Data;
using BlogPost.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.Services.Posts
{
    public class AdminPostsService : IAdminPostsService
    {
        private readonly ApplicationDbContext _context;

        public AdminPostsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Post> GetForAdmin()
        {
            return _context.Posts.Include(p => p.Author).Include(p => p.Status)
                .Where(p => p.StatusId != Enums.StatusesEnum.Draft).ToList();
        }

        public Post Approve(Post post)
        {
            post.StatusId = Enums.StatusesEnum.Published;

            _context.Update(post);
            _context.SaveChanges();

            return post;
        }

        public Post Reject(Post post)
        {
            post.StatusId = Enums.StatusesEnum.Rejected;

            _context.Update(post);
            _context.SaveChanges();

            return post;
        }
    }
}
