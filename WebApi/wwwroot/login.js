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
                localStorage.setItem('jwtToken', response);
                window.location.href = 'logs.html';
            },
            error: function() {
                $('#errorMessage').show();
            }
        });
    });
});