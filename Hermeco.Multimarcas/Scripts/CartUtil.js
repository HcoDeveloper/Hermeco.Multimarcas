function AsignAllColors(element) {
    $(element.parentElement.parentElement).each(function () {
        var value = "";
        var row = this;
        var values = 0;
        var countElements = 0;
        var nElement = 0;
        var flag = 0;
        var first = null;
        var last = null;
        $('input', this).each(function () {
            if (flag === 0) {
                value =Number($(this).val());
                flag = 1;
                first = $(this);
            }
        });
        
        $('input[type=text]', this).each(function () {
            first.val("");
            $(this).val(value);
            last = $(this);
            countElements++;
        });
        last.val(value * (countElements - 2));
    });
}

function AgregarItem() {
    var table = document.getElementById("ItemPedido");
    jsonObj = [];
    for (var i = 1, row; row = table.rows[i]; i++) {

        for (var j = 3, col; col = row.cells[j]; j++) {

            var referencia = col.getElementsByTagName("input")[0];
            var oferta = col.getElementsByTagName("input")[1];
            var plu = col.getElementsByTagName("input")[2];
            var talla = col.getElementsByTagName("input")[3];
            var color = col.getElementsByTagName("input")[4];
            var cantidad = col.getElementsByTagName("input")[5];

            if (plu != undefined || cantidad != undefined) {

                item = {}
                item["Plu"] = plu.value;
                item["Referencia"] = referencia.value;
                item["Oferta"] = oferta.value;
                item["Nit"] = oferta.value;
                item["Talla"] = talla.value;
                item["Color"] = color.value;
                item["Cantidad"] = cantidad.value;

                jsonObj.push(item);
            }

        }
    }

    $.ajax({
        type: "POST",
        //data: JSON.stringify(jsonObj),
        url: apiBaseUrl + "/Pedido",
        //contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        //dataType: 'json'
        data: JSON.stringify(jsonObj),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json'
    });

}



