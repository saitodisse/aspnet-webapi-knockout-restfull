using FluentNHibernate.Mapping;
using PizzaModel.Entities;

namespace PizzaNHibernate.Mappings
{
    public class IngredientMap : ClassMap<Ingredient>
    {
        public IngredientMap()
        {
            Id(x => x.Id).Column("Id").GeneratedBy.Native();
            Map(x => x.Name);
        }
    }
}