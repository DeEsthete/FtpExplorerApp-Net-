using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FtpExplorerApp
{
    /// <summary>
    /// Логика взаимодействия для SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        private Window window;
        private string address;
        private string user;
        private string password;
        private bool isProtected;
        private FtpWebRequest ftpWebRequest;
        public SignInPage(Window window)
        {
            InitializeComponent();
            isProtectedCheckBox.Content = false;
            this.window = window;
        }

        private void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            address = "ftp://ftp.dlptest.com/";//addressTextBox.Text;
            user = "dlpuser@dlptest.com";//userTextBox.Text;
            password = "eiTqR7EMZD5zy7M";//passwordTextBox.Text;
            isProtected = (bool)isProtectedCheckBox.Content;
            Connect();
        }

        private void Connect()
        {
            try
            {
                //не работает checkBox
                ftpWebRequest = (FtpWebRequest)WebRequest.Create(address);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.Credentials = new NetworkCredential(user, password);
                window.Content = new MainPage(window, ftpWebRequest, address, user, password);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
