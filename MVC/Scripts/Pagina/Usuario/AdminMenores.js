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
                $("#sctModificarSexoUsuario").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctModificarSexoUsuario").append($('<option>', {
                        value: data.Respuesta[i].Id,
                        text: data.Respuesta[i].Descripcion
                    }));
                }
            }
        });
}

function selectEstatusMenor() {
    $.ajax
        ({
            type: 'POST',
            url: "https://localhost:44335/api/Catalogo/ListarCatalogoEstatusMenor",
            success: function (data) {
                $("#sltModificarEstatus").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sltModificarEstatus").append($('<option>', {
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
                $("#sctModificarCentroLabores").children('option:not(:first)').remove();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#sctModificarCentroLabores").append($('<option>', {
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
                tblAdminMenores.clear().draw();
                for (i = 0; i < data.Respuesta.length; i++) {
                    tblAdminMenores.row.add({
                        "nombres": data.Respuesta[i].nombres,
                        "EdadTexto": data.Respuesta[i].EdadTexto,
                        "antecedentes": data.Respuesta[i].antecedentes,
                        "CentroAdopcion": data.Respuesta[i].CentroAdopcion,
                        "Estatus": data.Respuesta[i].Estatus
                    }).node().id = data.Respuesta[i].idMenorAdopcion;
                    tblAdminMenores.draw();
                }
            }
        });
}

function listaTramites() {
    $.ajax
        ({
            type: 'POST',
            url: urlListaTramites,
            success: function (data) {
                tblTramites.clear().draw();
                for (i = 0; i < data.Respuesta.length; i++) {
                    tblTramites.row.add({
                        "FechaTramite": ddMMyyyy(data.Respuesta[i].FechaTramite),
                        "Solicitante": data.Respuesta[i].Solicitante,
                        "Menor": data.Respuesta[i].Menor,
                        "CentroAdopcion": data.Respuesta[i].CentroAdopcion,
                        "Estatus": data.Respuesta[i].Estatus,
                    }).node().id = data.Respuesta[i].IdTramite;
                    tblTramites.draw();
                }
            }
        });
}

function listaEvaluaciones() {
    $.ajax
        ({
            type: 'POST',
            url: urlListaEvaluaciones,
            success: function (data) {
                for (i = 0; i < data.Respuesta.length; i++) {
                    tblEvaluaciones.row.add({
                        "Nombre": data.Respuesta[i].Nombre,
                        "CURP": data.Respuesta[i].CURP,
                        "CorreoElectronico": data.Respuesta[i].CorreoElectronico,
                        "Telefono": data.Respuesta[i].Telefono,
                        "FechaTest": ddMMyyyy(data.Respuesta[i].FechaTest),
                        "EstiloCrianza": data.Respuesta[i].EstiloCrianza,
                    }).draw();
                }
            }
        });
}

function listaSolicitantes() {
    $.ajax
        ({
            type: 'POST',
            url: urlListaSolicitantes,
            success: function (data) {
                tblSolicitantes.clear().draw();
                for (i = 0; i < data.Respuesta.length; i++) {
                    tblSolicitantes.row.add({
                        "Nombre": data.Respuesta[i].Nombre,
                        "CURP": data.Respuesta[i].CURP,
                        "CorreoElectronico": data.Respuesta[i].CorreoElectronico,
                        "Telefono": data.Respuesta[i].Telefono,
                        "Estatus": data.Respuesta[i].Estatus
                    }).node().id = data.Respuesta[i].IdUsuario;
                    tblSolicitantes.draw();
                }
            }
        });
}

