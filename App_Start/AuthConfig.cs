using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using SupportFriends.Models;
using SupportFriends.Code;
using System.Configuration;
using System.Web;



namespace SupportFriends
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //var extraData1 = new Dictionary<string, object>();
            //extraData1.Add("scope", "email, user_friends");

            if (ConfigurationManager.AppSettings["FBconsumerKey"] == null || ConfigurationManager.AppSettings["FBconsumerSecret"] == null)
            {
                //no wiork
                //krneki
                Neolab.Common.NeoException.Handle(new Exception("Missing FBconsumerKey or FBconsumerSecret in web.config"));
            }
            else
            {

                OAuthWebSecurity.RegisterClient(new FacebookScopedClient(

                        appId: ConfigurationManager.AppSettings["FBconsumerKey"].ToString(),
                        appSecret: ConfigurationManager.AppSettings["FBconsumerSecret"].ToString(),
                        scope: "email"),
                        "facebook",
                null
                );
            }

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}

