using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlogPost.ViewModels;
using BlogPost.Services.Interfaces;

namespace BlogPost.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class PostController : Controller
    {
        private readonly IUserPostsService _userPostsService;

        public PostController(IUserPostsService userPostsService)
        {
            _userPostsService = userPostsService;
        }

        // GET: User/Author
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = _userPostsService.GetByAuthorId(currentUserId);

            return View(post);
        }

        // GET: User/Author/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _userPostsService.GetById(id.Value);

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
                _userPostsService.Create(newPost); 
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: User/Author/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _userPostsService.GetById(id.Value);

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
            var post = _userPostsService.GetById(id);

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

                    _userPostsService.Edit(post);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userPostsService.PostExists(postVM.Id))
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
            if (id == null)
            {
                return NotFound();
            }

            var post = _userPostsService.GetById(id.Value);

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
            var post = _userPostsService.GetById(id);

            if (post != null)
            {
                _userPostsService.Delete(post);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}