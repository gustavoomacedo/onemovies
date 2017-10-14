$(document).ready(function () {

    $('#formEsenha').on('submit', function (e) {
        if (!e.isDefaultPrevented()) {
            e.preventDefault();

            waitingDialog.show('Enviando Email...');

            $.ajax({
                url: 'Cadastro/EnviarEmailEsqueciSenha',
                type: "POST",
                data: new FormData(this),
                contentType: false,
                cache: false,
                processData: false,
                success: function (data) {
                    if (data.hasError) {
                        setTimeout(function () {
                            waitingDialog.hide();

                            swal({
                                title: "Ocorreu um Erro!",
                                text: data.message,
                                type: "error",
                                showCancelButton: false,
                                confirmButtonColor: "#FF8D1B",
                                confirmButtonText: "OK",
                                closeOnConfirm: true
                            });
                        }, 2000);
                    }
                    else {
                        setTimeout(function () {
                            waitingDialog.hide();

                            swal({
                                title: "Enviado email com Sucesso!",
                                text: data.message,
                                type: "success",
                                showCancelButton: false,
                                confirmButtonColor: '#FF8D1B',
                                confirmButtonText: "OK",
                                closeOnConfirm: true
                            },
                           function (isConfirm) {
                               location.reload();
                           });
                        }, 2000);
                    }
                }
            });
        }
    });

});