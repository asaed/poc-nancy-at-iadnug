using System;
using System.Collections.Generic;
using System.Linq;
using ASaed.POC.Nancy.Modules.Data.Model;

namespace ASaed.POC.Nancy.Modules.Data.Repositories
{
    public interface IAuthorRepository
    {
        IList<Author> GetAll();
        Author GetById(long id);
        IList<Author> GetByAuthorName(String authorName);
        long? Save(Author postedAuthor);
    }
    public class AuthorRepository : IAuthorRepository
    {
        private IList<Author> _data; 

        public AuthorRepository()
        {
            long id = 1;
            _data = new List<Author>()
            {
                new Author(){Id = id++, Name="J.D. Salinger", NumberOfPublications=5},
                new Author(){Id = id++, Name="George Orwell", NumberOfPublications=10},
                new Author(){Id = id++, Name="Joseph Heller", NumberOfPublications=6},
                new Author(){Id = id++, Name="Henry James", NumberOfPublications=8},
                new Author(){Id = id++, Name="H.P. Lovecraft", NumberOfPublications=15},
                new Author(){Id = id++, Name="Stephen King", NumberOfPublications=20},
                new Author(){Id = id++, Name="Alexandre Dumas", NumberOfPublications=25},
            };
        }

        public IList<Author> GetAll()
        {
            return _data.ToList();
        }

        public Author GetById(long id)
        {
            return _data.SingleOrDefault(x => x.Id == id);
        }


        public IList<Author> GetByAuthorName(String authorName)
        {
            return _data.Where(a => a.Name.ToLower().Contains(authorName.ToLower()))
                        .ToList();
        }

        public long? Save(Author postedAuthor)
        {
            if (postedAuthor != null && postedAuthor.IsValid())
            {
                postedAuthor.Normalize();
                postedAuthor.Id = _data.Max(x => x.Id) + 1;
                _data.Add(postedAuthor);
                return postedAuthor.Id;
            }
            return null;
        }

    }
}
