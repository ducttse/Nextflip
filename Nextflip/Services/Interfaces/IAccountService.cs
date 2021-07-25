using Nextflip.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IAccountService
    {
        public string RegisterAccount(string userEmail, string password, string fullname, DateTime dateOfBirth, string pictureURL, string token);
        public bool ChangeProfile(string userID, string fullname, DateTime dateOfBirth, string pictureURL);
        public Account Login(string email, string password);
        public bool IsExistedEmail(string email);
        public bool ChangePassword(string userID, string password);
        public Account CheckGoogleLogin(string googleID);
        public Account GetProfile(string userID);
        public string ConfirmEmail(string userID, string token);
        public string ForgotPassword(string userEmail, string token);
    }
}