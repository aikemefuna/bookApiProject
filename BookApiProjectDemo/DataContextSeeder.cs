using BookApiProjectDemo.Entities;
using BookApiProjectDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo
{
    public static class DataContextSeeder
    {
        public static void SeedData(this BookApiDbContext contex)
        {
            var booksAuthors = new List<BookAuthor>()
            {
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "12345",
                        Title = "Game Of Throne Nigeria",
                        DatePublished = new DateTime(2011,2,2),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category(){ Name = "Action"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { HeadLine = "Awesome Book", ReviewText = "The book is so nice and very Awesome", Rating = 5,Reviewer  = new Reviewer(){ FirstName = "John", LastName = "Wick"}},
                            new Review { HeadLine = " Terrible Book", ReviewText = " Dont waste your money on this book, its never like the Movie, Scam ", Rating = 1, Reviewer = new Reviewer(){ FirstName = "Adamu", LastName = "Muazu"}},
                            new Review { HeadLine = " Okay", ReviewText = " The book is okay for Purchase ", Rating = 5, Reviewer = new Reviewer(){ FirstName = "Mike", LastName = "Anthony"}},
                        }
                    },
                    Author = new Author()
                    {
                       FirstName = " Anthony",
                       LastName = "Ikemefuna",
                       Country = new Country()
                       {
                            Name = "Nigeria"
                       }
                    }
                },


                 new BookAuthor()
                 {
                    Book = new Book()
                    {
                        Isbn = "65789",
                        Title = "Falling Heroes",
                        DatePublished = new DateTime(1985,2,2),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category(){ Name = "History"}},
                            new BookCategory {Category = new Category(){ Name = " Education"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { HeadLine = "Made me Cry", ReviewText = "These book made me cry so hard, ohh Nigeria my Country", Rating = 5,Reviewer  = new Reviewer(){ FirstName = "Ifeanyi", LastName = "Gerald"}},
                            new Review { HeadLine = " Historical Indeed", ReviewText = " the book is very detailed with a lot of content ", Rating = 5, Reviewer = new Reviewer(){ FirstName = "Reetah", LastName = "Dolor"}},
                            new Review { HeadLine = " Okay", ReviewText = " The book is okay for Purchase ", Rating = 5, Reviewer = new Reviewer(){ FirstName = "Mike", LastName = "Anthony"}},
                        }
                    },
                    Author = new Author()
                    {
                       FirstName = " Reetah",
                       LastName = "Ikemefuna",
                       Country = new Country()
                       {
                            Name = "Nigeria"
                       }
                    }
                 },


                  new BookAuthor()
                  {
                    Book = new Book()
                    {
                        Isbn = "789654",
                        Title = "My Love My World",
                        DatePublished = new DateTime(2018,7,8),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category(){ Name = "Love"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { HeadLine = "Awesome Book", ReviewText = "The book is so nice and very Awesome", Rating = 5,Reviewer  = new Reviewer(){ FirstName = "Johnson", LastName = "May"}},
                            new Review { HeadLine = " Good Book", ReviewText = "Reading his made me want to love again", Rating = 3, Reviewer = new Reviewer(){ FirstName = "Anthony", LastName = "Ikemefuna"}},
                            new Review { HeadLine = " VeryGoodBook", ReviewText = " The book is very very good ", Rating = 3, Reviewer = new Reviewer(){ FirstName = "Monica", LastName = "Daniel"}},
                        }
                    },
                    Author = new Author()
                    {
                       FirstName = " Lucky",
                       LastName = "Dube",
                       Country = new Country()
                       {
                            Name = "Nigeria"
                       }
                    }
                  },


                  new BookAuthor()
                  {
                    Book = new Book()
                    {
                        Isbn = "74258",
                        Title = "Romance Sky",
                        DatePublished = new DateTime(2018,7,8),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category(){ Name = "Love"}},
                            new BookCategory { Category = new Category(){ Name = "Romance"}}                           
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { HeadLine = "Terrible book", ReviewText = "My wife made me read this book and i hated it", Rating = 1,Reviewer  = new Reviewer(){ FirstName = "Daniel", LastName = "Okoro"}},
                            new Review { HeadLine = " Good Book", ReviewText = "Reading this made me want to love again", Rating = 4, Reviewer = new Reviewer(){ FirstName = "John", LastName = "Dumelo"}},
                            new Review { HeadLine = " VeryGoodBook", ReviewText = " The book is very very good ", Rating = 5, Reviewer = new Reviewer(){ FirstName = "Monica", LastName = "Daniel"}},
                        }
                    },
                    Author = new Author()
                    {
                       FirstName = " Lucky",
                       LastName = "Dube",
                       Country = new Country()
                       {
                            Name = "Nigeria"
                       }
                    }
                  }
            };

            contex.AddRange(booksAuthors);
            contex.SaveChanges();
        }
    }
}
