using System;

namespace ASaed.POC.Nancy.Modules.Data.Model
{
    public class Genre
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public long NumberOfAvailableTitles { get; set; }

        internal bool IsValid(){
            return this.Name != null
                   && this.Name.Trim() != ""
                   && this.NumberOfAvailableTitles >= 0; 
        }

        internal void Normalize()
        {
            this.Name = this.Name.Trim();
        }

        override public String ToString()
        {
            return string.Format("[Id={0}, Name: {1}, NumberOfAvailableTitles: {2}]", 
                this.Id,
                this.Name,
                this.NumberOfAvailableTitles);
        }
    }
}