$(function () {
    $.validator.addMethod("requiredwhencreating", function (value, element, params) {
        var valid = true;
        var id= $(element).parent().parent().children().eq(0).val();
        if (id == 0)
            valid = value !== "";
        return valid;
    });
    $.validator.unobtrusive.adapters.addBool("requiredwhencreating");
    $.validator.defaults.ignore = "input[type=hidden]";
//# sourceURL=customValidation.js
}(jQuery));