var correoElectronicoDisponible = false;
var boletaEmpleadoDisponible = false;
var usuario = new Object();
var image = null;
var listaMascotas = [];
var currentMascota = 0;
var maxMascotas = 0;

function llenarSelectSexo() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoSexoMascota,
            success: function (data) {
                $("#sctModalSexo").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#sctModalSexo").append($('<option>', {
                        value: data[i].ID,
                        text: data[i].Descripcion
                    }));
                }
            }
        });
}

function llenarSelectTipo() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoTipoMascota,
            success: function (data) {
                $("#sctModalTipo").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#sctModalTipo").append($('<option>', {
                        value: data[i].ID,
                        text: data[i].Descripcion
                    }));
                }
            }
        });
}

function llenarDatos() {
    $.ajax
        ({
            type: 'POST',
            url: urlDatosUsuario,
            success: function (data) {
                $("#txtModalNombre").val(data.Nombre);
                $("#txtModalDireccion").val(data.Direccion);
                $("#sctModalTipo").prop('selectedIndex', data.IDTipoMascota);
                $("#sctModalSexo").prop('selectedIndex', data.IDSexo);
                $("#txtModalNombreMascota").val(data.NombreMascota);
                $("#txtModalCorreoElectronico").val(data.CorreoElectronico);
                $("#txtModalRaza").val(data.Raza);
                $("#modalMyImg").attr("src", data.URLImagen + "?" + Math.random());
                $("#modalFileImagen").val("");
                usuario.CorreoElectronico = data.CorreoElectronico;
                usuario.IDSexo = data.IDSexo;
                usuario.IDTipoMascota = data.IDTipoMascota;
                usuario.URLImagen = data.URLImagen;
                selectListaMascotas();
            }
        });
}


function validarCorreoElectronicoDisponible() {
    if ($("#txtModalCorreoElectronico").val().localeCompare(usuario.CorreoElectronico) != 0) {
        var pmtPeticion = new Object();
        pmtPeticion.CorreoElectronico = $("#txtModalCorreoElectronico").val();
        $.ajax
            ({
                type: 'POST',
                data: pmtPeticion,
                url: urlValidarCorreoElectronicoDisponible,
                success: function (data) {
                    if (data[0]) {
                        $("#txtModalCorreoElectronico").css({ 'border-color': '' });
                    }
                    else {
                        $("#txtModalCorreoElectronico").css("border-color", "red");
                    }
                    correoElectronicoDisponible = data[0];
                }
            });
    }
    else {
        $("#txtModalCorreoElectronico").css({ 'border-color': '' });
        correoElectronicoDisponible = true;
    }
}

function validarContraseña() {
    if ($("#txtModalConfirmarPassword").val() != $("#txtModalNuevaPassword").val()) {
        $("#txtModalConfirmarPassword").css("border-color", "red");
        return false;
    }
    else {
        $("#txtModalConfirmarPassword").css({ 'border-color': '' });
        return true;
    }
}

function modificarDatos() {
    var pmtPeticion = new FormData();
    if ($("#txtModalNombre").val() == "") {
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
    if ($("#sctModalTipo option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#sctModalSexo option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalNombreMascota").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalRaza").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtModalCorreoElectronico").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if (!correoElectronicoDisponible) {
        bootbox.alert
            ({
                message: '<center><label>El correo electr&oacute;nico no est&aacute; disponible.</label></center>'
            });
        return;
    }
    if ($("#txtModalContraseña").val() != "" || $("#txtModalNuevaContraseña").val() != "" || $("#txtModalConfirmarContraseña").val() != "") {
        if (!validarContraseña()) {
            bootbox.alert
                ({
                    message: '<center><label>Las contrase&ntilde;as no coinciden.</label></center>'
                });
            return;
        }
    }
    pmtPeticion.append("Nombre", $("#txtModalNombre").val());
    pmtPeticion.append("Direccion", $("#txtModalDireccion").val());
    pmtPeticion.append("IDTipoMascota", $("#sctModalTipo option:selected").val());
    pmtPeticion.append("IDSexo", $("#sctModalSexo option:selected").val());
    pmtPeticion.append("NombreMascota", $("#txtModalNombreMascota").val());
    pmtPeticion.append("Raza", $("#txtModalRaza").val());
    pmtPeticion.append("CorreoElectronico", $("#txtModalCorreoElectronico").val());
    pmtPeticion.append("Password", $("#txtModalPassword").val());
    pmtPeticion.append("NuevaPassword", $("#txtModalConfirmarPassword").val());
    pmtPeticion.append("UploadImagen", image);
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlModificarDatos,
            contentType: false,
            processData: false,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                $("#modalFileImagen").val("");
                image = null;
                $("#menu-model").html(data);
            }
        });
    if (pmtPeticion.get("Password") != "" || pmtPeticion.get("NuevaPassword") != "") {
        $.ajax
            ({
                type: 'POST',
                data: pmtPeticion,
                url: urlModificarContraseña,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    MensajeCargando();
                },
                success: function (data) {
                    bootbox.hideAll();
                    if (!data.Exitoso) {
                        bootbox.alert
                            ({
                                message: '<center><label>La contrase&ntilde;a es incorrecta. La contrase&ntilde;a no fue modificada.</label></center>'
                            });
                    }
                    $("#txtModalPassword").val("");
                    $("#txtModalNuevaPassword").val("");
                    $("#txtModalConfirmarPassword").val("");
                }
            });
    }
}

function like() {
    $.ajax
        ({
            type: 'POST',
            data: listaMascotas[currentMascota],
            url: urlInsertLike,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                listaMascotas.splice(currentMascota, 1);
                maxMascotas = listaMascotas.length;
                if (currentMascota == maxMascotas - 1) {
                    currentMascota = 0;
                }
                else {
                    currentMascota++;
                }
                mostrarInfo();
            }
        });
}

function pasar() {
    if (currentMascota == maxMascotas - 1) {
        currentMascota = 0;
    }
    else {
        currentMascota++;
    }
    mostrarInfo();
}

function mostrarInfo() {
    $("#perfilNombre").html(listaMascotas[currentMascota].NombreMascota);
    $("#perfilDatos").html(listaMascotas[currentMascota].Nombre + ", " + listaMascotas[currentMascota].CorreoElectronico + ", " + listaMascotas[currentMascota].Raza + ", " + listaMascotas[currentMascota].Direccion);
    $("#perfilImg").attr("src", listaMascotas[currentMascota].URLImagen + "?" + Math.random());
}

function selectListaMascotas() {
    $.ajax
        ({
            type: 'POST',
            data: usuario,
            url: urlSelectListaMascotas,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                listaMascotas = data.Respuesta;
                maxMascotas = listaMascotas.length;
                mostrarInfo();
            }
        });
}

$(document).ready(function () {
    llenarSelectTipo();
    llenarSelectSexo();
    llenarDatos();
    $('#txtModalCorreoElectronico').on('input', function (e) {
        validarCorreoElectronicoDisponible();
    });
    $('#txtModalConfirmarPassword').on('input', function (e) {
        validarContraseña();
    });
    $("#btnModificarInformacion").click(function () {
        validarCorreoElectronicoDisponible();
        modificarDatos();
    });
    $("#btnLike").click(function () {
        like();
    });
    $("#btnPasar").click(function () {
        pasar();
    });
    $('input[type="file"]').change(function () {
        var imagePreview = document.getElementById('modalMyImg');
        image = event.target.files[0];
        imagePreview.src = URL.createObjectURL(event.target.files[0]);
    });

});