using System.Collections.Generic;
using PizzaModel.Entities;

namespace PizzaModel.Repos
{
    public interface IPizzaDAO : IRepo<Pizza>
    {
        IList<Pizza> GetByName(string name);
    }
}