function modificarMenor() {
    var pmtPeticion = new Object();
    pmtPeticion.idMenorAdopcion = $("#idMenorAdopcion").val();
    pmtPeticion.nombres = $("#txtModalModificarNombreMenor").val();
    pmtPeticion.apellidos = $("#txtModalModificarApellidosMenor").val();
    pmtPeticion.idSexo = $("#sctModificarSexoUsuario option:selected").val();
    pmtPeticion.antecedentes = $("#txtModalModificarAntecedentesMenor").val();
    pmtPeticion.idCentroAdopcion = $("#sctModificarCentroLabores option:selected").val();
    pmtPeticion.idEstatus = $("#sltModificarEstatus option:selected").val();
    pmtPeticion.cAl = $("#sltModificarcAl option:selected").val();
    pmtPeticion.cAp = $("#sltModificarcAp option:selected").val();
    pmtPeticion.cAs = $("#sltModificarcAs option:selected").val();
    pmtPeticion.cAt = $("#sltModificarcAt option:selected").val();
    pmtPeticion.cRp = $("#sltModificarcRp option:selected").val();
    pmtPeticion.cEm = $("#sltModificarcEm option:selected").val();
    pmtPeticion.cEe = $("#sltModificarcEe option:selected").val();
    pmtPeticion.cIn = $("#sltModificarcIn option:selected").val();
    pmtPeticion.cFl = $("#sltModificarcFl option:selected").val();
    pmtPeticion.cRf = $("#sltModificarcRf option:selected").val();
    pmtPeticion.cSc = $("#sltModificarcSc option:selected").val();
    pmtPeticion.cTf = $("#sltModificarcTf option:selected").val();
    pmtPeticion.cAg = $("#sltModificarcAg option:selected").val();
    pmtPeticion.cDl = $("#sltModificarcDl option:selected").val();
    pmtPeticion.edad = $("#txtModalModificarAniosMenor option:selected").val();
    pmtPeticion.edadMeses = $("#txtModalModificarMesesMenor option:selected").val();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlModificarMenor,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                $("#modalModificarMenor").modal('hide');
                if (data.Exitoso === false) {
                    bootbox.alert(data.Mensaje);
                }
                else {
                    ObtenerMenores();
                    bootbox.alert("Se modificó el menor correctamente.");
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
                if (data.Exitoso === false) {
                    bootbox.alert(data.Mensaje);
                }
                else {
                    ObtenerMenores();
                    bootbox.alert("Se agregó el menor correctamente.");
                }
            }
        });
}


