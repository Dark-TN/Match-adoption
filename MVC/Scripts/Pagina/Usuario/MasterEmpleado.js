const { selectCatalogCentrosLaborales } = require("../../inputmask/global/window");

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
                $("#sctSexoUsuario2").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctSexoUsuario2").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
                selectCatalogoNivelEstudios();
            }
        });
}

function selectCatalogCentrosLaborales() {
    $.ajax
        ({
            type: 'POST',
            url: "https://localhost:44335/api/Catalogo/ListarCatalogoCentrosAdopcion",
            success: function (data) {
                $("#sctCentroLabores").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctCentroLabores").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
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
                $("#dtFechaIngreso").val(ddMMyyyy(data.Fecha));
                $("#sctSexoUsuario2").prop('selectedIndex', data.IdSexo);
                $("#txtTelefonoEmpleado").val(data.Telefono);
                $("#txtCorreoRegistroEmpleado").val(data.CorreoElectronico);
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
    if ($("#dtFechaIngreso").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#sctSexoUsuario2 option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    } if ($("#sctCentroLabores option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtTelefonoEmpleado").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtCorreoRegistroEmpleado").val() == "") {
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
    pmtPeticion.FechaNacimiento = $("#dtFechaIngreso").val();
    pmtPeticion.IdSexo = $("#sctSexoUsuario2 option:selected").val();
    pmtPeticion.Telefono = $("#txtTelefonoEmpleado").val();
    pmtPeticion.CorreoElectronico = $("#txtCorreoRegistroEmpleado").val(); 
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
    selectCatalogCentrosLaborales();
    $("#btnModificarInformacion").click(function () {
        editarDatosUsuario();
    });
});