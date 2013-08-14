using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using LetsDonateStuff.DAL;
using Microsoft.Web.WebPages.OAuth;

namespace LetsDonateStuff.Services
{
    public interface IMembershipService
    {
        // WebSecurity
        bool Login(string userName, string password, bool persistCookie = false);
        string CreateAccount(string userName, string password);
        string CreateUserAndAccount(string userName, string password);
        int GetUserId(string userName);
        bool ChangePassword(string userName, string currentPassword, string newPassword);
        void Logout();

        // OAuthWebSecurity
        ICollection<AuthenticationClientData> RegisteredClientData { get; }
        string GetUserName(string providerName, string providerUserId);
        bool HasLocalAccount(int userId);
        ICollection<OAuthAccount> GetAccountsFromUserName(string userName);
        bool DeleteAccount(string providerName, string providerUserId);
        AuthenticationResult VerifyAuthentication(string returnUrl);
        bool OAuthLogin(string providerName, string providerUserId, bool createPersistentCookie);
        void CreateOrUpdateAccount(string providerName, string providerUserId, string userName);
        string SerializeProviderUserId(string providerName, string providerUserId);
        AuthenticationClientData GetOAuthClientData(string providerName);
        bool TryDeserializeProviderUserId(string data, out string providerName, out string providerUserId);

        // Other
        //MembershipCreateStatus CreateUser(string name, string email);
        void AddUserToRole(int userId, string role);
        MembershipUser GetUser(string username);
    }
}
