$(document).ready(function () {
    $('#CPF').mask("999.999.999-99");
    $('#DataNasc_').mask("99/99/9999");
    $('#CEP').mask("99999-999");
    $('#Telefone').mask("(99) 9999-9999");

    $('#Celular').focusout(function () {
        var phone, element;
        element = $(this);
        element.unmask();
        phone = element.val().replace(/\D/g, '');
        if (phone.length > 10) {
            element.mask("(99) 99999-999?9");
        } else {
            element.mask("(99) 9999-9999?9");
        }
    }).trigger('focusout');

    $("#CEP").change(function () {
        waitingDialog.show('Aguarde...');
        var cep = $(this).val().replace(/\D/g, '');

        if (cep != null) {
            var validacep = /^[0-9]{8}$/;

            if (validacep.test(cep)) {
                $.getJSON("//viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {
                    if (!("erro" in dados)) {
                        $("#Logradouro").val(dados.logradouro);
                        $("#Bairro").val(dados.bairro);
                        $("#Cidade").val(dados.localidade);
                        $("#UF").val(dados.uf);
                        $("#Numero").val('');
                        $("#Complemento").val('');

                        waitingDialog.hide();
                    }
                    else {
                        waitingDialog.hide();

                        $("#CEP").val('');
                        $("#Logradouro").val('');
                        $("#Bairro").val('');
                        $("#Cidade").val('');
                        $("#UF").val('');
                        $("#Numero").val('');
                        $("#Complemento").val('');

                        swal({
                            title: "Ops!",
                            text: "CEP não encontrado.",
                            type: "error",
                            showCancelButton: false,
                            confirmButtonColor: "#F27474",
                            confirmButtonText: "OK",
                            closeOnConfirm: false
                        });
                    }
                });
            }
            else {
                waitingDialog.hide();

                swal({
                    title: "Ops!",
                    text: "Formato de CEP inválido.",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonColor: "#F27474",
                    confirmButtonText: "OK",
                    closeOnConfirm: false
                });

                $("#CEP").val('');
                $("#Logradouro").val('');
                $("#Bairro").val('');
                $("#Cidade").val('');
                $("#UF").val('');
                $("#Numero").val('');
                $("#Complemento").val('');

            }
        }
    });
    //Salvar
    $('#formCadastro').on('submit', function (e) {
        if (!e.isDefaultPrevented()) {
            e.preventDefault();
            if ($('#ssenha').val() != '' && $('#ssenha').val() != $('#csenha').val()) {
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

                waitingDialog.show('Salvando Cliente...');

                $.ajax({
                    url: 'Cadastro/saveCliente',
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
                                    title: "Perfil atualizado com Sucesso!",
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