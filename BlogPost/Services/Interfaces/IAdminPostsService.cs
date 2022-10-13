namespace BlogPost.Services.Interfaces
{
    public interface IAdminPostsService : IBasePostsService
    {
        List<Post> GetForAdmin();
        Post Approve(Post post);
        Post Reject(Post post);
    }
}