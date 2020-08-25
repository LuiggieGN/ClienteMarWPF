using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;


namespace MAR.BusinessLogic.Code
{
    public class EmailLogic
    {
        public static void SendEmail(string pCliente, string pMensaje, string[] pEmail, List<string[]> pData,string pSubjet, string pTitulo, string pConsorcio,string pEmailFromConfig)
        {
           
            //Trying to send mail
            try
            {
                string[] emailconfig = pEmailFromConfig.Split(',');
                string emailHost = emailconfig[0];
                int emailPort = int.Parse(emailconfig[1]);
                string emailFrom = emailconfig[2];
                string emailPass = emailconfig[3];

                var message = new MailMessage();
                for (int i = 0; i < pEmail.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(pEmail[i]) && !pEmail[i].ToUpper().Contains("ENTREGADO POR"))
                    {
                        message.To.Add(new MailAddress(pEmail[i]));
                    }
                }

                message.From = new MailAddress(emailFrom);    // <- postmaster@newsoftevolution.com
                message.Subject = pSubjet;


                string text = "";
                if (pData != null)
                {
                    for (int i = 0; i < pData.Count; i++)
                    {
                        if (!pData[i][0].ToString().ToUpper().Contains("ENTREGADO") || !pData[i][0].ToString().ToUpper().Contains("RECIBIDO"))
                        {
                            text = text + "<tr><td style = 'font-family: Courier New !important; font-size: 18px !important;'>" + pData[i][0].ToString() + "</td ></tr > ";
                        }
                    }
                }
               

                string htmlBody = "<div style='width: 100%; font-size:18px !important; padding: 30px 10px 10px 10px;'>"
                                 + "Saludos Estimado " + pCliente + "! <br/><br/> " + pTitulo
                                 + " <div style=\"background-color: red; margin-top: 10px; z-index: 1000; min-height:105px; border:1px solid transparent; display:block; width: 477px; border-top-left-radius: 6px; border-top-right-radius: 6px;\">"
                                 + "    <div style=\"display:block; box-sizing: border-box; font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; \">"
                                 + "         <div style=\"float:left;\">"
                                 + "           <header style=\" display:block; float:left; width:100%; padding-left:10px;\">"
                                 + "                <h1 style=\"font-family: Arial; font-size: 36px; margin-top: 20px; margin-bottom: 10px; font-weight:500; line-height: 1.1;\">"
                                 + "				  <span style=\"color: white;\" class=\"fa fa-edit \">" + pTitulo + "</span>"
                                 + "			   </h1>"
                                 + "           </header>"
                                 + "         </div>"
                                 + "    </div>"
                                 + " </div>"
                                 + "<table style=\"border-collapse: collapse; display:block; width: 477px; border: solid 1px #CCCCCC; color: #020202;\">"

                                    + "<td style=\"padding-bottom:10px; padding-top:10px; border-bottom: 1px solid #eae3e3;\">"
                                            + "<label style=\"float:right;\">Mensaje: " + pMensaje+ "</label>"
                                       + "</td>"
                                       + "<td class=\"text-center\" style=\"padding-bottom:10px; padding-top:10px; border-bottom: 1px solid #eae3e3; text-align: center;\">"
                                            + "<label style=\"padding-left:28px; padding-right: 28px;\">" + "" + "</label>"
                                       + "</td>"
                                       +
                                       text
                                  

                                     + "</table>"
                            + "</div>";

                message.Body = htmlBody;
                message.IsBodyHtml = true;


                var smtp = new SmtpClient(emailHost, emailPort);

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //
                smtp.Credentials = new NetworkCredential
                {
                    UserName = emailFrom,
                    Password = emailPass
                };
                //smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = emailHost;
                smtp.Send(message);
             
            }
            catch (Exception ex)
            {
                string t = ex.Message;
            
            }
        }

        private void CrearPDF()
        {


        }
    }

}