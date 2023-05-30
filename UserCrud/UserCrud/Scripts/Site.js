function sendSms(toNumber) {
    $.ajax({
     
        type: "GET",
        url: '/User/SendSMS',
        data: {toNumber: toNumber} ,
        success: function () { alert('Success'); },
        error: function (error) { alert(error); console.log(error) }
    });
}

const input = document.querySelector('#input');


$("input").change(function (e) {
    const isValid = e.target.checkValidity();
    console.log(e.target.innerHTML)
    console.log(isValid);
    if (isValid) {
        this.addClass("valid")
    }
});

function editDetails(userID) {
    console.log(userID);
    $("#userId")
}

$(document).ready(function () {
    var dataTable = $('#userTable').DataTable({
        "processing": true,
        "ordering": true,
        "searching": false, // Disable DataTable's built-in searching
        "serverSide": true,
        "ajax": {
            "url": "/User/GetDetails",
            "type": "POST",
            "data": function (d) {
                d.PageNumber = d.start / d.length;
                d.PageSize = d.length;
                d.SortColumn = getColumnName(d.order[0].column); 
                d.SortDirection = d.order[0].dir;
                d.FirstName = $('#firstNameFilter').val();
                d.LastName = $('#lastNameFilter').val(); 
                d.Country = $('#countryFilter').val(); 
                d.City = $('#cityFilter').val(); 
                d.FromDate = $('#fromDateFilter').val(); 
                d.ToDate = $('#toDateFilter').val(); 
            },
            "dataSrc": function (response) {
                console.log(response)
                return response.data;
            },
            "error": function (error) {
                console.log(error)
            }
        },
        "columns": [
            { "data": "FirstName", "name": "First Name" },
            { "data": "LastName", "name": "Last Name" },
            { "data": "dateOfBirth", "name": "Date OF Birth" },
            { "data": "email", "name": "Email" },
            { "data": "phoneNumber", "name": "Mobile No." },
            { "data": "Address", "name": "Address" },
            { "data": "City", "name": "City" },
            { "data": "Country", "name": "Country" },
            { "data": "pincode", "name": "Pincode" },
            //{
            //    "data": "id", "render": function (data, type, row) {
            //        return '<div style="overflow: hidden; position: relative; height: 35px;">' +
            //            '<i class="bi bi-pen icon" data-toggle="modal" data-target="#exampleModalCenter" onclick="editDetails(' + data + ')"></i>' +
            //            '<a href="/User/deleteUserDetail?userID=' + data + '" class="trash"></a>' +
            //            '</div>';
            //    }
            //}

        ],
        "lengthMenu": [2, 3, 5, 7]
    });

    // Add an event listener to trigger searching when filter inputs change
    $('#firstNameFilter, #lastNameFilter, #countryFilter, #cityFilter, #fromDateFilter, #toDateFilter').on('input', function () {
        dataTable.ajax.reload();
    });
});
