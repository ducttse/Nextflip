using Nextflip.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IAccountService
    {
        public bool RegisterAccount(string userEmail, string password, string fullname, DateTime dateOfBirth, string pictureURL);
        public bool ChangeProfile(string userID, string fullname, DateTime dateOfBirth, string pictureURL);
        public Account Login(string email, string password);
        public bool IsExistedEmail(string email);
        public bool ChangePassword(string userID, string password);
        public Account CheckGoogleLogin(string googleID);
        public Account GetProfile(string userID);
        public bool CheckWallet(string userID, double money);
        public bool PurchaseSubscription(string userID, double money);
    }
}