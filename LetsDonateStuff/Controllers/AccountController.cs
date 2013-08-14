using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using LetsDonateStuff.Models;
using LetsDonateStuff.Services;
using System.Web.Configuration;
using LetsDonateStuff.Configuration;
using LetsDonateStuff.DAL;
using WebMatrix.WebData;
using Microsoft.Web.WebPages.OAuth;
using System.Transactions;
using LetsDonateStuff.Models.Account;
using DotNetOpenAuth.AspNet;

namespace LetsDonateStuff.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IMembershipService membershipService;
        string adminOpenId;

        public AccountController(IMembershipService membershipService, AppSettings settings)
        {
            this.membershipService = membershipService;
            this.adminOpenId = settings.AdminOpenId;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && membershipService.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                return RedirectToLocal(returnUrl);

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            membershipService.Logout();

            return RedirectToAction("Index", "Post");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    membershipService.CreateUserAndAccount(model.UserName, model.Password);
                    membershipService.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Post");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = membershipService.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = membershipService.HasLocalAccount(membershipService.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || membershipService.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        membershipService.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = membershipService.HasLocalAccount(membershipService.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = membershipService.HasLocalAccount(membershipService.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = membershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        membershipService.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = membershipService.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (membershipService.OAuthLogin(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                membershipService.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = membershipService.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = membershipService.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !membershipService.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (var context = new DonationsContext())
                {
                    MembershipUser user = membershipService.GetUser(model.UserName);
                    // Check if user already exists
                    if (user == null)
                    {
                        membershipService.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        membershipService.OAuthLogin(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = membershipService.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", membershipService.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            var accounts = membershipService.GetAccountsFromUserName(User.Identity.Name);
            var externalLogins = new List<ExternalLoginModel>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = membershipService.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLoginModel
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || membershipService.HasLocalAccount(membershipService.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        //[ValidateInput(false)]
        //public ActionResult LogOn(LogOnModel model)
        //{
        //    var response = openId.GetResponse();
        //    if (response == null)
        //        return RelayAuthentication(model);

        //    return HandleOpenIdResponse(response);
        //}       

        //public ActionResult LogOff()
        //{
        //    Session.Abandon();
        //    FormsAuthentication.SignOut();

        //    return RedirectToAction("Index", "Post");
        //}

        //ActionResult RelayAuthentication(LogOnModel model)
        //{
        //    //Let us submit the request to OpenID provider
        //    Identifier id;
        //    if (Identifier.TryParse(model.openid_username, out id))
        //    {
        //        try
        //        {
        //            var request = openId.CreateRequest(id);

        //            var claimsRequest = new ClaimsRequest() { FullName = DemandLevel.Require, Email = DemandLevel.Require };
        //            request.AddExtension(claimsRequest);
        //            request.AddCallbackArguments("returnUrl", model.returnUrl ?? Url.Action("Index", "Post"));
        //            request.AddCallbackArguments("remember", model.RememberMe.ToString());

        //            return request.RedirectingResponse.AsActionResult();
        //        }
        //        catch (ProtocolException ex)
        //        {
        //            ViewBag.Message = ex.Message;
        //            return View("LogOn");
        //        }
        //    }

        //    ViewBag.Message = "Invalid identifier";
        //    return View("LogOn");
        //}

        //ActionResult HandleOpenIdResponse(IAuthenticationResponse response)
        //{
        //    string returnUrl = response.GetCallbackArgument("returnUrl");
        //    bool rememberMe = Boolean.Parse(response.GetCallbackArgument("remember"));

        //    var claim = response.GetExtension<ClaimsResponse>();

        //    if (claim == null || String.IsNullOrEmpty(claim.Email))
        //    {
        //        ModelState.AddModelError(String.Empty, "Your openid provider did not return email. Please log in with a different service or configure your provider to return email information.");
        //        return View();
        //    }

        //    string fullName = claim.FullName;
        //    string email = claim.Email;

        //    switch (response.Status)
        //    {
        //        case AuthenticationStatus.Authenticated:
        //            string openId = response.ClaimedIdentifier;
        //            DAL.User user = membershipService.GetUser(openId);
        //            if (user == null)
        //            {
        //                MembershipCreateStatus status = CreateUser(fullName, email, openId, out user);
        //                if (status != MembershipCreateStatus.Success)
        //                {
        //                    ModelState.AddModelError(String.Empty, AccountValidation.ErrorCodeToString(status));
        //                    return View();
        //                }
        //            }
        //            authenticationService.SignIn(user.Membership.UserName, rememberMe);
        //            return Redirect(returnUrl);

        //        case AuthenticationStatus.Canceled:
        //            ViewBag.Message = "Canceled at provider";
        //            return View();
        //        case AuthenticationStatus.Failed:
        //            ViewBag.Message = response.Exception.Message;
        //            return View();
        //    }

        //    return new EmptyResult();
        //}

        //MembershipCreateStatus CreateUser(string fullName, string email, string openId, out DAL.User user)
        //{
        //    user = null;
        //    MembershipCreateStatus status = membershipService.CreateUser(fullName, email, openId);
        //    if (status == MembershipCreateStatus.Success)
        //    {
        //        user = membershipService.GetUser(openId);
        //        if (openId.Equals(adminOpenId, StringComparison.InvariantCultureIgnoreCase))
        //            membershipService.AddUserToRole(user.Id, UserRoles.Admin);
        //    }
        //    return status;
        //}

        ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Post");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}
