$(document).ready(function () {
    $.ajax({
        url: '/Home/getCinemas',
        type: "POST",
        dataType: 'json',
        success: function (data) {
            $.each(data, function (i, item) {
                $('#cmbCinema').append($("<option></option>")
                .attr("value", item.CinemaId)
                .text(item.Nome));
            });
        }
    });

    $.ajax({
        url: '/Home/getSalas',
        type: "POST",
        dataType: 'json',
        success: function (data) {
            $.each(data, function (i, item) {
                $('#cmbSala').append($("<option></option>")
                .attr("value", item.SalaId)
                .text(item.Nome));
            });
        }
    });

    $('#cmbSala').change(function () {
        var idSala = $(this).val();

        if (idSala == "") {
            $('#cmbSala').empty();
            $('#cmbSala').append($("<option></option>").attr("value", "").text("-- Selecione --"));
        } else {

            $.ajax({
                url: '/Home/getHorarios',
                type: "POST",
                data: { CinemaId: $('#cmbCinema').val(), SalaId: idSala, FilmeId: $('#FilmeId').val() },
                dataType: 'json',
                success: function (data) {
                    $.each(data, function (i, item) {
                        $('#cmbHorario').append($("<option></option>").attr("value", item.SessaoId).text(item.Horario));
                    });

                }
            });
        }
    });

    $('#cmbHorario').change(function () {
        var idHorario = $(this).val();

        if (idHorario == "") {
            $('#cmbHorario').empty();
            $('#cmbHorario').append($("<option></option>").attr("value", "").text("-- Selecione --"));
        } else {

            $.ajax({
                url: '/Home/getLugares',
                type: "POST",
                data: { SessaoId: $('#cmbHorario').val() },
                dataType: 'json',
                success: function (data) {
                    $.each(data, function (i, item) {
                        $('#' + item.Assento).attr('disabled', 'false');
                    });

                }
            });

        }

    });


});

$("#clickme").click(function (e) {
    var selected = $(".fuselage input:checked").map(function (i, el) { return el.id; }).get();
    var _pgt = $('#pagamento:checked').val();
    //alert("selected = [ " + selected + " ]\n as string = \"" + selected.join(";") + "\"");

    swal({
        title: "Deseja confirmar sua compra?",
        text: "Após a confirmação, as informações serão enviadas via Email.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Sim",
        closeOnConfirm: false
    },
    function () {
        //SessaoId,int CinemaId,string valor,string assento
        $.ajax({
            url: '/Home/saveIngresso',
            type: "POST",
            data: { SessaoId: $('#cmbHorario').val(), CinemaId: $('#cmbCinema').val(), valor: $('#cmbPreco').val(), assento: selected, pgt: _pgt },
            dataType: 'json',
            success: function (data) {
                if (data.hasError) {
                    swal({
                        title: "Ocorreu um Erro!",
                        text: data.message,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonColor: "#FF8D1B",
                        confirmButtonText: "OK",
                        closeOnConfirm: true
                    });

                } else {
                    swal({
                        title: "Compra realizada com sucesso!",
                        text: "Verifique seu e-mail com as informações.",
                        type: "success",
                        showCancelButton: false,
                        confirmButtonColor: '#FF8D1B',
                        confirmButtonText: "OK",
                        closeOnConfirm: true
                    },
                    function (isConfirm) {
                        window.location.href = "/Home";
                    });
                }

            }
        });

    });



});