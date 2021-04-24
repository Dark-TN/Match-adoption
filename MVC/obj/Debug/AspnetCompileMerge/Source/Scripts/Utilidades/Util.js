function MensajeCargando() {
    bootbox.dialog({
        message: '<div class="loader"></div><br/><label class="ui-widget ui-state-default ui-corner-all">Cargando....</label>',
        closeButton: false
    }).css({
        'margin-top': function () {
            var w = $(window).height();
            var b = $(".modal-dialog").height();
            // should not be (w-h)/2
            var h = (screen.height-300) / 2;
            return h + "px";
        },
        'height':'300px',
        'margin-left': function () {
            var w = $(window).height();
            var b = $(".modal-dialog").height();
            // should not be (w-h)/2
            var h = (screen.width-150) / 2;
            return h + "px";
        },
        'width': "150px"
    });
}

function Hoy() {
    var d = new Date();
    return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
}

function FormatoFechaJSON(fecha) {
    var d = new Date(parseInt(fecha.substr(6)));
    if (d.getFullYear() > 1753) {
        return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
    }
    else {
        return "N/A";
    }
}