$(document).ready(function () {

    tblAdminMenores = $('#tblAdminMenores').DataTable({
        responsive: true,
        ordering: true,
        bFilter: true,
        bLengthChange: true,
        bInfo: true,
        columns: [
            { "data": 'nombres' },
            { "data": 'EdadTexto' },
            { "data": 'antecedentes' },
            { "data": 'CentroAdopcion' },
            { "data": 'Estatus'},
            {
                "data": 'Tramite',
                "render": function (data, type, row, meta) {
                    return '<button type="button" id="btnDetallesMenor" data-toggle="modal" data-target="#modalModificarMenor" class="btn btn-info">Detalles / Modificar</button>';
                }
            }
        ],
        dom: 'l<"toolbar">frtip',
        initComplete: function () {
            $("div.toolbar").html('<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modalRegistroMenor">Agregar registro</button>');
        }
    });
    $('#tblAdminMenores tbody').on('click', '#btnDetallesMenor', function () {
        var row = $(this).closest('tr')[0];
        var idMenorAdopcion = tblAdminMenores.row(row).node().id;
        var pmtPeticion = new Object();
        pmtPeticion.idMenorAdopcion = idMenorAdopcion;
        $.ajax
            ({
                type: 'POST',
                url: urlDetalleMenor,
                data: pmtPeticion,
                success: function (data) {
                    if (data.Exitoso === false) {
                        bootbox.alert(data.Mensaje);
                    }
                    else {
                        $("#idMenorAdopcion").val(data.Respuesta[0].idMenorAdopcion);
                        $("#txtModalModificarNombreMenor").val(data.Respuesta[0].nombres);
                        $("#txtModalModificarApellidosMenor").val(data.Respuesta[0].apellidos);
                        $("#txtModalModificarAniosMenor").prop('selectedIndex', data.Respuesta[0].edad);
                        $("#txtModalModificarMesesMenor").prop('selectedIndex', data.Respuesta[0].edadMeses);
                        $("#sctModificarSexoUsuario").prop('selectedIndex', data.Respuesta[0].idSexo);
                        $("#sctModificarCentroLabores").prop('selectedIndex', data.Respuesta[0].idCentroAdopcion);
                        $("#sltModificarEstatus").prop('selectedIndex', data.Respuesta[0].idEstatus);
                        $("#txtModalModificarAntecedentesMenor").val(data.Respuesta[0].antecedentes);
                        $("#sltModificarcAl").prop('selectedIndex', data.Respuesta[0].cAl);
                        $("#sltModificarcAp").prop('selectedIndex', data.Respuesta[0].cAp);
                        $("#sltModificarcAs").prop('selectedIndex', data.Respuesta[0].cAs);
                        $("#sltModificarcAt").prop('selectedIndex', data.Respuesta[0].cAt);
                        $("#sltModificarcRp").prop('selectedIndex', data.Respuesta[0].cRp);
                        $("#sltModificarcEm").prop('selectedIndex', data.Respuesta[0].cEm);
                        $("#sltModificarcEe").prop('selectedIndex', data.Respuesta[0].cEe);
                        $("#sltModificarcIn").prop('selectedIndex', data.Respuesta[0].cIn);
                        $("#sltModificarcFl").prop('selectedIndex', data.Respuesta[0].cFl);
                        $("#sltModificarcRf").prop('selectedIndex', data.Respuesta[0].cRf);
                        $("#sltModificarcSc").prop('selectedIndex', data.Respuesta[0].cSc);
                        $("#sltModificarcTf").prop('selectedIndex', data.Respuesta[0].cTf);
                        $("#sltModificarcAg").prop('selectedIndex', data.Respuesta[0].cAg);
                        $("#sltModificarcDl").prop('selectedIndex', data.Respuesta[0].cDl);
                    }
                }
            });
    });
    tblTramites = $('#tblTramites').DataTable({
        responsive: true,
        ordering: true,
        bFilter: true,
        bLengthChange: true,
        bInfo: true,
        columns: [
            { "data": 'FechaTramite' },
            { "data": 'Solicitante' },
            { "data": 'Menor' },
            { "data": 'CentroAdopcion' },
            { "data": 'Estatus' },
            {
                "data": 'Aprobar',
                "render": function (data, type, row, meta) {
                    if (row.Estatus.localeCompare("Aceptado") != 0) {
                        return '<button type="button" id="btnAprobarTramite" class="btn btn-success">Aprobar</button>';
                    }
                    else {
                        return '<button type="button" id="btnAprobarTramite" class="btn btn-success" disabled>Aprobar</button>';
                    }
                }
            },
            {
                "data": 'Rechazar',
                "render": function (data, type, row, meta) {
                    if (row.Estatus.localeCompare("Rechazado") != 0) {
                        return '<button type="button" id="btnRechazarTramite" class="btn btn-danger">Rechazar</button>';
                    }
                    else {
                        return '<button type="button" id="btnRechazarTramite" class="btn btn-danger" disabled>Rechazar</button>';
                    }
                }
            }
        ]
    });
    $('#tblTramites tbody').on('click', '#btnAprobarTramite', function () {
        var row = $(this).closest('tr')[0];
        var idTramite = tblTramites.row(row).node().id;
        var pmtPeticion = new Object();
        pmtPeticion.IdTramite = idTramite;
        pmtPeticion.IdEstatus = 1;
        $.ajax
            ({
                type: 'POST',
                url: urlModificarEstatusTramite,
                data: pmtPeticion,
                beforeSend: function () {
                    MensajeCargando();
                },
                success: function (data) {
                    bootbox.hideAll();
                    if (data.Exitoso === false) {
                        bootbox.alert(data.Mensaje);
                    }
                    else {
                        bootbox.alert("Se aprobó el trámite.");
                        listaTramites();
                    }
                }
            });
    });
    $('#tblTramites tbody').on('click', '#btnRechazarTramite', function () {
        var row = $(this).closest('tr')[0];
        var idTramite = tblTramites.row(row).node().id;
        var pmtPeticion = new Object();
        pmtPeticion.IdTramite = idTramite;
        pmtPeticion.IdEstatus = 2;
        $.ajax
            ({
                type: 'POST',
                url: urlModificarEstatusTramite,
                data: pmtPeticion,
                beforeSend: function () {
                    MensajeCargando();
                },
                success: function (data) {
                    bootbox.hideAll();
                    if (data.Exitoso === false) {
                        bootbox.alert(data.Mensaje);
                    }
                    else {
                        bootbox.alert("Se rechazó el trámite.");
                        listaTramites();
                    }
                }
            });
    });
    tblEvaluaciones = $('#tblEvaluaciones').DataTable({
        responsive: true,
        ordering: true,
        bFilter: true,
        bLengthChange: true,
        bInfo: true,
        columns: [
            { "data": 'Nombre' },
            { "data": 'CURP' },
            { "data": 'CorreoElectronico' },
            { "data": 'Telefono' },
            { "data": 'FechaTest' },
            { "data": 'EstiloCrianza' },
        ]
    });
    tblSolicitantes = $('#tblSolicitantes').DataTable({
        responsive: true,
        ordering: true,
        bFilter: true,
        bLengthChange: true,
        bInfo: true,
        columns: [
            { "data": 'Nombre' },
            { "data": 'CURP' },
            { "data": 'CorreoElectronico' },
            { "data": 'Telefono' },
            { "data": 'Estatus' },
            {
                "data": 'Aprobar',
                "render": function (data, type, row, meta) {
                    if (row.Estatus.localeCompare("Activo") != 0) {
                        return '<button type="button" id="btnValidarSolicitante" class="btn btn-success">Validar</button>';
                    }
                    else {
                        return '<button type="button" id="btnValidarSolicitante" class="btn btn-success" disabled>Validar</button>';
                    }
                }
            },
            {
                "data": 'Rechazar',
                "render": function (data, type, row, meta) {
                    if (row.Estatus.localeCompare("Inactivo") != 0) {
                        return '<button type="button" id="btnInactivarSolicitante" class="btn btn-danger">Inactivar</button>';
                    }
                    else {
                        return '<button type="button" id="btnInactivarSolicitante" class="btn btn-danger" disabled>Inactivar</button>';
                    }
                }
            }
        ]
    });
    $('#tblSolicitantes tbody').on('click', '#btnValidarSolicitante', function () {
        var row = $(this).closest('tr')[0];
        var idUsuario = tblSolicitantes.row(row).node().id;
        var pmtPeticion = new Object();
        pmtPeticion.IdUsuario = idUsuario;
        pmtPeticion.IdEstatus = 1;
        $.ajax
            ({
                type: 'POST',
                url: urlModificarEstatusSolicitante,
                data: pmtPeticion,
                beforeSend: function () {
                    MensajeCargando();
                },
                success: function (data) {
                    bootbox.hideAll();
                    if (data.Exitoso === false) {
                        bootbox.alert(data.Mensaje);
                    }
                    else {
                        bootbox.alert("Se activó el solicitante.");
                        listaSolicitantes();
                    }
                }
            });
    });
    $('#tblSolicitantes tbody').on('click', '#btnInactivarSolicitante', function () {
        var row = $(this).closest('tr')[0];
        var idUsuario = tblSolicitantes.row(row).node().id;
        var pmtPeticion = new Object();
        pmtPeticion.IdUsuario = idUsuario;
        pmtPeticion.IdEstatus = 2;
        $.ajax
            ({
                type: 'POST',
                url: urlModificarEstatusSolicitante,
                data: pmtPeticion,
                beforeSend: function () {
                    MensajeCargando();
                },
                success: function (data) {
                    bootbox.hideAll();
                    if (data.Exitoso === false) {
                        bootbox.alert(data.Mensaje);
                    }
                    else {
                        bootbox.alert("Se inactivó el solicitante.");
                        listaSolicitantes();
                    }
                }
            });
    });
    selectCatalogoSexo();
    selectCatalogCentrosLaborales();
    selectEstatusMenor();
    ObtenerMenores();
    listaTramites();
    listaEvaluaciones();
    listaSolicitantes();
    $("#confirmarRegistro").click(function () {
        registrarMenor();
    });
    $("#btnModificarMenor").click(function () {
        modificarMenor();
    });

});