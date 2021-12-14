using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class BookVM
    {
        public Book Book { get; set; }

        //to store publisher into dropdown list
        public IEnumerable<SelectListItem> PublisherList { get; set; }
    }
}
