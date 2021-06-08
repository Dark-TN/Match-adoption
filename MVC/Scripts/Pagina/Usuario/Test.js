var tblTest;

function formatNumber(number) {
    if (number < 10) {
        return "0" + number;
    }
    else {
        return "" + number;
    }
}

function startTick(tiempoRestante) {
    var minsCounter = Math.floor(tiempoRestante % 3600 / 60)
    var segsCounter = Math.floor(tiempoRestante % 3600 % 60);
    $("#segRest").text(formatNumber(parseInt(segsCounter)));
    $("#minRest").text(formatNumber(parseInt(minsCounter)));
    var tick = setInterval(function () {
        if (tiempoRestante > 0) {
            if (segsCounter === 0) {
                segsCounter = 60;
            }
            tiempoRestante = tiempoRestante - 1;
            segsCounter = segsCounter - 1;
            minsCounter = Math.floor(tiempoRestante % 3600 / 60)
            $("#segRest").text(formatNumber(parseInt(segsCounter)));
            $("#minRest").text(formatNumber(parseInt(minsCounter)));
        }
        else {
            bootbox.alert("El tiempo para responder el test se ha terminado.")
            clearInterval(tick);
            var pmtPeticion = [];
            var test = tblTest.$('select');
            pmtPeticion = JSON.stringify({ 'pmtPeticion': pmtPeticion });
            $.ajax
                ({
                    type: 'POST',
                    url: urlContestarTest,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: pmtPeticion,
                    success: function (data) {
                        window.location.href = data;
                    }
                });
        }
    }, 1000);
}

function setTest() {
    $.ajax
        ({
            type: 'POST',
            url: urlSetTest,
            success: function (data) {
                for (i = 0; i < data.length; i++) {
                    tblTest.row.add({
                        "IdPregunta": data[i].IdPregunta,
                        "Pregunta": data[i].Pregunta
                    }).draw();
                }
                setTimeTest();
            }
        });
}

function validarTest() {
    var pmtPeticion = [];
    var test = tblTest.$('select');
    for (i = 0; i < test.length; i++) {
        if (test[i].value == 0) {
            bootbox.alert
                ({
                    message: '<center><label>La pregunta ' + test[i].id + ' no está respondida.</label></center>'
                });
            return;
        }
        var pregunta = new Object();
        pregunta.IdPregunta = test[i].id;
        pregunta.Respuesta = test[i].value;
        pmtPeticion.push(pregunta);
    }
    pmtPeticion = JSON.stringify({ 'pmtPeticion': pmtPeticion });
    $.ajax
        ({
            type: 'POST',
            url: urlContestarTest,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: pmtPeticion,
            success: function (data) {
                window.location.href = data;
            }
        });
}

function setTimeTest() {
    $.ajax
        ({
            type: 'POST',
            url: urlSetTimeTest,
            success: function (data) {
                startTick(data);
            }
        });
}

$(document).ready(function () {
    tblTest = $('#tblTest').DataTable({
        responsive: true,
        ordering: false,
        bFilter: false,
        bLengthChange: false,
        bInfo: false,
        rowId: 'IdPregunta',
        columns: [
            { "data": 'IdPregunta', "width": "3%" },
            { "data": 'Pregunta', "width": "92%" },
            {
                "data": 'Respuesta',
                "render": function (data, type, row, meta) {
                    return '<select class="form-control" id = "' + (meta.row + 1) + '" name="sctRespuesta">' +
                        '<option value = "0"> Selecciona una opción</option>' +
                        '<option value = "1">En desacuerdo</option>' +
                        '<option value = "2">Algo en desacuerdo</option>' +
                        '<option value = "3">Algo de acuerdo</option>' +
                        '<option value = "4">De acuerdo</option></select>';
                }
                , "width": "5%"
            }
        ]
    });
    setTest(tblTest);
    $('#finalizarTest').click(function () {
        validarTest(tblTest);
    });
});