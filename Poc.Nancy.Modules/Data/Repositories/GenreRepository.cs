using System;
using System.Collections.Generic;
using System.Linq;
using ASaed.POC.Nancy.Modules.Data.Model;

namespace ASaed.POC.Nancy.Modules.Data.Repositories
{
    public interface IGenreRepository
    {
        IList<Genre> GetAll();
        Genre GetById(long id);
        IList<Genre> GetByName(String name);
        long? Save(Genre postedGenre);
    }

    public class GenreRepository : IGenreRepository
    {
        private IList<Genre> _data;

        public GenreRepository()
        {
            long id = 1;

            _data = new List<Genre>()
            {
                new Genre(){
                    Id = id++,
                    Name="Science Fiction", 
                    NumberOfAvailableTitles=51
                },
                new Genre(){
                    Id = id++,
                    Name="Romance", 
                    NumberOfAvailableTitles=19
                },
                new Genre(){
                    Id = id++,
                    Name="Mystery", 
                    NumberOfAvailableTitles=94
                },
                new Genre(){
                    Id = id++,
                    Name="Drama", 
                    NumberOfAvailableTitles=22
                },
                new Genre(){
                    Id = id++,
                    Name="Poetry", 
                    NumberOfAvailableTitles=64 
                },
                new Genre(){
                    Id = id++,
                    Name="Comics", 
                    NumberOfAvailableTitles=15 
                },
                new Genre(){
                    Id = id++,
                    Name="Horror", 
                    NumberOfAvailableTitles=20
                }
            };
        }

        public IList<Genre> GetAll()
        {
            return _data.ToList();
        }

        public Genre GetById(long id)
        {
            return _data.SingleOrDefault(x => x.Id == id);
        }

        public IList<Genre> GetByName(string name)
        {
            return _data.Where(a => a.Name.ToLower().Contains(name.ToLower()))
                                    .ToList();
        }

        public long? Save(Genre postedGenre)
        {
            if (postedGenre!=null && postedGenre.IsValid())
            {
                postedGenre.Normalize();
                postedGenre.Id = _data.Max(x => x.Id) + 1;
                _data.Add(postedGenre);
                return postedGenre.Id;
            }
            return null;
        }
    }
}
