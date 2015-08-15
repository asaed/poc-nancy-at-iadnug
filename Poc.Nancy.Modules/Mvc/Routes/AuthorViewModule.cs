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
    public class AuthorViewModule : NancyModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AuthorViewModule(IAuthorRepository repository)
            : base("/authors")
        {

            //redirects
            Get["/"] = _ => Response.AsRedirect("~/homeWithStylishMaster");





            //dynamic view with expando object 
            Get["/all"] = _ => {
                var allAuthors = repository.GetAll();

                dynamic model = new ExpandoObject();
                model.HasAuthors = allAuthors.Any();
                model.Authors = allAuthors;

                return View["authors/all", model];
            };






            //dynamic view with partials
            Get["/search"] = _ =>
            {
                dynamic authorName = this.Request.Query.authorName;

                IList<Author> foundAuthors = null;
                if (authorName.HasValue && ((string)authorName).Trim() != "")
                {
                    foundAuthors = repository.GetByAuthorName(authorName); 
                    
                }
                else
                {
                    foundAuthors = repository.GetAll();
                }

                dynamic model = new ExpandoObject();
                model.HasAuthors = foundAuthors.Any();
                model.Authors = foundAuthors;

                return View["authors/search", model];
            };






            //post data and bind model 
            Get["/addAuthor"] = _ =>
            {
                dynamic model = new ExpandoObject();
                model.HasMessage = false;
                AddAuthorsToModel(model, repository.GetAll());
                return View["authors/addAuthor", model];
            };





            Post["/addAuthor"] = _ => {
                Author postedAuthor = this.Bind<Author>();
                Logger.Debug(string.Format("[{0}] postedAuthor was {1}", this.Request.Url, postedAuthor));

                var newAuthorId = repository.Save(postedAuthor);
                dynamic model = new ExpandoObject();
                if (newAuthorId == null)
                {
                    model.HasMessage = true;
                    model.Message = "Author was not saved";
                    model.MessageStyle = "color:red;";
                }
                else
                {
                    model.HasMessage = true;
                    model.Message = "Author was saved with Id " + newAuthorId;
                    model.MessageStyle = "";
                }

                AddAuthorsToModel(model, repository.GetAll());

                return View["authors/addAuthor", model];
            };


        }

        private void AddAuthorsToModel(dynamic model, IList<Author> authors)
        {
            model.HasAuthors = authors.Any();
            model.Authors = authors;
        }
    }
}
