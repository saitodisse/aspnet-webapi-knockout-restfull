using System.Collections.Generic;
using NHibernate;
using PizzaModel.Entities;
using PizzaModel.Repos;
using PizzaNHibernate.Helpers;

namespace PizzaNHibernate.Repos
{
    public interface IAdminRepo
    {
        void RecreateDataBase();
    }

    public class AdminRepo : IAdminRepo
    {
        private ISession _session;
        public AdminRepo(ISession session)
        {
            _session = session;
        }

        public void RecreateDataBase()
        {
            var nhCastle = new NhCastle();
            nhCastle.RecreateDb(_session);
        }
    }
}