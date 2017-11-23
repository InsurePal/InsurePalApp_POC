using Newtonsoft.Json;
using SupportFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;

namespace SupportFriends.Code
{
    public static class FacebookDataHelper
    {
        public static List<FacebookFriend> GetFriends()
        {
            FacebookFriendsModel friends = new FacebookFriendsModel();

            FacebookClient client = new FacebookClient(HttpContext.Current.Session["FBAccessToken"].ToString());
            dynamic fbresult = client.Get("me/friends");
            var data = fbresult["data"].ToString();

            friends.friendsListing = JsonConvert.DeserializeObject<List<FacebookFriend>>(data);

            return friends.friendsListing;
        }
    }
}