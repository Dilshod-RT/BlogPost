namespace BlogPost.Services.Interfaces
{
    public interface IUserPostsService : IBasePostsService
    {
        Post GetId(int id);
        List<Post> GetByAuthorId(string authorId);
        Post Create(Post post);
        Post Edit(Post post);
        Post Delete(Post post);
        bool PostExists(int id);
    }
}