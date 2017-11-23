using Neolab.Common;
using SupportFriends.Models.DAL;
using SupportFriends.Code.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using SupportFriends.Filters;

namespace SupportFriends.Areas.Admin.Controllers
{
    [InitializeSimpleMembership]
    public class DashboardController : Controller
    {
        //
        // GET: /Admin/Dashboard/

        public ActionResult Index()
        {
            ViewBag.ActiveLink = 0;
            return View();
        }

        public ActionResult BetList(string sortOrder)
        {
            ViewBag.ActiveLink = 1;
            BetDataRepository repo = new BetDataRepository();
            ViewBag.User1SortParam = String.IsNullOrEmpty(sortOrder) ? "user1_desc" : "";
            ViewBag.User2SortParam = sortOrder == "user2" ? "user2_desc" : "user2";
            ViewBag.SupportValueSortParam = sortOrder == "supportValue" ? "supportValue_desc" : "supportValue";
            ViewBag.StatusSortParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.VoucherAValueSortParam = sortOrder == "voucherAValue" ? "voucherAValue_desc" : "voucherAValue";
            ViewBag.VoucherACodeSortParam = sortOrder == "voucherACode" ? "voucherACode_desc" : "voucherACode";
            ViewBag.VoucherAStatusSortParam = sortOrder == "voucherAStatus" ? "voucherAStatus_desc" : "voucherAStatus";
            ViewBag.VoucherBValueSortParam = sortOrder == "voucherBValue" ? "voucherBValue_desc" : "voucherBValue";
            ViewBag.VoucherBCodeSortParam = sortOrder == "voucherBCode" ? "voucherBCode_desc" : "voucherBCode";
            ViewBag.VoucherBStatusSortParam = sortOrder == "voucherBStatus" ? "voucherBStatus_desc" : "voucherBStatus";
            ViewBag.InsertDateSortParam = sortOrder == "insertDate" ? "insertDate_desc" : "insertDate";

            var betList = repo.SelectList();

            switch (sortOrder)
            {
                case "user1_desc":
                    betList = betList.OrderByDescending(s => s.User1).ToList();
                    break;
                case "user2":
                    betList = betList.OrderBy(s => s.User2).ToList();
                    break;
                case "user2_desc":
                    betList = betList.OrderByDescending(s => s.User2).ToList();
                    break;
                case "supportValue":
                    betList = betList.OrderBy(s => s.BetValue).ToList();
                    break;
                case "supportValue_desc":
                    betList = betList.OrderByDescending(s => s.BetValue).ToList();
                    break;
                case "status":
                    betList = betList.OrderBy(s => s.BetStatus).ToList();
                    break;
                case "status_desc":
                    betList = betList.OrderByDescending(s => s.BetStatus).ToList();
                    break;
                case "voucherAValue":
                    betList = betList.OrderBy(s => s.Voucher1Value).ToList();
                    break;
                case "voucherAValue_desc":
                    betList = betList.OrderByDescending(s => s.Voucher1Value).ToList();
                    break;
                case "voucherACode":
                    betList = betList.OrderBy(s => s.Voucher1Code).ToList();
                    break;
                case "voucherACode_desc":
                    betList = betList.OrderByDescending(s => s.Voucher1Code).ToList();
                    break;
                case "voucherAStatus":
                    betList = betList.OrderBy(s => s.Voucher1Status).ToList();
                    break;
                case "voucherAStatus_desc":
                    betList = betList.OrderByDescending(s => s.Voucher1Status).ToList();
                    break;
                case "voucherBValue":
                    betList = betList.OrderBy(s => s.Voucher2Value).ToList();
                    break;
                case "voucherBValue_desc":
                    betList = betList.OrderByDescending(s => s.Voucher2Value).ToList();
                    break;
                case "voucherBCode":
                    betList = betList.OrderBy(s => s.Voucher2Code).ToList();
                    break;
                case "voucherBCode_desc":
                    betList = betList.OrderByDescending(s => s.Voucher2Code).ToList();
                    break;
                case "voucherBStatus":
                    betList = betList.OrderBy(s => s.Voucher2Status).ToList();
                    break;
                case "voucherBStatus_desc":
                    betList = betList.OrderByDescending(s => s.Voucher2Status).ToList();
                    break;
                case "insertDate":
                    betList = betList.OrderBy(s => s.InsertDate).ToList();
                    break;
                case "insertDate_desc":
                    betList = betList.OrderByDescending(s => s.InsertDate).ToList();
                    break;
                default:
                    betList = betList.OrderBy(s => s.User1).ToList();
                    break;
            }
            return View(betList);
        }

