using BlogPost.Data;
using BlogPost.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.Services.Posts
{
    public class BasePostsService : IBasePostsService
    {
        protected readonly ApplicationDbContext _context;

        public BasePostsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Post GetById(int id)
        {
            var post = _context.Posts.Include(p => p.Author).Include(p => p.Status)
              .FirstOrDefault(m => m.Id == id);

            return post;
        }
    }
}