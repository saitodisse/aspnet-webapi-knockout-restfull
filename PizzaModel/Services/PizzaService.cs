using System.Collections.Generic;
using PizzaModel.Entities;
using PizzaModel.Repos;

namespace PizzaModel.Services
{
    public interface IPizzaService
    {
        Pizza GetById(int id);
        IList<Pizza> GetByName(string nome);
        IList<Pizza> GetAll();
        IList<Pizza> GetByIngretient(int ingredientId);
        void Save(Pizza pizza);
        void Delete(int id);
    }

    public class PizzaService : IPizzaService
    {
        private readonly IIngredientDAO _ingredientDAO;
        private readonly IPizzaDAO _pizzaDAO;

        public PizzaService(IPizzaDAO pizzaDAO, IIngredientDAO ingredientDAO)
        {
            _pizzaDAO = pizzaDAO;
            _ingredientDAO = ingredientDAO;
        }

        #region IPizzaService Members

        public Pizza GetById(int id)
        {
            return _pizzaDAO.Get(id);
        }

        public IList<Pizza> GetByName(string nome)
        {
            return _pizzaDAO.GetByName(nome);
        }

        public IList<Pizza> GetByIngretient(int ingredientId)
        {
            //fixme: não serve pra nada
            Ingredient ingredient = _ingredientDAO.Get(ingredientId);
            return new List<Pizza>();
        }

        public IList<Pizza> GetAll()
        {
            return _pizzaDAO.GetAll();
        }

        public void Save(Pizza pizza)
        {
            _pizzaDAO.Save(pizza);
        }

        public void Delete(int id)
        {
            _pizzaDAO.Delete(id);
        }

        #endregion
    }
}