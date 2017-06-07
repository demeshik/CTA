$(function () {
    $('#add-lot').click(function () {
        $('#addLotModal').modal('show');
    });

    $('#add-new-lot-button').click(function (e) {
        e.preventDefault();

        var data = {
            Title: $('#new-lot-title').val(),
            Description: $('#new-lot-description').val(),
            MainImage: $.fn.new_lot_mainImage,
            Images: $.fn.new_lot_images.toString(),
            ExpiredDate: $('#new-lot-expired').val(),
            MinBid: parseInt($('#new-lot-minbid').val()),
            UserId: $.fn.currentUserId
        };

        $.ajax({
            type: 'POST',
            url: '/api/lots',
            contentType: 'application/json;charset=utf-8',
            data: JSON.stringify(data)
        }).success(function (dat) {
            new Noty({
                type: 'success',
                text: 'All is good! Your lot was added to database'
            }).show();
        }).fail(function (err) {

            if (!!err.responseJSON.errors) {
                var errs = err.responseJSON.errors;
                for (var i = 0; i < errs.length; i++) {
                    new Noty({
                        type: 'error',
                        text: errs[i].key + ": " + errs[i].value[0]
                    }).show();
                }
            }
            else {
                var errs = err.responseJSON;
                for (var i = 0; i < errs.length; i++) {
                    new Noty({
                        type: 'error',
                        text: errs[i].description
                    }).show();
                }
            }
        });

    })
})