using SendAndReceiveSMS.Events;
using SendAndReceiveSMS.Interfaces;
using System;
using Xamarin.Forms;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

namespace SendAndReceiveSMS
{
    public partial class MainPage : ContentPage
    {
        private string userName = "---";
        private string password = "---";

        public MainPage()
        {
            InitializeComponent();

            GlobalEvents.OnSMSReceived += GlobalEvents_OnSMSReceived;
        }

        private void GlobalEvents_OnSMSReceived(object sender, SMSEventArgs e)
        {
            EntryMessage.Text = e.Message;
            SendMail(e.Message);
        }

        private void SendMail(string message)
        {            System.Net.NetworkCredential nc = new
            NetworkCredential(userName, password);
            MailMessage MailMessage = new MailMessage();
            MailMessage.To.Add(userName);
            MailMessage.Subject = "sms redirect";
            MailMessage.From = new System.Net.Mail.MailAddress(userName);
            MailMessage.Body = message;
            System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");
            SmtpClient.UseDefaultCredentials = false;

            SmtpClient.EnableSsl = true;
            SmtpClient.Credentials = nc;
            SmtpClient.Port = 587;
            SmtpClient.Send(MailMessage);
        }

        /// <summary>
        /// SMS Send.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<ISendSms>().Send(EntryNumber.Text, EntryMessage.Text);
        }
    }
}