function openAddEditPersonModal(personId) {
    $.ajax({
        type: "GET",
        url: "/Home/GetAddEditPersonModal",
        data:
        {
            ajax: true,
            personId: personId
        },
        success: function (response) {
            $("#addEditModalContainer").html(response);
            $("#addEditPersonModal").modal('show');
        }
    });
}

function savePersonData() {
    $("#personDataForm").ajaxSubmit({
        type: "POST",
        url: "/Home/AddEditPersonData",
        success: function (response) {
            alert(response.message);
            setTimeout(location.reload(), 2000);
        },
        error: function (response) {
            alert(response.responseJSON.message);
        }
    })
}

function deleteSelectedPerson(personId) {
    $.ajax({
        type: "POST",
        url: "/Home/DeleteSelectedPerson",
        data:
        {
            ajax: true,
            personId: personId
        },
        success: function (response) {
            alert(response.message);
            setTimeout(location.reload(), 2000);
        },
        error: function (response) {
            alert(response.responseJSON.message);
        }
    });
}

function login() {
    $("#loginForm").ajaxSubmit({
        type: "POST",
        url: "/Login/LoginUser",
        success: function (response) {
            //window.location.href = "https://localhost:44318/Home/Index";
            window.location.href = "https://localhost:9000/Home/Index";
        },
        error: function (response) {
            alert(response.responseJSON.message);
        }
    })
}

function register(isAdmin) {
    $("#IsAdmin").val(isAdmin);

    $("#registerForm").ajaxSubmit({
        type: "POST",
        url: "/Login/RegisterUser",
        success: function (response) {
            //window.location.href = "https://localhost:44318/Login/Login";
            window.location.href = "https://localhost:9000/Login/Login";
        },
        error: function (response) {
            alert(response.responseJSON.message);
        }
    })
}