$(document).ready(function() {
    $('#loginForm').on('submit', function(event) {
        event.preventDefault();

        const username = $('#username').val();
        const password = $('#password').val();

        $.ajax({
            url: '/Api/User/Login',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Username: username, Password: password }),
            success: function(response) {
                window.location.href = 'logs.html';
                console.log(response);
            },
            error: function() {
                $('#errorMessage').show();
            }
        });
    });
});