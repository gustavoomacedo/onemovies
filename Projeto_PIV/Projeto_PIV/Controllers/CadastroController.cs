using Model;
using Projeto_PIV.App_Start;
using Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto_PIV.Controllers
{
    public class CadastroController : Controller
    {
        ClienteRepository clienteREPO = new ClienteRepository();
        public ActionResult Index()
        {
            CLIENTE _cliente = Session["Cliente"] as CLIENTE;

            if (_cliente != null)
                return View(_cliente);
            else
                return View();
        }

        public ActionResult saveCliente(CLIENTE cliente, string ssenha,string DataNasc_)
        {
            bool _hasError = false;
            string msg = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(DataNasc_))
                    cliente.DataNasc = DateTime.ParseExact(DataNasc_, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (cliente.ClienteId != 0)
                {
                    CLIENTE _cliente = Session["Cliente"] as CLIENTE;

                    if (!string.IsNullOrEmpty(ssenha))
                        cliente.Senha = CalculaHash(ssenha);
                    else
                        cliente.Senha = _cliente.Senha;

                    cliente.CPF = cliente.CPF.Replace(".", "").Replace("-", "");
                    cliente.TrocaSenha = false;
                    clienteREPO.editCliente(cliente);
                }
                else
                {
                    cliente.Senha = CalculaHash(ssenha);
                    cliente.DataCadastro = DateTime.Now;
                    cliente.CPF = cliente.CPF.Replace(".", "").Replace("-", "");
                    cliente.TrocaSenha = false;
                    clienteREPO.saveCliente(cliente);
                    Session.Add("Cliente", cliente);
                }
            }
            catch (Exception e)
            {
                _hasError = true;
                msg = e.Message + DataNasc_;
            }

            return Json(new { hasError = _hasError, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EsqueciSenha(int cliente)
        {
            CLIENTE _cliente = clienteREPO.getClienteById(cliente);

            if (_cliente == null || _cliente.TrocaSenha == false)
            {
                return RedirectToAction("/Home");
            }
            else
            {
                return View(_cliente);
            }
        }

        public ActionResult AtualizarSenha(string senha, int ClienteId)
        {
            bool _hasError = false;
            string msg = string.Empty;
            string nSenha = CalculaHash(senha);

            try
            {
                CLIENTE _cliente = clienteREPO.getClienteById(ClienteId);
                if (_cliente != null)
                {
                    _cliente.Senha = nSenha;
                    _cliente.TrocaSenha = false;

                    clienteREPO.editCliente(_cliente);
                }
                else
                {
                    _hasError = true;
                    msg = "Cliente não encontrado!";
                }
            }
            catch (Exception e)
            {
                _hasError = true;
                msg = e.Message;
            }

            return Json(new { hasError = _hasError, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarEmailEsqueciSenha(string email)
        {
            bool _hasError = false;
            string msg = string.Empty;
            Exception cpfException = new Exception("Cliente não encontrado");

            try
            {
                CLIENTE cliente = clienteREPO.getClienteByEmail(email);
                if (cliente != null)
                {
                    cliente.TrocaSenha = true;
                    clienteREPO.editCliente(cliente);
                    Email.EnviarEmailEsqueciSenha(cliente.Nome, cliente.Email, cliente.ClienteId.ToString());
                }
                else
                    throw cpfException;
            }
            catch (Exception e)
            {
                msg = e.Message;
                _hasError = true;
            }

            return Json(new { hasError = _hasError, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public static string CalculaHash(string Senha)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Senha);
                byte[] hash = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString(); // Retorna senha criptografada 
            }
            catch (Exception)
            {
                return null; // Caso encontre erro retorna nulo
            }
        }

    }
}