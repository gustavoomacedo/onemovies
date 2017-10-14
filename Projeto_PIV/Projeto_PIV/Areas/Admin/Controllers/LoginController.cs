using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto_PIV.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        ClienteRepository clienteREPO = new ClienteRepository();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logar(string email, string senha)
        {
            bool _hasError = false;
            string msg = string.Empty;

            senha = CalculaHash(senha);

            CLIENTE cliente = clienteREPO.getLoginCliente(email, senha);

            if (cliente == null)
            {
                //_hasError = true;
                //msg = "Email/Senha inválidos.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //if (cliente.Nome == "Admin")
                //{
                //    Session.Add("Admin", cliente);
                    return RedirectToAction("Index", "Home");
                //}
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