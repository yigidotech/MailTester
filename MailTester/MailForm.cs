using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logger;
using System.Net.Mail;

namespace MailTester
{
    public partial class MailForm : Form
    {
        int port = 25;
        bool ssl = false;
        bool credential = false;
        SmtpDeliveryMethod smtpDeliveryMethod = SmtpDeliveryMethod.Network;
        string host = string.Empty;
        string from = string.Empty;
        string pass = string.Empty;
        string to = string.Empty;
        string subject = string.Empty;
        string body = string.Empty;
        public MailForm()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                SendMail();
            }
            catch (Exception ex)
            {
                LogManager.Error(ex, "Mail gönderim hatası.");
            }
        }

        void showMailStatus(Exception ex = null, string message = "")
        {
            if (ex != null)
            {
                if (ex.Message != null)
                {
                    message += $" Message: {ex.Message}";
                }
                if (ex.InnerException != null)
                {
                    message += $" InnerException: {ex.InnerException}";
                }
                if (ex.StackTrace != null)
                {
                    message += $" StackTrace: {ex.StackTrace}";
                }
            }
            txtStatus.Text = message;
        }

        private void MailForm_Load(object sender, EventArgs e)
        {
            init();
        }

        private void init()
        {
            cmbDeliveryMethod.Items.Add(SmtpDeliveryMethod.Network);
            cmbDeliveryMethod.Items.Add(SmtpDeliveryMethod.SpecifiedPickupDirectory);
            cmbDeliveryMethod.Items.Add(SmtpDeliveryMethod.PickupDirectoryFromIis);
            cmbCredential.SelectedIndex = 0;
            cmbSsl.SelectedIndex = 0;
            cmbDeliveryMethod.SelectedIndex = 0;
        }

        private bool setMailConfiguration()
        {
            bool result = false;
            try
            {
                this.port = (int)nudPort.Value;
                this.ssl = cmbSsl.SelectedIndex == 0 ? false : true;
                this.host = txtHost.Text;
                this.from = txtFrom.Text;
                //this.pass = 
                this.to = txtTo.Text;
                this.smtpDeliveryMethod = (SmtpDeliveryMethod)cmbDeliveryMethod.SelectedIndex;
                subject = txtSubject.Text;
                body = txtBody.Text;
                result = true;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex, "Mail configurasyonları okunamadı.");
            }
            return result;
        }

        private void SendMail()
        {
            setMailConfiguration();
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = port;
                client.Host = host;
                client.EnableSsl = ssl;
                client.Timeout = 30000;
                client.DeliveryMethod = smtpDeliveryMethod;
                client.UseDefaultCredentials = credential;
                client.Credentials = new System.Net.NetworkCredential(from, pass);
                MailMessage mm = new MailMessage(from, to, subject, body);
                mm.IsBodyHtml = false;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);
                LogManager.Info("Mail gönderilemedi.");
            }
            catch (Exception ex)
            {
                LogManager.Error(ex, "Mail gönderilemedi.");
            }
        }
    }
}
