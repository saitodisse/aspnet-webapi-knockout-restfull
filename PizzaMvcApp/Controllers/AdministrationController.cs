using System.Web.Mvc;
using PizzaModel.Entities;
using PizzaModel.Services;
using PizzaNHibernate.Repos;

namespace PizzaMvcApp.Controllers
{
    public class AdministrationController : Controller
    {
        private IAdminRepo _adminRepo;
        private IIngredientService _ingredientService;
        private IPizzaService _pizzaService;

        public AdministrationController(IAdminRepo adminRepo, IIngredientService ingredientService, IPizzaService pizzaService)
        {
            _adminRepo = adminRepo;
            _ingredientService = ingredientService;
            _pizzaService = pizzaService;
        }

        //
        // GET: /Ingredient/

        public ActionResult RecreateDataBase()
        {
            return View();
        }

        [HttpPost]
        public string RecreateDataBase(FormCollection collection)
        {
            string password = collection["txtPassword"];
            if (password == null
            || password != "I am sure")
            {
                return "wrong password";
            }

            _adminRepo.RecreateDataBase();
            InsertStubData();

            return "ok";
        }

        private void InsertStubData()
        {
            // Insere cada um dos ingredientes
            var cebola = new Ingredient { Name = "Cebola" };
            _ingredientService.Save(cebola);

            var muçarela = new Ingredient { Name = "Muçarela" };
            _ingredientService.Save(muçarela);

            var molhoDeTomate = new Ingredient { Name = "Molho de Tomate" };
            _ingredientService.Save(molhoDeTomate);

            var ovo = new Ingredient { Name = "Ovo" };
            _ingredientService.Save(ovo);

            var calabreza = new Ingredient { Name = "Calabresa" };
            _ingredientService.Save(calabreza);


            var pizza = new Pizza { Name = "Portuguesa" };

            pizza.AddIngredient(molhoDeTomate);
            pizza.AddIngredient(cebola);
            pizza.AddIngredient(ovo);
            _pizzaService.Save(pizza);

            pizza = new Pizza { Name = "Calabresa" };
            pizza.AddIngredient(molhoDeTomate);
            pizza.AddIngredient(cebola);
            pizza.AddIngredient(calabreza);
            _pizzaService.Save(pizza);

            pizza = new Pizza { Name = "Muçarela" };
            pizza.AddIngredient(molhoDeTomate);
            pizza.AddIngredient(muçarela);
            _pizzaService.Save(pizza);

            pizza = new Pizza { Name = "Pizza de vento" };
            _pizzaService.Save(pizza);
        }
    }
}
