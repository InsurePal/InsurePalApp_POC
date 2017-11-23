using SupportFriends.Code.Enumerators;
using SupportFriends.Filters;
using SupportFriends.Models.DAL;
using SupportFriends.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace SupportFriends.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            List<BetData> bets = new List<BetData>();
            BetDataRepository _repositoryB = new BetDataRepository();
            bets = _repositoryB.SelectVouchers(WebSecurity.GetUserId(User.Identity.Name));
            int totalValue = 0;
            string friends = "";
            foreach (BetData b in bets)
            {
                string color = ((b.BetValue == 500) ? "red" : ((b.BetValue == 200) ? "violet" : ((b.BetValue == 100) ? "blue" : "")));
                string user = b.User2;
                if (user != null && user.Contains(" "))
                    user= user.Remove(user.IndexOf(" "));

                if (b.IsSupporter && b.BetStatusID == BetStatus.Voucher1Cashed)
                { 
                    totalValue += Convert.ToInt32(b.Voucher1Value);

                }
                
                else if (!b.IsSupporter && b.BetStatusID == BetStatus.Guaranteed)
                {
                    totalValue += Convert.ToInt32(b.Voucher2Value);
   
                }

                if (b.User2ID != 0)
                {
                    friends += "<div class=\"friend\">" +
                     "<div class=\"circle " + color + "\"></div>" +
                     "<div class=\"name\">" + user + "</div>" +
                     "</div>";
                }
            }

            ViewBag.FriendsList = friends;
            ViewBag.TotalValue = totalValue;

            if (TempData.ContainsKey("NeoNotification"))
            {
                NotificationModel notification = (NotificationModel)TempData["NeoNotification"];
                ViewBag.Notification = notification;
            }

            ViewBag.ActiveLink = 0;
            return View(bets);
        }
        [HttpPost]
        public ActionResult Index(string Id, int Type, int NextActivity, int NextStatus)
        {
            BetDataRepository _repositoryB = new BetDataRepository();

            switch (NextActivity)
            {
                case 202:
                    return RedirectToAction("Invited", "Home", new { id = Id, type = Type });
                case 203:
                    _repositoryB.VoucherUpdateStatus(Guid.Parse(Id), NextStatus);

                    //ZAPIŠEMO HISTORY!!!
                    new EventData(Guid.Parse(Id), WebSecurity.GetUserId(User.Identity.Name), NextActivity).Add();
                    break;
                default:
                    break;
            }

            List<BetData> bets = new List<BetData>();
            bets = _repositoryB.SelectVouchers(WebSecurity.GetUserId(User.Identity.Name));

            ViewBag.ActiveLink = 0;
            return View("Index", bets);
        }

        public ActionResult Bets()
        {
            ViewBag.ActiveLink = 2;
            return View();
        }

        public ActionResult Friends()
        {
            ViewBag.ActiveLink = 1;

            IUserDataRepository repoU = new UserDataRepository();
            List<UserData> friends = repoU.SelectFriends(User.Identity.Name);

            return View(friends);
        }

        [HttpPost]
        public ActionResult PostCreditInfo(string Id, int Type, int NextActivity, int NextStatus, HttpPostedFileBase uplFile)
        {
            BetDataRepository _repositoryB = new BetDataRepository();

            Guid tmpGuid = Guid.Parse(Id);

            _repositoryB.VoucherUpdateStatus(tmpGuid, NextStatus);

            string extension = String.Empty;
            if (uplFile != null && uplFile.ContentLength > 0)
            {
                extension = uplFile.FileName.Substring(uplFile.FileName.IndexOf(".") + 1, uplFile.FileName.Length - uplFile.FileName.IndexOf(".") - 1).ToLower();

                int tmpuserID = WebSecurity.GetUserId(User.Identity.Name);

                var fileName = tmpGuid.ToString() + "_" + tmpuserID + "." + extension;//Path.GetFileName(uplFile.FileName);
                var path = Path.Combine(Server.MapPath("~/upload/credit-notes/"), fileName);
                uplFile.SaveAs(path);

                IFileDataRepository repositoryF = new FileDataRepository();
                FileData file = new FileData();
                file.BetGuid = tmpGuid;
                file.FileTypeID = 208; //CREDIT NOTE
                file.FileExtension = extension;
                file.FilePath = path;
                file.InsertUserID = tmpuserID;

                repositoryF.Insert(file);
            }

            //ZAPIŠEMO HISTORY!!!
            new EventData(Guid.Parse(Id), WebSecurity.GetUserId(User.Identity.Name), !String.IsNullOrEmpty(extension) ? 209: NextActivity).Add();

            List<BetData> bets = new List<BetData>();
            bets = _repositoryB.SelectVouchers(WebSecurity.GetUserId(User.Identity.Name));

            ViewBag.ActiveLink = 0;
            return RedirectToAction("Index");
        }
        public ActionResult EnterCreditInfo()
        {
            return PartialView("_EnterCreditInfo");
        }

    }
}
