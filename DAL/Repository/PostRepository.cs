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
                    return context.Set<Post>().Include(p => p.Product).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Post> GetPostByCreateBy(int id)
        {
            return dbContext.Set<Post>().Include(p => p.Product).Where(x => x.CreateBy == id).ToList();
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
            var trackedEntity = dbContext.ChangeTracker.Entries<Post>()
                                  .FirstOrDefault(e => e.Entity.PostId == post.PostId);
            if (trackedEntity != null)
            {
                dbContext.Entry(trackedEntity.Entity).State = EntityState.Detached;
            }

            var existingPost = dbContext.Posts.FirstOrDefault(p => p.PostId == post.PostId);
            if (existingPost != null)
            {
                dbContext.Entry(existingPost).CurrentValues.SetValues(post);
            }
            else
            {
                dbContext.Posts.Add(post);
            }

            dbContext.SaveChanges();
        }


    }
}
