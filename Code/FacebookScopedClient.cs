using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DotNetOpenAuth.AspNet;
using Newtonsoft.Json;

namespace SupportFriends.Code
{
    public class FacebookScopedClient : IAuthenticationClient
    {
        private string appId;
        private string appSecret;
        private string scope;

        private const string baseUrl = "https://www.facebook.com/dialog/oauth?client_id=";
        public const string graphApiToken = "https://graph.facebook.com/oauth/access_token?";
        public const string graphApiMe = "https://graph.facebook.com/me?";

        private static string GetHTML(string URL)
        {
            string connectionString = URL;

            try
            {
                System.Net.HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(connectionString);
                myRequest.Credentials = CredentialCache.DefaultCredentials;
                //// Get the response
                WebResponse webResponse = myRequest.GetResponse();
                Stream respStream = webResponse.GetResponseStream();
                ////
                StreamReader ioStream = new StreamReader(respStream);
                string pageContent = ioStream.ReadToEnd();
                //// Close streams
                ioStream.Close();
                respStream.Close();
                return pageContent;
            }
            catch (Exception exc)
            {
                
            }
            return null;
        }

        private IDictionary<string, string> GetUserData(string accessCode, string redirectURI)
        {
            string token =
                GetHTML(graphApiToken + "client_id=" + appId + "&redirect_uri=" + HttpUtility.UrlEncode(redirectURI) +
                        "&client_secret=" + appSecret + "&code=" + accessCode);
            if (token == null || token == "")
            {
                return null;
            }
            // string access_token = token.Substring(token.IndexOf("access_token="), token.IndexOf("&"));
            string token2 = token.Replace("\"", "");
            string access_token = token2.Substring(token2.IndexOf("access_token:"), token2.IndexOf(",token_type:"));
            access_token = access_token.Replace("access_token:", "access_token=");
            string data = GetHTML(graphApiMe + "fields=id,name,email,gender,link&" + access_token);

            if (!String.IsNullOrEmpty(access_token))
            {
                HttpContext.Current.Session["FBAccessToken"] = access_token.Replace("access_token=", "");
            }

            // this dictionary must contains
            Dictionary<string, string> userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            return userData;
        }

        public FacebookScopedClient(string appId, string appSecret, string scope)
        {
            this.appId = appId;
            this.appSecret = appSecret;
            this.scope = scope;
        }

        public string ProviderName
        {
            get { return "Facebook"; }
        }

        public void RequestAuthentication(System.Web.HttpContextBase context, Uri returnUrl)
        {
            string url = baseUrl + appId + "&redirect_uri=" + HttpUtility.UrlEncode(returnUrl.ToString()) + "&scope=" +
                            scope;
            context.Response.Redirect(url);
        }

        public AuthenticationResult VerifyAuthentication(System.Web.HttpContextBase context)
        {
            string code = context.Request.QueryString["code"];

            string rawUrl = context.Request.Url.OriginalString;
            if (rawUrl.Contains(":80/"))
            {
                rawUrl = rawUrl.Replace(":80/", "/");
            }
            if (rawUrl.Contains(":443/"))
            {
                rawUrl = rawUrl.Replace(":443/", "/");
            }
            //From this we need to remove code portion
            rawUrl = Regex.Replace(rawUrl, "&code=[^&]*", "");

            IDictionary<string, string> userData = GetUserData(code, rawUrl);

            if (userData == null)
                return new AuthenticationResult(false, ProviderName, null, null, null);

            string id = userData["id"];
            string username = userData["email"];



            AuthenticationResult result = new AuthenticationResult(true, ProviderName, id, username, userData);
            return result;
        }
    }
} 
