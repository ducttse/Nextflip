using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IAccountService
    {
        public string RegisterAccount(string userEmail, string googleID, string googleEmail, string password, string fullname, string dateOfBirth);
        public bool ChangeProfile(string userID, string userEmail, string password, string fullname, string dateOfBirth, string pictureURL);
        public bool Login(string email, string password);
        public bool IsExistedEmail(string email);
    }
}
