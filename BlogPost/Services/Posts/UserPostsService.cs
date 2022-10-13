using BlogPost.Data;
using BlogPost.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.Services.Posts
{
    public class UserPostsService : BasePostsService, IUserPostsService
    {
        public UserPostsService(ApplicationDbContext context) : base(context)
        {
        }
        
        public Post GetId(int id)
        {
            var post = _context.Posts.Find(id);

            return post;
        }

        public List<Post> GetByAuthorId(string authorId)
        {
            return _context.Posts.Include(p => p.Author).Include(p => p.Status)
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.CreatedDate).ToList();
        }

        public Post Create(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();

            return post;
        }

        public Post Edit(Post post)
        {
            _context.Update(post);
            _context.SaveChanges();

            return post;
        }

        public Post Delete(Post post)
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();

            return post;
        }

        public bool PostExists(int id)
        {
            var post = _context.Posts.Any(e => e.Id == id);

            return post;
        }
    }
}