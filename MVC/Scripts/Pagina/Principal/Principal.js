function selectCatalogoSexo() {
    $.ajax
        ({
            type: 'POST',
            url: "https://localhost:44335/api/Catalogo/ListarCatalogoSexo",
            success: function (data) {
                $("#sctSexoUsuario").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctSexoUsuario").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
            }
        });
}

function selectCatalogoNivelEstudios() {
    $.ajax
        ({
            type: 'POST',
            url: "https://localhost:44335/api/Catalogo/ListarCatalogoNivelEstudios",
            success: function (data) {
                $("#sctNivelEstudios").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctNivelEstudios").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
            }
        });
}

function selectCatalogoEstadoCivil() {
    $.ajax
        ({
            type: 'POST',
            url: "https://localhost:44335/api/Catalogo/ListarCatalogoEstadoCivil",
            success: function (data) {
                $("#sctEstadoCivil").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctEstadoCivil").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
            }
        });
}


function registroSolicitante() {
    var pmtPeticion = new Object();
    pmtPeticion.CURP = $("#txtCURPUsuario").val();
    pmtPeticion.Nombre = $("#txtNombreUsuario").val();
    pmtPeticion.FechaNacimiento = $("#dtFechaNacimientoUsuario").val();
    pmtPeticion.IdSexo = $("#sctSexoUsuario option:selected").val();
    pmtPeticion.IdEstadoCivil = $("#sctEstadoCivil option:selected").val();
    pmtPeticion.IdNivelEstudios = $("#sctNivelEstudios option:selected").val();
    pmtPeticion.Ocupacion = $("#txtOcupacion").val();
    pmtPeticion.Direccion = $("#txtDireccion").val();
    pmtPeticion.Telefono = $("#txtTelefonoUsuario").val();
    pmtPeticion.CorreoElectronico = $("#txtCorreoRegistroUsuario").val();
    pmtPeticion.Password = $("#txtPasswordRegistroUsuario").val();
    $.ajax
        ({
            type: 'POST',
            data: { PmtPeticion: pmtPeticion, ReCaptchaResponse: grecaptcha.getResponse() },
            url: urlRegistroSolicitante,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                if (data.Exitoso === false) {
                    bootbox.alert(data.Mensaje);
                }
            }
        });
}

$(document).ready(function () {
    $('.carousel').carousel({
        interval: 5000,
        pause: true,
        wrap: true
    });
    $("#dtFechaNacimientoUsuario").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy'
    });
    selectCatalogoSexo();
    selectCatalogoEstadoCivil();
    selectCatalogoNivelEstudios();
});