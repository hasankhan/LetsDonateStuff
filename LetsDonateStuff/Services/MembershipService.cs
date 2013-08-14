using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using LetsDonateStuff.DAL;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace LetsDonateStuff.Services
{
    public class MembershipService : IMembershipService
    {
        readonly MembershipProvider membershipProvider;
        readonly RoleProvider roleProvider;

        public MembershipService(MembershipProvider membershipProvider, RoleProvider roleProvider)
        {
            this.membershipProvider = membershipProvider;
            this.roleProvider = roleProvider;
        }

        public bool Login(string userName, string password, bool persistCookie)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public string CreateAccount(string userName, string password)
        {
            return WebSecurity.CreateAccount(userName, password);
        }

        public string CreateUserAndAccount(string userName, string password)
        {
            return WebSecurity.CreateUserAndAccount(userName, password);
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public ICollection<AuthenticationClientData> RegisteredClientData
        {
            get { return OAuthWebSecurity.RegisteredClientData; }
        }

        public string GetUserName(string providerName, string providerUserId)
        {
            return OAuthWebSecurity.GetUserName(providerName, providerUserId);
        }

        public bool HasLocalAccount(int userId)
        {
            return OAuthWebSecurity.HasLocalAccount(userId);
        }

        public AuthenticationResult VerifyAuthentication(string returnUrl)
        {
            return OAuthWebSecurity.VerifyAuthentication(returnUrl);
        }

        public bool OAuthLogin(string providerName, string providerUserId, bool createPersistentCookie)
        {
            return OAuthWebSecurity.Login(providerName, providerUserId, createPersistentCookie);
        }

        public void CreateOrUpdateAccount(string providerName, string providerUserId, string userName)
        {
            OAuthWebSecurity.CreateOrUpdateAccount(providerName, providerUserId, userName);
        }

        public string SerializeProviderUserId(string providerName, string providerUserId)
        {
            return OAuthWebSecurity.SerializeProviderUserId(providerName, providerUserId);
        }

        public AuthenticationClientData GetOAuthClientData(string providerName)
        {
            return OAuthWebSecurity.GetOAuthClientData(providerName);
        }

        public bool TryDeserializeProviderUserId(string data, out string providerName, out string providerUserId)
        {
            return OAuthWebSecurity.TryDeserializeProviderUserId(data, out providerName, out providerUserId);
        }

        public ICollection<OAuthAccount> GetAccountsFromUserName(string userName)
        {
            return OAuthWebSecurity.GetAccountsFromUserName(userName);
        }

        public bool DeleteAccount(string providerName, string providerUserId)
        {
            return DeleteAccount(providerName, providerUserId);
        }

        public void AddUserToRole(int userId, string role)
        {
            if (!roleProvider.RoleExists(role))
                roleProvider.CreateRole(role);

            roleProvider.AddUsersToRoles(new[] { userId.ToString() }, new[] { role });
        }

        //public MembershipCreateStatus CreateUser(string name, string email)
        //{
        //    if (String.IsNullOrEmpty(email))
        //        throw new ArgumentException("Value cannot be null or empty.", "email");

        //    User result = GetAndUpdate(repository =>
        //    {
        //        User user = this.GetUser(name);
        //        if (user == null)
        //        {
        //            user = new User() { Name = name, OpenId = openID };
        //            repository.AddUser(user);
        //        }
        //        return user;
        //    });

        //    MembershipCreateStatus status;
        //    membershipProvider.CreateUser(result.Id.ToString(), Guid.NewGuid().ToString(), email, null, null, true, null, out status);

        //    return status;
        //}

        //public User GetUser(string openID)
        //{
        //    return GetMembership(repository => repository.GetUser(openID));
        //}

        public MembershipUser GetUser(string username)
        {
            return membershipProvider.GetUser(username, true);
        }
    }
}