
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

MultimarcasApp.run(function ($rootScope, $http) {
    $http.get(apiBaseUrl + '/Account')
       .then(function (response) {
           $rootScope.infoCliente = response.data;
       });
});

MultimarcasApp.controller('OfertaController', OfertaController);




