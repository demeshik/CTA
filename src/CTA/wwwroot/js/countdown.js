(function ($) {

    var days = 24 * 60 * 60,
        hours = 60 * 60,
        minutes = 60;

    $.fn.countdown = function (prop) {

        var options = $.extend({
            callback: function () { },
            timestamp: 0
        }, prop);

        var left, d, h, m, s;

        //init(this, options);


        (function tick() {

            // Осталось времени
            left = Math.floor((options.timestamp - (new Date())) / 1000);

            if (left < 0) {
                left = 0;
            }

            // Осталось дней
            d = Math.floor(left / days);
            left -= d * days;

            // Осталось часов
            h = Math.floor(left / hours);
            left -= h * hours;

            // Осталось минут
            m = Math.floor(left / minutes);
            left -= m * minutes;

            // Осталось секунд
            s = left;

            // Вызываем возвратную функцию пользователя
            options.callback(d, h, m, s);

            // Планируем следующий вызов данной функции через 1 секунду
            setTimeout(tick, 1000);
        })();
        return this;
    };

})(jQuery);