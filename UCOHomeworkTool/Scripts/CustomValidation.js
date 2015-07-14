$.validator.addMethod("requiredwhencreating", function (value, element) {
    var valid = true;
    var id = $(element).parent().parent().find("input :first:hidden").val();
    if (id == 0)
        valid = value != null;
    return valid;
});
$.validator.unobtrusive.adapters.add("requiredwhencreating", function (options) {
    options.rules["requiredwhencreating"] = true;
    options.messages["requiredwhencreating"] = options.message;
});
//# sourceURL=customValidation.js