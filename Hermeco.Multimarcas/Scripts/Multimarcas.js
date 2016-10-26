
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

MultimarcasApp.run(function ($rootScope, $http, $location) {

    $rootScope.alerts = [];

    $rootScope.addAlert = function (tipo, message) {
        $rootScope.$apply(function () {
            $rootScope.alerts.push({ type: tipo, msg: message });
        });
    };

    $rootScope.closeAlert = function (index) {
        $rootScope.alerts.splice(index, 1);
    };

    $http.get(apiBaseUrl + '/Account')
       .then(function (response) {
           $rootScope.infoCliente = response.data;
       });
});

MultimarcasApp.controller('OfertaController', OfertaController);


MultimarcasApp.
  filter('capitalize', function () {
      return function (input, all) {
          return input.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
      }
  });
