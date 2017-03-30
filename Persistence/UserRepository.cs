using System.Collections.Generic;
using Model;
using Model.Repository;

namespace Persistence
{
    public class UserRepository: IUserRepository
    {
        private readonly SqlHandler _sqlHandler;
        public UserRepository(SqlHandler sqlHandler)
        {
            _sqlHandler = sqlHandler;
        }

        public int InsertUser(string query, User aUser)
        {
            return _sqlHandler.Insert(query, aUser);
        }

        public User GetUser(string query)
        {
            return _sqlHandler.GetRecord<User>(query, null);
        }

        public User GetUser(string query, string resultMap)
        {
            return _sqlHandler.GetRecord<User>(query, resultMap);
        }

        public IEnumerable<User> GetAllUsers(string query)
        {
            return _sqlHandler.GetRecords<User>(query, null);
        }

        public IEnumerable<User> GetAllUsers(string query, string resultMap)
        {
            return _sqlHandler.GetRecords<User>(query, resultMap);
        }

        public int UpdateUser(string query, User aUser)
        {
            return _sqlHandler.Update(query, aUser);
        }

        public IEnumerable<UserRole> GetUserRoles(string query)
        {
            return _sqlHandler.GetRecords<UserRole>(query, null);
        }
    }
}
