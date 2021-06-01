var tblTests;
var labels = [];
var charts = {};
var colors = ['red', 'blue', 'green'];


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
                    for (i = 0; i < data.Respuesta.length; i++) {
                        tblTests.row.add({
                            "FechaFin": ddMMyyyy(data.Respuesta[i].FechaFin),
                            "EstiloCrianza": data.Respuesta[i].EstiloCrianza
                        }).draw();
                        var cont = 0;
                        for (var key in data.Respuesta[i].CalificacionesHabilidades) {
                            labels.push(key);
                            cont++;
                        }
                    }
                }
            }
        });
}

$(document).ready(function () {
    tblTests = $('#tblTests').DataTable({
        responsive: true,
        ordering: false,
        bFilter: false,
        bLengthChange: false,
        bInfo: false,
        columns: [
            { "data": 'FechaFin'},
            { "data": 'EstiloCrianza'}
        ]
    });
    const data = {
        labels: [
            'Eating',
            'Drinking',
            'Sleeping',
            'Designing',
            'Coding',
            'Cycling',
            'Running'
        ],
        datasets: [{
            label: 'My First Dataset',
            data: [65, 59, 90, 81, 56, 55, 40],
            fill: true,
            backgroundColor: 'rgba(255, 99, 132, 0.2)',
            borderColor: 'rgb(255, 99, 132)',
            pointBackgroundColor: 'rgb(255, 99, 132)',
            pointBorderColor: '#fff',
            pointHoverBackgroundColor: '#fff',
            pointHoverBorderColor: 'rgb(255, 99, 132)'
        }, {
            label: 'My Second Dataset',
            data: [28, 48, 40, 19, 96, 27, 100],
            fill: true,
            backgroundColor: 'rgba(54, 162, 235, 0.2)',
            borderColor: 'rgb(54, 162, 235)',
            pointBackgroundColor: 'rgb(54, 162, 235)',
            pointBorderColor: '#fff',
            pointHoverBackgroundColor: '#fff',
            pointHoverBorderColor: 'rgb(54, 162, 235)'
        }]
    };

    var ctx = $('#myChart');
    var myRadarChart = new Chart(ctx, {
        type: 'radar',
        data: data,
        options: {
            elements: {
                line: {
                    borderWidth: 3
                }
            }
        },
    });
    ListaTest();
});