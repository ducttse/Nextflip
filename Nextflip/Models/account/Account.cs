using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.account
{
    public class Account
    {
        public string userID { get; set; }
        public string userEmail { get; set; }
        public string roleName { get; set; }
        public string hashedPassword { get; set; }
        public string fullname { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string status { get; set; }
        public string pictureURL { get; set; }
        public string note { get; set; }

    }
}
