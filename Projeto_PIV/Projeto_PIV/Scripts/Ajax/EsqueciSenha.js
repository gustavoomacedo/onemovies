
$(document).ready(function () {
    //Salvar
    $('#formEsqueci').on('submit', function (e) {
        if (!e.isDefaultPrevented()) {
            e.preventDefault();

            if ($('#Senha').val() != '' && $('#Senha').val() != $('#csenha').val()) {
                swal({
                    title: "Atenção!",
                    text: 'Senhas digitadas não conferem!',
                    type: "warning",
                    showCancelButton: false,
                    confirmButtonColor: '#FF8D1B',
                    confirmButtonText: "OK",
                    closeOnConfirm: true
                });
            } else {


                waitingDialog.show('Atualizando nova senha...');

                $.ajax({
                    url: 'AtualizarSenha',
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
                                    title: "Senha atualizada com Sucesso!",
                                    text: data.message,
                                    type: "success",
                                    showCancelButton: false,
                                    confirmButtonColor: '#FF8D1B',
                                    confirmButtonText: "OK",
                                    closeOnConfirm: true
                                },
                               function (isConfirm) {
                                   window.location.href = "/Home";
                               });
                            }, 2000);
                        }
                    }
                });
            }
        }
    });
});