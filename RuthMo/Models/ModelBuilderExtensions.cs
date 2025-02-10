using Microsoft.EntityFrameworkCore;
using RuthMo.Models;

namespace RuthMo.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData([
                new Author
                {
                    Id = 1,
                    FirstName = "Yashar",
                    LastName = "Sanaei",
                    NickName = "Batman",
                    Motivations = new List<Motivation>(),
                },
                new Author
                {
                    Id = 2,
                    FirstName = "Arsham",
                    LastName = "Sanaei",
                    NickName = "Nightwing",
                    Motivations = new List<Motivation>(),
                }
            ]);

            modelBuilder.Entity<Motivation>().HasData([
                new Motivation { Id = 1, Content = "Hello there! 1", AuthorId = 1 },
                new Motivation { Id = 2, Content = "Hello there! 2", AuthorId = 1 },
                new Motivation { Id = 3, Content = "Hello there! 3", AuthorId = 2 },
                new Motivation { Id = 4, Content = "Hello there! 4", AuthorId = 2 },
            ]);
        }
    }
}