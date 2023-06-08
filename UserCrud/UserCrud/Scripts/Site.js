var DataTable = "";

function sendSms(toNumber) {
    $.ajax({

        type: "GET",
        url: '/User/SendSMS',
        data: { toNumber: toNumber },
        success: function () {
            alert('Success'); const link = document.createElement("a");
            if (link.download !== undefined) {
                const url = URL.createObjectURL(csvBlob);
                link.setAttribute("href", url);
                link.setAttribute("download", filename);
                link.style.visibility = "hidden";
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            } },
        error: function (error) { alert(error); console.log(error) }
    });
}


function editDetails(userID) {
    console.log(userID);
  
    $.ajax({
        type: "GET",
        url: '/User/GetUserById',
        data: { id: userID },
        success: function (response) {

            $(".UserDetail").html(response);

            $("#exampleModalCenter").modal("show");
            if (userID != 0) {


                $("#password").addClass("hide");
                $("#Cpassword").remove();
            }
        },

        error: function (error) { alert(error); console.log(error) }
    });
}


function deleteDetails(userID) {
    var confirmBox = `
    <div class="modal fade" id="deleteUser" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
    <div class="modal-header">
    <h5 class="modal-title">Confirm Delete</h5>
    <button type="button" class="closed btn" data-bs-dismiss="modal" aria-label="Close">
   <span aria-hidden="true" class="fs-3">&times;</span>
    </button>
    </div>
    <div class="modal-body">
    <p>Are you sure you want to delete this User?</p>
    </div>
    <div class="modal-footer d-flex justify-content-center">
    <button type="button" class="cancelbtn me-2 btn bg-light" data-bs-dismiss="modal" >Cancel</button>
    <button type="button" class="m-0 confirmDelete btn bg-danger" id="apply-button">Delete</button>
    </div>
    </div></div></div>`;

    $("body").append(confirmBox);

    $("#deleteUser").modal("show");

    $(".confirmDelete").on("click", function () {


        $("#userId");
        $.ajax({
            type: "GET",
            url: '/User/deleteUserDetail',
            data: { userID: userID },
            success: function (response) {
                window.location.href = "UserDetail";
            },

            error: function (error) { alert(error); console.log(error) }
        });
        $('#deleteUser').modal("hide");

    });

    $('#deleteUser').on('hidden.bs.modal', () => {
        $('#deleteUser').remove();
    });
}

$(document).ready(function () {
    getDataTable();
    $('#firstNameFilter, #lastNameFilter, #countryFilter, #cityFilter, #DateFilter').on('input', function () {
        dataTable.ajax.reload();
    });

    setTimeout(function () {
        $("#successMessage").fadeOut("slow");
    }, 1500);
    

});


function closeModel() {
    $(".modal-backdrop").remove();
    $("#myForm")[0].reset();
    $("#postForm")[0].reset();
    $('#exampleModalCenter').modal('hide');
    $("#password").removeClass("hide");
    $("#Cpassword").removeClass("hide");
}
function closeModel2() {
    $("#postForm")[0].reset();
    remove();
    
}

function numberOnly(event) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function getDataTable() {
   
     dataTable = $('#userTable').DataTable({
        "processing": true,
        "ordering": true,
        "order": [[9, 'desc']],
        "searching": false,
        "serverSide": true,
        "ajax": {
            "url": "/User/GetDetails",
            "type": "POST",
            "data": function (d) {

                d.PageNumber = d.start / d.length;
                d.PageSize = d.length;
                d.SortColumn = d.order[0] ? d.columns[d.order[0].column].data : null;
                d.SortDirection = d.order[0].dir;
                d.FirstName = $('#firstNameFilter').val();
                d.LastName = $('#lastNameFilter').val();
                d.Country = $('#countryFilter').val();
                d.City = $('#cityFilter').val();
                d.FromDate = $('#DateFilter').val().slice(0, 10);
                d.ToDate = $('#DateFilter').val().slice(13, 23);
               
            },
            "dataSrc": function (response) {
                
                    return response.data;
                
            },
            "error": function (error) {
              console.log(error)
            }
        },
        "columns": [

            { "data": "FirstName", "name": "First Name", "visible": true, },
            { "data": "LastName", "name": "Last Name" },

            {
                "data": "DAteOfBirth",
                "name": "DateOFBirth",
                "render": function (data, type, row) {
                    return new Date(data).toLocaleDateString('en-GB');
                }
            },
            { "data": "Email", "name": "Email" },
            { "data": "PhoneNUmber", "name": "Mobile No." },
            { "data": "Address", "name": "Address", "orderable": false },
            { "data": "City", "name": "City" },
            { "data": "Country", "name": "Country" },
            { "data": "PinCode", "name": "Pincode" },
            {
                "data": "ID", "render": function (data, type, row) {
                    return '<div style="overflow: hidden; position: relative; height: 35px;">' +
                        '<i class="bi bi-pen icon" data-toggle="modal" data-target="#exampleModalCenter" onclick="editDetails(' + data + ')"></i>' +
                        '<button onclick="deleteDetails(' + data + ')" class="trash"></button>' +
                        '</div>';
                }
            }

        ],
        "lengthMenu": [2, 3, 5, 7]
    });

}

function showPassword() {
    var passwordInput = $('input[name="Password"]');
    if (passwordInput.attr('type') === 'password') {
        $("#eyeold").hide();
        $("#eyeSlashold").show();
        passwordInput.attr('type', 'text');
    } else {
        $("#eyeold").show();
        $("#eyeSlashold").hide();
        passwordInput.attr('type', 'password');
    }
};

