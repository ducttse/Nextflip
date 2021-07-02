using Nextflip.Models.account;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDAO _accountDAO;

        public AccountService(IAccountDAO accountDAO)
        {
            _accountDAO = accountDAO;
        }
        public bool ChangeProfile(string userID, string userEmail, string fullname, string dateOfBirth, string pictureURL) => _accountDAO.ChangeProfile(userID, userEmail, fullname, dateOfBirth, pictureURL);

        public Account GetAccountByID(string userID) => _accountDAO.GetAccountByID(userID);

        public bool Login(string email, string password) => _accountDAO.Login(email, password);
        public bool IsExistedEmail(string email) => _accountDAO.IsExistedEmail(email);

        public string RegisterAccount(string userEmail, string googleID, string googleEmail, string password, string fullname, string dateOfBirth) => _accountDAO.RegisterAnAccount(userEmail, googleID, googleEmail, password, fullname, dateOfBirth);
        public bool ChangePassword(string userID, string password) => _accountDAO.ChangePassword(userID, password);
    }
}