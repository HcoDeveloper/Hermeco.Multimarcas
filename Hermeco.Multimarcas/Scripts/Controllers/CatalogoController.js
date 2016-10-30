var CatalogoController =  ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams, $rootScope) {
    $('body').removeClass('loaded');
    $http.get(apiBaseUrl + '/oferta/?oferta=' + $routeParams.oferta)
    .success(function (data, status, headers) {
        $scope.ofertas = data;
        $scope.ofertaActiva = headers('ofertaActiva');
        console.log($scope.ofertaActiva);
    })
    .error(function (error, status) {
        window.location.replace(apiBaseUrl);
    })
    .finally(function () {
        //$('body').addClass('loaded');
    });

    $http.get(apiBaseUrl + '/Account')
       .then(function (response) {
           $scope.infoCliente = response.data;
       });
    function load(page) {
        if ($scope.running) return;
        $scope.page = page;
        $scope.running = true;
        $scope.loadSpin = true;
        //$scope.loading = true;
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
            $('body').addClass('loaded');
            $scope.loadSpin = false;
            $scope.running = false;
        });
    }

    // Register event handler
    $scope.$on('endlessScroll:next', function () {
        $scope.page = $scope.page + 1;
        load($scope.page);
    });


    $scope.AsignAllTallas = function (color) {
        console.log(color);
        var elements = document.getElementsByName(color);
        var total = 0;
        for (i = 1; i < elements.length; i++) {
            elements[i].value = elements[0].value;
            total += Number(elements[0].value);
        }
        elements[elements.length-1].value = total;
    }

    //$scope.$on('endlessScroll:previous', function () {
    //    $scope.page = $scope.page - 1;
    //    load($scope.page);
    //});
    load(1);

    $scope.loadReferencia = function (oferta, refid) {
        $http.get(apiBaseUrl + '/Referencia/?oferta=' + oferta + "&refid=" + refid + "&loadImages=true")
            .success(function (data, status, headers) {
                $scope.refdetail = data;
            });
    }


    
}]

CatalogoController.$inject = ['$scope', '$http', '$routeParams', '$rootScope'];
