function deleteRole(data) {
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
                url: '/roles/' + data,
                method: 'Delete',
                beforeSend: (xhr) => {
                    document.getElementById(data).classList.toggle('d-none');
                },
                success: (result, status, xhr) => {
                    const spinner = document.getElementById(data);
                    spinner.classList.toggle('d-none');
                    spinner.parentElement.parentElement.className = 'd-none';
                    Swal.fire(
                        'Deleted!',
                        'Role has been deleted.',
                        'success'
                    );
                },
                error: (result, status, xhr) => {
                    document.getElementById(data).classList.toggle('d-none');
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