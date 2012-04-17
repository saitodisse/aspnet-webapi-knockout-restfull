using System.Collections.Generic;
using System.Linq;
using PizzaModel.Entities;
using PizzaModel.Repos;
using PizzaModel.Services;

namespace PizzaModel.Services
{
    public interface IIngredientService
    {
        IList<Ingredient> GetAll();
        Ingredient GetById(int Id);
        void Save(Ingredient ingredient);
        void Delete(int id);
    }

    public class IngredientService : IIngredientService
    {
        private readonly IIngredientDAO _ingredientDAO;
        private readonly IPizzaService _pizzaService;

        public IngredientService(IIngredientDAO ingredientDAO, IPizzaService pizzaService)
        {
            _ingredientDAO = ingredientDAO;
            _pizzaService = pizzaService;
        }

        #region IIngredientService Members

        public IList<Ingredient> GetAll()
        {
            return _ingredientDAO.GetAll();
        }

        public Ingredient GetById(int id)
        {
            return _ingredientDAO.Get(id);
        }

        public void Save(Ingredient ingredient)
        {
            _ingredientDAO.Save(ingredient);
        }

        public void Delete(int id)
        {
            // apaga as dependencias
            IList<Pizza> pizzas = _pizzaService.GetByIngretient(id);

            foreach (Pizza pizza in pizzas)
            {
                List<Ingredient> ingredients = pizza.Ingredients.Where(i => i.Id == id).ToList();
                foreach (Ingredient ingredient in ingredients)
                {
                    pizza.Ingredients.Remove(ingredient);
                    _pizzaService.Save(pizza);
                }
            }

            // apaga o ingredient solitário
            _ingredientDAO.Delete(id);
        }

        #endregion
    }
}