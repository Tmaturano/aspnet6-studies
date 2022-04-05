using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    internal class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.LastUpdateDate)
                .IsRequired()
                .HasColumnName("LastUpdateDate")
                .HasColumnType("SMALLDATETIME")
                //.HasDefaultValueSql("GETDATE()"); //sql server function
                .HasDefaultValue(DateTime.Now.ToUniversalTime());

            builder
                .HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            // Relationship
            builder.HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Author")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Category")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag", //table name
                    role => role
                        .HasOne<Tag>() // one tag
                        .WithMany() // with many posts
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_TagPost_TagId")
                        .OnDelete(DeleteBehavior.Cascade),
                    user => user
                        .HasOne<Post>() // one post
                        .WithMany() // with many tags
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_TagPost_PostId")
                        .OnDelete(DeleteBehavior.Cascade));
        }
    }
}
