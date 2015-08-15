using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ASaed.POC.Nancy.Modules.Data.Model;
using ASaed.POC.Nancy.Modules.Data.Repositories;
using Nancy;
using Nancy.ModelBinding;
using NLog;

namespace ASaed.POC.Nancy.Modules.Mvc.Routes
{
    public class GenreRestfulViewModule : NancyModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public GenreRestfulViewModule(IGenreRepository repository)
            : base("/genres")
        {

            Get["/"] = _ =>
            {
                var allGenres = repository.GetAll();

                dynamic model = new ExpandoObject();
                model.HasGenres = allGenres.Any();
                model.Genres = allGenres;

                //return View["genres/all", model];
                return Negotiate
                    .WithView("genres/all")
                    .WithModel((object) model)
                    .WithMediaRangeModel("application/json", allGenres)
                    .WithMediaRangeModel("application/xml", allGenres);
            };





            Get["/search"] = _ =>
            {
                dynamic name = this.Request.Query.name;

                IList<Genre> foundGenres = null;
                if (name.HasValue && ((string)name).Trim() != "")
                {
                    foundGenres = repository.GetByName(name); 
                    
                }
                else
                {
                    foundGenres = repository.GetAll();
                }

                dynamic model = new ExpandoObject();
                model.HasGenres = foundGenres.Any();
                model.Genres = foundGenres;

                //return View["genres/search", model];
                return Negotiate
                 .WithView("genres/search")
                 .WithModel((object)model)
                 .WithMediaRangeModel("application/json", foundGenres)
                 .WithMediaRangeModel("application/xml", foundGenres);
            };





            Get["/addGenre"] = _ =>
            {
                dynamic model = new ExpandoObject();
                model.HasMessage = false;
                AddGenresToModel(model, repository.GetAll());
                return View["genres/addGenre", model];
            };





            Post["/"] = _ => {
                Genre postedGenre = this.Bind<Genre>();
                Logger.Debug(string.Format("[{0}] postedGenre was {1}", this.Request.Url, postedGenre));

                var newGenreId = repository.Save(postedGenre);
                dynamic model = new ExpandoObject();
                if (newGenreId == null)
                {
                    model.HasMessage = true;
                    model.Message = "Genre was not saved";
                    model.MessageStyle = "color:red;";
                }
                else
                {
                    model.HasMessage = true;
                    model.Message = "Genre was saved with Id " + newGenreId;
                    model.MessageStyle = "";
                }

                var currentGenres = repository.GetAll();
                AddGenresToModel(model, currentGenres);

                //return View["genres/addGenre", model];
                return Negotiate
                 .WithView("genres/addGenre")
                 .WithModel((object)model)
                 .WithHeader("Location", "genres/" + newGenreId)
                 .WithMediaRangeModel("application/json", new { })
                 .WithMediaRangeModel("application/xml", new { });
            };


        }

        private void AddGenresToModel(dynamic model, IList<Genre> genres)
        {
            model.HasGenres = genres.Any();
            model.Genres = genres;
        }
    }
}
