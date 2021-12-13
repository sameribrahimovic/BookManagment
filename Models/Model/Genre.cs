using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class Genre
    {
        [Key]
        public int Genre_Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Book Book { get; set; }
    }
}
