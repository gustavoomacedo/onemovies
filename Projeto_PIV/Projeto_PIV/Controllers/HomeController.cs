using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository;
using Projeto_PIV.App_Start;

namespace Projeto_PIV.Controllers
{
    public class HomeController : Controller
    {
        FilmeRepository filmeREPO = new FilmeRepository();
        ClienteRepository clienteREPO = new ClienteRepository();
        CinemaRepository cinemaREPO = new CinemaRepository();
        public ActionResult Index()
        {
            AtualizarSessao();
            var filmes = filmeREPO.getFilmes();
            return View(filmes);
        }

        public ActionResult Shorts()
        {
            return View();
        }

        public ActionResult Lista()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Session["Cliente"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Logar(string email,string senha)
        {
            bool _hasError = false;
            string msg = string.Empty;

            senha = CalculaHash(senha);

            CLIENTE cliente = clienteREPO.getLoginCliente(email, senha);

            if (cliente == null)
            {
                _hasError = true;
                msg = "Email/Senha inválidos.";
            }else
                Session.Add("Cliente", cliente);
            
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

        private void AtualizarSessao()
        {
            CLIENTE _cliente = Session["Cliente"] as CLIENTE;

            if(_cliente != null)
            {
                CLIENTE cliente = clienteREPO.getClienteById(_cliente.ClienteId);
                Session.Add("Cliente", cliente);
            }
        }

        public ActionResult Single(int? id)
        {
            if (id != null)
            {
                var filme = filmeREPO.getFilmeById(id.Value);
                return View(filme);
            }
            else
               return RedirectToAction("Index");
        }

        public ActionResult saveIngresso(int SessaoId, int CinemaId, string valor, string[] assento, string pgt)
        {
            bool _hasError = false;
            string _msg = string.Empty;
            CLIENTE _cliente = Session["Cliente"] as CLIENTE;

            if (_cliente != null)
            {
                try
                {
                    string ingressos = string.Empty;
                    //var assentos = assento.Split(',');

                    foreach (var item in assento)
                    {
                        var ingresso = new INGRESSO();
                        var poltrona = new POLTRONA();
                        poltrona.SessaoId = SessaoId;
                        poltrona.Assento = item;
                        cinemaREPO.salvarPoltrona(poltrona);
                        ingresso.ClienteId = _cliente.ClienteId;
                        ingresso.CinemaId = CinemaId;
                        ingresso.PoltronaId = poltrona.PoltronaId;
                        ingresso.Valor = Convert.ToDecimal(valor);
                        ingresso.Pagamento = pgt == "cc" ? "Cartão de crédito" : "Dinheiro";
                        cinemaREPO.salvarIngresso(ingresso);
                        ingressos += "Poltrona " + item + " ";
                    }
                    var sessao = cinemaREPO.geSessaoById(SessaoId);
                    Email.EnviarEmailNovaCompra(_cliente.Nome, _cliente.Email, ingressos, sessao.Horario, sessao.FILME.Titulo);
                }
                catch (Exception e)
                {
                    _hasError = true;
                    _msg = e.Message;
                }
            }
            else
            {
                _hasError = true;
                _msg = "Para finalizar a compra você precisa estar logado.";
            }

            return Json(new
            {
                hasError = _hasError,
                message = _msg
            }, JsonRequestBehavior.AllowGet);
        }

        #region FIltros
        public ActionResult getCinemas()
        {
            List<CINEMA> lstUT = cinemaREPO.getCinemas();

            var result = (from s in lstUT
                          select new
                          {
                              s.CinemaId,
                              s.Nome
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSalas()
        {
            List<SALA> lstUT = cinemaREPO.getSalas();

            var result = (from s in lstUT
                          select new
                          {
                              s.SalaId,
                              s.Nome
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getHorarios(int CinemaId,int SalaId,int FilmeId)
        {
            List<SESSAO> lstUT = cinemaREPO.getSessoes(CinemaId,SalaId,FilmeId);

            var result = (from s in lstUT
                          select new
                          {
                              s.SessaoId,
                              s.Horario
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLugares(int SessaoId)
        {
            List<POLTRONA> lstUT = cinemaREPO.getPoltronas(SessaoId);

            var result = (from s in lstUT
                          select new
                          {
                              s.PoltronaId,
                              s.Assento
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}