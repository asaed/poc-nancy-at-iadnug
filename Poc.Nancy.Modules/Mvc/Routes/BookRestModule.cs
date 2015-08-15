using System.Linq;
using ASaed.POC.Nancy.Modules.Data.Model;
using ASaed.POC.Nancy.Modules.Data.Repositories;
using Nancy;
using Nancy.ModelBinding;
using NLog;

namespace ASaed.POC.Nancy.Modules.Mvc.Routes
{
    public class BookRestModule : NancyModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public BookRestModule(IBookRepository repository)
            : base("/books")
        {
            //Examples of pattern matching 
            Get["/"] = parameters => repository.GetAll().OrderBy(x=>x.Id).ToList();





            Get["/{bookId:long}"] = parameters => repository.GetById(parameters.bookId);





            Get["/search/{author?}"] = parameters => {
                if (parameters.author.HasValue)
                    return repository.GetByAuthorName(parameters.author);
                else
                    return "You didn't pass an author name";
            };





            Get["/searchQuery"] = _ =>
            {
                dynamic author = this.Request.Query.author;
                if (author != null && ((string)author).Trim() != "")
                    return repository.GetByAuthorName(author);
                else
                    return "You didn't pass an author name";
            };



            
            
            //Model binding and returning Response 
            Post["/"] = _ => {
                Book postedBook = this.Bind<Book>();
                Logger.Debug(string.Format("[{0}] postedBook was {1}", this.Request.Url, postedBook));

                var newBookId = repository.Save(postedBook);
                return newBookId == null ? "Book was not saved" : "Book saved with id: " + newBookId;
            };





            Post["/better"] = _ => {
                var postedBook = this.Bind<Book>();
                Logger.Debug(string.Format("[{0}] postedBook was {1}", this.Request.Url, postedBook));

                var newBookId = repository.Save(postedBook);
                return newBookId == null ? HttpStatusCode.BadRequest : HttpStatusCode.Created;
            };





            Post["/best"] = _ =>
            {
                var postedBook = this.Bind<Book>();
                Logger.Debug(string.Format("[{0}] postedBook was {1}", this.Request.Url, postedBook));

                var newBookId = repository.Save(postedBook);
                return newBookId == null ?
                                    new Response() { StatusCode = HttpStatusCode.BadRequest} 
                                    : new Response() { StatusCode = HttpStatusCode.Created}
                                            .WithHeader("Location", "/books/"+newBookId);
            };





            /*
            Post["/best"] = _ =>
            {
                //just in case someone over posts data and include stuff like 'id' 
                var postedBook = this.Bind<Book>(b => b.Id);
                logger.Debug(string.Format("[{0}] postedBook was {1}", this.Request.Url, postedBook));

                var newBookId = _repository.Save(postedBook);
                return newBookId == null ?
                                    new Response() { StatusCode = HttpStatusCode.BadRequest }
                                    : new Response() { StatusCode = HttpStatusCode.Created }
                                            .WithHeader("Location", "/books/" + newBookId);
            };
            */

        }
    }
}
