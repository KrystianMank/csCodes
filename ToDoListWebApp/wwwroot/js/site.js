async function changeStatus(rowId) {
    const row = document.getElementById("row_" + rowId);
    const isDone = !row.classList.contains('task-done');

    try {
        const response = await fetch(`/Task/UpdateTaskStatus?id=${rowId}&isDone=${isDone}`, {
            method: 'POST'
        });

        if (response.ok) {
            row.classList.toggle('task-done');
            location.reload();
        } else {
            console.error('Error updating task status');
        }
    } catch (error) {
        console.error('Network error:', error);
    }
}

$('.dropdown-toggle').dropdown();