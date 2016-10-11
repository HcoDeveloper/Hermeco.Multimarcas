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



