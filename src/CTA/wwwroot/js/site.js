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
        window.location.replace("/");
    });
});

$('#signup').click(function (e) {
    e.preventDefault();

    var data = {
        UserName: $('#username').val(),
        Password: $('#password').val(),
        Name: $('#name').val(),
        ConfirmPassword: $('#confirmpassword').val(),
        Surname: $('#surname').val(),
        Email: $('#email').val(),
        Image: $('.cloudinary-fileupload').attr('value'),
        PhoneNumber: $('#phone').val(),
        Country: $('#country').val(),
        City: $('#city').val(),
        CreditCard: $('#credit').val()
    };
    $.ajax({
        type: 'POST',
        url: '/api/users',
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify(data)
    }).success(function (dat) {
        new Noty({
            type: 'success',
            text: 'All is good! Now you can login by your username and password'
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
        else
            var errs = err.responseJSON;
        for (var i = 0; i < errs.length; i++) {
            new Noty({
                type: 'error',
                text: errs[i].description
            }).show();
        }
    });
});