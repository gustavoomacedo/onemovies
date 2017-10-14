using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CinemaRepository
    {
        Entities db = new Entities();

        public List<CINEMA> getCinemas()
        {
            return (from c in db.CINEMA select c).ToList();
        }

        public List<SALA> getSalas()
        {
            return (from s in db.SALA select s).ToList();
        }

        public SESSAO geSessaoById(int id)
        {
            return (from s in db.SESSAO where s.SessaoId == id select s).SingleOrDefault();
        }

        public List<SESSAO> getSessoes(int CinemaId,int SalaId,int FilmeId)
        {
            return (from s in db.SESSAO where s.CinemaId == CinemaId && 
                    s.SalaId == SalaId && 
                    s.FilmeId == FilmeId select s).ToList();
        }

        public List<POLTRONA> getPoltronas(int SessaoId)
        {
            return (from p in db.POLTRONA where p.SessaoId == SessaoId select p).ToList();
        }

        public void salvarIngresso(INGRESSO ingresso)
        {
            db.INGRESSO.Add(ingresso);
            db.SaveChanges();
        }

        public void salvarPoltrona(POLTRONA poltrona)
        {
            db.POLTRONA.Add(poltrona);
            db.SaveChanges();
        }
    }
}
