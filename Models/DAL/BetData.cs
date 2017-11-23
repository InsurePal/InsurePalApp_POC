using Neolab.Code.DAL;
using SupportFriends.Code.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public class BetData : BaseData
    {
        public BetData ()
        {

        }

        public int BetID { get; set; }
        public int User1ID { get; set; }
        public int User2ID { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }
        public string Username { get; set; }
        public int BetActionID { get; set; }
        public string BetActions { get; set; }
        public BetStatus BetStatusID { get; set; }
        public string BetStatus { get; set; }
        public decimal BetValue { get; set; }
        public decimal Voucher1Value { get; set; }
        public int Voucher1StatusID { get; set; }
        public string Voucher1Status { get; set; }
        public string Voucher1Code { get; set; }
        public string Voucher1Reference { get; set; }
        public decimal Voucher2Value { get; set; }
        public int Voucher2StatusID { get; set; }
        public string Voucher2Status { get; set; }
        public string Voucher2Code { get; set; }
        public string Voucher2Reference { get; set; }
        public Guid Guid { get; set; }
        public string File { get; set; }
        public string FileExtension { get; set; }
        public int ParentBetID { get; set; }
        public bool IsSupporter { get; set; }
        public int NextStatusID { get; set; }
        public int NextActivityID { get; set; }
        public string NextActivity { get; set; }
        public int BetPhaseID { get; set; }
        public bool CashIn { get; set; } //grega testira, za ugotavljanje, kaj je pocheckano
        public List<EventData> EventsLog { get; set; }
        public List<FileData> FilesList { get; set; }
    }
}