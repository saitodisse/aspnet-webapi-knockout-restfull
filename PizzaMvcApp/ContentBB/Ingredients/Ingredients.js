/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    sample from : http://coenraets.org/blog/2011/12/backbone-js-wine-cellar-tutorial-part-1-getting-started/
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 

// Models
/*
    IngredientModel: Notice that we don’t need to 
    explicitly define the attributes (name, country, year, etc). 
    You could add validation, default values, etc. More on that in Part 2.
*/
window.Ingredient = Backbone.Model.extend();

/*
IngredientCollection: “model” 
indicates the nature of the collection. “url” 
provides the endpoint for the RESTFul API. This is 
all that’s needed to retrieve, create, update, and 
delete ingredients with Backbone’s simple Model API.
*/
window.IngredientCollection = Backbone.Collection.extend({
    model: Ingredient,
    url: "../../PizzaMvcWebApi/api/ingredient"
});


// Views
/*
IngredientListView: The render() 
function iterates through the collection, 
instantiates a IngredientListItemView for each 
ingredient in the collection, and adds it to the 
ingredientList.
*/
window.IngredientListView = Backbone.View.extend({

    tagName: 'ul',

    initialize: function () {
        this.model.bind("reset", this.render, this);
    },

    render: function (eventName) {
        _.each(this.model.models, function (ingredient) {
            $(this.el).append(new IngredientListItemView({ model: ingredient }).render().el);
        }, this);
        return this;
    }

});

/*
IngredientListItemView: The render() 
function merges the model data into the 
“ingredient-list-item” template (defined in 
index.html). By defining a separate View for list 
items, you will make it easy to update (re-render) a 
specific list item when the backing model changes 
without re-rendering the entire list. More on that in 
Part 2.
*/
window.IngredientListItemView = Backbone.View.extend({

    tagName: "li",

    template: _.template($('#tpl-ingredient-list-item').html()),

    render: function (eventName) {
        $(this.el).html(this.template(this.model.toJSON()));
        return this;
    }

});

/*
IngredientView: The view responsible 
for displaying the ingredient details in the 
Ingredient form. The render() function merges the 
model data (a specific ingredient) into the 
“ingredient-details” template retrieved from 
index.html.
*/
window.IngredientView = Backbone.View.extend({

    template: _.template($('#tpl-ingredient-details').html()),

    render: function (eventName) {
        $(this.el).html(this.template(this.model.toJSON()));
        return this;
    }

});


// Router
/*
AppRouter (lines 52 to 71): Provides the entry points 
for the application through a set of (deep-linkable) 
URLs. Two routes are defined: The default route (“”) 
displays the list of ingredient. The 
“ingredients/:id” route displays the details of a 
specific ingredient in the ingredient form. Note that 
in Part 1, this route is not deep-linkable. You have 
to start the application with the default route and 
then select a specific ingredient. In Part 3, you 
will make sure you can deep-link to a specific 
ingredient.
*/
var AppRouter = Backbone.Router.extend({

    routes: {
        "": "list",
        "ingredient/:id": "ingredientDetails"
    },

    list: function () {
        this.ingredientList = new IngredientCollection();
        this.ingredientListView = new IngredientListView({ model: this.ingredientList });
        this.ingredientList.fetch();
        $('#sidebar').html(this.ingredientListView.render().el);
    },

    ingredientDetails: function (id) {
        this.ingredient = this.ingredientList.get(id);
        this.ingredientView = new IngredientView({ model: this.ingredient });
        $('#content').html(this.ingredientView.render().el);
    }
});

var app = new AppRouter();
Backbone.history.start();