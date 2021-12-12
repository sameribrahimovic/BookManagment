using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class Publisher
    {
        [Key]
        public int Publisher_Id { get; set; }

        [Required]
        public string Name { get; set; }

       

        public List<Book> Books { get; set; }
    }
}
