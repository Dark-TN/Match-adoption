function ddMMyyyy(ts) {
    var date = new Date(ts.match(/\d+/)[0] * 1);
    var mm = date.getMonth() + 1; // getMonth() is zero-based
    var dd = date.getDate();
    var fecha = [(dd > 9 ? '' : '0') + dd,
    (mm > 9 ? '' : '0') + mm,
    date.getFullYear()
    ].join('/');
    return fecha;
};

function selectCatalogoSexo() {
    $.ajax
        ({
            type: 'POST',
            url: "https://localhost:44335/api/Catalogo/ListarCatalogoSexo",
            success: function (data) {
                $("#sctModalSexoUsuario").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctModalSexoUsuario").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
                selectCatalogoNivelEstudios();
            }
        });
}

function selectCatalogoNivelEstudios() {
    $.ajax
        ({
            type: 'POST',
            url: "https://localhost:44335/api/Catalogo/ListarCatalogoNivelEstudios",
            success: function (data) {
                $("#sctModalNivelEstudios").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctModalNivelEstudios").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
                selectCatalogoEstadoCivil()
            }
        });
}

function selectCatalogoEstadoCivil() {
    $.ajax
        ({
            type: 'POST',
            url: "https://localhost:44335/api/Catalogo/ListarCatalogoEstadoCivil",
            success: function (data) {
                $("#sctModalEstadoCivil").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctModalEstadoCivil").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
                llenarDatos();
            }
        });
}

function llenarDatos() {
    $.ajax
        ({
            type: 'POST',
            url: urlDatosSolicitante,
            success: function (data) {
                $("#txtModalCURPUsuario").val(data.CURP);
                $("#txtModalNombreUsuario").val(data.Nombre);
                $("#dtModalFechaNacimientoUsuario").val(ddMMyyyy(data.FechaNacimiento));
                $("#sctModalSexoUsuario").prop('selectedIndex', data.IdSexo);
                $("#sctModalEstadoCivil").prop('selectedIndex', data.IdEstadoCivil);
                $("#sctModalNivelEstudios").prop('selectedIndex', data.IdNivelEstudios);
                $("#txtModalOcupacion").val(data.Ocupacion);
                $("#txtModalDireccion").val(data.Direccion);
                $("#txtModalTelefonoUsuario").val(data.Telefono);
                $("#txtModalCorreoUsuario").val(data.CorreoElectronico);
            }
        });
}

function editarDatosUsuario() {
    var pmtPeticion = new Object();
    if ($("#txtModalCURPUsuario").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalNombreUsuario").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#dtModalFechaNacimientoUsuario").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#sctModalSexoUsuario option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#sctModalEstadoCivil option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#sctModalNivelEstudios option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalOcupacion").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalDireccion").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalTelefonoUsuario").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalCorreoUsuario").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalPassword").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalNuevaPassword").val() != $("#txtModalConfirmarPassword").val()) {
        bootbox.alert
            ({
                message: '<center><label>Las contraseñas no coinciden.</label></center>'
            });
        return;
    }
    pmtPeticion.CURP = $("#txtModalCURPUsuario").val();
    pmtPeticion.Nombre = $("#txtModalNombreUsuario").val();
    pmtPeticion.FechaNacimiento = $("#dtModalFechaNacimientoUsuario").val();
    pmtPeticion.IdSexo = $("#sctModalSexoUsuario option:selected").val();
    pmtPeticion.IdEstadoCivil = $("#sctModalEstadoCivil option:selected").val();
    pmtPeticion.IdNivelEstudios = $("#sctModalNivelEstudios option:selected").val();
    pmtPeticion.Ocupacion = $("#txtModalOcupacion").val();
    pmtPeticion.Direccion = $("#txtModalDireccion").val();
    pmtPeticion.Telefono = $("#txtModalTelefonoUsuario").val();
    pmtPeticion.CorreoElectronico = $("#txtModalCorreoUsuario").val(); 
    pmtPeticion.Password = $("#txtModalPassword").val();
    pmtPeticion.NuevaPassword = $("#txtModalNuevaPassword").val();
    pmtPeticion.ConfirmarPassword = $("#txtModalConfirmarPassword").val();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlModificarDatosSolicitante,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                alert(data.Nombre);
            }
        });
}

$(document).ready(function () {
    selectCatalogoSexo();
    $("#btnModificarInformacion").click(function () {
        editarDatosUsuario();
    });
});