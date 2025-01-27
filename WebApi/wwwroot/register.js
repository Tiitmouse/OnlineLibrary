$(document).ready(function() {
    $('#registerForm').on('submit', function(event) {
        event.preventDefault();

        const username = $('#username').val();
        const fullName = $('#fullName').val();
        const password = $('#password').val();
        const id = 1;

        const user = {
            id : id,
            username: username,
            fullName: fullName,
            password: password
        };

        $.ajax({
            url: '/api/Authorisation/Register',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(user),
            success: function(response) {
                alert('Registration successful!');
                window.location.href = 'index.html';
            },
            error: function(xhr, status, error) {
                $('#errorMessage').text('Registration failed' + xhr.responseText).show();
            }
        });
    });
});