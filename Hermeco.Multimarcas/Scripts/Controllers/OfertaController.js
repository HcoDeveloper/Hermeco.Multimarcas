var OfertaController = ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
    $http.get(apiBaseUrl + '/oferta/?oferta=' + $routeParams.oferta)
       .success(function (response, status, headers) {
           $scope.ofertaActiva = headers('ofertaActiva');
           $scope.ofertas = response;
           console.log($scope.ofertaActiva);
       })
        .error(function (response, status) {
            //alert(response);
        })
       .finally(function () {
           // Flag loading as complete
           $scope.loading = false;
           $scope.running = false;
       });

}]

OfertaController.$inject = ['$scope', '$http'];
