var idAlumnoCreditos = 0;
var idActividad = 0;

function pendienteValidacion(tblValidacion) {
    $.ajax
        ({
            type: 'POST',
            url: urlPendienteValidacion,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                var index = 0;
                for (index = 0; index < data.length; index++) {
                    tblValidacion.row.add({
                        "idAlumno": data[index].idAlumno,
                        "alumno": data[index].nombreAlumno,
                        "idActividad": data[index].idActividad,
                        "actividad": data[index].nombre,
                        "creditos": data[index].creditosInscritos
                    }).draw();
                }
            }
        });
}

function validarCreditos() {
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
    pmtPeticion.idActividad = idActividad;
    pmtPeticion.idAlumno = idAlumnoCreditos;
    $.ajax
        ({
            type: 'POST',
            url: urlValidarCreditos,
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
    var tblValidacion = $('#tblValidacion').DataTable({
        responsive: true,
        columns: [
            { "data": 'idAlumno' },
            { "data": 'alumno' },
            { "data": 'idActividad' },
            { "data": 'actividad' },
            { "data": 'creditos' },
            {
                "data": 'validar',
                "render": function () {
                    return '<button class="btn btn-mini btn-primary pull-right" data-toggle="modal" data-target="#modalValidar">Validar</button>';
                }
            }
        ]
    });
    pendienteValidacion(tblValidacion);
    $('#tblValidacion tbody').on('click', 'button', function () {
        var current_row = $(this).parents('tr');
        if (current_row.hasClass('child')) {
            current_row = current_row.prev();
        }
        var data = tblValidacion.row(current_row).data();
        idAlumnoCreditos = data.idAlumno;
        idActividad = data.idActividad;
    });
    $("#btnCreditos").click(function () {
        validarCreditos();
    });
});