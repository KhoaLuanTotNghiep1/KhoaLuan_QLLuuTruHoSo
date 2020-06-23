$(document).ready(function () {
    $('#myForm input[type="text"]').blur(function () {
        if (!$(this).val()) {
            $(this).addClass("is-invalid");
        } else {
            $(this).addClass("is-valid");
            $(this).removeClass("is-invalid");
        }
    });

    $('#myForm input[type="date"]').on('change', function () {
        var selectedDate = new Date($(this).val());

        var todaysDate = new Date();

        if (selectedDate.setHours(0, 0, 0, 0) < todaysDate.setHours(0, 0, 0, 0)) {
            alert('Ngày chọn phải lớn hơn ngày hiện tại.');
            $(this).val("");
        }
    });


    $('.End-Date').on('change', function () {
        var endDate = new Date($(this).val());

        var startDate = new Date($('.Start-Date').val());

        if (endDate.setHours(0, 0, 0, 0) <= startDate.setHours(0, 0, 0, 0)) {
            alert('Ngày kết thúc phải lớn hơn ngày bắt đầu.');
            $(this).val("");
        }
    });
});