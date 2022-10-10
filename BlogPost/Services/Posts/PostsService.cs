using BlogPost.Data;
using BlogPost.Models;
using BlogPost.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.Services.Posts
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext _context;

        public PostsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Post> GetAllPublished()
        {
            return _context.Posts
                .Where(p => p.StatusId == Enums.StatusesEnum.Published)
                .OrderByDescending(p => p.CreatedDate).Take(8).ToList();
        }
        
        public Post GetById(int id)
        {
            var post = _context.Posts.Include(p => p.Author).Include(p => p.Status)
              .FirstOrDefault(m => m.Id == id);

            return post;
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
