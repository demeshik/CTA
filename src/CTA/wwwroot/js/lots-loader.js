﻿$(function () {
    $.cloudinary.config({ cloud_name: 'djrazor308', api_key: '699756615932382' });


    $.get('/api/lots/', {
        page: 0,
        count:20
    }, onSuccess)

    function onSuccess(data)
    {
        data.data.forEach(function (element, index) {
            var mainDiv = $('<div />', { "class": 'col-sm-6 col-md-4' });
            var thumbnail = $('<div />', { "class": 'thumbnail' });
            var caption = $('<div />', { "class": 'caption' });

            var currimage = element.mainImage.slice(1);
            var finalimage = currimage.substring(0, currimage.length - 1);

            var img = $.cloudinary.image(finalimage);

            var imgClass = 'new-image-' + index;

            img.addClass(imgClass);

            mainDiv.html(thumbnail);
            thumbnail.html(img);
            caption.insertAfter(img);

            var header = $('<h3 />', { text: element.title });

            var src = $('<a />', { href: '/lots/' + element.id });
            src.html(header);

            caption.html(src);

            mainDiv.appendTo($('.row.contents'));
        })
    }
})