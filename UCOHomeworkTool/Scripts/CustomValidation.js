$(function () {
    $.validator.addMethod("requiredwhencreating", function (value, element, params) {
        var valid = true;
        var id= $(element).parent().parent().children().eq(0).val();
        if (id == 0)
            valid = value !== "";
        return valid;
    });
    //$.validator.unobtrusive.adapters.add("requiredwhencreating", function (options) {
    //    options.messages["requiredwhencreating"] = options.message;
    //});
    $.validator.unobtrusive.adapters.addBool("requiredwhencreating");
//# sourceURL=customValidation.js
}(jQuery));