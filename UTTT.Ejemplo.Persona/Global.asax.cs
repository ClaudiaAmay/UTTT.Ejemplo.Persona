using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace UTTT.Ejemplo.Persona
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError().Message;
            SendEmail(ex);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

       

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public static void SendEmail(string Body)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

            mail.From = new MailAddress("emaildesdedondeenviar@gmail.com");

            mail.To.Add("19300629@uttt.edu.mx");
            mail.Subject = "Error en el programa";
            mail.Body = Body;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587; //465;  //25; 
            smtp.Credentials = new NetworkCredential("claudiazuniga15hurtado@gmail.com", "claudia15A.");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido enviar el email", ex.InnerException);
            }
            finally
            {
                smtp.Dispose();
            }

        }
    }
}