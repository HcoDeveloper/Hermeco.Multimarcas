
var CatalogoController = function ($scope, $http, $routeParams) {

    $http.get(apiBaseUrl + '/oferta')
       .then(function (response) {
           $scope.ofertas = response.data;
       });
    
    $http.get(apiBaseUrl + '/Account')
       .then(function (response) {
           $scope.infoCliente = response.data;
       });

    /*
    $http.get(apiBaseUrl + '/catalogo/?oferta=' + $routeParams.oferta)

        .then(function (response) {
            $scope.referencias = response.data;
        });
    */
    function load(page) {
        if ($scope.running) return;
        $scope.page = page;
        $scope.running = true;
        $scope.loading = true;
        $http.get(apiBaseUrl + '/catalogo/?oferta=' + $routeParams.oferta + "&page=" + page)
        .success(function (data, status, headers) {
            // Create an array if not already created
            $scope.pages = headers('pages');
            $scope.referencias = $scope.referencias || [];

            // Append new items (or prepend if loading previous pages)
            $scope.referencias.push.apply($scope.referencias, data);
        })
        .finally(function () {
            // Flag loading as complete
            $scope.loading = false;
            $scope.running = false;
        });
    }

    // Register event handler
    $scope.$on('endlessScroll:next', function () {
        $scope.page = $scope.page + 1;
        load($scope.page);
    });

    $scope.$on('endlessScroll:previous', function () {
        $scope.page = $scope.page - 1;
        load($scope.page);
    });
    load(1);

    $scope.loadReferencia = function (oferta, refid) {
        $http.get(apiBaseUrl + '/Referencia/?oferta=' + oferta + "&refid=" + refid + "&loadImages=true")
            .success(function (data, status, headers) {
                $scope.refdetail = data;
            });
    }

}

CatalogoController.$inject = ['$scope', '$http', '$routeParams'];
