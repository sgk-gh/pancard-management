using System.Collections.Generic;

namespace Model.Repository
{
    public interface IPanCardRepository
    {
        int InsertPanCard(string query, PanCard aPanCard);
        PanCard GetPanCard(string query);
        PanCard GetPanCard(string query, string resultMap);
        IEnumerable<PanCard> GetAllPanCards(string query);
        IEnumerable<PanCard> GetAllPanCards(string query, string resultMap);
        int UpdatePanCard(string query, PanCard aPanCard);

        int InsertUser(string query, User aPanCard);
        User GetUser(string query);
        User GetUser(string query, string resultMap);
        IEnumerable<User> GetAllUsers(string query);
        IEnumerable<User> GetAllUsers(string query, string resultMap);
        int UpdateUser(string query, User aUser);

        IEnumerable<UserRole> GetUserRoles(string query);

        //IEnumerable<PanCard> GetAllPanCardDetails(string query, PanCard aPanCard);
        //IEnumerable<PanCard> GetAllPanCardDetails(string query, int startRecordIndex, int maxRecords, PanCard aPanCard);
        //IEnumerable<PanCard> GetPanCardDetailsBySearchTerms(string query, PanCard aPanCard);
        //IEnumerable<PanCard> GetPanCardDetailsBySearchTerms(string query, int startRecordIndex, int maxRecords, PanCard aPanCard);

        //User GetUserDetailsByLoginNameAndPassword(string query, User aUser);

        //IEnumerable<User> GetUserRoles(string query);
        //int InsertUserDetails(string query, User aUser);


    }
}
