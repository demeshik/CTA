$('#sign-in').click(function (e) {
    e.preventDefault();

    var $this = $(this);
    $this.button('loading');

    var data = {
          UserName: $('#username').val(),
          Password: $('#password').val(),
          RememberMe: true
      };
 
      $.ajax({
          type: 'POST',
          url: '/api/session',
          contentType: 'application/json;charset=utf-8',
          data: JSON.stringify(data)
      }).success(function (dat) {
          location.reload();
      }).fail(function (err) {

          $this.button('reset');
          var errs = JSON.parse(err.responseText).errors;

          /*for (var i = 0; i < errs.length; i++) {
              $("<label for='" + errs[i].key.toLowerCase() + "' class='error'></label>")
                  .html(errs[i].value[0])
                  .css("color","red").appendTo($("input#" + errs[i].key.toLowerCase()).parent());
          }*/

          for (var i = 0; i < errs.length; i++) {
              new Noty({
                  type: 'error',
                  text: errs[i].key + ": " + errs[i].value[0]
              }).show();
          }
      });
});

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