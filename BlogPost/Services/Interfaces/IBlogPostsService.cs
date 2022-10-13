namespace BlogPost.Services.Interfaces
{
    public interface IBlogPostsService : IBasePostsService
    {
        List<Post> GetAllPublished();
    }
}