using FluentNHibernate.Mapping;
using PizzaModel.Entities;

namespace PizzaNHibernate.Mappings
{
    public class PizzaMap : ClassMap<Pizza>
    {
        public PizzaMap()
        {
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.Name);

            HasManyToMany(x => x.Ingredients).Table("Pizza_Ingredients").ParentKeyColumn("Pizza_id").ChildKeyColumn("Ingredients_id");
        }
    }
}