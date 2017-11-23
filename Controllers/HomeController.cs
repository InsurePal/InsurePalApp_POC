using SupportFriends.Code.Enumerators;
using SupportFriends.Filters;
using SupportFriends.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace SupportFriends.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Invitation = false;
            ViewBag.ActionTypeID = 0;

            return View();
        }

        [ActionName("Index-Alt")]
        public ActionResult IndexAlt()
        {
            ViewBag.Invitation = false;
            ViewBag.ActionTypeID = 0;

            return View("IndexAlt");
        }

        [ActionName("Index-Alt2")]
        public ActionResult IndexAlt2()
        {
            ViewBag.Invitation = false;
            ViewBag.ActionTypeID = 0;

            return View("IndexAlt2");
        }
        [ActionName("Index-Alt3")]
        public ActionResult IndexAlt3()
        {
            ViewBag.Invitation = false;
            ViewBag.ActionTypeID = 0;

            return View("IndexAlt3");
        }
        public ActionResult Invited()
        {
            BetData bet = new BetData();
            ViewBag.Invitation = false;
            ViewBag.ActionTypeID = 0;

            if (RouteData.Values["id"] != null)
            {
                int actionTypeID = 0;
                int.TryParse(RouteData.Values["type"].ToString(), out actionTypeID);

                ViewBag.Invitation = true;
                ViewBag.ActionTypeID = actionTypeID;
                

                IBetDataRepository _repositoryB = new BetDataRepository();
                bet = _repositoryB.Find(new Guid(RouteData.Values["id"].ToString()), actionTypeID);

                if (bet != null)
                    ViewBag.TotalVouchers = Convert.ToInt32(bet.Voucher1Value) + Convert.ToInt32(bet.Voucher2Value);
            }

            TempData["BetData"] = bet;

            if (bet != null)
            {
                if (((int)bet.BetStatusID) > 101)
                    return RedirectToAction("Index", "Dashboard");
                else
                    return View("Invited", bet);
            }
            else
                return Redirect("~/");
        }

        [InitializeSimpleMembership]
        public ActionResult Respond(bool accept)
        {
            IBetDataRepository _repositoryB = new BetDataRepository();
            BetData bet = (BetData)TempData["BetData"];
            int? userID = null;

            if (User.Identity.IsAuthenticated)
                userID = (int)Membership.GetUser().ProviderUserKey;

            if (userID == null)
                userID = 0;

            //if (userID != null) //sem zlogiran
            //{
                    if (bet.BetActionID == 115) //jaz stavim (operiram z User1ID)
                {
                    if (bet.User1ID == 0) //povabljen sem prek FB
                    {
                        bet.Guid = Guid.Parse(RouteData.Values["id"].ToString());
                        bet.User1ID = (int)userID;
                        bet.BetStatusID = accept ? BetStatus.Accepted : BetStatus.Rejected;
                        _repositoryB.Update(bet); //update-am bet s svojim ID-jem in statusom

                        //ZAPIŠEMO HISTORY!!!
                        new EventData(bet.Guid, (int)userID, accept ? 202 : 206).Add();
                    }
                    else if (bet.User1ID == userID) //sem logiran in sem povabljen user
                    {
                        bet.Guid = Guid.Parse(RouteData.Values["id"].ToString());
                        bet.BetStatusID = accept ? BetStatus.Accepted : BetStatus.Rejected;
                        _repositoryB.Update(bet); //update-am bet status

                        //ZAPIŠEMO HISTORY!!!
                        new EventData(bet.Guid, (int)userID, accept ? 202 : 206).Add();
                    }
                }
                else if (bet.BetActionID == 114) //nekdo stavi name (operiram z User2ID)
                {
                    if (bet.User2ID == 0) //povabljen sem prek FB
                    {
                        bet.Guid = Guid.Parse(RouteData.Values["id"].ToString());
                        bet.User2ID = (int)userID;
                        bet.BetStatusID = accept ? BetStatus.Accepted : BetStatus.Rejected;
                        _repositoryB.Update(bet); //update-am bet s svojim ID-jem in statusom

                        //ZAPIŠEMO HISTORY!!!
                        new EventData(bet.Guid, (int)userID, accept ? 202 : 206).Add();
                    }
                    else if (bet.User2ID == userID) //sem logiran in sem povabljen user
                    {
                        bet.Guid = Guid.Parse(RouteData.Values["id"].ToString());
                        bet.BetStatusID = accept ? BetStatus.Accepted : BetStatus.Rejected;
                        _repositoryB.Update(bet); //update-am bet status

                        //ZAPIŠEMO HISTORY!!!
                        new EventData(bet.Guid, (int)userID, accept ? 202 : 206).Add();
                    }
                }
            //}
            //else
            //{
            //    return RedirectToAction("Register", "Account", new { id = RouteData.Values["id"], type = RouteData.Values["type"], betStatus = accept ? 102 : 103 }); //preusmerim na registracijo
            //}

            if (userID == 0)
                return Redirect("~/");
            else
                return RedirectToAction("Index", "Dashboard", new { popup = "typeCredit" + bet.BetID });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FBtest()
        {
            ViewBag.Message = "FB testing.";

            return View();
        }
    }
}
