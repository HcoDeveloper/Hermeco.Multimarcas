var BusquedaController =  ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams, $rootScope) {
    $http.get(apiBaseUrl + '/oferta/?oferta=' + $routeParams.oferta)
    .success(function (data, status, headers) {
        $scope.ofertas = data;
        $scope.ofertaActiva = headers('ofertaActiva');
        console.log($scope.ofertaActiva);
    })

    $http.get(apiBaseUrl + '/catalogo/?refid=' + $routeParams.refid)
    .success(function (data, status, headers) {
        $scope.referencias = data;
    }).finally(function () {
        // Flag loading as complete
        $('body').addClass('loaded');
        //$scope.loading = false;
        $scope.running = false;
    });
    
    $scope.loadReferencia = function (oferta, refid) {
        $http.get(apiBaseUrl + '/Referencia/?oferta=' + oferta + "&refid=" + refid + "&loadImages=true")
            .success(function (data, status, headers) {
                $scope.refdetail = data;
            });
    }

    $scope.AsignAllTallas = function (color) {
        console.log(color);
        var elements = document.getElementsByName(color);
        var total = 0;
        for (i = 1; i < elements.length; i++) {
            elements[i].value = elements[0].value;
            total += Number(elements[0].value);
        }
        elements[elements.length - 1].value = total;
    }
}];