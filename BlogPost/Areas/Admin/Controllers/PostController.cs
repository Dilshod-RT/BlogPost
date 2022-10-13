using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using BlogPost.Services.Interfaces;

namespace BlogPost.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PostController : Controller
    {
        private readonly IAdminPostsService _adminPostsService;

        public PostController(IAdminPostsService adminPostsService)
        {
            _adminPostsService = adminPostsService;
        }

        // GET: Admin/Post
        public async Task<IActionResult> Index()
        {
            var posts = _adminPostsService.GetForAdmin();

            return View(posts);
        }

        // GET: Admin/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _adminPostsService.GetById(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // Post: Admin/Post/Approve/5
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _adminPostsService.GetById(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            _adminPostsService.Approve(post);
            return Ok();
        }

        // Post: Admin/Post/Reject/5
        public async Task<IActionResult> Reject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _adminPostsService.GetById(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            _adminPostsService.Reject(post);
            return Ok();
        }
    }
}