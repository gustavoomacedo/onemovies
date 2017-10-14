using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FilmeRepository
    {
        Entities db = new Entities();

        public void saveFilme(FILME filme)
        {
            db.FILME.Add(filme);
            db.SaveChanges();
        }

        public void saveFilmeImagens(FILME_IMAGEM filme)
        {
            db.FILME_IMAGEM.Add(filme);
            db.SaveChanges();
        }

        public List<FILME> getFilmes()
        {
            return (from f in db.FILME select f).ToList();
        }

        public FILME getFilmeById(int id)
        {
            return (from f in db.FILME where f.FilmeId == id select f).SingleOrDefault();
        }
    }
}
