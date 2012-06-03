using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using PizzaModel.Entities;
using PizzaModel.Services;
using PizzaMvcWebApi.DTOs;

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
            Mapper.CreateMap<Ingredient, IngredientDto>();
            Mapper.CreateMap<Ingredient, IngredientDto>();
        }


        // GET /api/<controller>
        public IList<IngredientDto> Get()
        {
            var Ingredients = _IngredientServico.GetAll();

            IList<IngredientDto> IngredientDtos = Mapper.Map<IList<Ingredient>, IList<IngredientDto>>(Ingredients);

            return IngredientDtos;
        }

        // GET /api/<controller>/5
        public IngredientDto Get(int id)
        {
            var Ingredient = _IngredientServico.GetById(id);

            var IngredientDto = Mapper.Map<Ingredient, IngredientDto>(Ingredient);

            return IngredientDto;
        }

        // POST /api/<controller>
        public int Post(IngredientDto IngredientDto)
        {
            var IngredientIncluir = new Ingredient();
            IngredientIncluir.Name = IngredientDto.Name;
            _IngredientServico.Save(IngredientIncluir);
            return IngredientIncluir.Id;

        }

        // PUT /api/<controller>/5
        public string Put(int id, IngredientDto IngredientDto)
        {
            var IngredientAlterar = _IngredientServico.GetById(id);
            IngredientAlterar.Name = IngredientDto.Name;
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