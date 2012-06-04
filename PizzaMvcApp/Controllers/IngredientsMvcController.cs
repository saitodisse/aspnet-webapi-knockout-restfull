using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PizzaModel.Entities;
using PizzaModel.Services;

namespace PizzaMvcApp.Controllers
{
    public class IngredientsMvcController : Controller
    {
        private IIngredientService _ingredientService;

        public IngredientsMvcController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        //
        // GET: /IngredientsMvc/

        public ActionResult Index()
        {
            IList<Ingredient> ingredients = _ingredientService.GetAll();
            return View(ingredients);
        }

        //
        // GET: /IngredientsMvc/Details/5

        public ActionResult Details(int id)
        {
            return View(_ingredientService.GetById(id));
        }

        //
        // GET: /IngredientsMvc/Create

        public ActionResult Create()
        {
            return View(new Ingredient());
        }

        //
        // POST: /IngredientsMvc/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
                Ingredient ingredient = new Ingredient();
                ingredient.Name = name;
                _ingredientService.Save(ingredient);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /IngredientsMvc/Edit/5

        public ActionResult Edit(int id)
        {
            Ingredient ingredient = _ingredientService.GetById(id);
            return View(ingredient);
        }

        //
        // POST: /IngredientsMvc/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Ingredient ingredient = _ingredientService.GetById(id);
                string name = collection["Name"];
                ingredient.Name = name;

                _ingredientService.Save(ingredient);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /IngredientsMvc/Delete/5

        public ActionResult Delete(int id)
        {
            Ingredient ingredient = _ingredientService.GetById(id);
            return View(ingredient);
        }

        //
        // POST: /IngredientsMvc/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _ingredientService.Delete(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.message = ex.Message;
                return View();
            }
        }
    }
}
