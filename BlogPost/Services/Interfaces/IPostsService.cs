namespace BlogPost.Services.Interfaces
{
    public interface IPostsService
    {
        List<Post> GetAllPublished();
        Post GetById(int id);
        Post GetId(int id);
        List<Post> GetByAuthorId(string authorId);
        Post Create(Post post);
        Post Edit(Post post);
        Post Delete(Post post);
        bool PostExists(int id);
    }
}