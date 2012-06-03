using System.Collections.Generic;

namespace PizzaModel.Entities
{
    public class Pizza
    {
        public Pizza()
        {
            Ingredients = new List<Ingredient>();
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Ingredient> Ingredients { get; set; }

        public virtual void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
        }
    };
}