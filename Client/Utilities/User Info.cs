using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities
{
    public static class User_Info
    {
        public static string GetUserName()
        {
            string environment = Files_And_Pathing.RemoveInvalidChars(Environment.UserName);
            if (string.IsNullOrWhiteSpace(environment))
            {
                string userdomainname = Files_And_Pathing.RemoveInvalidChars(Environment.UserDomainName);
                if (string.IsNullOrWhiteSpace(userdomainname))
                {
                    string principalname = Files_And_Pathing.RemoveInvalidChars(WindowsIdentity.GetCurrent().Name);
                    if (string.IsNullOrWhiteSpace(principalname)) return "Unknown User";
                    else return principalname;
                }
                else return userdomainname;
            }
            else return environment;
        }
    }
}