function exportToCSV() {
    var firstName = $('#firstNameFilter').val();
    var lastName = $('#lastNameFilter').val();
    var country = $('#countryFilter').val();
    var city = $('#cityFilter').val();
    var fromDate = $('#DateFilter').val().slice(0, 10);
    var toDate = $('#DateFilter').val().slice(13, 23);

    var exportUrl = '/User/ExportCSV?FirstName=' + encodeURIComponent(firstName) +
        '&LastName=' + encodeURIComponent(lastName) +
        '&Country=' + encodeURIComponent(country) +
        '&City=' + encodeURIComponent(city) +
        '&FromDate=' + encodeURIComponent(fromDate) +
        '&ToDate=' + encodeURIComponent(toDate);

    $('#exportanchor').attr('href', exportUrl);
}



function remove() {
    $("#preview").hide();
    $('#imageUrl').attr('src', "");
}
function ClickInput() {
    $("#fileinput").click();
};

function preview(uploader) {
    if (uploader.files && uploader.files[0]) {
        $("#preview").show();
        $('#imageUrl').attr('src', window.URL.createObjectURL(uploader.files[0]));
       
    }
}

function previewImage(something) {
    preview(something);
};

function createPost() {
    var postForm= new FormData();
    postForm.append('Body', $('#textarea').val());
    postForm.append('ImagePath', $("#fileinput")[0].files[0]);
    
    console.log(postForm);

    $.ajax({
        type: 'POST',
        url: "/User/SavePost",
        data:postForm
    ,
        processData: false,
        contentType: false,
        success: function (message) {
            $('#post').modal("hide");
            $("#postForm")[0].reset();
            remove();
            location.reload(true)
        },
        error: function (error) {
            console.log(error);
            console.log("error in adding story");
        }


    });
}

function deletePost(postID) {
    var confirmBox = `
    <div class="modal fade" id="deletePost" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
    <div class="modal-header">
    <h5 class="modal-title">Confirm Delete</h5>
    <button type="button" class="closed btn" data-bs-dismiss="modal" aria-label="Close">
   <span aria-hidden="true" class="fs-3">&times;</span>
    </button>
    </div>
    <div class="modal-body">
    <p>Are you sure you want to delete this User?</p>
    </div>
    <div class="modal-footer d-flex justify-content-center">
    <button type="button" class="cancelbtn me-2 btn bg-light" data-bs-dismiss="modal" >Cancel</button>
    <button type="button" class="m-0 confirmDelete btn bg-danger" id="apply-button">Delete</button>
    </div>
    </div></div></div>`;

    $("body").append(confirmBox);

    $("#deletePost").modal("show");

    $(".confirmDelete").on("click", function () {


        $("#userId");
        $.ajax({
            type: "GET",
            url: '/User/deletePost',
            data: { postID: postID },
            success: function (response) {
                window.location.href = "UserProfile";
            },

            error: function (error) { alert(error); console.log(error) }
        });
        $('#deletePost').modal("hide");

    });

    $('#deletePost').on('hidden.bs.modal', () => {
        $('#deletePost').remove();
    });
}

function postLike(postID) {

    $.ajax({
        type: "Post",
        url: "/User/LikePost",
        data: { postID: postID },
        success: function (response) {

            let likeicon = $("i." + postID)
            if (likeicon.hasClass("bi-heart")) {
                likeicon.removeClass("bi-heart")
                likeicon.addClass("bi-heart-fill").addClass("text-danger");
            }
            else {
                likeicon.removeClass("bi-heart-fill").removeClass("text-danger")
                likeicon.addClass("bi-heart");
            }
            $(".like-" + postID).empty().text(response);
        },
        error: function (error) {
            console.log(error);
        }
    })

}



function showComment(postID) {
    var commentSection = $(".CommentPartial");
    var isVisible = commentSection.is(":visible");
    
    if (isVisible) {
        commentSection.toggle();
    }
    else
    {
        callAjaxComment(postID);
    }
}

function callAjaxComment(postID) {
    $.ajax({
        type: "Post",
        url: "/User/GetCommentsByPostId",
        data: { id: postID },
        success: function (response) {

            $(".comment-" + postID).empty().html(response);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function saveComment(postID) {
    var commentText = jQuery.trim($("#commentText-" + postID).val());
    if (commentText.length < 1) {
        alert("Bolana Comment likho Phele !!!")
    } else {
        $.ajax({
            type: "Post",
            url: "/User/SaveComment",
            data: { postID: postID, commentText: commentText },
            success: function (response) {
                console.log("Comment submitted successfully!");
                $("#comments-" + postID).empty().text(response);
                $("#commentText-" + postID).val("");
                callAjaxComment(postID);
            },
            error: function (error) {
                console.log("Error submitting comment:", error);
            }
        });
    }
    
}

function deleteComment(commentID , postID){
    
        $.ajax({
            type: "Post",
            url: "/User/DeleteComment",
            data: { commentID: commentID, postId: postID},
            success: function (response) {
                console.log("Comment deleted successfully!");
                $("#comments-" + postID).empty().text(response);
                callAjaxComment(postID);
            },
            error: function (error) {
                console.log("Error submitting comment:", error);
            }
        });
    

}

function showPostButton(postID) {
    var inputlength = jQuery.trim($("#commentText-" + postID).val());
    if (inputlength.length > 0) {
        $("#postComment-" + postID).show();
        $("#postComment-" + postID).prop("disabled", false);

    }
    else {
        $("#postComment-" + postID).hide();
        $("#postComment-" + postID).prop("disabled", true)
    }

}

function followRequest(toUserID) {

    $.ajax({
        type: "Post",
        url: "/User/FollowRequest",
        data: { toUserID: toUserID },
        success: function (response) {
            console.log("folllowed");
            $("#follow-" + toUserID).text("Requested").addClass("requested");
        },
        error: function (error) {
            console.log("Error requesting:", error);
        }
    });

}