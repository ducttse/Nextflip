using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nextflip.utils
{
    public static class EmailUtil
    {
        public static bool IsValidEmail(string email)
        {
            bool isEmail = false;
            try
            {
                string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
                isEmail = Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isEmail;
        }
    }
}
