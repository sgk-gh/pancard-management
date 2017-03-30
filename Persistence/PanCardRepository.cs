using System.Collections.Generic;
using Model;
using Model.Repository;

namespace Persistence
{
    public class PanCardRepository : IPanCardRepository
    {
        private readonly SqlHandler _sqlHandler;

        public PanCardRepository(SqlHandler sqlHandler)
        {
            _sqlHandler = sqlHandler;
        }

        public int InsertPanCard(string query,PanCard aPanCard)
        {
            return _sqlHandler.Insert(query, aPanCard);
        }

        public PanCard GetPanCard(string query)
        {
            return _sqlHandler.GetRecord<PanCard>(query, null);
        }

        public PanCard GetPanCard(string query, string resultMap)
        {
            return _sqlHandler.GetRecord<PanCard>(query, null);
        }

        public IEnumerable<PanCard> GetAllPanCards(string query)
        {
            return _sqlHandler.GetRecords<PanCard>(query, null);
        }

        public IEnumerable<PanCard> GetAllPanCards(string query, string resultMap)
        {
            return _sqlHandler.GetRecords<PanCard>(query, resultMap);
        }

        public int UpdatePanCard(string query, PanCard aPanCard)
        {
            return _sqlHandler.Update(query, aPanCard);
        }        
    }
}
