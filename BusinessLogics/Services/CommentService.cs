using BusinessObjects;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Services
{
    public interface ICommentService
    {
        public void Comment(Comment comment);
        public void DeleteComment(int id);
        public List<Comment> CommentInAPost(int id);
        public List<Comment> GetComments();
        public Comment GetComment(int id);
    }
    public class CommentService : ICommentService
    {
        private CommentRepository commentRepository;
        public CommentService()
        {
            commentRepository = new CommentRepository();
        }
        public void Comment(Comment comment) => commentRepository.commentInPost(comment);

        public void DeleteComment(int id) => commentRepository.deleteComment(id);

        public List<Comment> CommentInAPost(int id) => commentRepository.listCommentInAPost(id);
        public List<Comment> GetComments() => commentRepository.GetComments();
        public Comment GetComment(int id) => commentRepository.GetComment(id);

    }
}
