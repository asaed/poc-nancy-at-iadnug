using System;
using System.Collections.Generic;
using System.Linq;
using ASaed.POC.Nancy.Modules.Data.Model;

namespace ASaed.POC.Nancy.Modules.Data.Repositories
{
    public interface IBookRepository
    {
        IList<Book> GetAll();
        Book GetById(long id);
        IList<Book> GetByTitle(String bookTitle);
        IList<Book> GetByAuthorName(string authorName);
        long? Save(Book postedBook);
    }

    public class BookRepository : IBookRepository
    {
        private IList<Book> _data;

        public BookRepository()
        {
            long id = 1;

            _data = new List<Book>()
            {
                new Book(){
                    Id = id++,
                    AuthorName="J.D. Salinger", 
                    Title="THE CATCHER IN THE RYE", 
                    PublicationYear=1951
                },
                new Book(){
                    Id = id++,
                    AuthorName="George Orwell", 
                    Title="1984",
                    PublicationYear=1949
                },
                new Book(){
                    Id = id++,
                    AuthorName="George Orwell", 
                    Title="ANIMAL FARM",
                    PublicationYear=1945
                },
                new Book(){
                    Id = id++,
                    AuthorName="Joseph Heller", 
                    Title="CATCH-22",
                    PublicationYear=1961
                },
                new Book(){
                    Id = id++,
                    AuthorName="Henry James", 
                    Title="THE WINGS OF THE DOVE",
                    PublicationYear=1902 
                },
                new Book(){
                    Id = id++,
                    AuthorName="Henry James", 
                    Title="THE AMBASSADORS",
                    PublicationYear=1903 
                },
                new Book(){
                    Id = id++,
                    AuthorName="Henry James", 
                    Title="THE GOLDEN BOWL",
                    PublicationYear=1904 
                },
                new Book(){
                    Id = id++,
                    AuthorName="H.P. Lovecraft", 
                    Title="AT THE MOUNTAINS OF MADNESS",
                    PublicationYear=1936
                },
                new Book(){
                    Id = id++,
                    AuthorName="Stephen King", 
                    Title="IT",
                    PublicationYear=1986
                },
                new Book(){
                    Id = id++,
                    AuthorName="Stephen King", 
                    Title="THE STAND",
                    PublicationYear=1978
                },
            };
        }

        public IList<Book> GetAll()
        {
            return _data.ToList();
        }

        public Book GetById(long id)
        {
            return _data.SingleOrDefault(x => x.Id == id);
        }

        public IList<Book> GetByTitle(String titleName)
        {
            return _data.Where(a => a.Title.ToLower().Contains(titleName.ToLower()))
                        .ToList();
        }

        public IList<Book> GetByAuthorName(string authorName)
        {
            return _data.Where(a => a.AuthorName.ToLower().Contains(authorName.ToLower()))
                                    .ToList();
        }

        public long? Save(Book postedBook)
        {
            if (postedBook!=null && postedBook.IsValid())
            {
                postedBook.Normalize();
                postedBook.Id = _data.Max(x => x.Id) + 1;
                _data.Add(postedBook);
                return postedBook.Id;
            }
            return null;
        }
    }
}
