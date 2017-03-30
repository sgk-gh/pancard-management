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
    }
}
