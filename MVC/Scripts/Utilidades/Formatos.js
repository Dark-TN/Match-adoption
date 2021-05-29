$(document).ready(function () {
    // /^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$/

    Inputmask({ regex: "[0-9]*" }).mask($(".Numerico"));

    Inputmask({ regex: "[0-9]{10}" }).mask($(".Telefono"));

    Inputmask({ regex: "[a-zA-z0-9#-_ ]*" }).mask($(".Alfanumerico"));

    Inputmask({ regex: "[a-zA-z0-9#-_]{0,20}" }).mask($(".Password"));

    Inputmask({ regex: "[a-zA-ZñÑ ]*" }).mask($(".Alfa"));

    Inputmask({ regex: "[0-9]{2}\/[0-9]{2}\/[0-9]{4}" }).mask($(".Fecha"));

    $(".RFC").inputmask(
        {
            regex: "^[a-zA-Z]{3,4}[0-9]{6}[0-9a-zA-Z]{3}$",
            "onincomplete": function () {
                var ElementoMod = this.id;
                if ($(this).val() != "") {
                    bootbox.alert({
                        message: '<center><label>Formato del RFC no valido' + '</label></center>',
                        callback: function (){
                            setTimeout(function () {
                                $("#" + ElementoMod).focus();
                            }, 100);

                        }
                    });
                }
            }
        });

    $(".CURP").inputmask(
        {
            regex: "^[a-zA-Z]{3,4}[0-9]{6}[hmHM]{1}[a-zA-Z]{1,2}[a-zA-z]{3}[0-9]{2}$",
            "onincomplete": function () {
                var ElementoMod = this.id;
                if ($(this).val() != "") {
                    bootbox.alert({
                        message: '<center><label>Formato del CURP no valido' + '</label></center>',
                        callback: function () {
                            setTimeout(function () {
                                $("#" + ElementoMod).focus();
                            }, 100);

                        }
                    });
                }
            }
        });


    $(".Correo").inputmask({
        mask: "*{1,20}[.*{1,20}][.*{1,20}][.*{1,20}]@*{1,20}[.*{2,6}][.*{1,2}]",
        greedy: false,
        onBeforePaste: function (pastedValue, opts) {
            pastedValue = pastedValue.toLowerCase();
            return pastedValue.replace("mailto:", "");
        },
        definitions: {
            '*': {
                validator: "[0-9A-Za-z!#$%&'*+/=?^_`{|}~\-]",
                cardinality: 1,
                casing: "lower"
            }
        }
    });

    $(".Moneda").inputmask('decimal', {
        'integerDigits': 8,
        'alias': 'decimal',
        'min': '0.00',
        'max': '99999999.99',
        'groupSeparator': ',',
        'autoGroup': true,
        'digits': 2,
        'placeholder': '0.00',
        'autoUnmask': true,
        'clearMaskOnLostFocus': !1,
        'prefix': '$ '
    });
});