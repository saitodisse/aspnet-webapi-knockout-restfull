/// <reference path="../../Scripts/backbone.js" />
/// <reference path="../../Scripts/jquery-1.7.2-vsdoc.js" />
/// <reference path="../../Scripts/underscore.js" />

/* -----------------------------------------------------------------------------------------------------------
::::::::::::::::::::::::::::::::
:: Backbone.js Wine Cellar Tutorial ::
::::::::::::::::::::::::::::::::

>> http://coenraets.org/blog/2011/12/backbone-js-ingredient-cellar-tutorial-part-1-getting-started/

>> http://coenraets.org/blog/2011/12/backbone-js-wine-cellar-tutorial-part-2-crud/

>> http://coenraets.org/blog/2011/12/backbone-js-wine-cellar-tutorial-part-3-deep-linking-and-application-states/

>> http://coenraets.org/blog/2012/05/single-page-crud-application-with-backbone-js-and-twitter-bootstrap/

----------------------------------------------------------------------------------------------------------- */





$(function () {

    //Models
    var Ingredient = Backbone.Model.extend({
        idAttribute: "Id",
        defaults: {"Id": null,"Name": ""},
        urlRoot: "../../PizzaMvcWebApi/api/ingredient"
    });

    var IngredientCollection = Backbone.Collection.extend({
        model: Ingredient,
        url: "../../PizzaMvcWebApi/api/ingredient"
    });


    // Views
    var IngredientListView = Backbone.View.extend({
        tagName: 'ul',

        initialize: function () {
            this.model.bind("reset", this.render, this);
            var self = this;
            this.model.bind("add", function (ingredient) {
                $(self.el).append(new IngredientListItemView({ model: ingredient }).render().el);
            });
        },

        render: function (eventName) {
            _.each(this.model.models, function (ingredient) {
                var ingrListView = new IngredientListItemView({ model: ingredient });
                $(this.el).append(ingrListView.render().el);
            }, this);
            return this;
        }
    });

    var IngredientListItemView = Backbone.View.extend({
        tagName: "li",

        template: _.template($('#tpl-ingredient-list-item').html()),

        initialize: function () {
            this.model.bind("change", this.render, this);
            this.model.bind("destroy", this.close, this);
        },

        render: function (eventName) {
            $(this.el).html(this.template(this.model.toJSON()));
            return this;
        },

        close: function () {
            $(this.el).unbind();
            $(this.el).remove();
        }
    });

    var IngredientView = Backbone.View.extend({
        template: _.template($('#tpl-ingredient-details').html()),

        initialize: function () {
            this.model.bind("change", this.render, this);
        },

        render: function (eventName) {
            $(this.el).html(this.template(this.model.toJSON()));
            return this;
        },

        events: {
            "change input": "change",
            "click #save": "saveIngredient",
            "click #delete": "deleteIngredient"
        },

        change: function (event) {
            var target = event.target;
            console.log('changing ' + target.id + ' from: ' + target.defaultValue + ' to: ' + target.value);
            // You could change your model on the spot, like this:
            // var change = {};
            // change[target.name] = target.value;
            // this.model.set(change);
        },

        saveIngredient: function () {
            this.model.set({
                Name: $('#Name').val()
            });

            /*
            The problem: Add a new Ingredient, and click Save. The id that 
            has been assigned to the newly created wine appears in the form 
            field. However the URL is still:
            http://localhost/backbone-cellar/part2/ when it should really be: 
            http://localhost/backbone-cellar/part2/#ingredients/[id].

            You can easily fix that issue by using the router’s navigate 
            function to change the URL. The second argument (false), 
            indicates that we actually don’t want to “execute” that route: we 
            just want to change the URL.
            */
            if (this.model.isNew()) {
                var self = this;
                app.ingredientList.create(this.model, {
                    success: function () {
                        app.navigate('ing/' + self.model.id, false);
                    }
                });
            } else {
                this.model.save();
            }
            return false;
        },

        deleteIngredient: function () {
            this.model.destroy({
                success: function () {
                    //alert('Ingredient deleted successfully');
                    window.history.back();
                }
            });
            return false;
        },

        close: function () {
            $(this.el).unbind();
            $(this.el).empty();
        }
    });

    /*
    Backbone.js Views are typically used to render domain models 
    (as done in IngredientListView, IngredientListItemView, and Ingredient View). But 
    they can also be used to create composite UI components. For 
    example, in this application, we define a Header View (a 
    toolbar) that could be made of different components and that 
    encapsulates its own logic.
    */
    var HeaderView = Backbone.View.extend({
        template: _.template($('#tpl-header').html()),

        initialize: function () {
            this.render();
        },

        render: function (eventName) {
            $(this.el).html(this.template());
            return this;
        },

        events: {
            "click #new": "newIngredient"
        },

        newIngredient: function (event) {
            app.navigate("ing/new", true);
            return false;
        }
    });

    // Router
    var AppRouter = Backbone.Router.extend({
        routes: {
            "": "list",
            "ing/new": "newIngredient",
            "ing/:id": "ingredientDetails"
        },

        initialize: function () {
            $('#header').html(new HeaderView().render().el);
        },

        list: function () {
            this.ingredientList = new IngredientCollection();
            var self = this;
            this.ingredientList.fetch({
                success: function () {
                    self.ingredientListView = new IngredientListView({ model: self.ingredientList });
                    $('#sidebar').html(self.ingredientListView.render().el);
                    if (self.requestedId) self.ingredientDetails(self.requestedId);
                }
            });
        },

/*
Another approach is to check if the collection exists in the 
ingredientDetails function. If it does, we simply “get” the requested 
item and render it as we did before. If it doesn’t, we store the 
requested id in a variable, and then invoke the existing list() 
function to populate the list. We then modify the list function: 
When we get the list from the server (on success), we check if 
there was a requested id. If there was, we invoke the wineDetails 
function to render the corresponding item.
*/
        ingredientDetails: function (id) {
            if (this.ingredientList) {
                this.ingredient = this.ingredientList.get(id);
                if (this.ingredientView) this.ingredientView.close();
                this.ingredientView = new IngredientView({ model: this.ingredient });
                $('#divDetails').html(this.ingredientView.render().el);
            } else {
                this.requestedId = id;
                this.list();
            }
        },

        newIngredient: function () {
            if (app.ingredientView) app.ingredientView.close();
            app.ingredientView = new IngredientView({ model: new Ingredient() });
            $('#divDetails').html(app.ingredientView.render().el);
        }
    });

    var app = new AppRouter();
    Backbone.history.start();

});