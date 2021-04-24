function LlenarComboGenero() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoGenero,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                    bootbox.hideAll();
                    $("#txtGeneroAl").children('option:not(:first)').remove();
                    $("#txtGeneroEx").children('option:not(:first)').remove();
                    for (i = 0; i < data.length; i++) {
                        $("#txtGeneroAl").append($('<option>', {
                            value: data[i].Id,
                            text: data[i].Descripcion
                        }));
                        $("#txtGeneroEx").append($('<option>', {
                            value: data[i].Id,
                            text: data[i].Descripcion
                        }));
                    }
            }
            });
}

function LlenarComboNivel() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoNivel,
            success: function (data) {
                $("#txtNivel").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#txtNivel").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                }
            }
        });
}

function LlenarComboEscuela() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoEscuela,
            success: function (data) {
                $("#txtEscuelaAl").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#txtEscuelaAl").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                }
            }
        });
}

function LlenarComboCarrera() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoCarrera,
            success: function (data) {
                $("#txtCarreraAl").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#txtCarreraAl").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                }
            }
        });
}

function LlenarComboSituacion() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoSituacion,
            success: function (data) {
                $("#txtSituaAl").children('option:not(:first)').remove();
                $("#txtSituaEx").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#txtSituaAl").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                    $("#txtSituaEx").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                }
            }
        });
}

function LlenarComboMedio() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoMedio,
            success: function (data) {
                $("#txtMedioAl").children('option:not(:first)').remove();
                $("#txtMedioEx").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#txtMedioAl").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                    $("#txtMedioEx").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                }
            }
        });
}

function LlenarComboEvento() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoEvento,
            success: function (data) {
                $("#txtEventoAl").children('option:not(:first)').remove();
                $("#txtEventoEx").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#txtEventoAl").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                    $("#txtEventoEx").append($('<option>', {
                        value: data[i].Id,
                        text: data[i].Descripcion
                    }));
                }
            }
        });
}

function LimpiarFormularios() {
    $("#txtNombreAl").val('');
    $("#txtApPatAl").val('');
    $("#txtApMatAl").val('');
    $("#txtBoleta").val('');
    $("#txtEventoAl").prop('selectedIndex', 0);
    $("#txtEdadAl").val('');
    $("#txtEscuelaAl").prop('selectedIndex', 0);
    $("#txtNivel").prop('selectedIndex', 0);
    $("#txtCarreraAl").prop('selectedIndex', 0);
    $("#txtEmailAl").val('');
    $("#txtGeneroAl").prop('selectedIndex', 0);
    $("#txtSituaAl").prop('selectedIndex', 0);
    $("#txtMedioAl").prop('selectedIndex', 0);
    $("#txtNombreEx").val('');
    $("#txtApPatEx").val('');
    $("#txtApMatEx").val('');
    $("#txtEventoEx").prop('selectedIndex', 0);
    $("#txtEdadEx").val('');
    $("#txtEscuelaEx").val('');
    $("#txtCarreraEx").val('');
    $("#txtEmailEx").val('');
    $("#txtGeneroEx").prop('selectedIndex', 0);
    $("#txtSituaEx").prop('selectedIndex', 0);
    $("#txtMedioEx").prop('selectedIndex', 0);
}

function RegistrarAlumno() {
    if ($("#txtNombreAl").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtApPatAl").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtApMatAl").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtBoleta").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtEdadAl").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtGeneroAl option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtEscuelaAl option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtNivel option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtCarreraAl option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtSituaAl option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtEventoAl option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtEmailAl").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtMedioAl option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    var pmtPeticion = new Object();
    pmtPeticion.Nombre = $("#txtNombreAl").val();
    pmtPeticion.ApPaterno = $("#txtApPatAl").val();
    pmtPeticion.ApMaterno = $("#txtApMatAl").val();
    pmtPeticion.Boleta = $("#txtBoleta").val();
    pmtPeticion.TipoFeria = $("#txtEventoAl option:selected").val();
    pmtPeticion.Edad = $("#txtEdadAl").val();
    pmtPeticion.Escuela = $("#txtEscuelaAl option:selected").val();
    pmtPeticion.Nivel = $("#txtNivel option:selected").val();
    pmtPeticion.Carrera = $("#txtCarreraAl option:selected").val();
    pmtPeticion.Email = $("#txtEmailAl").val();
    pmtPeticion.Genero = $("#txtGeneroAl option:selected").val();
    pmtPeticion.Situacion = $("#txtSituaAl option:selected").val();
    pmtPeticion.Medio = $("#txtMedioAl option:selected").val();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlRegistroAlumno,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                LimpiarFormularios();
            }
        });
}

function RegistrarExterno() {
    if ($("#txtNombreEx").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtApPatEx").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtApMatAlEx").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtEdadAlEx").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtGeneroEx option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtEscuelaEx").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtCarreraEx").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtSituaEx option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtEventoEx option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtEmailEx").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtMedioEx option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Por favor, asegúrate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    var pmtPeticion = new Object();
    pmtPeticion.Nombre = $("#txtNombreEx").val();
    pmtPeticion.ApPaterno = $("#txtApPatEx").val();
    pmtPeticion.ApMaterno = $("#txtApMatEx").val();
    pmtPeticion.TipoFeria = $("#txtEventoEx option:selected").val();
    pmtPeticion.Edad = $("#txtEdadEx").val();
    pmtPeticion.Escuela = $("#txtEscuelaEx").val();
    pmtPeticion.Carrera = $("#txtCarreraEx").val();
    pmtPeticion.Email = $("#txtEmailEx").val();
    pmtPeticion.Genero = $("#txtGeneroEx option:selected").val();
    pmtPeticion.Situacion = $("#txtSituaEx option:selected").val();
    pmtPeticion.Medio = $("#txtMedioEx option:selected").val();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlRegistroExterno,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                LimpiarFormularios();
            }
        });
}

$(document).ready(function () {
    LlenarComboGenero();
    LlenarComboNivel();
    LlenarComboEscuela();
    LlenarComboCarrera();
    LlenarComboSituacion();
    LlenarComboMedio();
    LlenarComboEvento();
    $("#btnRegistrarAl").click(function () {
        RegistrarAlumno();
    });
    $("#btnRegistrarEx").click(function () {
        RegistrarExterno();
    });
});