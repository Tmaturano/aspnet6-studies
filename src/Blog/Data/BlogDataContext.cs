using Blog.Data.Mappings;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PostWithTagsCount> PostWithTagsCount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
            => options.UseSqlServer("Server=localhost,1433;Database=Blog;User ID=sa;Password=Abcd1234!");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new PostMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new TagMap());
            modelBuilder.ApplyConfiguration(new UserMap());

            // Mapping queries and Views
            modelBuilder.Entity<PostWithTagsCount>(x =>
            {
                x.HasNoKey();
                x.ToSqlQuery(@"SELECT 
                                [Title] AS [Name], 
                                (SELECT COUNT([PostId]) 
                                FROM [PostTag] 
                                WHERE [PostId] = [Id]) AS [Count]
                            FROM [Post]");
            });
        }
    }
}
