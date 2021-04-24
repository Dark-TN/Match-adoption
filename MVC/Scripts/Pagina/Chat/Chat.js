function listaMatch() {
    var pmtPeticion = new Object();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlSelectListaMatch,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                for (i = 0; i < data.Respuesta.length; i++) {
                    $("#listaMatch").append('<li class="list-group-item"><a href="#">' + data.Respuesta[i].Nombre + '</a></li>');
                }
            }
        });
}

$(document).ready(function () {
    listaMatch();
});