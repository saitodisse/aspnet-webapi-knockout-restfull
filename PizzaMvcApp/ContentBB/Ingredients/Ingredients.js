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
        defaults: { "Id": null, "Name": "" },
        urlRoot: "../../PizzaMvcWebApi/api/ingredient"
    });

    var IngredientCollection = Backbone.Collection.extend({
        model: Ingredient,
        url: "../../PizzaMvcWebApi/api/ingredient"
    });


    // Views
    var IngredientListView = Backbone.View.extend({
        tagName: 'tbody',

        initialize: function () {
            this.model.bind("reset", this.render, this);
            var self = this;
            this.model.bind("add", function (ingredient) {
                var ingredientListItemView = new IngredientListItemView({ model: ingredient });
                var ingEle = ingredientListItemView.render().el;
                $(self.el).append(ingEle);
            });
        },

        render: function (eventName) {
            $(this.el).html('');
            _.each(this.model.models, function (ingredient) {
                var ingrListView = new IngredientListItemView({ model: ingredient });
                $(this.el).append(ingrListView.render().el);
            }, this);
            return this;
        }
    });

    var IngredientListItemView = Backbone.View.extend({
        tagName: "tr",

        template: _.template($('#tpl-ingredient-list-item').html()),

        initialize: function () {
            this.model.bind("change", this.render, this);
            this.model.bind("destroy", this.close, this);
        },

        render: function (eventName) {
            $(this.el).html(this.template(this.model.toJSON()));
            return this;
        },

        events: {
            "click td": "goToDetail"
        },

        goToDetail: function () {
            var id = this.model.get("Id");
            window.location = "#ing/" + id;
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
        },

        saveIngredient: function () {
            this.model.set({
                Name: $('#Name').val()
            });

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
                    window.history.back(); //fixme: porra, que merda é essa?
                }
            });
            return false;
        },

        close: function () {
            $(this.el).unbind();
            $(this.el).empty();
        }
    });

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
                    
                    // limpa a lista
                    $('#tableIngredients tbody').remove();
                    
                    // limpa o detalhe
                    $('#divDetails').html("");
                    
                    // "appenda" a nova lista
                    $('#tableIngredients').append(self.ingredientListView.render().el);
                    if (self.requestedId) {
                        self.ingredientDetails(self.requestedId);  
                    }
                }
            });
        },

        ingredientDetails: function (id) {
            if (this.ingredientList) {
                this.ingredient = this.ingredientList.get(id);

                // limpa o detalhe anterior
                if (this.ingredientView) {
                    this.ingredientView.close();
                }
                this.ingredientView = new IngredientView({ model: this.ingredient });
                $('#divDetails').html(this.ingredientView.render().el);

                // seleciona coluna com mesmo id
                var td = _.find($("#tableIngredients td:first-child"),
                    function (jqueryItem) {
                        return parseInt($(jqueryItem).text()) === this.ingredient.attributes.Id
                    }, this)

                // tira o .linhaSelecionada de todos os TDs
                $("#tableIngredients td").removeClass("linhaSelecionada");

                // acrescenta o .linhaSelecionada na coluna e no seu next
                $(td).addClass("linhaSelecionada").next().addClass("linhaSelecionada");

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