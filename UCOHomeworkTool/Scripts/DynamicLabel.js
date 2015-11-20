$(document).ready(function () {
    $("#studentRadio").change(function () {
            $('#usernameLabel').html("Student ID");
            $('#usernameTextBox').attr("placeholder","Student ID");
            $('#adminCheckbox').attr("disabled", true);
            $('#adminCheckbox').removeAttr("checked");
    });
    $("#teacherRadio").change(function () {
            $('#usernameLabel').html("Username");
            $('#usernameTextBox').attr("placeholder","Username");
            $('#adminCheckbox').removeAttr("disabled");
    })
})
//# sourceURL=DynamicLabel.js: