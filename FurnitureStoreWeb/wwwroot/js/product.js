

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        /*https://localhost:7239/api/ProductAPI/*/
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'category', "width": "15%" },
            { data: 'productCode', "width": "10%" },
            { data: 'stock', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/product/edit?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> edit</a>               
                     <a onclick=Delete('https://localhost:7239/api/ProductAPI/DeleteProduct/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
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
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}