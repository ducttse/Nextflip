using Nextflip.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IAccountService
    {
        public string RegisterAccount(string userEmail, string password, string fullname, DateTime dateOfBirth, string pictureURL);
        public bool ChangeProfile(string userID, string userEmail, string fullname, string dateOfBirth, string pictureURL);
        public Account Login(string email, string password);
        public bool IsExistedEmail(string email);
        public bool ChangePassword(string userID, string password);
        public Account CheckGoogleLogin(string googleID);
    }
}