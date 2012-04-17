using System.Collections.Generic;

namespace Pizzaria.DTOs
{
    public class PizzaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public IList<IngredienteDto> Ingredientes { get; set; }
    }
}