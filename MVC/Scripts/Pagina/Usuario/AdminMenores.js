
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
                $("#sctSexoUsuario2").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctSexoUsuario2").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
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


function ObtenerMenores() {
    $.ajax
        ({
            type: 'POST',
            url: urlListaMenores,
            success: function (data) {
                for (i = 0; i < data.length; i++) {
                    tblAdminMenores.row.add({                      
                        "Nombre": data[i].nombres + data[i].apellidos,
                        "Edad": data[i].edad + data[i].edadMeses,
                        "idCentroAdopcion": data[i].idCentroAdopcion,
                        "idEstatus": data[i].idEstatus,
                    }).draw();
                }
            }
        });
}


function registrarMenor() {
    var pmtPeticion = new Object();
    pmtPeticion.nombres = $("#txtModalNombreMenor").val();
    pmtPeticion.apellidos = $("#txtModalApellidosMenor").val();
    pmtPeticion.idSexo = $("#sctSexoUsuario option:selected").val();
    pmtPeticion.antecedentes = $("#txtModalAntecedentesMenor").val();
    pmtPeticion.idCentroAdopcion = $("#sctCentroLabores option:selected").val();
    pmtPeticion.cAl = $("#sltcAl option:selected").val();
    pmtPeticion.cAp = $("#sltcAp option:selected").val();
    pmtPeticion.cAs = $("#sltcAs option:selected").val();
    pmtPeticion.cAt = $("#sltcAt option:selected").val();
    pmtPeticion.cRp = $("#sltcRp option:selected").val();
    pmtPeticion.cEm = $("#sltcEm option:selected").val();
    pmtPeticion.cEe = $("#sltcEe option:selected").val();
    pmtPeticion.cIn = $("#sltcIn option:selected").val();
    pmtPeticion.cFl = $("#sltcFl option:selected").val();
    pmtPeticion.cRf = $("#sltcRf option:selected").val();
    pmtPeticion.cSc = $("#sltcSc option:selected").val();
    pmtPeticion.cTf = $("#sltcTf option:selected").val();
    pmtPeticion.cAg = $("#sltcAg option:selected").val();
    pmtPeticion.cDl = $("#sltcDl option:selected").val();
    pmtPeticion.edad = $("#txtModalAniosMenor option:selected").val();
    pmtPeticion.edadMeses = $("#txtModalMesesMenor option:selected").val();
    
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlRegistrarMenor,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                $("#modalRegistroMenor").modal('hide');
                bootbox.alert(data);
                
            }
        });
}


$(document).ready(function () {

    tblAdminMenores = $('#tblAdminMenores').DataTable({
        responsive: true,
        ordering: false,
        bFilter: false,
        bLengthChange: false,
        bInfo: false,
        columns: [
            { "data": 'Nombre', "width": "30%" },
            { "data": 'Edad', "width": "15%" },
            { "data": 'idCentroAdopcion', "width": "20%" },
            { "data": 'idEstatus', "width": "15%" },
            {
                "data": 'Opciones',
                "render": function () {
                    return '<nav class="navbar navbar-expand-sm navbar-light bg-light">' +
                        '<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#opciones">' +
                        '<span class="navbar-toggler-icon"></span>' +
                        '</button>' +
                        '<a class="navbar-brand" href="#">' +
                        '<img src="http://www.tutorialesprogramacionya.com/imagenes/foto1.jpg" width="30" height="30" alt="">' +
                        '</a>  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<div class="collapse navbar-collapse" id="opciones">  ' +
                        '<ul class="navbar-nav">' +
                        '<li class="nav-item">' +
                        '<a class="nav-link" href="#">Opción 1</a>' +
                        '</li>' +
                        '<li class="nav-item">' +
                        '<a class="nav-link" href="#">Opción 2</a>' +
                        '</li>' +
                        '<li class="nav-item">' +
                        '<a class="nav-link" href="#">Opción 3</a>' +
                        '</li>' +
                        '<li class="nav-item">' +
                        '<a class="nav-link" href="#">Opción 4</a>' +
                        '</li>' +
                        '</ul>' +
                        '</div>'+
                        '</nav>';
                }, "width": "20%" }]
    });


    
    ObtenerMenores();
    selectCatalogoSexo();
    selectCatalogCentrosLaborales();

    $("#confirmarRegistro").click(function () {
        registrarMenor();
    });

});