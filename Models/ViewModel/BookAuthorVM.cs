using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class BookAuthorVM
    {
        public BookAuthor BookAuthor { get; set; }
        public Book Book { get; set; }
        public IEnumerable<BookAuthor> BookAuthorsList { get; set; }
        public IEnumerable<SelectListItem> AuthorList { get; set; }
    }
}
