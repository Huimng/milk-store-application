using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DAL.Repository
{
    public class CommentRepository
    {
        public static BSADBContext dbContext = new BSADBContext();

        public void commentInPost(Comment comment)
        {
            dbContext.Comments.Add(comment);
            dbContext.SaveChanges();
        }

        public void deleteComment(int id)
        {
            var comment = dbContext.Comments.FirstOrDefault(x => x.CommentId == id);
            if (comment == null)
                return;
            dbContext.Comments.Remove(comment);
            dbContext.SaveChanges();
        }

        public List<Comment> listCommentInAPost(int id)
        {
            return dbContext.Comments.Where(x => x.PostId == id).ToList();

        }

        public List<Comment> GetComments()
        {
            return dbContext.Comments.ToList();

        }

        public Comment? GetComment(int id)
        {
            var comment = dbContext.Comments.FirstOrDefault(x => x.CommentId == id);
            return comment;
        }
    }
}   
