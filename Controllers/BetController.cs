using Microsoft.Web.WebPages.OAuth;
using SupportFriends.Filters;
using SupportFriends.Models.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace SupportFriends.Controllers
{
    [InitializeSimpleMembership]
    public class BetController : Controller
    {
        //
        // GET: /Bet/

        public ActionResult Index()
        {
            return View();
        }

        [ActionName("Support-Me")]
        public ActionResult SupportMe()
        {
            return View();
        }

        [ActionName("Support-Friends")]
        public ActionResult SupportFriends()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SupportMePlaceBet(HttpPostedFileBase uplFile)
        {
            int amount = 0; Int32.TryParse(Request["hfAmount"], out amount);
            string name = Request["tbName"];
            string email = Request["tbEmail"];

            //DO THE MAGIC HERE
           
            IUserDataRepository _repositoryU = new UserDataRepository();

            UserData user = new UserData();
            user =  _repositoryU.FindByUsername(email);

            if (user == null)
            {
                user = new UserData();
                user.FullName = name;
                if (String.IsNullOrEmpty(name) || name == null)
                    user.FullName = email;
                user.Email = email;
                user = _repositoryU.Insert(user);
            }
            
            IBetDataRepository _repositoryB = new BetDataRepository();
            BetData bet = new BetData();
            bet.Username = User.Identity.Name;
            bet.User2ID = user.UserID;
            bet.BetActionID = 115;
            bet.BetValue = amount;

            string extension = String.Empty;
            if (uplFile != null && uplFile.ContentLength > 0)
            {
                extension = uplFile.FileName.Substring(uplFile.FileName.IndexOf(".") + 1, uplFile.FileName.Length - uplFile.FileName.IndexOf(".") - 1).ToLower();
                bet.FileExtension = extension;
            }

            bet = _repositoryB.Insert(bet);

            if (uplFile != null && uplFile.ContentLength > 0)
            {
                int tmpuserID = WebSecurity.GetUserId(User.Identity.Name);

                var fileName = bet.Guid.ToString() + "_" + tmpuserID + "." + extension;//Path.GetFileName(uplFile.FileName);
                var path = Path.Combine(Server.MapPath("~/upload/policies/"), fileName);
                uplFile.SaveAs(path);

                IFileDataRepository repositoryF = new FileDataRepository();
                FileData file = new FileData();
                file.BetGuid = bet.Guid;
                file.FileTypeID = 207; //POLICY
                file.FileExtension = extension;
                file.FilePath = path;
                file.InsertUserID = tmpuserID;

                repositoryF.Insert(file);
            }            

            //ZAPIŠEMO HISTORY!!!
            new EventData(bet.Guid, WebSecurity.GetUserId(User.Identity.Name), 115).Add();

            string hostName = System.Configuration.ConfigurationManager.AppSettings["HostName"];
            string inviteUrl = hostName + "Home/Invited/" + bet.Guid + "/" + bet.BetActionID;
            string mailBody = "Hi " + name + ",<br><br>";
            mailBody += User.Identity.Name + " is hoping you can confirm his driving skills and bet €" + amount.ToString() + " on that.<br><br>";
            mailBody += "Join SupportFriend, the social insurance provider and get up to 80% discount on your car insurance, now! <br><br>";
            mailBody += inviteUrl;

            //Code.BasicMailing.SendEmail(email, "", User.Identity.Name + " needs your support on SupportFriend", mailBody);

            return Content("<li>" + email + "</li>");

            //return RedirectToAction("Index", "Dashboard");
            //return View("Support-Me");
        }


        [HttpPost]
        public ActionResult SupportFriendPlaceBet()
        {
            int amount = 0; Int32.TryParse(Request["hfAmount"], out amount);
            string name = Request["tbName"];
            string email = Request["tbEmail"];

            //DO THE MAGIC HERE

            IUserDataRepository _repositoryU = new UserDataRepository();

            UserData user = new UserData();
            user = _repositoryU.FindByUsername(email);

            if (user == null)
            {
                user = new UserData();
                user.FullName = name;
                if (String.IsNullOrEmpty(name) || name == null)
                    user.FullName = email;
                user.Email = email;
                user = _repositoryU.Insert(user);
            }

            IBetDataRepository _repositoryB = new BetDataRepository();
            BetData bet = new BetData();
            bet.Username = User.Identity.Name;
            bet.User2ID = user.UserID;
            bet.BetActionID = 114;
            bet.BetValue = amount;
            bet = _repositoryB.Insert(bet);

            //ZAPIŠEMO HISTORY!!!
            new EventData(bet.Guid, WebSecurity.GetUserId(User.Identity.Name), 114).Add();

            string hostName = System.Configuration.ConfigurationManager.AppSettings["HostName"];
            string inviteUrl = hostName + "Home/Invited/" + bet.Guid + "/" + bet.BetActionID;
            string mailBody = "Hi " + name + ",<br><br>";
            mailBody += User.Identity.Name + " is so certain of your driving skills he is prepared to bet €" + amount.ToString() + " on that.<br><br>";
            mailBody += "Join SupportFriend, the social insurance provider and get up to 80% discount on your car insurance, now! <br><br>";
            mailBody += inviteUrl;

            //Code.BasicMailing.SendEmail(email, "", User.Identity.Name + " wants to support you on SupportFriend", mailBody);
            
            return Content("<li>" + email + "</li>");
            /*return RedirectToAction("Index", "Dashboard");*/
            //return View("Support-Me");
        }


        public ActionResult FacebookExplaination()
        {
            return PartialView("_FacebookExplaination");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult FacebookMessage(string amount, string betActionID, HttpPostedFileBase uplFile)
        {
            int amountInt = 0; Int32.TryParse(amount, out amountInt);


            IBetDataRepository _repositoryB = new BetDataRepository();
            BetData bet = new BetData();
            bet.Username = User.Identity.Name;
            bet.User2ID = 0;
            bet.BetActionID = Convert.ToInt16(betActionID);
            bet.BetValue = amountInt;

            string extension = String.Empty;
            if (uplFile != null && uplFile.ContentLength > 0)
            {
                extension = uplFile.FileName.Substring(uplFile.FileName.IndexOf(".") + 1, uplFile.FileName.Length - uplFile.FileName.IndexOf(".") - 1).ToLower();
                bet.FileExtension = extension;
            }

            bet = _repositoryB.Insert(bet);

            if (uplFile != null && uplFile.ContentLength > 0)
            {
                int tmpuserID = WebSecurity.GetUserId(User.Identity.Name);

                var fileName = bet.Guid.ToString() + "_" + tmpuserID + "." + extension;//Path.GetFileName(uplFile.FileName);
                var path = Path.Combine(Server.MapPath("~/upload/policies/"), fileName);
                uplFile.SaveAs(path);

                IFileDataRepository repositoryF = new FileDataRepository();
                FileData file = new FileData();
                file.BetGuid = bet.Guid;
                file.FileTypeID = 207; //POLICY
                file.FileExtension = extension;
                file.FilePath = path;
                file.InsertUserID = tmpuserID;

                repositoryF.Insert(file);
            }



            //ZAPIŠEMO HISTORY!!!
            new EventData(bet.Guid, WebSecurity.GetUserId(User.Identity.Name), Convert.ToInt16(betActionID)).Add();

            string hostName = System.Configuration.ConfigurationManager.AppSettings["HostName"];

            return Json(new { success = true, host = hostName,  guid = bet.Guid, action = betActionID }, JsonRequestBehavior.AllowGet);
        }




        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalInvitation()
        {

            ViewBag.id = RouteData.Values["id"];
            ViewBag.Text = RouteData.Values["Text"];
            ViewBag.Label = RouteData.Values["Label"];
            return PartialView("_ExternalInvitationPartial", OAuthWebSecurity.RegisteredClientData);
        }
    }
}
