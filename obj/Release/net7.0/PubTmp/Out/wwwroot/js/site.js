// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Нажатие вкладки Справка в модальном окне //

function Reference(ID) {
    $.ajax({
        url: "/Home/Reference/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
            $('#ServicesModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    })
}
//-------Добавление пользователя--------------//
function AddLoginPassword() {
    $.ajax({
        url: "/Home/AddLoginPassword/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
            $('#ServicesModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    })
}
//-------Сохранение добавления пользователя----------//
function AddPLoginPasswordSave() {

    var isValid = true;

    if ($('#Login').val() == "") {
        $('#Login').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Login').css('border-color', 'lightgrey');
    }

    if ($('#Password').val() == "") {
        $('#Password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Password').css('border-color', 'lightgrey');
    }

    if ($('#Employee').val() == "") {
        $('#Employee').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Employee').css('border-color', 'lightgrey');
    }

    if ($('#Primechanie').val() == "") {
        $('#Primechanie').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Primechanie').css('border-color', 'lightgrey');
    }

    if ($('#FilialLogin').val() == -1) {
        $('#FilialLogin').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#FilialLogin').css('border-color', 'lightgrey');
    }

    if (isValid == false) {
        return false;
    }
    var ch = 0;
    let checkbox = document.getElementById('Admin');
    if (checkbox.checked) {
        ch = 1;
    } else {
        ch = 0;
    }

    var data = {
        //'ID': ID,
        'Login': $('#Login').val(),
        'Password': $('#Password').val(),
        'Employee': $('#Employee').val(),
        'Primechanie': $('#Primechanie').val(),
        'FillialId': $('#FilialLogin').val(),
        'Admin': ch,
        'UserMod': $('#UserMod').val(),

    };

    $.ajax({
        url: "/Home/AddLoginPasswordSave/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(data),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}
//--------------------------------//

// Удаление пользователя//

function DeleteLoginPassword(ID) {
    $.ajax({
        url: "/Home/DeleteLoginPassword/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalDeleteContent').html(result);
            $('#ServicesModalDelete').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//-----------------------------//
//Подтверждение удаления пользователя//
function DeleteLoginPasswordOK(ID) {

    $.ajax({
        url: "/Home/DeleteLoginPasswordOK/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalDeleteContent').html(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//-----------------------------//
// Редактирование пользователя//

function LoginPasswordEdit(ID) {
    $.ajax({
        url: "/Home/LoginPasswordEdit/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
            $('#ServicesModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    })
}
//-------------------------//
// Сохранение редактирования пользователя//

function LoginPasswordEditSave() {

    var isValid = true;

    if ($('#LoginEdit').val() == "") {
        $('#LoginEdit').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#LoginEdit').css('border-color', 'lightgrey');
    }

    if ($('#PasswordEdit').val() == "") {
        $('#PasswordEdit').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PasswordEdit').css('border-color', 'lightgrey');
    }

    if ($('#EmployeeEdit').val() == "") {
        $('#EmployeeEdit').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#EmployeeEdit').css('border-color', 'lightgrey');
    }
        

    if ($('#FilialLoginEdit').val() == -1) {
        $('#FilialLoginEdit').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#FilialLoginEdit').css('border-color', 'lightgrey');
    }



    if (isValid == false) {
        return false;
    }

    var ch1 = 0;
    let checkbox = document.getElementById('AdminEdit');
    if (checkbox.checked) {
        ch1 = 1;
    } else {
        ch1 = 0;
    }

    var data = {
        'Id': $('#IDEdit').val(),
        'Login': $('#LoginEdit').val(),
        'Password': $('#PasswordEdit').val(),
        'Employee': $('#EmployeeEdit').val(),
        'Primechanie': $('#PrimechanieEdit').val(),
        'FillialId': $('#FilialLoginEdit').val(),
        'Admin': ch1,
        'UserMod': $('#UserModEdit').val(),

    };

    $.ajax({
        url: "/Home/LoginPasswordEditSave/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(data),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//------------------------------------------------------------//

//-------Сохранение категорий для филиалов----------//
function AddKateg() {

    var RT = document.getElementById('RT');

    //-------------------------------------------------------------------------------

    var selected = []
    var checkboxes = document.querySelectorAll("input[type=checkbox]:checked");

    for (var i = 0; i < checkboxes.length; i++) {
        selected.push({
            'FillialId': checkboxes[i].id,
            'PlansId': checkboxes[i].name
        })
    }
        //--------------------------------------------------------------------------------
        var data = {
            'UserMod': $('#UserMod').val(),
            'check': selected
        };

        $.ajax({
            url: "/Home/AddKateg",
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            data: JSON.stringify(data),
            dataType: "html",
            success: function (result) {
                $('#ServicesModalContent').html(result);               
                $('#ServicesModal').modal('show');
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }

//-------Добавление вида--------------//
function AddPlan() {
    $.ajax({
        url: "/Home/AddPlan/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
            $('#ServicesModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    })
}

//-------Сохранение добавления плана----------//
function AddPlanSave() {

    var isValid = true;
    if ($('#Name').val() == "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }

    if (isValid == false) {
        return false;
    }

    var data = {
        //'ID': ID,
        'Name': $('#Name').val(),
        'Primechanie': $('#Primechanie').val(),
        'UserMod': $('#UserMod').val(),

    };

    $.ajax({
        url: "/Home/AddPlanSave/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(data),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
            
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}
//--------------------------------//
// Удаление плана//

function DeletePlan(ID) {
    $.ajax({
        url: "/Home/DeletePlan/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalDeleteContent').html(result);
            $('#ServicesModalDelete').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//-----------------------------//
//Подтверждение удаления плана//
function DeletePlanOK(ID) {


    $.ajax({
        url: "/Home/DeletePlanOK/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalDeleteContent').html(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//-----------------------------//
// Редактирование плана//

function PlanEdit(ID) {
    $.ajax({
        url: "/Home/PlanEdit/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
            $('#ServicesModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    })
}
//-------------------------//
// Сохранение редактирования плана//

function PlanEditSave() {

    var isValid = true;

    if ($('#NameEdit').val() == "") {
        $('#NameEdit').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NameEdit').css('border-color', 'lightgrey');
    }

    

    if (isValid == false) {
        return false;
    }

    var data = {

        'Id': $('#IDEdit').val(),
        'Name': $('#NameEdit').val(),
        'Primechanie': $('#PrimechanieEdit').val(),
        'UserMod': $('#UserModEdit').val(),

    };

    $.ajax({
        url: "/Home/PlanEditSave/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(data),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//------------------------------------------------------------//


function TableZatraty() {

    var isValid = true;

    if ($('#Filial').val() == "") {
        $('#Filial').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Filial').css('border-color', 'lightgrey');
    }

    if ($('#Month').val() == "") {
        $('#Month').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Month').css('border-color', 'lightgrey');
    }

    if ($('#Year').val() == "") {
        $('#Year').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Year').css('border-color', 'lightgrey');
    }

    if (isValid == false) {
        return false;
    }

    var data = {

        'FilialId': $('#FilialId').val(),
        'Month': $('#Month').val(),
        'Year': $('#Year').val(),
        
    };

    $.ajax({
        url: "/Home/TableZatraty/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(data),
        dataType: "html",
        success: function (result) {
            $('#TableZatr').html(result);
              },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//------------------------------------------------------------//
// Сохранение таблицы затрат//

function TableSave() {          

    var table = document.getElementById('mytab');
    
    //var table = $('#mytab');
    var rows = table.querySelectorAll('tr');
    
    let TableZat = [];

    for (var i = 1; i < rows.length; i++) {
        var cellss = rows[i].querySelectorAll('td');
                
        for (var item = 3; item < cellss.length; item++)
        {
            var val = null;
            if (cellss[item].querySelector('input').value == "")
            {
                val = null
            }
            else {
                val = Number(cellss[item].querySelector('input').value);
            }
            var data = {
                'Id': 1,
                'FilialId': Number($('#FilialId').val()),
                'Month': Number($('#Month').val()),
                'Year': Number($('#Year').val()),
                'ExpensesId': Number(cellss[item].querySelector('input').id.split('_')[0]),
                'PlanId': Number(cellss[item].querySelector('input').id.split('_')[1]),
                'Value': val,
                'UserMod': "",
                'DateMod': null,
                'Primechanie': "",
            };
                   TableZat.push(data);
        }
        
    }       

    $.ajax({
        url: "/Home/TableSave/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(TableZat),
        dataType: "html",
        success: function (result) {
            $('#ServicesModalContent').html(result);
            $('#ServicesModal').modal('show');
            
          
            
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//----ФОРМИРОВАНИЕ ОТЧЕТОВ НА СТРАНИЦЕ ----------------//
// Сохранение редактирования пользователя//

function HtmlZatraty() {

    var isValid = true;

    if ($('#Period').val() == "") {
        $('#Period').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Period').css('border-color', 'lightgrey');
    }

    if ($('#Year').val() == "") {
        $('#Year').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Year').css('border-color', 'lightgrey');
    }

    if (($('#Period').val() == 1) && ($('#Month').val() == "")) {
        $('#Month').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Month').css('border-color', 'lightgrey');
    }
    

    if (isValid == false) {
        return false;
    }

    var selected = []
    var checkboxes = document.querySelectorAll('input[name=inList]:checked');

    for (var i = 0; i < checkboxes.length; i++) {
        selected.push(checkboxes[i].value);
    }

    var mont = null;
    
    var data = {
        'Month':Number($('#Month').val()),
        'Year': $('#Year').val(),
        'Period': $('#Period').val(),
        'Zatraty': selected        
    };

    $.ajax({
        url: "/Home/HtmlZatraty/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(data),
        dataType: "html",
        success: function (result) {
            $('#TableReport').html(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function HtmlFilialy() {

    var isValid = true;

    if ($('#Period').val() == "") {
        $('#Period').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Period').css('border-color', 'lightgrey');
    }

    if ($('#Year').val() == "") {
        $('#Year').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Year').css('border-color', 'lightgrey');
    }

    if (($('#Period').val() == 1) && ($('#Month').val() == "")) {
        $('#Month').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Month').css('border-color', 'lightgrey');
    }


    if (isValid == false) {
        return false;
    }

    var selected = []
    var checkboxes = document.querySelectorAll('input[name=inList]:checked');

    for (var i = 0; i < checkboxes.length; i++) {
        selected.push(checkboxes[i].value);
    }

    var mont = null;

    var data = {
        'Month': Number($('#Month').val()),
        'Year': $('#Year').val(),
        'Period': $('#Period').val(),
        'Zatraty': selected
    };

    $.ajax({
        url: "/Home/HtmlFilialy/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(data),
        dataType: "html",
        success: function (result) {
            $('#TableReport').html(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//------------------------------------------------------------//