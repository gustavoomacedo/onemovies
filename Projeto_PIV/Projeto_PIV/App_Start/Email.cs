using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Projeto_PIV.App_Start
{
    public class Email
    {
        public static void EnviarEmailEsqueciSenha(string nome, string email, string clienteID)
        {
            string remetenteEmail = "grupobsi2017@gmail.com"; //O e-mail do remetente
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(remetenteEmail, "ONE MOVIES", System.Text.Encoding.UTF8);
            mail.Subject = "Resgate de senha";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High; //Prioridade do E-Mail

            string body = string.Empty;

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Content/HTML/EsqueciSenha.html")))
            {
                body = reader.ReadToEnd();
            }

            string link = "http://onemovies.azurewebsites.net/Cadastro/EsqueciSenha?cliente=" + clienteID;

            body = body.Replace("{Nome}", nome);
            body = body.Replace("{Link}", link);

            mail.Body = body;

            SmtpClient client = new SmtpClient();  //Adicionando as credenciais do seu e-mail e senha:
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(remetenteEmail, "Grupo@pi");

            client.Port = 587; // Esta porta é a utilizada pelo Gmail para envio
            client.Host = "smtp.gmail.com"; //Definindo o provedor que irá disparar o e-mail
            client.EnableSsl = true; //Gmail trabalha com Server Secured Layer

            client.Send(mail);

        }

        public static void EnviarEmailNovaCompra(string nome, string email, string poltronas,string horario, string filme)
        {
            string remetenteEmail = "grupobsi2017@gmail.com"; //O e-mail do remetente
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(remetenteEmail, "ONE MOVIES", System.Text.Encoding.UTF8);
            mail.Subject = "Compra de ingressos";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High; //Prioridade do E-Mail

            string body = string.Empty;

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Content/HTML/NovaCompra.html")))
            {
                body = reader.ReadToEnd();
            }
            
            body = body.Replace("{Nome}", nome);
            body = body.Replace("{Poltronas}", poltronas);
            body = body.Replace("{Horario}", horario);
            body = body.Replace("{Filme}", filme);

            mail.Body = body;

            SmtpClient client = new SmtpClient();  //Adicionando as credenciais do seu e-mail e senha:
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(remetenteEmail, "Grupo@pi");

            client.Port = 587; // Esta porta é a utilizada pelo Gmail para envio
            client.Host = "smtp.gmail.com"; //Definindo o provedor que irá disparar o e-mail
            client.EnableSsl = true; //Gmail trabalha com Server Secured Layer

            client.Send(mail);

        }
    }
}