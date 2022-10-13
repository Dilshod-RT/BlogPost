using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BlogPost.Services.Interfaces;

namespace BlogPost.Controllers
{
    public class PostController : Controller
    {
        private readonly IBlogPostsService _blogPostsService;

        public PostController(IBlogPostsService blogPostsService)
        {
            _blogPostsService = blogPostsService;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var posts = _blogPostsService.GetAllPublished();

            return View(posts);
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _blogPostsService.GetById(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}