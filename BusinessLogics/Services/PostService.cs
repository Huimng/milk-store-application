using BusinessObjects;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Services
{
    public interface IPostService
    {
        public List<Post> GetPosts();
        public Post GetPost(int id);
        public void AddPost(Post post);
        public void DeletePost(int id);
        public void UpdatePost(Post post);

    }
    public class PostService : IPostService
    {
        private PostRepository postRepository;
        public PostService() 
        {
            postRepository = new PostRepository();
        }
        public void AddPost(Post post) => postRepository.CreatePost(post);

        public void DeletePost(int id) => postRepository.DeletePost(id);

        public Post GetPost(int id) => postRepository.GetPost(id);

        public List<Post> GetPosts() => postRepository.GetAllPost();

        public void UpdatePost(Post post) => postRepository.UpdatePost(post);
    }
}