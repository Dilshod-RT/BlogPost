using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogPost.Data;
using Microsoft.AspNetCore.Authorization;

namespace BlogPost.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Post
        public async Task<IActionResult> Index()
        {
            var posts = _context.Posts.Include(p => p.Author).Include(p => p.Status)
                .Where(p => p.StatusId != Enums.StatusesEnum.Draft);
            return View(await posts.ToListAsync());
        }

        // GET: Admin/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // Post: Admin/Post/Approve/5
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            post.StatusId = Enums.StatusesEnum.Published;

            _context.Update(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Post: Admin/Post/Reject/5
        public async Task<IActionResult> Reject(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            post.StatusId = Enums.StatusesEnum.Rejected;

            _context.Update(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return _context.Posts.Any(e => e.Id == id);
        }
    }
}
