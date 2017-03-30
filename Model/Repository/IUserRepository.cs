using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Repository
{
    public interface IUserRepository
    {
        int InsertUser(string query, User aPanCard);
        User GetUser(string query);
        User GetUser(string query, string resultMap);
        IEnumerable<User> GetAllUsers(string query);
        IEnumerable<User> GetAllUsers(string query, string resultMap);
        int UpdateUser(string query, User aUser);

        IEnumerable<UserRole> GetUserRoles(string query);
    }
}
