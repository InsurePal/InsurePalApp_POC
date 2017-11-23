using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SupportFriends.Filters;
using SupportFriends.Models;
using SupportFriends.Models.DAL;
using SupportFriends.Code.Enumerators;
using SupportFriends.Code;

namespace SupportFriends.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.ID = RouteData.Values["id"];
                ViewBag.type = RouteData.Values["type"];
                ViewBag.betStatus = Request.QueryString["betStatus"];
                return View();
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    Guid id = Guid.Empty;

                    //ČE JE POVABLJEN POBEREMO GUID
                    if (RouteData.Values["id"] != null)
                    {
                        Guid.TryParse(RouteData.Values["id"].ToString(), out id);
                        ViewBag.ID = id;
                    }

                    //UPDATEAMO BET
                    if (id != Guid.Empty)
                    {
                        IBetDataRepository _repositoryB = new BetDataRepository();
                        int userID = WebSecurity.GetUserId(model.UserName);

                        BetData bet = new BetData();
                        bet.Guid = id;
                        bet.BetActionID = Convert.ToInt16(RouteData.Values["type"]);

                        if (bet.BetActionID == 115)
                            bet.User1ID = userID;
                        else
                            bet.User2ID = userID;

                        bet.BetStatusID = Request.QueryString["betStatus"].ToEnum<BetStatus>();
                        _repositoryB.Update(bet);

                        //ZAPIŠEMO HISTORY!!!
                        new EventData(bet.Guid, userID, bet.BetStatusID == BetStatus.Accepted ? 202 : 206).Add();

                        //redirect
                        if (returnUrl != null)
                        {
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }

                    //return RedirectToAction("Index", "Bet");
                    return RedirectToAction("Support-Me", "Bet");
                }

                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                WebSecurity.Logout();

                return Redirect("~/");
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            try
            {
                Guid id = Guid.Empty;

                //ČE JE POVABLJEN POBEREMO GUID
                if (RouteData.Values["id"] != null)
                {
                    Guid.TryParse(RouteData.Values["id"].ToString(), out id);
                    ViewBag.ID = id;
                    ViewBag.type = RouteData.Values["type"];
                    ViewBag.betStatus = Request.QueryString["betStatus"];
                }

                return View();
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            try
            {
                Guid id = Guid.Empty;

                //ČE JE POVABLJEN POBEREMO GUID
                if (RouteData.Values["id"] != null)
                {
                    Guid.TryParse(RouteData.Values["id"].ToString(), out id);
                    ViewBag.ID = id;
                }

                if (ModelState.IsValid)
                {
                    // Attempt to register the user
                    try
                    {
                        if (String.IsNullOrEmpty(model.UserName))
                            model.UserName = model.Email;

                        if (!WebSecurity.UserExists(model.UserName))
                            WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Email = model.Email, FullName = !String.IsNullOrEmpty(model.FullName) ? model.FullName : model.UserName });
                        else
                        {
                            WebSecurity.CreateAccount(model.UserName, model.Password);
                        }

                        WebSecurity.Login(model.UserName, model.Password);

                        //POVEŽEMO PRIJATELJA
                        //if(id != Guid.Empty)
                        //{
                        //    IUserDataRepository _repository = new UserDataRepository();
                        //    int userID = WebSecurity.GetUserId(model.UserName);
                        //    _repository.AddFriend(userID, id);
                        //}

                        //UPDATEAMO BET
                        if (id != Guid.Empty)
                        {
                            IBetDataRepository _repositoryB = new BetDataRepository();
                            int userID = WebSecurity.GetUserId(model.UserName);
                            //if (userID == -1)
                            //{
                            //    IUserDataRepository _repositoryU = new UserDataRepository();

                            //    UserData user = new UserData();
                            //    user = _repositoryU.FindByUsername(User.Identity.Name);
                            //    userID = user.UserID;
                            //}

                            BetData bet = new BetData();
                            bet.Guid = id;
                            bet.BetActionID = Convert.ToInt16(RouteData.Values["type"]);

                            if (bet.BetActionID == 115)
                                bet.User1ID = userID;
                            else
                                bet.User2ID = userID;

                            bet.BetStatusID = Request.QueryString["betStatus"].ToEnum<BetStatus>();
                            _repositoryB.Update(bet);

                            //ZAPIŠEMO HISTORY!!!
                            new EventData(bet.Guid, userID, bet.BetStatusID == BetStatus.Accepted ? 202 : 206).Add();

                            return RedirectToAction("Index", "Dashboard");
                        }

                        //return RedirectToAction("Index", "Bet");
                        return RedirectToAction("Support-Me", "Bet");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            try
            {
                string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
                ManageMessageId? message = null;

                // Only disassociate the account if the currently logged in user is the owner
                if (ownerAccount == User.Identity.Name)
                {
                    // Use a transaction to prevent the user from deleting their last login credential
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                    {
                        bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                        if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                        {
                            OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                            scope.Complete();
                            message = ManageMessageId.RemoveLoginSuccess;
                        }
                    }
                }

                return RedirectToAction("Manage", new { Message = message });
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            try
            {
                ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
                ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                ViewBag.ReturnUrl = Url.Action("Manage");
                return View();
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            try
            {
                bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
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
                            changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
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
                            WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
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
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            try
            {
                return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl, id = RouteData.Values["id"], type = RouteData.Values["type"], betStatus = Request.QueryString["betStatus"] }));
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            try
            {
                int tmpUserID = -1;

                AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
                if (!result.IsSuccessful)
                {
                    TempData["Error"] = "Error description: " + result.Error + " " + result.IsSuccessful.ToString();
                    return RedirectToAction("ExternalLoginFailure");
                }

                if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
                {
                    //return RedirectToLocal(returnUrl);
                    tmpUserID = WebSecurity.GetUserId(result.UserName);
                }

                if (User.Identity.IsAuthenticated)
                {
                    // If the current user is logged in add the new account
                    OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                    //return RedirectToLocal(returnUrl);
                    tmpUserID = WebSecurity.GetUserId(result.UserName);
                }
                else
                {
                    // User is new, ask for their desired membership name
                    string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                    ViewBag.ReturnUrl = returnUrl;
                    //return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });

                    RegisterExternalLoginModel model = new RegisterExternalLoginModel { UserName = result.UserName, Email = result.ExtraData["email"], FullName = result.ExtraData["name"], ExternalLoginData = loginData };
                    string provider = null;
                    string providerUserId = null;

                    if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
                    {
                        return RedirectToAction("Manage");
                    }

                    if (ModelState.IsValid)
                    {
                        //Insert a new user into the database
                        using (UsersContext db = new UsersContext())
                        {
                            UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                            // Check if user already exists
                            if (user == null)
                            {
                                // Insert name into the profile table
                                //GREGA HACKS -- > model.username v providerUserID
                                //db.UserProfiles.Add(new UserProfile { UserName = model.UserName, UserGuid = Guid.NewGuid(), FullName = model.UserName });
                                db.UserProfiles.Add(new UserProfile { UserName = model.UserName, UserGuid = Guid.NewGuid(), FullName = model.FullName, Email = model.Email });
                                db.SaveChanges();

                                //GREGA HACKS -- > model.username v providerUserID
                                OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName); //???model.Email???
                                                                                                                  //OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, providerUserId);

                                OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                                tmpUserID = WebSecurity.GetUserId(result.UserName);
                            }
                            else
                            {
                                ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                            }
                        }
                    }
                }

                List<FacebookFriend> friendsList = FacebookDataHelper.GetFriends();
                IUserDataRepository _repositoryU = new UserDataRepository();
                _repositoryU.AddFBFriends(result.UserName, friendsList);

                if (tmpUserID != -1)
                {
                    Guid id = Guid.Empty;
                    if (RouteData.Values["id"] != null)
                    {
                        Guid.TryParse(RouteData.Values["id"].ToString(), out id);
                    }

                    //UPDATEAMO BET
                    if (id != Guid.Empty)
                    {
                        IBetDataRepository _repositoryB = new BetDataRepository();

                        BetData bet = new BetData();
                        bet.Guid = id;
                        bet.BetActionID = Convert.ToInt16(RouteData.Values["type"]);

                        if (bet.BetActionID == 115)
                            bet.User1ID = tmpUserID;
                        else
                            bet.User2ID = tmpUserID;

                        bet.BetStatusID = BetStatus.Accepted;  ///!!!!Request.QueryString["betStatus"].ToEnum<BetStatus>();
                        _repositoryB.Update(bet);

                        //ZAPIŠEMO HISTORY!!!
                        new EventData(bet.Guid, tmpUserID, bet.BetStatusID == BetStatus.Accepted ? 202 : 206).Add();

                        return RedirectToAction("Index", "Dashboard", new { popup = "typeCredit" + bet.BetID });
                    }

                    //return RedirectToAction("Index", "Bet");
                    return RedirectToAction("Support-Me", "Bet");
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return View();
                }
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            try
            {
                string provider = null;
                string providerUserId = null;

                if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
                {
                    return RedirectToAction("Manage");
                }

                if (ModelState.IsValid)
                {
                    // Insert a new user into the database
                    //using (UsersContext db = new UsersContext())
                    //{
                    //    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    //    // Check if user already exists
                    //    if (user == null)
                    //    {
                    //        // Insert name into the profile table
                    //        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                    //        db.SaveChanges();

                    //        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                    //        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                    //        return RedirectToLocal(returnUrl);
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    //    }
                    //}
                }

                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            try
            {
                ViewBag.Error = TempData["Error"];
                return View();
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.id = RouteData.Values["id"];
                ViewBag.type = RouteData.Values["type"];
                ViewBag.betStatus = Request.QueryString["betStatus"];
                ViewBag.Text = RouteData.Values["Text"];
                ViewBag.Button = RouteData.Values["Button"];
                return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            try
            {
                ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
                List<ExternalLogin> externalLogins = new List<ExternalLogin>();
                foreach (OAuthAccount account in accounts)
                {
                    AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                    externalLogins.Add(new ExternalLogin
                    {
                        Provider = account.Provider,
                        ProviderDisplayName = clientData.DisplayName,
                        ProviderUserId = account.ProviderUserId,
                    });
                }

                ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                return PartialView("_RemoveExternalLoginsPartial", externalLogins);
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("~/");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
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

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
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
        #endregion


        [AllowAnonymous]
        public ActionResult LoginPopup()
        {
            return PartialView("_LoginPopup");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPopup(LoginModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    Guid id = Guid.Empty;

                    //ČE JE POVABLJEN POBEREMO GUID
                    if (RouteData.Values["id"] != null)
                    {
                        Guid.TryParse(RouteData.Values["id"].ToString(), out id);
                        ViewBag.ID = id;
                    }

                    //UPDATEAMO BET
                    if (id != Guid.Empty)
                    {
                        IBetDataRepository _repositoryB = new BetDataRepository();
                        int userID = WebSecurity.GetUserId(model.UserName);

                        BetData bet = new BetData();
                        bet.Guid = id;
                        bet.BetActionID = Convert.ToInt16(RouteData.Values["type"]);

                        if (bet.BetActionID == 115)
                            bet.User1ID = userID;
                        else
                            bet.User2ID = userID;

                        bet.BetStatusID = Request.QueryString["betStatus"].ToEnum<BetStatus>();
                        _repositoryB.Update(bet);

                        //ZAPIŠEMO HISTORY!!!
                        new EventData(bet.Guid, userID, bet.BetStatusID == BetStatus.Accepted ? 202 : 206).Add();

                        //redirect
                        if (returnUrl != null)
                        {
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            //return RedirectToAction("Index", "Dashboard");
                            return RedirectToAction("Index", "Dashboard", new { popup = "typeCredit" + bet.BetID });
                        }
                    }

                    //return RedirectToAction("Index", "Bet");
                    //return RedirectToAction("Support-Me", "Bet");
                    return Redirect("/Bet/Support-Me");
                }

                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return PartialView("_LoginPopup", model);
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }

        [AllowAnonymous]
        public ActionResult RegisterPopup()
        {
            return PartialView("_RegisterPopup");
        }
        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterPopup(RegisterModel model)
        {
            try
            {
                Guid id = Guid.Empty;

                //ČE JE POVABLJEN POBEREMO GUID
                if (RouteData.Values["id"] != null)
                {
                    Guid.TryParse(RouteData.Values["id"].ToString(), out id);
                    ViewBag.ID = id;
                }

                if (ModelState.IsValid)
                {
                    // Attempt to register the user
                    try
                    {
                        if (String.IsNullOrEmpty(model.UserName))
                            model.UserName = model.Email;

                        if (!WebSecurity.UserExists(model.UserName))
                            WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Email = model.Email, FullName = !String.IsNullOrEmpty(model.FullName) ? model.FullName : model.UserName });
                        else
                        {
                            WebSecurity.CreateAccount(model.UserName, model.Password);
                        }

                        WebSecurity.Login(model.UserName, model.Password);

                        //POVEŽEMO PRIJATELJA
                        //if(id != Guid.Empty)
                        //{
                        //    IUserDataRepository _repository = new UserDataRepository();
                        //    int userID = WebSecurity.GetUserId(model.UserName);
                        //    _repository.AddFriend(userID, id);
                        //}

                        //UPDATEAMO BET
                        if (id != Guid.Empty)
                        {
                            IBetDataRepository _repositoryB = new BetDataRepository();
                            int userID = WebSecurity.GetUserId(model.UserName);
                            //if (userID == -1)
                            //{
                            //    IUserDataRepository _repositoryU = new UserDataRepository();

                            //    UserData user = new UserData();
                            //    user = _repositoryU.FindByUsername(User.Identity.Name);
                            //    userID = user.UserID;
                            //}

                            BetData bet = new BetData();
                            bet.Guid = id;
                            bet.BetActionID = Convert.ToInt16(RouteData.Values["type"]);

                            if (bet.BetActionID == 115)
                                bet.User1ID = userID;
                            else
                                bet.User2ID = userID;

                            bet.BetStatusID = Request.QueryString["betStatus"].ToEnum<BetStatus>();
                            _repositoryB.Update(bet);

                            //ZAPIŠEMO HISTORY!!!
                            new EventData(bet.Guid, userID, bet.BetStatusID == BetStatus.Accepted ? 202 : 206).Add();

                            return RedirectToAction("Index", "Dashboard");
                        }

                        //return RedirectToAction("Index", "Bet");
                        return RedirectToAction("Support-Me", "Bet");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }

                // If we got this far, something failed, redisplay form
                return PartialView("_RegisterPopup", model);
            }
            catch (Exception e)
            {
                Neolab.Common.NeoException.Handle(e);
                return RedirectToAction("Index", "Error");
            }
        }
    }
}
