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
    $('#userTable').DataTable({
        "processing": true,
        "ordering": true,
        "searching": false,
        "serverSide": true,
        "ajax": {
            "url": "/User/GetDetails",
            "type": "Post",
            "data": function (d) {
                d.pageNumber = d.start / d.length;
                d.pageSize = d.length;
                d.sortColumn = d.order[0].column;
                d.sortDir = d.order[0].dir;
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
            { "data": "id" },
            { "data": "FirstName"},
            { "data": "LastName" },
            { "data": "email"  },
            { "data": "dateOfBirth" },
            { "data": "phoneNumber" },
            { "data": "Country" },
            { "data": "City" },
            { "data": "Address" },
            { "data": "pincode" }
        ],
        "lengthMenu": [2, 3, 5, 7]
    });
});