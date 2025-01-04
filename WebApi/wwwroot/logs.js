$(document).ready(function() {
    let currentPage = 1;
    let logsPerPage = 10;

    function fetchLogs(page, perPage) {
        const token = localStorage.getItem('jwtToken');
        $.ajax({
            url: `/api/Log/FetchLogs?n=${perPage}&page=${page}`,
            type: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            },
            success: function(logs) {
                updateTable(logs);
                updatePagination(page, perPage);
            },
            error: function(xhr, status, error) {
                console.error('Failed to fetch logs:', error);
            }
        });
    }

    function updateTable(logs) {
        const tbody = $('#logsTableBody');
        tbody.empty();
        logs.forEach(log => {
            const rowClass = getRowClass(log.importance);
            tbody.append(`<tr class="${rowClass}"><td>${log.message}</td></tr>`);
        });
    }

    function getRowClass(importance) {
        switch (importance) {
            case 1: return 'low-importance';
            case 2: return 'medium-importance';
            case 3: return 'high-importance';
            default: return 'none-importance';
        }
    }

    function updatePagination(page, perPage) {
        // Implement pagination logic here
    }

    $('#logsPerPage').change(function() {
        logsPerPage = $(this).val();
        fetchLogs(currentPage, logsPerPage);
    });

    $('#refreshLogs').click(function() {
        fetchLogs(currentPage, logsPerPage);
    });

    $('#logout').click(function() {
        localStorage.removeItem('jwtToken');
        window.location.href = 'login.html';
    });

    fetchLogs(currentPage, logsPerPage);
});