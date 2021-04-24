var idAlumnoCreditos = 0;

function llenarSelectAreaActividad() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlCatalogoAreaActividad,
            success: function (data) {
                $("#sctArea").children('option:not(:first)').remove();
                for (i = 0; i < data.length; i++) {
                    $("#sctArea").append($('<option>', {
                        value: data[i].id,
                        text: data[i].descripcion
                    }));
                }
            }
        });
}

function registrarActividad() {
    var pmtPeticion = new Object();
    if ($("#txtNombre").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtCreditos").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtTiempo").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#sctArea option:selected").val() == "0") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtHorario").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#txtDescripcion").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    pmtPeticion.nombre = $("#txtNombre").val();
    pmtPeticion.creditos = $("#txtCreditos").val();
    pmtPeticion.tiempoCreditos = $("#txtTiempo").val();
    pmtPeticion.idAreaActividad = $("#sctArea option:selected").val();
    pmtPeticion.descripcion = $("#txtDescripcion").val();
    pmtPeticion.horario = $("#txtHorario").val();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlRegistrarActividad,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
            }
        });
}

function alumnosInscritos(tblAlumnos) {
    $.ajax
        ({
            type: 'POST',
            url: urlAlumnosInscritos,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                var index = 0;
                for (index = 0; index < data.length; index++) {
                    tblAlumnos.row.add({
                        "id": data[index].idAlumno,
                        "nombre": data[index].nombre,
                        "creditos": data[index].creditos
                    }).draw();
                }
            }
        });
}

function añadirCreditos() {
    if ($("#txtModalCreditos").val() == "") {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    if ($("#fleLlave").get(0).files.length == 0) {
        bootbox.alert
            ({
                message: '<center><label>Aseg&uacute;rate de llenar todos los campos correctamente.</label></center>'
            });
        return;
    }
    var files = $("#fleLlave").get(0).files;
    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        formData.append(files[i].name, files[i]);
    }
    var pmtPeticion = new Object();
    pmtPeticion.creditos = $("#txtModalCreditos").val();
    pmtPeticion.idAlumno = idAlumnoCreditos;
    $.ajax
        ({
            type: 'POST',
            url: urlRegistrarCreditos,
            data: pmtPeticion,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
            }
        });

}

$(document).ready(function () {
    var tblAlumnos = $('#tblAlumnos').DataTable({
        responsive: true,
        columns: [
            { "data": 'id' },
            { "data": 'nombre' },
            { "data": 'creditos' },
            {
                "data": 'validar',
                "render": function () {
                    return '<button class="btn btn-mini btn-primary pull-right" data-toggle="modal" data-target="#modalAlumnos">Validar</button>';
                }
            }
        ]
    });
    $('#tblAlumnos tbody').on('click', 'button', function () {
        var current_row = $(this).parents('tr');
        if (current_row.hasClass('child')) {
            current_row = current_row.prev();
        }
        var data = tblAlumnos.row(current_row).data();
        idAlumnoCreditos = data.id;
    });
    alumnosInscritos(tblAlumnos);
    llenarSelectAreaActividad();
    $("#btnRegistrarActividad").click(function () {
        registrarActividad();
    });
    $("#btnCreditos").click(function () {
        añadirCreditos();
    });
});