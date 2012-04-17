using System.Collections.Generic;
using NHibernate;
using PizzaModel.Entities;
using PizzaModel.Repos;

namespace PizzaNHibernate.Repos
{
    public class IngredientDAO : DAO<Ingredient>, IIngredientDAO
    {
        public IngredientDAO(ISession session) : base(session)
        {
        }

        #region IIngredientDAO Members

        public IList<Ingredient> PesquisarApimentados()
        {
            return GetAll();
        }

        #endregion
    }
}