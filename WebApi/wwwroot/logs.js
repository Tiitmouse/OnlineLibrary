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
                //TODO: 401 redirect to login
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
        $('#pageInfo').text(`Page ${page}`);
        $('#prevPage').prop('disabled', page === 1);
        // Assuming you have a way to determine if there are more logs
        $('#nextPage').prop('disabled', logs.length < perPage);
    }

    $('#logsPerPage').change(function() {
        logsPerPage = $(this).val();
        currentPage = 1;
        fetchLogs(currentPage, logsPerPage);
    });

    $('#refreshLogs').click(function() {
        fetchLogs(currentPage, logsPerPage);
    });

    $('#logout').click(function() {
        localStorage.removeItem('jwtToken');
        window.location.href = 'index.html';
    });

    $('#prevPage').click(function() {
        if (currentPage > 1) {
            currentPage--;
            fetchLogs(currentPage, logsPerPage);
        }
    });

    $('#nextPage').click(function() {
        currentPage++;
        fetchLogs(currentPage, logsPerPage);
    });

    fetchLogs(currentPage, logsPerPage);
});