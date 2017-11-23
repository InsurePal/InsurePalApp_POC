using Neolab.Code.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public interface IUserDataRepository : IDataRepository<UserData>
    {
        UserData FindByUsername(string username);
        void AddFBFriends(string username, List<FacebookFriend> fbFriendIds);
        List<UserData> SelectFriends(string username);
    }
}