using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.account
{
    [Table("account")]
    public class Account
    {
        [Key]
        public string userID { get; set; }
        public string userEmail { get; set; }
        public string googleID { get; set; }
        public string googleEmail { get; set; }
        public int roleID { get; set; }
        public string fullname { get; set; }
        [DataType(DataType.Date)]
        public DateTime dateOfBirth { get; set; }
        public string status { get; set; }
        public string pictureURL { get; set; }

    }
}
