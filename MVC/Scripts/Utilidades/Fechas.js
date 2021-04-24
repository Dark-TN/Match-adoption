$(document).ready(function () {
    $.fn.datepicker.dates['es'] = {
        days: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"], daysShort: ["Dom", "Lun", "Mar", "Mi�", "Jue", "Vie", "S�b", "Dom"], daysMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa", "Do"], months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"], monthsShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"], today: "Hoy", clear: "Borrar", weekStart: 1, format: "dd/mm/yyyy"
    };
    $('.Fecha').datepicker({
        autoclose: true,
        endDate: '0d',
        format: "dd/mm/yyyy",
        language: "es",
        clearBtn: true
    })
})

function ValidarFechas(Fechainicio, FechaFin) {
    var respuesta = "";
    if (Fechainicio.length >1) {
        if (FechaFin.length == 1) {
             respuesta = "Debe ingresar un período de búsqueda.";
        }
        else {
            var dFechaInicio = new Date(Fechainicio[2], Fechainicio[1] - 1, Fechainicio[0]);
            var dFechaFin = new Date(FechaFin[2], FechaFin[1] - 1, FechaFin[0]);
            if (dFechaInicio.getTime() > dFechaFin.getTime()) {
                respuesta = "La fecha inicial no puede ser mayor a la fecha final.";
            }
        }
    }
    else {
        if (FechaFin.length > 1) {
            respuesta = "Debe ingresar un período de búsqueda.";
        }
    } 
    return respuesta
}