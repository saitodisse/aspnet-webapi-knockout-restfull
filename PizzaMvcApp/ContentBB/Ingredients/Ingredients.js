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

    var self = {};


    //Models
    var Ingredient = Backbone.Model.extend({
        idAttribute: "Id",

        /*
        defaults: Default values used when a new instance of the model 
        is created. This attribute is optional. However, it was 
        required in this application for the ingredient-details template to 
        render an ‘empty’ ingredient model object (which happens when adding 
        a new ingredient).
        */
        defaults: {
            "Id": null,
            "Name": ""
        }

        /*
        urlRoot: RESTful service endpoint to retrieve or persist Model 
        data. Note that this attribute is only needed when 
        retrieving/persisting Models that are not part of a Collection. 
        If the Model is part of a Collection, the url attribute defined 
        in the Collection is enough for Backbone.js to know how to 
        retrieve, update, or delete data using your RESTful API.

        //,urlRoot: "../../PizzaMvcWebApi/api/ingredient"

        */
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
            /*
            When a new ingredient is added, you want it to automatically appear 
            in the list. To make that happen, you bind the View to the add 
            event of the IngredientListView model (which is the collection of ingredients).
            
            When that event is fired, a new instance of IngredientListItemView is 
            created and added to the list.
            */
            this.model.bind("add", function (ingredient) {
                $(this.el).append(new IngredientListItemView({ model: ingredient }).render().el);
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
            /*
            When a ingredient is changed, you want the corresponding 
            IngredientListItemView to re-render automatically to reflect the 
            change. To make that happen, you bind the View to the change 
            event of its model, and execute the render function when the 
            event is fired.
            */
            this.model.bind("change", this.render, this);

            /*
            Similarly, when a ingredient is deleted, you want the list item to be 
            removed automatically. To make that happen, you bind the view 
            to the destroy event of its model and execute our custom close 
            function when the event is fired. 
            */
            this.model.bind("destroy", this.close, this);
        },

        render: function (eventName) {
            $(this.el).html(this.template(this.model.toJSON()));
            return this;
        },

        close: function () {
            /*
            To avoid memory leaks and 
            events firing multiple times, it is important to unbind the 
            event listeners before removing the list item from the DOM.

            Note that in either case we don’t have the overhead of 
            re-rendering the entire list: we only re-render or remove the 
            list item affected by the change.
            */
            $(this.el).unbind();
            $(this.el).remove();
        }
    });

    /*
    In the spirit of encapsulation, the event handlers for the Save 
    and Delete buttons are defined inside IngredientView, as opposed to 
    defining them as free-hanging code blocks outside the “class” 
    definitions. You use the Backbone.js Events syntax which uses 
    jQuery delegate mechanism behind the scenes.

    There are always different approaches to update the model based 
    on user input in a form:

    “Real time” approach: you use the change handler to update the 
    model as changes are made in the form. This is in essence 
    bi-directional data binding: the model and the UI controls are 
    always in sync. Using this approach, you can then choose 
    between sending changes to the server in real time (implicit save),
    or wait until the user clicks a Save button (explicit save).
    
    The first option can be chatty and unpractical when 
    there are cross-field validation rules. The second option may 
    require you to undo model changes if the user navigates to 
    another item without clicking Save.

    “Delayed” approach: You wait until the user clicks Save to 
    update the model based on the new values in UI controls, and 
    then send the changes to the server.

    This discussion is not specific to Backbone.js and is therefore 
    beyond the scope of this post. For simplicity, I used the 
    delayed approach here. However I still wired the change event, 
    and use it to log changes to the console. I found this very 
    useful when debugging the application, and particularly to make 
    sure I had cleaned up my bindings (see close function): I you 
    see the change event firing multiple times, you probably didn’t 
    clean up as appropriate.
    */
    self.IngredientView = Backbone.View.extend({
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
            "click .save": "saveIngredient",
            "click .delete": "deleteIngredient"
        },

        change: function (event) {
            var target = event.target;
            console.log('changing ' + target.id
                     + ' from: ' + target.defaultValue
                     + ' to: ' + target.value);
            // You could change your model on the spot, like this:
            // var change = {};
            // change[target.name] = target.value;
            // this.model.set(change);
        },

        saveIngredient: function () {
            this.model.set({
                Name: $('#Name').val()
            });
            if (this.model.isNew()) {
                app.ingredientList.create(this.model);
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
    self.HeaderView = Backbone.View.extend({
        template: _.template($('#tpl-header').html()),

        initialize: function () {
            this.render();
        },

        render: function (eventName) {
            $(this.el).html(this.template());
            return this;
        },

        events: {
            "click .new": "newIngredient"
        },

        newIngredient: function (event) {
            if (app.ingredientView) {
                app.ingredientView.close();
            }
            app.ingredientView = new self.IngredientView({
                model: new Ingredient()
            });
            $('#content').html(app.ingredientView.render().el);
            return false;
        }
    });

    // Router
    var AppRouter = Backbone.Router.extend({
        routes: {
            "": "list",
            ":Id": "ingredientDetails"
        },

        initialize: function () {
            $('#divHeader').html(new self.HeaderView().render().el);
        },

        list: function () {
            this.ingredientList = new IngredientCollection();
            this.ingredientListView = new IngredientListView({ model: this.ingredientList });
            this.ingredientList.fetch();
            $('#divIngredients').html(this.ingredientListView.render().el);
        },

        ingredientDetails: function (id) {
            //this.ingredient = this.ingredientList.where({ Id: parseInt(id) })[0];
            this.ingredient = this.ingredientList.get(id);
            if (app.ingredientView) {
                app.ingredientView.close();
            }
            this.ingredientView = new self.IngredientView({ model: this.ingredient });
            $('#divDetails').html(this.ingredientView.render().el);
        }
    });

    var app = new AppRouter();
    Backbone.history.start();

});