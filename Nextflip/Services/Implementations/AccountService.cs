﻿using Nextflip.Models.account;
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
        public bool ChangeProfile(string userID, string fullname, DateTime dateOfBirth, string pictureURL) => _accountDAO.ChangeProfile(userID, fullname, dateOfBirth, pictureURL);

        public Account GetAccountByID(string userID) => _accountDAO.GetAccountByID(userID);

        public Account Login(string email, string password) => _accountDAO.Login(email, password);
        public bool IsExistedEmail(string email) => _accountDAO.IsExistedEmail(email);

        public bool RegisterAccount(string userEmail, string password, string fullname, DateTime dateOfBirth, string pictureURL) => _accountDAO.RegisterAnAccount(userEmail, password, fullname, dateOfBirth, pictureURL);
        public bool ChangePassword(string userID, string password) => _accountDAO.ChangePassword(userID, password);
        public Account CheckGoogleLogin(string googleID) => _accountDAO.CheckGoogleLogin(googleID);
        public Account GetProfile(string userID) => _accountDAO.GetAccountByID(userID);
    }
}