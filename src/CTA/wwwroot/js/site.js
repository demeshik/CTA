

$('#logout').click(function (e) {
    e.preventDefault();

    $.ajax({
        type: 'DELETE',
        url: '/api/session',
        contentType: 'application/json;charset=utf-8'
    }).success(function (dat) {
            location.reload();
        }).fail
})