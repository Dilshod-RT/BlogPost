using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogPost.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlogPost.ViewModels;

namespace BlogPost.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User/Author
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var applicationDbContext = _context.Posts.Include(p => p.Author).Include(p => p.Status)
                .Where(p => p.AuthorId == currentUserId)
                .OrderByDescending(p => p.CreatedDate);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: User/Author/Details/5
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

        // GET: User/Author/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Author/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("Id,Title,Text,CreatedDate,AuthorId")]*/ PostCreateVM post, string submitbtn)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Post newPost = new()
                {
                    Title = post.Title,
                    Text = post.Text,
                    CreatedDate = DateTime.Now,
                    AuthorId = currentUserId
                };

                switch (submitbtn)
                {
                    case "Create as draft":
                        newPost.StatusId = Enums.StatusesEnum.Draft;
                        break;
                    case "Submit to check":
                        newPost.StatusId = Enums.StatusesEnum.WaitingForApproval;
                        break;
                }

                _context.Posts.Add(newPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET: User/Author/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.StatusId == Enums.StatusesEnum.WaitingForApproval)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: User/Author/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text,CreatedDate,AuthorId")] PostEditVM postVM, string submitbtn)
        {
            var post = await _context.Posts.FindAsync(id);

            if (id != postVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (post == null)
                    {
                        return NotFound();
                    }

                    post.Title = postVM.Title;
                    post.Text = postVM.Text;

                    switch (submitbtn)
                    {
                        case "Save as draft":
                            post.StatusId = Enums.StatusesEnum.Draft;
                            break;
                        case "Submit to check":
                            post.StatusId = Enums.StatusesEnum.WaitingForApproval;
                            break;
                    }

                    if (post.StatusId == Enums.StatusesEnum.WaitingForApproval)
                    {
                        return NotFound();
                    }
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(postVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: User/Author/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.StatusId == Enums.StatusesEnum.WaitingForApproval)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: User/Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return _context.Posts.Any(e => e.Id == id);
        }
    }
}
