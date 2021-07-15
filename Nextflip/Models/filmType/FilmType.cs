using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.filmType
{
    public class FilmType
    {
        public int TypeID { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
