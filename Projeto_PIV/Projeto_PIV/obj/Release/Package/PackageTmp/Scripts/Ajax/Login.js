$(document).ready(function () {

    $('#btnLogar').click(function (e) {
        e.preventDefault();

        var _email = $('#email').val();
        var _pass = $('#senha').val();

        if (_email == '' || _pass == '') {

            swal({
                title: "Ops!",
                text: 'Preencha todos os campos',
                type: "error",
                showCancelButton: false,
                confirmButtonColor: "#F27474",
                confirmButtonText: "OK",
                closeOnConfirm: false
            });

        } else {

            $.ajax({
                type: 'POST',
                url: '/Home/Logar',
                data: { email: _email, senha: _pass },
                beforeSend: function (xhr) {
                    waitingDialog.show('Conectando...');
                },
                success: function (data) {
                    if (data.hasError) {
                        waitingDialog.hide();

                        swal({
                            title: "Ops!",
                            text: data.message,
                            type: "error",
                            showCancelButton: false,
                            confirmButtonColor: "#F27474",
                            confirmButtonText: "OK",
                            closeOnConfirm: false
                        });
                    }
                    else {
                        window.location.href = "/Home";
                    }
                }
            });

        }

    });

});