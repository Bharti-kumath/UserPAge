function displayUserSuggestions(userSuggestions) {
    var dropdown = $('#suggestionsDropdown');
    dropdown.empty();

    userSuggestions.forEach(function(user) {
        var option = $('<option>').text(user);
        dropdown.append(option);
    });

    // Show the dropdown
    dropdown.show();
}




$('#commentBox').on('input', function() {
    var commentText = $(this).val();
    var atSymbolIndex = commentText.lastIndexOf('@');

    if (atSymbolIndex !== -1) {
        var followingText = commentText.substring(atSymbolIndex + 1);
        var username = followingText.split(' ')[0]; // Extract the username after "@"
        
        if (username.trim() !== '') {
            fetchUserSuggestions(username);
        }
    }
});


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
            }
        },
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
    <h5 class="modal-title text-light" >Confirm Delete</h5>
    <button type="button" class="closed btn" data-dismiss="modal" aria-label="Close">
   <span aria-hidden="true" class="fs-3">&times;</span>
    </button>
    </div>
    <div class="modal-body">
    <p style="color:#fff">Are you sure you want to delete this User?</p>
    </div>
    <div class="modal-footer d-flex justify-content-center">
    <button type="button" class="cancelbtn me-2 btn bg-light" data-dismiss="modal" >Cancel</button>
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

    const postID = window.location.hash.substring(1);

    if (postID) {
        $("#" + postID).addClass("effect").focus();
    }

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




