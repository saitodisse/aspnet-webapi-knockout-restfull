// ////////////////////////
//  IngredientVM :: VIEWMODEL
// ////////////////////////
var IngredientVM = function (ingredient) {
    var self = this;
    self.Id = ko.observable(0);
    self.Name = ko.observable("");

    if (!_.isUndefined(ingredient)) {
        self.Id = ko.observable(ingredient.Id);
        self.Name = ko.observable(ingredient.Name);
    }
};

// //////////////////////////////////////////////////////////////////////////////
//  MAIN :: VIEWMODEL
//  Sets observable items
//  controller.VmKO:
//   - list, select, id, selected, wasUpdated, delete, create, save, updating
// //////////////////////////////////////////////////////////////////////////////
var MainViewModel = function (ingredientsDto) {
    var self = this;

    var printLog = function (data) {
        var stringify;
        if (_.isObject(data)) {
            $("#preLog").append("JS received:\n");
            stringify = JSON.stringify(data, null, 2);
        }
        else {
            stringify = data;
        }
        $("#preLog").append(stringify);
        $("#preLog").append("\n\n");
    }

    var ajax_get_list_local = function (data) {
        printLog(data);
        ko.applyBindings(self); // This makes Knockout get to work
    };

    var ajax_save_local = function (data) {
        printLog(data);
    };

    var ajax_delete_local = function (data) {
        printLog(data);
    };

    var ajax_error_local = function (jqXHR) {
        printLog(jqXHR.responseText);
    };

    var ingredientController = new knockoutControllerInit({
        viewMoldel: self.ingredientVm = {},
        controllerName: "ingredient",
        webSite: "http://localhost/PizzaMvcWebApi/api",
        dtoData: ingredientsDto,
        viewModelClass: IngredientVM,
        ajax_get_list: ajax_get_list_local,
        ajax_save: ajax_save_local,
        ajax_delete: ajax_delete_local,
        ajax_error: ajax_error_local
    });


};

// //////////////////////////////////////////////////////////////////////////////
//  READY: Initializes knockout
// //////////////////////////////////////////////////////////////////////////////
var mainViewModel;
$().ready(function() {
    mainViewModel = new MainViewModel();

    $("#buttonShowDebugInfo").click(function(){
        $("#divLog").toggle();
        $("#divDebug").toggle();
    });
});

