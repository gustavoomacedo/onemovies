using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClienteRepository
    {
        Entities db = new Entities();

        public CLIENTE getClienteById(int id)
        {
            CLIENTE cliente = (from c in db.CLIENTE
                               where c.ClienteId == id
                               select c).SingleOrDefault();

            return cliente;
        }

        public CLIENTE getLoginCliente(string email,string senha)
        {
            CLIENTE cliente = (from c in db.CLIENTE
                               where c.Email.Equals(email) && c.Senha.Equals(senha)
                               select c).SingleOrDefault();

            return cliente;
        }

        public CLIENTE getClienteByEmail(string email)
        {
            CLIENTE cliente = (from c in db.CLIENTE
                               where c.Email.Equals(email)
                               select c).FirstOrDefault();

            return cliente;
        }


        public void saveCliente(CLIENTE cliente)
        {
            db.CLIENTE.Add(cliente);
            db.SaveChanges();
        }

        public void editCliente(CLIENTE cliente)
        {
            db.CLIENTE.Attach(cliente);
            db.Entry(cliente).State = EntityState.Modified;
            db.SaveChanges();
        }



    }
}