function createPost() {
    var postForm = new FormData();
    postForm.append('Body', $('#textarea').val());
    /* postForm.append('ImagePath', $("#fileinput")[0].files[0]);*/
    $.each($("#fileInput")[0].files, function (i, file) {
        postForm.append('ImagePath', file);
    });

    console.log(postForm);

    $.ajax({
        type: 'POST',
        url: "/User/SavePost",
        data: postForm
        ,
        processData: false,
        contentType: false,
        success: function (message) {
            $('#post').modal("hide");
            $("#postForm")[0].reset();

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
    <h5 class="modal-title text-light">Confirm Delete</h5>
    <button type="button" class="closed btn text-light" data-dismiss="modal" aria-label="Close">
   <span aria-hidden="true" class="fs-3">&times;</span>
    </button>
    </div>
    <div class="modal-body">
    <p class="text-light">Are you sure you want to delete this User?</p>
    </div>
    <div class="modal-footer d-flex justify-content-center">
    <button type="button" class="cancelbtn me-2 btn bg-light" data-dismiss="modal" >Cancel</button>
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

function postLike(postID, postUserID) {

    $.ajax({
        type: "Post",
        url: "/User/LikePost",
        data: { postID: postID, postUserID: postUserID },
        success: function (response) {
            var totallikes = Number(response)

            let message = '';
            let likeicon = $("i." + postID)
            if (likeicon.hasClass("bi-heart")) {
                likeicon.removeClass("bi-heart")
                likeicon.addClass("bi-heart-fill").addClass("text-danger");
                if (totallikes > 1) {
                    var likes = totallikes - 1
                    message = " by you and " + likes + " others";
                }
                else {
                    message = " by you";
                }

            }
            else {
                likeicon.removeClass("bi-heart-fill").removeClass("text-danger")
                likeicon.addClass("bi-heart");
                if (totallikes > 1) {

                    message = " by " + totallikes + " others";
                }
                else {
                    message = totallikes;
                }

            }
            $(".like-" + postID).empty().text(message);
        },
        error: function (error) {
            console.log(error);
        }
    })

}



function showComment(postID) {
    var commentSection = $(".CommentPartial-" + postID);
    var isVisible = commentSection.is(":visible");

    if (isVisible) {
        $(".comment-" + postID).empty();
    }
    else {
        callAjaxComment(postID);
    }
}

function callAjaxComment(postID) {
    $.ajax({
        type: "Post",
        url: "/User/GetCommentsByPostId",
        data: { id: postID },
        success: function (response) {
            var newResponse = `<h5>Comments</h5>` + response;
            $(".comment-" + postID).empty().html(newResponse);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function saveComment(postID, postUserID) {
    var commentText = jQuery.trim($("#commentText-" + postID).val());
    if (commentText.length < 1) {
        alert("Bolana Comment likho Phele !!!")
    } else {
        $.ajax({
            type: "Post",
            url: "/User/SaveComment",
            data: { postID: postID, commentText: commentText, postUserID: postUserID },
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

function deleteComment(commentID, postID) {

    $.ajax({
        type: "Post",
        url: "/User/DeleteComment",
        data: { commentID: commentID, postId: postID },
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

function getNotification() {

    window.location.href = 'Notification'
}
function openUser(userid) {
    window.location.href = '/User/FriendProfile?friendId=' + userid;
}

function notificationRead(id) {

    $.ajax({
        type: "GET",
        url: "/User/ReadNotification",
        data: { id: id },
        success: function () {
            $("#notificationread").prop("disabled", true);
        }
        ,
        error: function (error) {
            console.log(error)
        }
    })


};


setInterval(function () {
    $.ajax({
        url: '/User/GetNotificationCount',
        method: 'GET',
        success: function (response) {

            if (response > 0) {
                $(".notificationDot").show();
                $(".notificationDot p").empty();
                $(".notificationDot p").text(response)
            }

        },
        error: function (xhr, status, error) {
            console.log(error)
        }
    });
}, 10000);


function requestUpdate(followerId, action) {
    console.log(action);
    $.ajax({
        type: "Post",
        url: "/User/UpdateFollowRequest",
        data: { followerId: followerId, action: action },
        success: function () {
            console.log("follow request updated ");
            if (action = 1) {
                $("#confirm-" + followerId).text("Accepted");
                $("#delete-" + followerId).remove();
            }
            else {
                $("#delete-" + followerId).text("Declined");
                $("#confirm-" + followerId).remove();
            }
        }
        ,
        error: function (error) {
            console.log(error)
        }
    })

}

function focusPost(postID) {

    window.location.href = 'Landing/#post-' + postID;



}


$("#fileInput").on("change", function (e) {
    const files = e.target.files;
    $("#preview").show();
    const carouselInner = document.querySelector(".carousel-inner");
    const carouselIndicators = document.querySelector(
        ".carousel-indicators"
    );
    carouselInner.innerHTML = "";
    carouselIndicators.innerHTML = "";

    let activeIndex = 0;

    for (let i = 0; i < files.length; i++) {
        const file = files[i];

        // Check if the file is not empty and is an image
        if (file.size > 0 && file.type.includes("image")) {
            const reader = new FileReader();

            reader.onload = function (event) {
                const imageSrc = event.target.result;
                const active = i === activeIndex ? "active" : i;

                const carouselItem = document.createElement("div");
                carouselItem.classList.add("carousel-item", active);
                carouselItem.innerHTML = `
          <img src="${imageSrc}" class="d-block w-100" alt="Preview" id="imageUrl">
           <i class="bi bi-x-lg" id="cross"onclick="removePreview(this)"></i>
        `;

                carouselInner.appendChild(carouselItem);
            };

            reader.readAsDataURL(file);
        }
    }

    // Initialize the carousel after adding the items
    $(".carousel").carousel();
});

function removePreview(button) {
    const carouselItem = button.closest(".carousel-item");
    const carousel = carouselItem.parentElement;
    const carouselItems = Array.from(
        carousel.querySelectorAll(".carousel-item")
    );


    const activeIndex = carouselItems.findIndex(
        (item) => item === carouselItem
    );


    carousel.removeChild(carouselItem);


    let newActiveIndex = activeIndex - 1;


    if (newActiveIndex < 0) {
        newActiveIndex = carouselItems.length - 1;
    }


    carouselItems[newActiveIndex].classList.add("active");


    const inputFile = document.getElementById("fileInput");
    const selectedFiles = Array.from(inputFile.files);
    selectedFiles.splice(activeIndex, 1);

    inputFile.value = "";

    // Assign the new FileList object to the input element
    var dt = new DataTransfer();
    for (let i = 0; i < selectedFiles.length && i < 20; i++) {
        dt.items.add(selectedFiles[i]);
    }
    inputFile.files = dt.files;
    // Update carousel
    $(".carousel").carousel(newActiveIndex);
}

function ClickInput() {
    $("#fileInput").click();
};

function showReplyInput(userName, id) {
    var trimedusername = userName.replace(" ", "");

    $(".ReplyBox-" + id).show();
    $("#commentReply-" + id).val("@" + trimedusername + " ");
}
function showReReplyInput(userName, id) {
    var trimedusername = userName.replace(" ", "");
    console.log(trimedusername,id)
    $(".ReReplyBox-" + id).show();
    $("#commentReReply-" + id).val("@" + trimedusername + " ");
}

function saveCommentReply(commentId, postID) {
    var replyTextWithUsername = $("#commentReply-" + commentId).val();
    var replyText = replyTextWithUsername.replace(/@\w+\b/g, "");

    $.ajax({
        type: "Post",
        url: "/User/SaveCommentReply",
        data: { commentId: commentId, replyText: replyText },
        success: function (response) {
            console.log("Comment submitted successfully!");
            $("#commentReply-" + commentId).val("");
            $(".ReplyBox-" + commentId).hide();
            callAjaxComment(postID);
        },
        error: function (error) {
            console.log("Error submitting comment:", error);
        }
    });
}

var text = '';
function ShowReply(commentID, view) {
    var replySection = $(".ReplyPartial-" + commentID);
    var isVisible = replySection.is(":visible");
    
    if (isVisible) {
        $("#viewreply-" + commentID).text(text);
        
        replySection.toggle();
    }
    else {
        text = $("#viewreply-" + commentID).text();
        console.log(text);
        $("#viewreply-" + commentID).text("--- Hide Replies");
        callAjaxReply(commentID);
    }
}

function callAjaxReply(commentID) {
    $.ajax({
        type: "Post",
        url: "/User/GetReplyByCommentID",
        data: { commentId: commentID },
        success: function (response) {

            $(".Reply-" + commentID).empty().html(response);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function showLikeUsers(postId) {

    var replySection = $(".showlike");
    var isVisible = replySection.is(":visible");

    if (isVisible) {
        $(".comment-" + postId).empty();

    } else {
        $.ajax({
            type: "Post",
            url: "/User/GetLikeUserList",
            data: { postId: postId },
            success: function (response) {
                console.log(response)
                var users = '<h5>Liked By </h5>';
                for (i = 0; i < response.length; i++) {
                    var item = response[i];
                    users += `<div class="d-flex flex-row  align-items-center my-3 showlike ">
<img src="https://mdbootstrap.com/images/avatars/img%20(4).jpg" alt="" class="img-circle mx-2 img-fluid post-Userimg">
<span onclick="openUser(${item.Id})" class="mx-2">${item.SuggestedName}</span>
<i class="bi bi-heart-fill text-danger fs-5 mx-2" ></i>
</div>`
                }
                $(".comment-" + postId).empty().html(users);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

   
}
