function formatNumber(number) {
    if (number < 10) {
        return "0" + number;
    }
    else {
        return "" + number;
    }
}

function startTick() {
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
        }
    }, 1000);
}

$(document).ready(function () {
    startTick();
});