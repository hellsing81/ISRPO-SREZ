using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Library.service;

namespace Library.page
{
    /// <summary>
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        public AuthorizationPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            try
            {

                var request = (HttpWebRequest)WebRequest.Create("https://localhost:7256/Login");

                var postData = "login=" + Uri.EscapeDataString(this.Login.Trim());
                postData += "&password=" + Uri.EscapeDataString(Secure.Sha256(this.tbPassword.Password.Trim()));
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                string resp = new StreamReader(response.GetResponseStream()).ReadToEnd();

               NavigationService.Navigate(new ReadersPage(resp));

            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
