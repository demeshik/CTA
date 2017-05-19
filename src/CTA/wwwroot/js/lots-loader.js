$.cloudinary.config({ cloud_name: 'djrazor308', api_key: '699756615932382' })

function getimage() {

    for (var i = 0; i < 12; i++)
    {
        var mainDiv = $('<div />', { "class": 'col-sm-6 col-md-4' });
        var thumbnail = $('<div />', { "class": 'thumbnail' });
        var caption = $('<div />', { "class": 'caption' });
        var img = $.cloudinary.image('sample.jpg');

        var imgClass = 'new-image-' + i;

        img.addClass(imgClass);

        mainDiv.html(thumbnail);
        thumbnail.html(img);
        caption.insertAfter(img);

        var header = $('<h3 />', { text: "Заголовок" });

        caption.html(header);

        mainDiv.appendTo($('.row.contents'));
    }
}