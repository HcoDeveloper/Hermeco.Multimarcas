var PedidoController = ['$scope', '$http', '$routeParams',function ($scope, $http, $routeParams) {

    $http.get(apiBaseUrl + '/pedido')
       .then(function (response) {
           $scope.refPedido = response.data;
       });

    $scope.GetSummGroup = function (group) {
        var summ = 0;
        for (var i in group) {
            summ = summ + Number(group[i].Cantidad);
        }
        return summ;
    };

    $scope.lessThan = function (prop, val) {
        return function (item) {
            return item[prop] > val;
        }
    }
}]

PedidoController.$inject = ['$scope', '$http', '$routeParams'];