using System.Collections.Generic;

namespace PizzaMvcWebApi.DTOs
{
    public class PizzaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<IngredientDto> Ingredients { get; set; }
    }
}