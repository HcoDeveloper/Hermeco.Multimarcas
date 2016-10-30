var OfertaController = ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
    $scope.loadSpin = true;
    $http.get(apiBaseUrl + '/oferta/?oferta=' + $routeParams.oferta)
       .success(function (response, status, headers) {
           $scope.ofertaActiva = headers('ofertaActiva');
           $scope.ofertas = response;
           console.log($scope.ofertaActiva);
       })
        .error(function (error, status){
            window.location.replace(apiBaseUrl);
        })
       .finally(function () {
           // Flag loading as complete
           $scope.loading = false;
           $scope.running = false;
           $scope.loadSpin = false;
       });

}]

OfertaController.$inject = ['$scope', '$http'];
