
var MultimarcasApp = angular.module('MultimarcasApp', ['ngRoute', 'dc.endlessScroll']);
MultimarcasApp.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $routeProvider
        .when('/', {
            templateUrl: '/Home/Ofertas',
            controller: 'OfertaController'
        })
        .when('/Catalogo/:oferta', {
            templateUrl: '/Home/Catalogo',
            controller: 'CatalogoController',
            
        })
        .otherwise({
           redirectTo: '/'
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




