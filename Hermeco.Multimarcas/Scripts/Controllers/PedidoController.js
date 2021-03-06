﻿var PedidoController = ['$scope', '$http', '$routeParams', '$route', '$rootScope' ,function ($scope, $http, $routeParams, $route, $rootScope) {
    $scope.loadSpin = true;

    $http.get(apiBaseUrl + '/pedido')
    .success(function (response) {
        $scope.refPedido = response;
        if ( response.length === 0) {
            $scope.hasCartItems = false;
        } else {
            $scope.hasCartItems = true;
        }
        $scope.loadSpin = false;
    })
     .error(function (error, status) {
        window.location.replace(apiBaseUrl);
    });

    $scope.GetSummGroup = function (group, color) {
        var summ = 0;
        for (var i in group) {
            if (group[i].Color === color) {
                summ = summ + Number(group[i].Cantidad);
            }
        }
        return summ;
    };

    $scope.lessThan = function (prop, val) {
        return function (item) {
            return item[prop] > val;
        }
    }



    $scope.incrementar = function (fieldName) {
            cantidad = document.getElementById('cantidad' + fieldName);
            stock = document.getElementById('stock' + fieldName);
            if (Number( cantidad.value) < Number( stock.value)) {
                cantidad.value = Number(cantidad.value) + 1;
                console.log(fieldName);
            }
    }

    $scope.decrementar = function (fieldName) {
        cantidad = document.getElementById('cantidad' + fieldName);
        if (Number(cantidad.value) > 0) {
            cantidad.value = Number(cantidad.value) - 1;
            console.log(fieldName);
        }
    }
    /*
    $scope.validarNumero = function (event, fieldName) {
        var keycode = event.which;
        if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
            event.preventDefault();
        }
        cantidad = document.getElementById('cantidad' + fieldName);
        stock = document.getElementById('stock' + fieldName);
        //value = cantidad.value.concat(event.key);
        if (cantidad.selectionStart == 0)
            inverseValue = event.key.concat(cantidad.value);
        else
            inverseValue = cantidad.value.substring(0, cantidad.selectionStart) + event.key + cantidad.value.substring(cantidad.selectionStart, cantidad.value.length)
        if (Number( inverseValue) > Number(stock.value)) {
            event.preventDefault();
        }
        console.log(cantidad.selectionStart);
        console.log(event);
        console.log(fieldName);
    }
    */
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

    $scope.loadReferencia = function (oferta, refid) {
        $http.get(apiBaseUrl + '/ItemPedido/?oferta=' + oferta + "&refid=" + refid + "&loadImages=true")
            .success(function (data, status, headers) {
                $scope.refdetail = data;
            });
    }

    $scope.eliminar = function (refeid, color) {
        $http.delete(apiBaseUrl + '/Pedido/?&referencia=' + refeid + "&color=" + color)
            .success(function (data, status, headers) {
                //$scope.addAlert(data.Type, data.Descripcion);
                //$rootScope.addAlert(data.Type, data.Descripcion);
                $route.reload();
            });
    }


    $scope.procesarPedido = function () {
        $http.post(apiBaseUrl + '/OrdenCompra')
            .success(function (data, status, headers) {
                $scope.resultado = data;
            });
    }


}]

PedidoController.$inject = ['$scope', '$http', '$routeParams'];

