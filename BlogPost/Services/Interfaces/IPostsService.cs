namespace BlogPost.Services.Interfaces
{
    public interface IPostsService
    {
        public List<Post> GetAllPublished();
        public Post GetById(int id);
        public Post GetId(int id);
        public List<Post> GetByAuthorId(string authorId);
        public Post Create(Post post);
        public Post Edit(Post post);
        public Post Delete(Post post);
        public bool PostExists(int id);
    }
}