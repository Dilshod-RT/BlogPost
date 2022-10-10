namespace BlogPost.Services.Interfaces
{
    public interface IAdminPostsService
    {
        public List<Post> GetForAdmin();
        public Post Approve(Post post);
        public Post Reject(Post post);
    }
}