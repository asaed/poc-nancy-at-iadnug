using System;

namespace ASaed.POC.Nancy.Modules.Data.Model
{
    public class Book
    {
        public long Id { get; set; }
        public String AuthorName { get; set; }
        public String Title { get; set; }
        public long PublicationYear { get; set; }

        internal bool IsValid(){
            return this.AuthorName != null
                   && this.AuthorName.Trim() != ""
                   && this.Title != null
                   && this.Title.Trim() != "";
        }

        internal void Normalize()
        {
            this.AuthorName = this.AuthorName.Trim();
            this.Title = this.Title.Trim();
        }

        override public String ToString()
        {
            return string.Format("[Id={0}, Title: {1}, AuthorName: {2}]", 
                this.Id,
                this.Title, 
                this.AuthorName);
        }
    }
}