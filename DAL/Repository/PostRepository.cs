﻿using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PostRepository
    {
        public static BSADBContext dbContext = new BSADBContext();
        public List<Post> GetAllPost()
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<Post>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Post? GetPost(int id)
        {
            var post = dbContext.Posts.FirstOrDefault(x => x.PostId == id);
            return post;
        }

        public void CreatePost(Post post)
        {
            dbContext.Posts.Add(post);
            dbContext.SaveChanges();
        }

        public void DeletePost(int id) 
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    var post = context.Set<Post>().FirstOrDefault(x => x.PostId == id);
                    context.Set<Post>().Remove(post);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void UpdatePost(Post post)
        {
            dbContext.Posts.Add(post);
            dbContext.SaveChanges();
        }


    }
}
