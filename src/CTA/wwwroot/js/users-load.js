$(function () {
    $.cloudinary.config({ cloud_name: 'djrazor308', api_key: '699756615932382' });


    $.get('/api/users/', {
        page: 0,
        count: 20
    }, onSuccess)

    function onSuccess(data) {
        data.data.forEach(function (element, index) {
            var mainDiv = $('<div />', { "class": 'col-sm-6 col-md-4' });
            var thumbnail = $('<div />', { "class": 'thumbnail' });
            var caption = $('<div />', { "class": 'caption' });
            var img = $.cloudinary.image(element.image);

            var imgClass = 'new-image-' + index;

            img.addClass(imgClass);

            mainDiv.html(thumbnail);
            thumbnail.html(img);
            caption.insertAfter(img);

            var header = $('<h3 />', { text: element.userName });

            var src = $('<a />', { href: '/users/' + element.id });
            src.html(header);

            caption.html(src);

            mainDiv.appendTo($('.row.contents'));
        })
    }
})