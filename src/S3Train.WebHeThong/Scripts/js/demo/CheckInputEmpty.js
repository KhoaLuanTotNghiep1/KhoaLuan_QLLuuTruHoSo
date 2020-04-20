$(document).ready(function () {
    $('#myForm input[type="text"]').blur(function () {
        if (!$(this).val()) {
            $(this).addClass("is-invalid");
        } else {
            $(this).addClass("is-valid");
            $(this).removeClass("is-invalid");
        }
    });
});