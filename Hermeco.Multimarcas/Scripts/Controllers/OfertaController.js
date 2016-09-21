var OfertaController = function ($scope, $http) {
    $http.get(apiBaseUrl+'/oferta').
       then(function (response) {
           $scope.ofertas = response.data;
       });
    $scope.models = {
        helloAngular: 'I work!'
    }
}

OfertaController.$inject = ['$scope', '$http'];
