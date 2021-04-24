

function format(rowData) {
    //alert(rowData.IdEstacion);
    $("#Accion").val('2');
    $("#txtPorcentaje").val(rowData.Porcentaje);
    $("#LsEstaciones").val(rowData.IdEstacion);
    $("#lsEstatus").prop('selectedIndex', rowData.Estatus);
    $("#lsProducto").prop('selectedIndex', rowData.IdProducto);
    $("#btnAccion").html('Actualizar');
}

function LimpiaCampos() {
    $("#LsEstaciones").prop('selectedIndex', 0);
    $("#lsEstatus").prop('selectedIndex', 0);
    $("#lsProducto").prop('selectedIndex', 0);
    $("#txtPorcentaje").val("");

}

$(document).ready(function () {
    LimpiaCampos();
    //Inicializa Datatable
    var dtable = $('#tbClientes').DataTable({

        "language": {
            "url": "../../Content/plugins/dataTables/js/Spanish.json"
        },
        //"serverSide": false,

        //dom: 'Bfrtip',
        //buttons: [
        //    'copy', 'csv', 'excel', 'pdf', 'print'
        //],

        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": "",
                "sortable": false ,
                "defaultContent": ''
            },

            { "title": "Id Estacion", "data": "NHD", "sortable": false },
            { "title": "Id Estacion", "data": "IdEstacion", "visible": false,"sortable":false },
            { "title": "Cliente", "data": "IdCliente", "sortable": false  },
            { "title": "Producto", "data": "Producto", "sortable": false  },
            { "title": "Porcentaje", "data": "Porcentaje", "sortable": false  },
           
            {
                "title":"Estatus",
                "data": "Estatus",
                "sortable": false ,
                "render": function (d) {
                  
                    if (d==1) 
                        return '<img src="./Recursos/Imagenes/ban_verde.png">'
                            
                    else 
                        return '<img src="./Recursos/Imagenes/ban_roja.png">'
                    
                }
            },
            { "title": "Id Producto", "data": "IdProducto", "visible": false, "sortable": false  }

        ]


    });
    //Llena combo Productos
    $.ajax({

        contentType: 'application/json',
        dataType: 'json',
        type: 'GET',
        url: urlLsProductos,
        success: function (data) {
            $("select[id$=lsProducto] > option").remove();

            var op1 = $('<option value="0">Seleccione</option>');
            $('#lsProducto').append(op1);

            $.each(data.LsCuerpo, function () {
                var option = $('<option value="' + this.IdProducto + '">' + this.DescProducto + '</option>');
                $('#lsProducto').append(option);
            });


        },

    });
    //Busca el cliente y carga el grid
    $("#btnBuscar").click(function (event) {
        LimpiaCampos();
        if ($("#IdCliente").val() == "") {
            bootbox.alert({
                message: '<center><label>Capture Id del Cliente' + '</label></center>',
            });
            return;
        }
       
        var ost = new Object()
        ost.IdCliente = $("#IdCliente").val();

        LlenaGrid();
        
          //LlenaCombo Estaciones
        $.ajax({

           // contentType: 'application/json',
            dataType: 'json',
            type: 'POST',
            url: urlLsEstaciones,
            data: ost,
            success: function (data) {
                $("select[id$=LsEstaciones] > option").remove();

                $.each(data.LsCuerpo, function () {
                    var option = $('<option value="' + this.IdEstacion + '">' + this.DescEstacion + '</option>');
                    $('#LsEstaciones').append(option);
                });


            },

        });



    });
    //Actualiza o Inserta
    $("#btnAccion").click(function () {
        var accion = $("#Accion").val();
       
        
        if ($('#lsProducto').val() == '0') {
            bootbox.alert({
                message: '<center><label>Seleccione Producto' + '</label></center>',
            });
            return;
        }
        if ($('#LsEstaciones').val() == '0') {
            bootbox.alert({
                message: '<center><label>Seleccione Estación' + '</label></center>',
            });
            return;
        }


        if ($('#lsEstatus').val() == 'Seleccione') {
            bootbox.alert({
                message: '<center><label>Seleccione Estatus' + '</label></center>',
            });
            return;
        }

        if ($("#txtPorcentaje").val() == "") {
            bootbox.alert({
                message: '<center><label>Capture Porcentaje' + '</label></center>',
            });
            return;
        }

        if (accion==1) {
           
            var pmtPeticion = new Object()
          
            pmtPeticion.IdCliente = $("#IdCliente").val();
            pmtPeticion.IdEstacion = $("#LsEstaciones").val();
            pmtPeticion.IdProducto = $("#lsProducto").val();
            pmtPeticion.Porcentaje = $("#txtPorcentaje").val();
            pmtPeticion.Estatus = $('#lsEstatus option:selected').attr('id');
           
         
            $.ajax({

                dataType: 'json',
                type: 'POST',
                data: pmtPeticion,
                url: urlAltaProducto,

                success: function (data) {
                    if (data.Exitoso == true) {
                        LlenaGrid();
                        LimpiaCampos();
                    }
                    else {
                        bootbox.alert(data.Mensaje);
                    }


                }

            });
        }

        if (accion == 2) {
           // alert(accion);
            var producto = new Object()
            producto.Estatus = $('#lsEstatus option:selected').attr('id');
            producto.IdCliente = $("#IdCliente").val();
            producto.IdEstacion = $("#LsEstaciones").val();
            producto.IdProducto = $("#lsProducto").val();
            producto.Porcentaje = $("#txtPorcentaje").val();
           // alert(producto.IdEstacion);
            

            $.ajax({

                dataType: 'json',
                type: 'POST',
                data: producto,
                url: urlUpdProducto,

                success: function (data) {
                    if (data.Exitoso == true) {
                        LlenaGrid();
                        LimpiaCampos();
                    }
                    else {
                        bootbox.alert(data.Mensaje);
                    }


                }

            });
        }

      
           

    });
    //Evento del grid para seleccionar 
    $('#tbClientes tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = dtable.row(tr);
        row.child(format(row.data()));
    });
    $('#tbClientes').on('click', 'tr', function () {
        if (!$(this).hasClass('Encabezado')) {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                $('#tbClientes tbody tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        }
    });
    function LlenaGrid() {

        var pmtPeticion = new Object()
        pmtPeticion.IdCliente = $("#IdCliente").val();



        $("#Accion").val('1');

        $.ajax({

            type: 'POST',
            data: pmtPeticion,
            url: urlLsEstacionesCliente,
            beforeSend: function () {
                MensajeCargando()
            },
            success: function (data) {

                if (data.Exitoso == true) {
                    bootbox.hideAll()
                    $("#divClientes").removeClass("hidden");
                    $("#DivAcciones").removeClass("hidden");
                    $("#btnAccion").html('Guardar');
                    dtable.clear().draw();
                    dtable.rows.add(data.LsCuerpo).draw();
                }
                else {
                    
                    bootbox.hideAll()
                    bootbox.alert(data.Mensaje);
                    $("#divClientes").addClass("hidden");
                    $("#DivAcciones").addClass("hidden");
                   
            }


            },

        });
    }
});

var min = 12,
    max = 100,
    select = document.getElementById('selectElementId');

for (var i = min; i <= max; i++) {
    var opt = document.createElement('option');
    opt.value = i;
    opt.innerHTML = i;
    select.appendChild(opt);
}