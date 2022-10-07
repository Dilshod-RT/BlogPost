using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogPost.Data;
using BlogPost.Services.Posts;

namespace BlogPost.Controllers
{
    public class PostController : Controller
    {
        private readonly PostsService _postsService;

        public PostController(ApplicationDbContext context)
        {
            _postsService = new PostsService(context);
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var posts = _postsService.GetAllPublished();

            return View(posts);
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postsService.GetById(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
