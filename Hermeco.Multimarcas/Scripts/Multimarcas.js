
var MultimarcasApp = angular.module('MultimarcasApp', ['ngRoute', 'dc.endlessScroll']);
MultimarcasApp.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $routeProvider
        .when(webBaseUrl, {
            templateUrl: webBaseUrl + 'Home/Ofertas',
            controller: 'OfertaController'
        })
        .when(webBaseUrl + 'Catalogo/:oferta', {
            templateUrl: webBaseUrl + 'Home/Catalogo',
            controller: 'CatalogoController',
            
        })
        .when(webBaseUrl + 'Pedido', {
            templateUrl: webBaseUrl + 'Home/Pedido',
            controller: 'PedidoController',

        })
        .when(webBaseUrl + 'Busqueda', {
            templateUrl: webBaseUrl + 'Home/Busqueda',
            controller: 'BusquedaController',

        })
        .when(webBaseUrl + 'Logout', {
            templateUrl: webBaseUrl + 'Account/Logout',
            controller: 'OfertaController'
        })
        .otherwise({
            redirectTo: webBaseUrl
        });
        $locationProvider.html5Mode(true);
    }
])

MultimarcasApp.directive("error", function ($rootScope) {
    return {
        restrict: "E",
        template: webBaseUrl + 'Account/Logout',
        link: function (scope) {
            $rootScope.$on("routeeChangeError", function (event, current, previous, rejection) {
                console.log(event);
            })
        }
    }
})

MultimarcasApp.run(function ($rootScope, $http, $location, $route) {

    $rootScope.alerts = [];

    $rootScope.addAlert = function (tipo, message) {
        $rootScope.$apply(function () {
            $rootScope.alerts.push({ type: tipo, msg: message });
        });
    };

    $rootScope.closeAlert = function (index) {
        $rootScope.alerts.splice(index, 1);
    };

    $rootScope.refreshPage = function () {
        $route.reload();
    };

    $rootScope.validarNumero = function (event, fieldName) {
        var keycode = event.which;
        if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
            event.preventDefault();
        }
        cantidad = document.getElementById('cantidad' + fieldName);
        stock = document.getElementById('stock' + fieldName);
        //value = cantidad.value.concat(event.key);
        if (cantidad.selectionStart == 0)
            inverseValue = event.key.concat(cantidad.value);
        else
            inverseValue = cantidad.value.substring(0, cantidad.selectionStart) + event.key + cantidad.value.substring(cantidad.selectionStart, cantidad.value.length)
        if (Number(inverseValue) > Number(stock.value)) {
            event.preventDefault();
        }
        console.log(cantidad.selectionStart);
        console.log(event);
        console.log(fieldName);
    }

    $http.get(apiBaseUrl + '/Account')
       .then(function (response) {
           $rootScope.infoCliente = response.data;
       });

    $rootScope.$on("routeeChangeError", function (event, current, previous, rejection) {
        console.log(event);
    });

    $rootScope.formatNumber = function (number) {
        return number.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
    }
});



MultimarcasApp.controller('OfertaController', OfertaController);


MultimarcasApp.
  filter('capitalize', function () {
      return function (input, all) {
          return input.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
      }
  });
