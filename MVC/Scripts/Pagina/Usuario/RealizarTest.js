function formatNumber(number) {
    if (number < 10) {
        return "0" + number;
    }
    else {
        return "" + number;
    }
}

function startTick() {
    var secondsCounter = tiempoRestante % 60;
    $("#segRest").text(formatNumber(secondsCounter));
    $("#minRest").text(formatNumber(parseInt(tiempoRestante / 60)));
    var tick = setInterval(function () {
        if (tiempoRestante > 0) {
            if (secondsCounter === 0) {
                secondsCounter = 60;
            }
            tiempoRestante = tiempoRestante - 1;
            secondsCounter = secondsCounter - 1;
            $("#segRest").text(formatNumber(secondsCounter));
            $("#minRest").text(formatNumber(parseInt(tiempoRestante / 60)));
        }
        else {
            bootbox.alert("El tiempo para responder el test se ha terminado.")
            clearInterval(tick);
        }
    }, 1000);
}

$(document).ready(function () {
    var tblValidacion = $('#grid').DataTable({
        responsive: true,
        searching: false,
        bSort: false,
        bLengthChange: false,
        bInfo: false
    });
    startTick();
});