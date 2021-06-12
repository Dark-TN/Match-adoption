function recuperarPassword() {
    var pmtPeticion = new Object();
    pmtPeticion.CorreoElectronico = $("#txtCorreoElectronico").val();
    $.ajax
        ({
            type: 'POST',
            data: pmtPeticion,
            url: urlRecuperarPassword,
            beforeSend: function () {
                MensajeCargando();
            },
            success: function (data) {
                bootbox.hideAll();
                $("#myModal").modal('hide');
                if (data.Exitoso === false) {
                    bootbox.alert(data.Mensaje);
                }
                else {
                    bootbox.alert("Se envió un correo a la direción de correo electrónico ingresada.");
                }
            }
        });
}

$(document).ready(function () {
    $("#btnRecuperarPassword").click(function () {
        recuperarPassword();
    });
});