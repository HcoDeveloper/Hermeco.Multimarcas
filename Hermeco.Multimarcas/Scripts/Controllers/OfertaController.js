var OfertaController = ['$scope', '$http', function ($scope, $http) {
    $http.get(apiBaseUrl + '/oferta')
       .then(function (response) {
           $scope.ofertas = response.data;
       });
}]

OfertaController.$inject = ['$scope', '$http'];
