using System;

namespace ASaed.POC.Nancy.Modules.Data.Model
{
    public class Author
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public long NumberOfPublications { get; set; }

        internal bool IsValid()
        {
            return this.Name != null
                   && this.Name.Trim() != ""
                   && this.NumberOfPublications >= 0;
        }

        internal void Normalize()
        {
            this.Name = this.Name.Trim();
        }

        override public String ToString()
        {
            return string.Format("[Id={0}, Name: {1}, NumberOfPublications: {2}]",
                this.Id,
                this.Name,
                this.NumberOfPublications);
        }
    }
}