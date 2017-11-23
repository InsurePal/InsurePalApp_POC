using System;
using System.Collections.Generic;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Neolab.Code.DAL;

namespace SupportFriends.Models.DAL
{
    public class UserDataRepository : DbAccessObject, IUserDataRepository
    {
        public List<UserData> SelectList()
        {
            //return this._db.Query<UserData>("SELECT * FROM Users").ToList();
            try
            {
                return this._db.Query<UserData>("spUserProfiles_SelectList", commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
                return new List<UserData>();
            }
        }

        public UserData Select(int id)
        {
            //return this._db.Query<UserData>("SELECT * FROM Users WHERE UserID = @UserID", new { id }).SingleOrDefault();
            try
            {
                return this._db.Query<UserData>("spUserProfile_Select", param: new { UserID = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
                return new UserData();
            }
        }

        public UserData FindByUsername(string username)
        {
            //return this._db.Query<UserData>("SELECT * FROM Users WHERE UserID = @UserID", new { id }).SingleOrDefault();
            try
            {
                return this._db.Query<UserData>("sp_UserProfile_Select", param: new { Username = username }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
                return new UserData();
            }
        }

        public UserData Insert(UserData user)
        {
            //var sqlQuery = "INSERT INTO Users (FirstName, LastName, Email) VALUES(@FirstName, @LastName, @Email); " + "SELECT CAST(SCOPE_IDENTITY() as int)";
            //var userId = this._db.Query<int>(sqlQuery, user).Single();

            try
            {
                var p = new DynamicParameters();
            p.Add("Email", user.Email);
            p.Add("FullName", user.FullName);
            p.Add("UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            this._db.Execute("sp_UserProfile_Insert", p, commandType: CommandType.StoredProcedure);

            user.UserID = p.Get<int>("UserId");
            return user;
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
                return new UserData();
            }
        }

        public void AddFBFriends(string username, List<FacebookFriend> fbFriendIds)
        {
            try
            {
                string ids = String.Empty;

                foreach (FacebookFriend fbFriend in fbFriendIds)
                {
                    ids += fbFriend.id + ",";
                }
                if (ids.Contains(","))
                {
                    ids = ids.Remove(ids.LastIndexOf(","));
                }

                this._db.Execute("spUserUser_InsertFBFriends", param: new { Username = username, FbFriendIds = ids }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
            }
        }
        public List<UserData> SelectFriends(string username)
        {
            try
            {
                return this._db.Query<UserData>("sp_UserProfile_SelectFriends", param: new { Username = username }, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
                return new List<UserData>();
            }
        }

        public UserData Update(UserData user)
        {
            //var sqlQuery =
            //    "UPDATE Users " +
            //    "SET FirstName = @FirstName, " +
            //    "    LastName  = @LastName, " +
            //    "    Email     = @Email " +
            //    "WHERE UserID = @UserID";
            //this._db.Execute(sqlQuery, user);

            try
            {
                this._db.Execute("spUserProfile_Update", param: user, commandType: CommandType.StoredProcedure);
            return user;
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
                return new UserData();
            }
        }

        public void Delete(int id)
        {
            try
            {
                this._db.Execute("spUserProfile_Update", param: new { UserID = id }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
            }
        }


        public void Cleanup()
        {
            try
            {
                this._db.Execute("spSYS_Cleanup", param: new { }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception exc)
            {
                Neolab.Common.NeoException.Handle(exc);
            }
        }
    }
}