        public ActionResult FindVoucherByCode()
        {
            List<BetData> bets = new List<BetData>();
            try
            {
                IBetDataRepository repository = new BetDataRepository();
                bets = repository.SelectVouchersByCode(Request["tbVoucherCode"]);
            }
            catch (Exception exc)
            {
                NeoException.Handle(exc);
            }

            return PartialView("_VoucherByCodePartial", bets);
        }

        public ActionResult Cleanup()
        {
            
            try
            {
                UserDataRepository repository = new UserDataRepository();
                repository.Cleanup();
            }
            catch (Exception exc)
            {
                NeoException.Handle(exc);
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult VouchersCashIn(List<BetData> bets)
        {
            foreach (BetData b in bets)
            {
                if (b.CashIn)
                {
                    Guid betGUID;
                    bool isGUID = Guid.TryParse(b.Guid.ToString(), out betGUID);

                    try
                    {
                        //najprej spremenimo status stave
                        IBetDataRepository repository = new BetDataRepository();

                        if (isGUID)
                        {
                            //ČE JE BET SKLENJEN DAMO STATUS NA PRVI UNOVČEN KUPON, DRUGAČE JE PRVI KUPON UNOVČEN IN DAMO STATUS NA DRUGI UNOVČEN KUPON
                            int newBetStatusID = Convert.ToInt32(b.BetStatusID) == 110 ? 112 : 113;
                            repository.VoucherUpdateStatusAndReference(betGUID, newBetStatusID, Request["tbReference"]);
                            b.BetStatusID = newBetStatusID.ToEnum<BetStatus>();
                            b.Voucher1Reference = Request["tbReference"];

                            //ZAPIŠEMO HISTORY!!!
                            //NI PRAV NI PRAV NI PRAV NI PRAV NI PRAV NI PRAV NI PRAV NI PRAV
                            //UPDATE: MALO MANJ NI PRAV AMPAK ZDAJ NE VEMO KATER AGENT JE AKTIVIRAL KUPON
                            new EventData(betGUID, newBetStatusID == 112 ? -1 : -2, 204).Add();
                        }

                        bets = repository.SelectVouchersByCode(b.Voucher1Code);
                    }
                    catch (Exception exc)
                    {
                        NeoException.Handle(exc);
                    }
                }
            }

            //IBetDataRepository repository2 = new BetDataRepository();
            //bets = repository2.SelectVouchersByCode(Request["hfVoucherCode"]);


            return PartialView("_VoucherByCodePartial", bets);
        }

        //replaced with multiple update
        //public ActionResult VoucherCashIn()
        //{
        //    List<BetData> bets = new List<BetData>();
        //    Guid betGUID;
        //    bool isGUID = Guid.TryParse(Request["hfBetGUID"], out betGUID);

        //    try
        //    {
        //        //najprej spremenimo status stave
        //        IBetDataRepository repository = new BetDataRepository();

        //        if (isGUID)
        //        {
        //            //ČE JE BET SKLENJEN DAMO STATUS NA PRVI UNOVČEN KUPON, DRUGAČE JE PRVI KUPON UNOVČEN IN DAMO STATUS NA DRUGI UNOVČEN KUPON
        //            int newBetStatusID = Convert.ToInt32(Request["hfBetStatus"]) == 110 ? 112 : 113;
        //            repository.VoucherUpdateStatus(betGUID, newBetStatusID);

        //            //ZAPIŠEMO HISTORY!!!
        //            //NI PRAV NI PRAV NI PRAV NI PRAV NI PRAV NI PRAV NI PRAV NI PRAV
        //            new EventData(betGUID, WebSecurity.GetUserId(User.Identity.Name), 204).Add();
        //        }

        //        bets = repository.SelectVouchersByCode(Request["hfVoucherCode"]);
        //    }
        //    catch (Exception exc)
        //    {
        //        NeoException.Handle(exc);
        //    }

        //    return PartialView("_VoucherByCodePartial", bets);
        //}

        public ActionResult VoucherList()
        {
            BetDataRepository repo = new BetDataRepository();
            var voucherList = repo.SelectVouchers();
            return View(voucherList);
        }
    }
}
