using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using PizzaModel.Entities;
using PizzaModel.Services;
using PizzaMvcWebApi.DTOs;

namespace Pizzaria
{
    public class PizzaController : ApiController
    {
        private readonly IPizzaService _pizzaServico;
        private readonly IIngredientService _ingredienteServico;

        public PizzaController(IPizzaService pizzaServico, IIngredientService ingredienteServico)
        {
            _pizzaServico = pizzaServico;
            _ingredienteServico = ingredienteServico;
            CriarMapeamentosDto();
        }

        private static void CriarMapeamentosDto()
        {
            Mapper.CreateMap<Pizza, PizzaDto>();
            Mapper.CreateMap<Ingredient, IngredientDto>();
        }


        // GET /api/pizza
        public IList<PizzaDto> Get()
        {
            var pizzas = _pizzaServico.GetAll();

            IList<PizzaDto> pizzaDtos = Mapper.Map<IList<Pizza>, IList<PizzaDto>>(pizzas);

            return pizzaDtos;
        }

        // GET /api/pizza/5
        public PizzaDto Get(int id)
        {
            var pizza = _pizzaServico.GetById(id);

            var pizzaDto = Mapper.Map<Pizza, PizzaDto>(pizza);

            return pizzaDto;
        }

        // POST /api/pizza
        public string Post(PizzaDto pizzaDto)
        {
            var pizzaIncluir = new Pizza();
            pizzaIncluir.Name = pizzaDto.Name;
            pizzaIncluir.Ingredients = new List<Ingredient>();
            _pizzaServico.Save(pizzaIncluir);

            if (pizzaDto.Ingredients != null)
            {
                foreach (var ingredienteDto in pizzaDto.Ingredients)
                {
                    var ingrediente = _ingredienteServico.GetById(ingredienteDto.Id);
                    pizzaIncluir.AddIngredient(ingrediente);
                }
            }

            _pizzaServico.Save(pizzaIncluir);
            return "Pizza [" + pizzaIncluir.Id + "] incluída com sucesso!";
        }

        // PUT /api/pizza/5
        public string Put(int id, PizzaDto pizzaDto)
        {
            // pesquisa a pizza no banco de dados
            // limpa seus filhos
            // e salva...
            var pizzaAlterar = _pizzaServico.GetById(id);
            pizzaAlterar.Name = pizzaDto.Name;

            var ingredientesJaExistiam = pizzaAlterar.Ingredients;
            var ingredienteChegando = pizzaDto.Ingredients;

            AlterarListaManyToMany(ingredienteChegando, ingredientesJaExistiam);

            _pizzaServico.Save(pizzaAlterar);

            return "Pizza [" + pizzaAlterar.Id + "] salva com sucesso!";
        }

        private void AlterarListaManyToMany(IList<IngredientDto> ingredienteChegando, IList<Ingredient> ingredientesJaExistiam)
        {
            if (ingredienteChegando != null)
            {
                // incluir itens novos
                foreach (var ingChegando in ingredienteChegando)
                {
                    // o item é novo
                    if (!ingredientesJaExistiam.Any(x => x.Id == ingChegando.Id))
                    {
                        // temos que adicionar o novo
                        var ingDoBanco = _ingredienteServico.GetById(ingChegando.Id);
                        ingredientesJaExistiam.Add(ingDoBanco);
                    }
                }
                // excluir os que foram retirados
                for (int i = ingredientesJaExistiam.Count - 1; i >= 0; i--)
                {
                    var ingJaExistia = ingredientesJaExistiam[i];
                    // o item foi removido
                    if (!ingredienteChegando.Any(x => x.Id == ingJaExistia.Id))
                    {
                        // retira da lista do banco
                        ingredientesJaExistiam.RemoveAt(i);
                    }
                }
            }
            else
            {
                // a nova lista não possuia nenhum item
                ingredientesJaExistiam.Clear();
            }
        }

        // DELETE /api/pizza/5
        public string Delete(int id)
        {
            var pizzaExcluir = _pizzaServico.GetById(id);
            pizzaExcluir.Ingredients.Clear();
            _pizzaServico.Save(pizzaExcluir);

            _pizzaServico.Delete(id);
            return "Pizza [" + id + "] apagada com sucesso!";
        }
    }
}