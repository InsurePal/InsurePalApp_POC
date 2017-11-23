using Dapper;
using Neolab.Code.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public class BetDataRepository : DbAccessObject, IBetDataRepository
    {

        public BetData Select(int id)
        {
            throw new NotImplementedException();
        }

        public List<BetData> SelectVouchersByCode(string voucherCode)
        {
            //return this._db.Query<BetData>("spSUP_Bet_SelectVoucherByCode", param: new { VoucherCode = voucherCode }, commandType: CommandType.StoredProcedure).ToList();

            List<BetData> bets = new List<BetData>();
            List<EventData> events = new List<EventData>();
            List<FileData> files = new List<FileData>();

            using (var multi = this._db.QueryMultiple("spSUP_Bet_SelectVoucherByCode", param: new { VoucherCode = voucherCode }, commandType: CommandType.StoredProcedure))
            {
                bets = multi.Read<BetData>().ToList();
                events = multi.Read<EventData>().ToList();
                files = multi.Read<FileData>().ToList();
            }

            foreach (BetData bet in bets)
            {
                bet.EventsLog = new List<EventData>();
                if (events.Any(eventData => eventData.BetID == bet.BetID))
                {
                    bet.EventsLog.AddRange(events.Where(eventData => eventData.BetID == bet.BetID).ToList());
                }

                bet.FilesList = new List<FileData>();
                if (files.Any(fileData => fileData.BetID == bet.BetID))
                {
                    bet.FilesList.AddRange(files.Where(fileData => fileData.BetID == bet.BetID).ToList());
                }
            }

            return bets;
        }


        public BetData Find(Guid id, int actionID)
        {
            return this._db.Query<BetData>("spSUP_Bet_SelectInvite", param: new { Guid = id, BetActionID = actionID }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }
        public List<BetData> SelectList()
        {
            return this._db.Query<BetData>("spSUP_Bet_SelectList", commandType: CommandType.StoredProcedure).ToList();
        }
        public List<BetData> SelectVouchers()
        {
            return this._db.Query<BetData>("spSUP_Bet_SelectVouchers", commandType: CommandType.StoredProcedure).ToList();
        }
        public List<BetData> SelectVouchers(int userID)
        {
            //return this._db.Query<BetData>("spSUP_Bet_SelectVouchers", param: new { UserID = userID }, commandType: CommandType.StoredProcedure).ToList();

            List<BetData> bets = new List<BetData>();
            List<EventData> events = new List<EventData>();

            using (var multi = this._db.QueryMultiple("spSUP_Bet_SelectVouchers", param: new { UserID = userID }, commandType: CommandType.StoredProcedure))
            {
                bets = multi.Read<BetData>().ToList();
                events = multi.Read<EventData>().ToList();
            }

            foreach (BetData bet in bets)
            {
                bet.EventsLog = new List<EventData>();
                if (events.Any(eventData => eventData.BetID == bet.BetID))
                {
                    bet.EventsLog.AddRange(events.Where(eventData => eventData.BetID == bet.BetID).ToList());
                }
            }

            return bets;
        }

        public BetData Insert(BetData bet)
        {
            var p = new DynamicParameters();
            p.Add("Username", bet.Username);
            p.Add("User2ID", bet.User2ID);
            p.Add("BetActionID", bet.BetActionID);
            p.Add("BetValue", bet.BetValue);
            p.Add("FileExtension", bet.FileExtension);
            p.Add("Guid", dbType: DbType.Guid, direction: ParameterDirection.Output);

            this._db.Execute("spSUP_Bet_Insert", p, commandType: CommandType.StoredProcedure);

            bet.Guid = p.Get<Guid>("Guid");
            return bet;
        }

        //public BetData Update(BetData bet)
        //{
        //    this._db.Execute("spSUP_Bet_Update", param: bet, commandType: CommandType.StoredProcedure);
        //    return bet;
        //}

        public BetData Update(BetData bet)
        {
            var p = new DynamicParameters();
            p.Add("Guid", bet.Guid, direction: ParameterDirection.InputOutput);
            p.Add("BetID", dbType:DbType.Int32, direction: ParameterDirection.Output);
            if (bet.BetActionID==115)
                p.Add("UserID", bet.User1ID);
            else
                p.Add("UserID", bet.User2ID);
            p.Add("BetStatusID", bet.BetStatusID);

            this._db.Execute("spSUP_Bet_Update", param: p, commandType: CommandType.StoredProcedure);

            bet.Guid = p.Get<Guid>("Guid");
            bet.BetID = p.Get<Int32>("BetID");
            return bet;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        //public void VoucherCashIn(Guid betGUID, string betStatus)
        //{
        //    this._db.Execute("spSUP_Bet_VoucherCashIn", param: new { BetGUID = betGUID, BetStatus = betStatus }, commandType: CommandType.StoredProcedure);
        //}

        public void VoucherUpdateStatus(Guid betGUID, int betStatusID)
        {
            this._db.Execute("spSUP_Bet_VoucherUpdateStatus", param: new { BetGUID = betGUID, BetStatusID = betStatusID}, commandType: CommandType.StoredProcedure);
        }
        public void VoucherUpdateStatusAndReference(Guid betGUID, int betStatusID, string voucherReference)
        {
            this._db.Execute("spSUP_Bet_VoucherUpdateStatus", param: new { BetGUID = betGUID, BetStatusID = betStatusID, Voucher1Reference = voucherReference }, commandType: CommandType.StoredProcedure);
        }
    }
}