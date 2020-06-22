$(document).ready(function () {

    $.ajax({
        url: '/ToDoes/BuildToDoTable',
        success: function (result) {
            $('#todo-table').html(result);
        }
    });

});