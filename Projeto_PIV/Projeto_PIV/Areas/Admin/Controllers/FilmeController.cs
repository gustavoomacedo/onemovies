using LINQtoCSV;
using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Projeto_PIV.Areas.Admin.Controllers
{
    public class FilmeController : Controller
    {
        CsvContext cc = new CsvContext();
        FilmeRepository filmeREPO = new FilmeRepository();

        // GET: Admin/Filme
        public ActionResult Index()
        {
            //if (Session["Admin"] == null)
            //    return RedirectToAction("Index", "Login");
            var filmes = filmeREPO.getFilmes();
            return View(filmes);
        }

        public ActionResult Upload()
        {
            //if (Session["Admin"] == null)
            //    return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult Imagens(int id)
        {
            var filme = filmeREPO.getFilmeById(id);
            var imagem = filme.FILME_IMAGEM.FirstOrDefault();

            if (imagem == null)
            {
                imagem = new FILME_IMAGEM();
                imagem.FilmeId = filme.FilmeId;
            }

            return View(imagem);
        }

        [HttpPost]
        public ActionResult Imagens(FILME_IMAGEM filme, HttpPostedFileBase ImagemBanner, HttpPostedFileBase ImagemLogo)
        {
            if (ImagemBanner != null)
            {
                if (ImagemBanner.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImagemBanner.FileName);
                    fileName = DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + "_" + fileName;
                    var path = Path.Combine(Server.MapPath("~/Content/images/uploads/"), fileName);
                    ImagemBanner.SaveAs(path);
                    filme.ImagemBanner = fileName;
                }
            }

            if (ImagemLogo != null)
            {
                if (ImagemLogo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImagemLogo.FileName);
                    fileName = DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + "_" + fileName;
                    var path = Path.Combine(Server.MapPath("~/Content/images/uploads/"), fileName);
                    ImagemLogo.SaveAs(path);
                    filme.ImagemLogo = fileName;
                }
            }

            filmeREPO.saveFilmeImagens(filme);
            return RedirectToAction("Index", "Filme");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    this.CarregarFilmes(file);
                    ViewData["success"] = "Planilha enviada com sucesso!";
                }
            }
            catch (Exception e)
            {
                ViewData["success"] = e.Message;
            }

            return View();
        }

        private void CarregarFilmes(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension == ".csv")
                {
                    string fileLocation = Server.MapPath("~/Areas/Admin/Content/Upload/Planilhas/") + DateTime.Now.Millisecond + "_" + file.FileName;

                    if (System.IO.File.Exists(fileLocation))
                        System.IO.File.Delete(fileLocation);

                    file.SaveAs(fileLocation);

                    if (ValidaNumeroColunas(fileLocation, 11))
                    {
                        CsvFileDescription inputFileDescription = new CsvFileDescription
                        {
                            SeparatorChar = ';',
                            FirstLineHasColumnNames = true,
                            IgnoreUnknownColumns = true,
                            TextEncoding = Encoding.Default
                        };

                        IEnumerable<CadastroFilme> preCadastroExcel = cc.Read<CadastroFilme>(fileLocation, inputFileDescription);
                        
                        foreach (var item in preCadastroExcel)
                        {
                            FILME filme = new FILME();
                            filme.Titulo = item.Titulo;
                            filme.Duracao = Convert.ToInt32(item.Duracao);
                            filme.Classificacao = Convert.ToInt32(item.Classificacao);
                            filme.Diretor = item.Diretor;
                            filme.Distribuidora = item.Distribuidora;
                            filme.Cartaz = item.Cartaz == "0" ? false : true;
                            filme.Estreia = item.Estreia == "0" ? false : true;
                            filme.Genero = item.Genero;
                            filme.Trailer = item.Trailer;
                            filme.DataCadastro = DateTime.ParseExact(item.DataEstreia, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            filme.Descricao = item.Descricao;

                            filmeREPO.saveFilme(filme);
                        }
                    }
                    else
                    {
                        ViewData["error"] = "Formato de arquivo inválido!";
                        System.IO.File.Delete(fileLocation);
                    }
                }
                else
                {
                    ViewData["error"] = "Formato de arquivo inválido! Somente arquivos csv";
                }
            }
        }

        protected bool ValidaNumeroColunas(string fileLocation, int numeroColunas)
        {
            var lines = System.IO.File.ReadAllLines(fileLocation);
            var rows = lines.Count();
            var columns = lines[0].Split(';').Count();

            return columns == numeroColunas;
        }

        public class CadastroFilme
        {
            public string Titulo { get; set; }
            public string Duracao { get; set; }
            public string Classificacao { get; set; }
            public string Diretor { get; set; }
            public string Distribuidora { get; set; }
            public string Cartaz { get; set; }
            public string Estreia { get; set; }
            public string Genero { get; set; }
            public string Trailer { get; set; }
            public string DataEstreia { get; set; }
            public string Descricao { get; set; }
        }

    }
}

