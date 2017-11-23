using Neolab.Code.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public class UserData : BaseData
    {
        public UserData()
        {
            
        }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public Guid UserGuid { get; set; }
    }
}