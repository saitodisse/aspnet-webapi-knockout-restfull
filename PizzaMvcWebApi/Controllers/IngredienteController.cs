using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using PizzaModel.Entities;
using PizzaModel.Services;
using Pizzaria.DTOs;

namespace PizzaMvcWebApi.Controllers
{
    public class IngredientController : ApiController
    {
        private readonly IIngredientService _IngredientServico;

        public IngredientController(IIngredientService ingredientServico)
        {
            _IngredientServico = ingredientServico;
            CriarMapeamentosDto();
        }

        private static void CriarMapeamentosDto()
        {
            Mapper.CreateMap<Ingredient, IngredienteDto>();
            Mapper.CreateMap<Ingredient, IngredienteDto>();
        }


        // GET /api/<controller>
        public IList<IngredienteDto> Get()
        {
            var Ingredients = _IngredientServico.GetAll();

            IList<IngredienteDto> IngredientDtos = Mapper.Map<IList<Ingredient>, IList<IngredienteDto>>(Ingredients);

            return IngredientDtos;
        }

        // GET /api/<controller>/5
        public IngredienteDto Get(int id)
        {
            var Ingredient = _IngredientServico.GetById(id);

            var IngredientDto = Mapper.Map<Ingredient, IngredienteDto>(Ingredient);

            return IngredientDto;
        }

        // POST /api/<controller>
        public int Post(IngredienteDto IngredientDto)
        {
            var IngredientIncluir = new Ingredient();
            IngredientIncluir.Name = IngredientDto.Nome;
            _IngredientServico.Save(IngredientIncluir);
            return IngredientIncluir.Id;

        }

        // PUT /api/<controller>/5
        public string Put(int id, IngredienteDto IngredientDto)
        {
            var IngredientAlterar = _IngredientServico.GetById(id);
            IngredientAlterar.Name = IngredientDto.Nome;
            _IngredientServico.Save(IngredientAlterar);
            return "Ingredient [" + IngredientAlterar.Id + "] alterado com sucesso!";
        }

        // DELETE /api/<controller>/5
        public string Delete(int id)
        {
            _IngredientServico.Delete(id);
            return "Ingredient [" + id + "] apagado com sucesso!";
        }
    }
}