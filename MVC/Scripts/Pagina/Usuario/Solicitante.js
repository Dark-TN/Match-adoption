var tblTests;
var tblMatch;
var tblTramites;
var labels = [];
var chartData = {
    labels: [],
    datasets: []
};

function Match() {
    $.ajax
        ({
            type: 'POST',
            url: urlMatch,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                if (data.Exitoso === false) {
                    bootbox.alert(data.Mensaje);
                }
                else {
                    tblMatch.clear().draw();
                    for (i = 0; i < data.Respuesta.length; i++) {
                        tblMatch.row.add({
                            "nombres": data.Respuesta[i].nombres,
                            "CentroAdopcion": data.Respuesta[i].CentroAdopcion,
                            "antecedentes": data.Respuesta[i].antecedentes,
                            "Porcentaje": data.Respuesta[i].Porcentaje.toString() + '%',
                        }).node().id = data.Respuesta[i].idMenorAdopcion;
                        tblMatch.draw();
                    }
                }
            }
        });
}

function ListaTramites() {
    $.ajax
        ({
            type: 'POST',
            url: urlListaTramitesUsuario,
            success: function (data) {
                if (data.Exitoso === false) {
                    bootbox.alert(data.Mensaje);
                }
                else {
                    tblTramites.clear().draw();
                    for (i = 0; i < data.Respuesta.length; i++) {
                        tblTramites.row.add({
                            "FechaTramite": ddMMyyyy(data.Respuesta[i].FechaTramite),
                            "Menor": data.Respuesta[i].Menor,
                            "CentroAdopcion": data.Respuesta[i].CentroAdopcion,
                            "Estatus": data.Respuesta[i].Estatus
                        }).node().id = data.Respuesta[i].IdTramite;
                        tblTramites.draw();
                    }
                }
            }
        });
}

function ListaTest() {
    $.ajax
        ({
            type: 'POST',
            url: urlListarTest,
            success: function (data) {
                if (data.Exitoso === false) {
                    bootbox.alert(data.Mensaje);
                }
                else {
                    if (data.Respuesta.length > 0) {
                        for (var key in data.Respuesta[0].CalificacionesHabilidades) {
                            chartData.labels.push(key);
                        }
                    }
                    for (i = 0; i < data.Respuesta.length; i++) {
                        var dataset = {
                            label: ddMMyyyy(data.Respuesta[i].FechaFin),
                            data: [],
                            fill: true,
                            backgroundColor: 'rgba(255, 99, 132, 0.2)',
                            borderColor: 'rgb(255, 99, 132)',
                            pointBackgroundColor: 'rgb(255, 99, 132)',
                            pointBorderColor: '#fff',
                            pointHoverBackgroundColor: '#fff',
                            pointHoverBorderColor: 'rgb(255, 99, 132)'
                        };
                        tblTests.row.add({
                            "FechaFin": ddMMyyyy(data.Respuesta[i].FechaFin),
                            "EstiloCrianza": data.Respuesta[i].EstiloCrianza
                        }).draw();
                        for (var key in data.Respuesta[i].CalificacionesHabilidades) {
                            dataset.data.push(data.Respuesta[i].CalificacionesHabilidades[key]);
                        }
                        chartData.datasets.push(dataset);
                    }
                    var ctx = $('#myChart');
                    var myRadarChart = new Chart(ctx, {
                        type: 'radar',
                        data: chartData,
                        options: {
                            elements: {
                                line: {
                                    borderWidth: 3
                                }
                            }
                        },
                    });
                }
            }
        });
}

$(document).ready(function () {
    tblTests = $('#tblTests').DataTable({
        responsive: true,
        ordering: true,
        bFilter: false,
        bLengthChange: false,
        bInfo: false,
        columns: [
            { "data": 'FechaFin' },
            { "data": 'EstiloCrianza' }
        ]
    });
    tblMatch = $('#tblMatch').DataTable({
        responsive: true,
        ordering: true,
        bFilter: true,
        bLengthChange: true,
        bInfo: true,
        columns: [
            { "data": 'nombres' },
            { "data": 'CentroAdopcion' },
            { "data": 'antecedentes' },
            { "data": 'Porcentaje' },
            {
                "data": 'Tramite',
                "render": function (data, type, row, meta) {
                    return '<button type="button" id="btnTramiteMatch" class="btn btn-info">Iniciar trámite</button>';
                }
            }
        ],
        dom: 'l<"toolbar">frtip',
        initComplete: function () {
            $("div.toolbar").html('<button type="button" id="btnMatch" class="btn btn-primary">Buscar candidatos</button>');
        }
    });
    $('#tblMatch tbody').on('click', '#btnTramiteMatch', function () {
        var row = $(this).closest('tr')[0];
        var idMenorAdopcion = tblMatch.row(row).node().id;
        var pmtPeticion = new Object();
        pmtPeticion.idMenorAdopcion = idMenorAdopcion;
        $.ajax
            ({
                type: 'POST',
                url: urlIniciarTramite,
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
                        bootbox.alert("Se inició el trámite correctamente.");
                        ListaTramites();
                    }
                }
            });
    });
    tblTramites = $('#tblTramites').DataTable({
        responsive: true,
        ordering: true,
        bFilter: true,
        bLengthChange: false,
        bInfo: false,
        columns: [
            { "data": 'FechaTramite' },
            { "data": 'Menor' },
            { "data": 'CentroAdopcion' },
            { "data": 'Estatus' }
        ]
    });
    ListaTest();
    ListaTramites();
    $("#btnMatch").click(function () {
        Match();
    });
});