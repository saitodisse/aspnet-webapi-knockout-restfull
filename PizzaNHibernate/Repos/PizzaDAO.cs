using System.Collections.Generic;
using NHibernate;
using PizzaModel.Entities;
using PizzaModel.Repos;

namespace PizzaNHibernate.Repos
{
    public class PizzaDAO : DAO<Pizza>, IPizzaDAO
    {
        public PizzaDAO(ISession session) : base(session)
        {
        }

        #region IPizzaDAO Members

        public IList<Pizza> GetByName(string name)
        {
            return _session.QueryOver<Pizza>().Where(i => i.Name == name).List<Pizza>();
        }

        #endregion
    }
}