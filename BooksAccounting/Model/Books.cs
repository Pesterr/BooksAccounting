using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAccounting.Model
{
    internal class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public int YearPublished { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }
    }
}
