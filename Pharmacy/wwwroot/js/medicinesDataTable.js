$(document).ready(fillStudentTable());

document.getElementById('medicinesTable_length').classList.add('mb-2');

function fillStudentTable() {
    $('#medicinesTable').dataTable({
        "responsive": true,
        "processing": true,
        "serverSide": true,
        "bDestroy": true,
        "filter": true,
        "ajax": {
            "url": "/api/EndPoints",
            "type": "Post",
            "datatype": "json",
        },
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false,
            }
        ],
        "columns": [
            { "data": "id", "name": "Id", "autowidth": true },
            { "data": "name", "name": "Name", "autowidth": true },
            { "data": "description", "name": "Description", "autowidth": true },
            { "data": "tabletsNumber", "name": "TabletsNumber", "autowidth": true },
            { "data": "price", "name": "Price", "autowidth": true },
            { "data": "category.name",
                    "name": "Category.Name",
                    "render": function (data, type, row) {
                        return `<td>
                                    ${row.categoryId === null ? 'Not Assigned' : row.categoryId}
                                </td>`;

                },
                    "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<button class="btn btn-danger" onclick=DeleteMedicine('${row.id}')>
                                <i class="fas fa-trash"></i>
                                <div class="spinner-border text-white spinner-border-sm d-none" role="status" id="${row.id}">
                                    <span class="visually-hidden">Loading...</span>
                                </div>
                            </button>
                            <a href="Edit/${row.id}" class="btn btn-primary">Edit</a>
                            <a href = "Details/${row.id}" class="btn btn-success" > Details</a>`;
                },
                "orderable": false
            }
        ]
    });
}
const DeleteMedicine = (data) => {
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
                beforeSend: (xhr) => {
                    document.getElementById(data).classList.toggle('d-none');
                },
                success: (result, status, xhr) => {
                    document.getElementById(data).classList.toggle('d-none');
                    fillStudentTable();
                    Swal.fire(
                        'Deleted!',
                        'Student has been deleted.',
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
