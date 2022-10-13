function deleteMedicine(data) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Medicines/delete/' + data,
                method: 'Delete',
                success: (result, status, xhr) => {
                    document.getElementById(data).parentElement.parentElement.className = 'd-none';
                    Swal.fire(
                        'Deleted!',
                        'Role has been deleted.',
                        'success'
                    );
                },
                error: (result, status, xhr) => {
                    Swal.fire(
                        'Error!',
                        'Something went wrong.',
                        'error'
                    );
                }
            });
        }
    });
}
function deleteCategory(data) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Categories/delete/' + data,
                method: 'Delete',
                success: (result, status, xhr) => {
                    document.getElementById(data).parentElement.parentElement.className = 'd-none';
                    Swal.fire(
                        'Deleted!',
                        'Role has been deleted.',
                        'success'
                    );
                },
                error: (result, status, xhr) => {
                    Swal.fire(
                        'Error!',
                        'Something went wrong.',
                        'error'
                    );
                }
            });
        }
    });
}